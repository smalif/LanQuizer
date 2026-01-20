using EmbedIO;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Windows.Forms;
using System.Threading;
using static LanQuizer.TeacherHome;

namespace LanQuizer
{
    public partial class StartQuiz : Form
    {
        /*==================Variables========================*/
        private string _examName = string.Empty;
        private string _duration = "0";
        private string _marks = "0";
        private string _features = string.Empty;
        private string _totalMarks = "0";
        private System.Windows.Forms.Timer? quizTimer = null;
        private int remainingSeconds;
        private int _quizID = 0;

        // ====== Data passed from TeacherHome ======
        private string _course = string.Empty;
        private string _section = string.Empty;
        private string _password = string.Empty;

        public void LoadQuestionsJson(string questionsJson)
        {
            _questionsJson = questionsJson ?? "[]";
        }

        public void SetMeta(string course, string section)
        {
            _course = course ?? string.Empty;
            _section = section ?? string.Empty;
        }

        private string _questionsJson = "[]"; // NEW: store questions JSON
        private DateTime? _startTime;  // NEW: store quiz scheduled start time (nullable)

        // Server lifetime management
        private DateTime? _serverEndTime;
        private int _extraMinutes = 5; // extra time server stays alive for late joiners
        private int _durationMinutesParsed = 0;

        private bool serverRunning = false;
        private WebServer? webServer = null;
        private int port;
        private Label? linkLbl;

        /*==================Database connection================*/
        string connStr = ConfigurationManager.ConnectionStrings["LanQuizerDB"]?.ConnectionString ?? string.Empty;

        public StartQuiz()
        {
            InitializeComponent();
            this.Load += StartQuiz_Load;
            this.FormClosing += StartQuiz_FormClosing;
        }

        public StartQuiz(
            string examName,
            string duration,
            string marks,
            string features,
            string allowedMark,
            int quizId,
            string section,
            string password)
        {
            InitializeComponent();

            _examName = examName ?? string.Empty;
            _duration = duration ?? "0";
            _marks = marks ?? "0";
            _features = features ?? string.Empty;
            _totalMarks = allowedMark ?? "0";
            _quizID = quizId;
            _section = section ?? string.Empty;
            _password = password ?? string.Empty;

            // parse duration for later use (safe fallback)
            int.TryParse(_duration, out _durationMinutesParsed);

            this.Load += StartQuiz_Load;
            this.FormClosing += StartQuiz_FormClosing;
        }

        /*==================For Test: Search Method========================*/
        public bool LoadQuiz(int quizId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    string query = @"
                        SELECT TOP 1 ExamName, DurationMinutes, TotalMarks, Features, AllowedQuestion
                        FROM QuizTable
                        WHERE QuizId = @quizId";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@quizId", quizId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                _examName = reader["ExamName"]?.ToString() ?? string.Empty;
                                _duration = reader["DurationMinutes"]?.ToString() ?? "0";
                                _marks = reader["TotalMarks"]?.ToString() ?? "0";
                                _features = reader["Features"]?.ToString() ?? string.Empty;
                                _totalMarks = reader["AllowedQuestion"]?.ToString() ?? "0";
                                return true; // Exam found
                            }
                            else
                            {
                                MessageBox.Show("Exam not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return false; // Exam not found
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading exam: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        /*==================Form Load========================*/
        private void StartQuiz_Load(object? sender, EventArgs e)
        {
            // Display exam info
            examLbl.Text = "Exam Name: " + _examName;
            timeLbl.Text = "Duration: " + _duration + " Minutes";
            tmLbl.Text = "Total Marks: " + _marks;
            questionLbl.Text = "No Of Questions: " + _totalMarks;
            featureOn.Text = string.IsNullOrEmpty(_features) ? "No feature selected" : TryGetFeatures(_features);

            string TryGetFeatures(string json)
            {
                try
                {
                    return string.Join(", ",
                        JObject.Parse(json).Properties()
                            .Where(p => p.Value.Type == JTokenType.Boolean && (bool)p.Value)
                            .Select(p => Regex.Replace(p.Name, "([a-z])([A-Z])", "$1 $2")));
                }
                catch { return "No feature selected"; }
            }
            timerLbl.Visible = false;
            UpdateConnectionStatus();
            RefreshNetworkInfo();
        }

        private void UpdateConnectionStatus()
        {
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                connected.Text = "CONNECTED";
                connected.ForeColor = Color.Green;
                connectImg.Visible = true;
                disconnectImg.Visible = false;
            }
            else
            {
                connected.Text = "NOT CONNECTED";
                connected.ForeColor = Color.Red;
                disconnectImg.Visible = true;
                connectImg.Visible = false;
            }
        }

        private void RefreshNetworkInfo()
        {
            string ip = GetLocalIPAddress();
            IpLbl.Text = "IP Address: " + ip;

            // Get or create a port (this now handles firewall automatically)
            port = GetOrCreatePort();
            if (port == -1)
            {
                portLbl.Text = "No available port";
                portLbl.ForeColor = Color.Red;
                return;
            }

            portLbl.Text = "Port: " + port;
            portLbl.ForeColor = Color.Green;
        }

        private string GetLocalIPAddress()
        {
            foreach (var ip in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                    return ip.ToString();
            }
            return "IP Not Found";
        }

        private int GetDynamicPort()
        {
            try
            {
                TcpListener listener = new TcpListener(IPAddress.Any, 0);
                listener.Start();
                int freePort = ((IPEndPoint)listener.LocalEndpoint).Port;
                listener.Stop();
                return freePort;
            }
            catch { return -1; }
        }

        /*===========================Port Management===========================*/

        private void StopWebServer()
        {
            if (webServer != null)
            {
                webServer.Dispose();
                webServer = null;
            }
        }

        private void startBtn_Click(object? sender, EventArgs e)
        {

            if (!serverRunning)
            {
                // ensure duration parsed
                if (_durationMinutesParsed <= 0)
                    int.TryParse(_duration, out _durationMinutesParsed);

                // teacher-facing countdown should be the quiz duration (not including extra)
                remainingSeconds = Math.Max(0, _durationMinutesParsed * 60);

                // set server start time and end time (duration + extra)
                _startTime = DateTime.Now;
                _serverEndTime = _startTime.Value.AddMinutes(_durationMinutesParsed + _extraMinutes);

                // update DB StartTime and Status for this quiz (best-effort)
                try
                {
                    using (var con = new SqlConnection(connStr))
                    {
                        con.Open();
                        using (var cmd = new SqlCommand("UPDATE QuizTable SET StartTime=@start, Status='Running' WHERE QuizID=@quizId", con))
                        {
                            cmd.Parameters.AddWithValue("@start", _startTime);
                            cmd.Parameters.AddWithValue("@quizId", _quizID);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch { /* swallow DB errors - non-fatal for running server */ }

                // Start server
                StartWebServer(port);

                // ===== START COUNTDOWN TIMER for teacher UI =====
                timerLbl.Visible = true;

                quizTimer = new System.Windows.Forms.Timer();
                quizTimer.Interval = 1000; // 1 second
                quizTimer.Tick += (s, ev) =>
                {
                    if (remainingSeconds > 0)
                    {
                        remainingSeconds--;
                        int min = remainingSeconds / 60;
                        int sec = remainingSeconds % 60;
                        timerLbl.Text = $"Time Left: {min:D2}:{sec:D2}";
                    }
                    else
                    {
                        quizTimer.Stop();
                        timerLbl.Text = "Time Left: 00:00";
                        // stop accepting new student sessions, keep server alive for extra minutes to allow submissions to arrive
                        StartButtonAfterDurationExpire();
                    }
                };
                quizTimer.Start();
                // ================================

                // Ensure link label exists
                if (linkLbl == null)
                {
                    linkLbl = new Label
                    {
                        Location = new Point(200, portLbl.Bottom + 20),
                        AutoSize = true,
                        ForeColor = Color.Blue,
                        Cursor = Cursors.Hand
                    };
                    linkLbl.Click += (s, e) =>
                    {
                        try
                        {
                            Process.Start(new ProcessStartInfo
                            {
                                FileName = linkLbl.Text.Replace("Visit this link: ", ""),
                                UseShellExecute = true
                            });
                        }
                        catch { }
                    };
                    this.Controls.Add(linkLbl);
                }

                string ip = GetLocalIPAddress();
                linkLbl.Text = $"Visit this link: http://{ip}:{port}/";
                linkLbl.BringToFront();
                startBtn.Text = "Server Running";
                startBtn.BackColor = Color.Red;
                serverRunning = true;
            }
            else
            {
                // manual stop: stop server and timer immediately
                timerLbl.Visible = false;
                StopWebServer();
                if (linkLbl != null) { linkLbl.Text = "Quiz Stopped!"; linkLbl.ForeColor = Color.Red; }
                startBtn.Text = "Start Quiz";
                startBtn.BackColor = Color.Green;
                serverRunning = false;
                MessageBox.Show("Server stopped.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void StartButtonAfterDurationExpire()
        {
            // Called once quiz duration has completed (teacher sees 00:00).
            // We do not immediately stop the server: keep it running until _serverEndTime to accept final submissions.
            // After that, stop the server automatically.
            var end = _serverEndTime ?? DateTime.Now;
            var ms = (int)(end - DateTime.Now).TotalMilliseconds;
            if (ms <= 0)
            {
                // already expired
                StopWebServer();
                serverRunning = false;
                startBtn.Text = "Start Quiz";
                startBtn.BackColor = Color.Green;
                return;
            }

            var t = new System.Threading.Timer(_ =>
            {
                StopWebServer();
                serverRunning = false;
                this.BeginInvoke(new Action(() =>
                {
                    if (linkLbl != null) linkLbl.Text = "Quiz Closed";
                    startBtn.Text = "Start Quiz";
                    startBtn.BackColor = Color.Green;
                    MessageBox.Show("Server closed after extra join time.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }));
            }, null, ms, System.Threading.Timeout.Infinite);
        }

        private void MarkQuizAsCompleted()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connStr))
                {
                    con.Open();
                    string q = "UPDATE QuizTable SET Status = 'Completed' WHERE QuizID = @quizId";
                    using (SqlCommand cmd = new SqlCommand(q, con))
                    {
                        cmd.Parameters.AddWithValue("@quizId", _quizID);
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Updating QuizID: " + _quizID);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to auto-complete quiz: " + ex.Message);
            }
        }

        private void connected_Click(object? sender, EventArgs e)
        {
            UpdateConnectionStatus();
        }

        private void exitBtn_Click(object? sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Are you sure you want to exit? This will close the quiz.",
                "Confirm Exit",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
                this.Close();
        }

        private int GetOrCreatePort()
        {
            int savedPort = 0;
            int.TryParse(ConfigurationManager.AppSettings["LanQuizPort"], out savedPort);

            // Use saved port if available
            if (savedPort > 0 && IsPortAvailable(savedPort))
                return savedPort;

            // Otherwise, find a free port
            int newPort = GetDynamicPort();
            if (newPort == -1) return -1;

            // Try to open firewall only once
            bool success = TryOpenFirewallPort(newPort);
            if (!success)
            {
                MessageBox.Show(
                    $"Cannot open port {newPort}. Please run as administrator.",
                    "Firewall Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return -1;
            }

            // Save port for next time
            SavePortToConfig(newPort);
            return newPort;
        }

        private bool TryOpenFirewallPort(int port)
        {
            try
            {
                string ruleName = $"LanQuiz_{port}";

                // Check if the rule already exists
                var processInfo = new ProcessStartInfo("netsh",
                    $"advfirewall firewall show rule name=\"{ruleName}\"")
                {
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (var process = Process.Start(processInfo))
                {
                    string output = process?.StandardOutput.ReadToEnd() ?? string.Empty;
                    process?.WaitForExit();

                    if (output.Contains(ruleName))
                        return true; // Already allowed, no PowerShell popup
                }

                // Rule doesn't exist → add it (admin popup may appear)
                var psi = new ProcessStartInfo("netsh")
                {
                    Arguments = $"advfirewall firewall add rule name=\"{ruleName}\" dir=in action=allow protocol=TCP localport={port}",
                    Verb = "runas",
                    WindowStyle = ProcessWindowStyle.Hidden,
                    CreateNoWindow = true,
                    UseShellExecute = true
                };

                Process.Start(psi)?.WaitForExit();
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void StartQuiz_FormClosing(object? sender, FormClosingEventArgs e)
        {
            StopWebServer();
        }

        public void SetQuizMeta(int quizID, string examName, string course, string section, string duration, string totalMarks)
        {
            _quizID = quizID;
            _examName = examName ?? string.Empty;
            _course = course ?? string.Empty;
            _section = section ?? string.Empty;
            _duration = duration ?? "0";
            _totalMarks = totalMarks ?? "0";
        }

        // ======================= CLASSES =======================
        public class QuizQuestion
        {
            public string Question { get; set; } = string.Empty;   // must match JSON key
            public List<string> Options { get; set; } = new List<string>();
            public int CorrectIndex { get; set; }
            public int Marks { get; set; }
        }

        public class StudentAnswer
        {
            public int QuestionIndex { get; set; }
            public int SelectedOption { get; set; }
        }

        private void StartWebServer(int port)
        {
            StopWebServer();

            string url = $"http://+:{port}/";

            webServer = new WebServer(url)

            // ========================== GET QUIZ PAGE ==========================
            .WithAction("/", HttpVerbs.Get, async ctx =>
            {
                // build HTML with Course & Section subheading and popup containing student id + password fields
                StringBuilder html = new StringBuilder();
                html.AppendLine("<html><head><title>LAN Quiz</title>");
                html.AppendLine("<style>");
                html.AppendLine(".question-box{border:1px solid #ccc;padding:15px;margin-bottom:20px;border-radius:10px;text-align:left}");
                html.AppendLine("</style>");
                html.AppendLine("<script>");
                html.AppendLine("var duration = 0;");
                html.AppendLine("function startValidate() {");
                html.AppendLine("  var sid = document.getElementById('studentId').value;");
                html.AppendLine("  var pwd = document.getElementById('studentPwd').value;");
                html.AppendLine("  if(!sid || !pwd) { alert('Enter Student ID and Password'); return; }");
                html.AppendLine("  var data = 'studentId=' + encodeURIComponent(sid) + '&password=' + encodeURIComponent(pwd) + '&section=' + encodeURIComponent(document.getElementById('section').value) + '&course=' + encodeURIComponent(document.getElementById('course').value);");
                html.AppendLine("  fetch('/validate', { method:'POST', headers:{'Content-Type':'application/x-www-form-urlencoded'}, body: data })");
                html.AppendLine("    .then(r => r.json())");
                html.AppendLine("    .then(j => {");
                html.AppendLine("      if(!j.success) { alert(j.message || 'Validation failed'); return; }");
                html.AppendLine("      document.getElementById('shuffledQuestions').value = j.shuffledQuestions;");
                html.AppendLine("      duration = j.studentSeconds;"); // seconds
                html.AppendLine("      document.getElementById('startPopup').style.display='none';");
                html.AppendLine("      document.getElementById('quizForm').style.display='block';");
                html.AppendLine("      renderQuestionsFromJson();");
                html.AppendLine("      startClientTimer(duration);");
                html.AppendLine("    })");
                html.AppendLine("    .catch(e => { alert('Validation error'); });");
                html.AppendLine("}");
                html.AppendLine("function startClientTimer(d) {");
                html.AppendLine("  var timerInterval = setInterval(function(){");
                html.AppendLine("    var m = Math.floor(d/60);");
                html.AppendLine("    var s = d % 60;");
                html.AppendLine("    document.getElementById('timer').textContent = m + ':' + (s<10?'0':'') + s;");
                html.AppendLine("    d--;");
                html.AppendLine("    if(d<0){");
                html.AppendLine("      clearInterval(timerInterval);");
                html.AppendLine("      alert('Time up! Submitting...');");
                html.AppendLine("      document.getElementById('quizForm').submit();");
                html.AppendLine("    }");
                html.AppendLine("  },1000);");
                html.AppendLine("}");
                html.AppendLine("function renderQuestionsFromJson() {");
                html.AppendLine("  var j = document.getElementById('shuffledQuestions').value;");
                html.AppendLine("  if(!j) return;");
                html.AppendLine("  var questions = JSON.parse(j);");
                html.AppendLine("  var html = '';");
                html.AppendLine("  for(var qi=0; qi<questions.length; qi++){");
                html.AppendLine("    var q = questions[qi];");
                html.AppendLine("    html += '<div class=\"question-box\"><p><strong>Q'+(qi+1)+': '+q.Question+'</strong><br/>';");
                html.AppendLine("    for(var oi=0; oi<q.Options.length; oi++) {");
                html.AppendLine("      html += '<label><input type=\"radio\" name=\"q'+(qi+1)+'\" value=\"'+oi+'\" required> '+q.Options[oi]+'</label><br/>';");
                html.AppendLine("    }");
                html.AppendLine("    html += '</p></div>'; ");
                html.AppendLine("  }");
                html.AppendLine("  document.getElementById('questionsContainer').innerHTML = html;");
                html.AppendLine("}");
                html.AppendLine("</script>");
                html.AppendLine("</head><body style='font-family:Arial;text-align:center'>");
                html.AppendLine($"<h1>Exam: {HttpUtility.HtmlEncode(_examName)}</h1>");
                html.AppendLine($"<h3 style='color:#333;margin-top:0'>Course: {HttpUtility.HtmlEncode(_course)} | Section: {HttpUtility.HtmlEncode(_section)}</h3>");

                // popup for student ID + password
                html.AppendLine("<div id='startPopup' style='border:2px solid #333;padding:20px;border-radius:10px;display:block'>");
                html.AppendLine("  <h3>Student Login</h3>");
                html.AppendLine("  Student ID: <input type='text' id='studentId'><br><br>");
                html.AppendLine("  Password: <input type='password' id='studentPwd'><br><br>");
                html.AppendLine($"  <input type='hidden' id='course' value='{HttpUtility.HtmlEncode(_course)}'>");
                html.AppendLine($"  <input type='text' id='section' value='{HttpUtility.HtmlEncode(_section)}' readonly><br><br>");
                html.AppendLine("  <button onclick='startValidate()'>Start Quiz</button>");
                html.AppendLine("</div>");

                // hidden form (will be populated by /validate response)
                html.AppendLine("<form id='quizForm' method='POST' action='/submit' style='display:none'>");
                html.AppendLine("<div id='timer' style='font-weight:bold;font-size:20px;margin-bottom:15px'></div>");
                html.AppendLine("<div id='questionsContainer'></div>");
                html.AppendLine("<input type='hidden' name='studentId' id='formStudentId'>");
                html.AppendLine("<input type='hidden' name='section' id='formSection'>");
                html.AppendLine("<input type='hidden' name='shuffledQuestions' id='shuffledQuestions'>");
                html.AppendLine("<button type='submit'>Submit</button>");
                html.AppendLine("</form>");

                html.AppendLine("<script>");
                html.AppendLine("document.getElementById('quizForm').addEventListener('submit', function(){");
                html.AppendLine("  document.getElementById('formStudentId').value = document.getElementById('studentId').value;");
                html.AppendLine("  document.getElementById('formSection').value = document.getElementById('section').value;");
                html.AppendLine("});");
                html.AppendLine("</script>");

                html.AppendLine("</body></html>");

                await ctx.SendStringAsync(html.ToString(), "text/html", Encoding.UTF8);
            })

            // ========================== VALIDATE student identity ==========================
            .WithAction("/validate", HttpVerbs.Post, async ctx =>
            {
                string body;
                using (var reader = new StreamReader(ctx.Request.InputStream, ctx.Request.ContentEncoding))
                    body = await reader.ReadToEndAsync();

                var form = HttpUtility.ParseQueryString(body);
                string studentId = form["studentId"] ?? "";
                string password = form["password"] ?? "";
                string section = form["section"] ?? _section ?? "";
                string course = form["course"] ?? _course ?? "";

                // verify student exists in Students table for the course & section
                bool studentExists = true;
                string quizPassword = "pass123";

                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT COUNT(1) FROM Students WHERE StudentID=@sid AND Section=@sec AND Course=@course", conn))
                    {
                        cmd.Parameters.AddWithValue("@sid", studentId);
                        cmd.Parameters.AddWithValue("@sec", section);
                        cmd.Parameters.AddWithValue("@course", course);
                        var cnt = Convert.ToInt32(cmd.ExecuteScalar() ?? 0);
                        studentExists = cnt > 0;
                    }

                    // fetch actual quiz password if not available
                    if (string.IsNullOrEmpty(quizPassword))
                    {
                        using (SqlCommand cmd2 = new SqlCommand("SELECT QuizPassword FROM QuizTable WHERE QuizID=@qId", conn))
                        {
                            cmd2.Parameters.AddWithValue("@qId", _quizID);
                            var res = cmd2.ExecuteScalar();
                            if (res != null) quizPassword = res.ToString() ?? "";
                        }
                    }
                }

                if (!studentExists)
                {
                    var resp = new { success = false, message = "Student ID not found for this course/section" };
                    await ctx.SendStringAsync(JsonConvert.SerializeObject(resp), "application/json", Encoding.UTF8);
                    return;
                }

                if (quizPassword != password)
                {
                    var resp = new { success = false, message = "Incorrect quiz password" };
                    await ctx.SendStringAsync(JsonConvert.SerializeObject(resp), "application/json", Encoding.UTF8);
                    return;
                }

                // allowed student seconds = remaining until end of quiz duration (not including extra join time)
                int studentSeconds = 0;
                if (_startTime.HasValue)
                {
                    var durationEnd = _startTime.Value.AddMinutes(_durationMinutesParsed);
                    studentSeconds = (int)Math.Max(0, (durationEnd - DateTime.Now).TotalSeconds);
                }
                else
                {
                    // if no start time set, allow full duration
                    studentSeconds = _durationMinutesParsed * 60;
                }

                if (studentSeconds <= 0)
                {
                    var resp = new { success = false, message = "Quiz duration has ended" };
                    await ctx.SendStringAsync(JsonConvert.SerializeObject(resp), "application/json", Encoding.UTF8);
                    return;
                }

                // prepare shuffled questions server-side (so same order for all students)
                string questionsJson = _questionsJson ?? "[]";
                var questions = JsonConvert.DeserializeObject<List<QuizQuestion>>(questionsJson) ?? new List<QuizQuestion>();
                var rnd = new Random();
                questions = questions.OrderBy(q => rnd.Next()).ToList();
                foreach (var q in questions)
                {
                    string correctText = q.Options.ElementAtOrDefault(q.CorrectIndex) ?? string.Empty;
                    q.Options = q.Options.OrderBy(o => rnd.Next()).ToList();
                    q.CorrectIndex = q.Options.IndexOf(correctText);
                }
                string shuffledJson = JsonConvert.SerializeObject(questions);

                var ok = new { success = true, shuffledQuestions = shuffledJson, studentSeconds = studentSeconds };
                await ctx.SendStringAsync(JsonConvert.SerializeObject(ok), "application/json", Encoding.UTF8);
            })

            // ========================== SUBMIT ==========================
            .WithAction("/submit", HttpVerbs.Post, async ctx =>
            {
                string body;
                using (var reader = new StreamReader(ctx.Request.InputStream, ctx.Request.ContentEncoding))
                    body = await reader.ReadToEndAsync();

                var form = HttpUtility.ParseQueryString(body);

                string studentId = form["studentId"] ?? "";
                string section = form["section"] ?? "";
                string shuffledJson = form["shuffledQuestions"] ?? "[]";

                var questions = JsonConvert.DeserializeObject<List<QuizQuestion>>(shuffledJson) ?? new List<QuizQuestion>();

                int score = 0;
                int totalMarks = questions.Sum(q => q.Marks);

                var studentAnswers = new List<StudentAnswer>();

                for (int i = 0; i < questions.Count; i++)
                {
                    string key = $"q{i + 1}";
                    if (form[key] != null)
                    {
                        int selectedIndex = int.Parse(form[key] ?? "0");
                        if (selectedIndex == questions[i].CorrectIndex)
                            score += questions[i].Marks;

                        studentAnswers.Add(new StudentAnswer
                        {
                            QuestionIndex = i,
                            SelectedOption = selectedIndex
                        });
                    }
                }

                // allow submission if within serverEndTime; still accept until serverEndTime (extra minutes)
                if (_serverEndTime.HasValue && DateTime.Now > _serverEndTime.Value)
                {
                    var denied = "<html><body><h1>Submission rejected: server closed</h1></body></html>";
                    await ctx.SendStringAsync(denied, "text/html", Encoding.UTF8);
                    return;
                }

                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    string insert = @"
INSERT INTO StudentAttempts
(QuizID, StudentID, Section, Course, RandomizedQuestions, Answers, Score, LoginTime, SubmitTime)
VALUES
(@quizId,@studentId,@section,@course,@randQ,@answers,@score,@loginTime,@submitTime)";

                    using (SqlCommand cmd = new SqlCommand(insert, conn))
                    {
                        cmd.Parameters.AddWithValue("@quizId", _quizID);
                        cmd.Parameters.AddWithValue("@studentId", studentId);
                        cmd.Parameters.AddWithValue("@section", section);
                        cmd.Parameters.AddWithValue("@course", _course ?? "");
                        cmd.Parameters.AddWithValue("@randQ", shuffledJson);
                        cmd.Parameters.AddWithValue("@answers", JsonConvert.SerializeObject(studentAnswers));
                        cmd.Parameters.AddWithValue("@score", score);
                        cmd.Parameters.AddWithValue("@loginTime", DateTime.Now);
                        cmd.Parameters.AddWithValue("@submitTime", DateTime.Now);
                        cmd.ExecuteNonQuery();
                    }
                }

                string resultHtml =
                    $"<html><body style='text-align:center;font-family:Arial'>" +
                    $"<h1>Result</h1>" +
                    $"<p>Total Marks: {totalMarks}</p>" +
                    $"<p>Your Score: {score}</p>" +
                    $"</body></html>";

                await ctx.SendStringAsync(resultHtml, "text/html", Encoding.UTF8);
            });

            webServer.RunAsync();
        }

        //===================================================================

        private void SavePortToConfig(int port)
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            if (config.AppSettings.Settings["LanQuizPort"] != null)
                config.AppSettings.Settings["LanQuizPort"].Value = port.ToString();
            else
                config.AppSettings.Settings.Add("LanQuizPort", port.ToString());

            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        private bool IsPortAvailable(int port)
        {
            try
            {
                TcpListener listener = new TcpListener(IPAddress.Any, port);
                listener.Start();
                listener.Stop();
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void startLbl_Click(object? sender, EventArgs e) { }

        private void StartQuiz_Load_1(object? sender, EventArgs e)
        {

        }

        public bool LoadQuizByID(int quizID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    string query = @"
                SELECT TOP 1 ExamName, DurationMinutes, Questions, StartTime
                FROM QuizTable
                WHERE QuizID = @quizID";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@quizID", quizID);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                _examName = reader["ExamName"]?.ToString() ?? string.Empty;
                                _duration = reader["DurationMinutes"]?.ToString() ?? "0";
                                _questionsJson = reader["Questions"]?.ToString() ?? "[]"; // store JSON string in a new variable
                                if (reader["StartTime"] != DBNull.Value)
                                    _startTime = Convert.ToDateTime(reader["StartTime"]);

                                return true;
                            }
                            else
                            {
                                MessageBox.Show("Quiz not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading quiz: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void Logo_Paint(object? sender, PaintEventArgs e)
        {

        }

        private void IpLbl_Click(object? sender, EventArgs e)
        {

        }
    }
}
