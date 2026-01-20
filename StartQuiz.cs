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

            // Determine number of questions to display per student
            int displayQuestionCount = 0;
            try
            {
                // parse features
                bool shuffleQuestions = false;
                try
                {
                    if (!string.IsNullOrWhiteSpace(_features))
                    {
                        var fobj = JObject.Parse(_features);
                        shuffleQuestions = fobj.Property("ShuffleQuestions", StringComparison.OrdinalIgnoreCase) != null && (bool?)fobj["ShuffleQuestions"] == true;
                    }
                }
                catch { }

                // parse questions JSON
                var questionsList = new List<QuizQuestion>();
                try
                {
                    var jarr = JArray.Parse(_questionsJson ?? "[]");
                    foreach (var j in jarr)
                    {
                        if (j is not JObject jo) continue;
                        string qtext = jo.Value<string>("Question") ?? jo.Value<string>("QuestionText") ?? jo.Value<string>("question") ?? string.Empty;
                        var optsToken = jo["Options"] ?? jo["options"];
                        var opts = new List<string>();
                        if (optsToken is JArray oa)
                        {
                            foreach (var o in oa)
                                opts.Add(o.ToString());
                        }
                        int corr = jo.Value<int?>("CorrectIndex") ?? jo.Value<int?>("correctIndex") ?? 0;
                        int marks = jo.Value<int?>("Marks") ?? jo.Value<int?>("marks") ?? 0;

                        questionsList.Add(new QuizQuestion
                        {
                            Question = qtext ?? string.Empty,
                            Options = opts,
                            CorrectIndex = corr,
                            Marks = marks
                        });
                    }
                }
                catch { }

                int totalQuestions = questionsList.Count;

                // get allowed marks (AllowedQuestion) from _totalMarks or DB
                int allowedMarks = 0;
                if (!int.TryParse(_totalMarks, out allowedMarks) || allowedMarks <= 0)
                {
                    try
                    {
                        using (var con = new SqlConnection(connStr))
                        {
                            con.Open();
                            using (var cmd = new SqlCommand("SELECT ISNULL(AllowedQuestion,0) FROM QuizTable WHERE QuizID=@qId", con))
                            {
                                cmd.Parameters.AddWithValue("@qId", _quizID);
                                allowedMarks = Convert.ToInt32(cmd.ExecuteScalar() ?? 0);
                            }
                        }
                    }
                    catch { }
                }

                if (allowedMarks > 0 && shuffleQuestions && questionsList.Count > 0)
                {
                    // attempt to find a subset of questions whose marks sum to allowedMarks
                    int totalMarksAvailable = questionsList.Sum(q => q.Marks);
                    if (allowedMarks <= totalMarksAvailable)
                    {
                        var rnd = new Random();
                        List<QuizQuestion>? chosen = null;
                        const int maxAttempts = 1000;
                        for (int attempt = 0; attempt < maxAttempts && chosen == null; attempt++)
                        {
                            var shuffled = questionsList.OrderBy(x => rnd.Next()).ToList();
                            var sel = new List<QuizQuestion>();
                            int sum = 0;
                            foreach (var q in shuffled)
                            {
                                if (sum + q.Marks <= allowedMarks)
                                {
                                    sel.Add(q);
                                    sum += q.Marks;
                                    if (sum == allowedMarks) break;
                                }
                            }

                            if (sum == allowedMarks)
                                chosen = sel;
                        }

                        if (chosen != null)
                            displayQuestionCount = chosen.Count;
                        else
                            displayQuestionCount = totalQuestions; // fallback
                    }
                    else
                    {
                        // not enough marks -> show total questions (teacher will be warned elsewhere)
                        displayQuestionCount = totalQuestions;
                    }
                }
                else
                {
                    // if not shuffling or no allowedMarks, show all questions
                    displayQuestionCount = totalQuestions;
                }
            }
            catch { displayQuestionCount = 0; }

            questionLbl.Text = "No Of Questions: " + displayQuestionCount.ToString();

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

        private void UpdateQuizStatus(string status, bool setStartTime = false, bool setEndTime = false)
        {
            if (_quizID <= 0) return;
            try
            {
                using (var con = new SqlConnection(connStr))
                {
                    con.Open();
                    var sb = new StringBuilder("UPDATE QuizTable SET Status=@status");
                    if (setStartTime) sb.Append(", StartTime=@start");
                    if (setEndTime) sb.Append(", EndTime=@end");
                    sb.Append(" WHERE QuizID=@quizId");

                    using (var cmd = new SqlCommand(sb.ToString(), con))
                    {
                        cmd.Parameters.AddWithValue("@status", status ?? (object)DBNull.Value);
                        if (setStartTime)
                            cmd.Parameters.AddWithValue("@start", DateTime.Now);
                        if (setEndTime)
                            cmd.Parameters.AddWithValue("@end", DateTime.Now);
                        cmd.Parameters.AddWithValue("@quizId", _quizID);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch { /* ignore DB errors for robustness */ }
        }

        private string GetQuizStatusFromDb()
        {
            if (_quizID <= 0) return string.Empty;
            try
            {
                using (var con = new SqlConnection(connStr))
                {
                    con.Open();
                    using (var cmd = new SqlCommand("SELECT ISNULL(Status,'') FROM QuizTable WHERE QuizID=@quizId", con))
                    {
                        cmd.Parameters.AddWithValue("@quizId", _quizID);
                        var res = cmd.ExecuteScalar();
                        return res == null || res == DBNull.Value ? string.Empty : res.ToString() ?? string.Empty;
                    }
                }
            }
            catch { return string.Empty; }
        }

        private void startBtn_Click(object? sender, EventArgs e)
        {

            if (!serverRunning)
            {
                // If quiz id missing, try to create/save quiz first so it has an ID
                if (_quizID <= 0)
                {
                    try
                    {
                        string teacherEmail = LoggedInUser.Email ?? string.Empty;

                        using (var con = new SqlConnection(connStr))
                        {
                            con.Open();

                            // Try find existing quiz by teacher + exam name
                            string check = "SELECT QuizID FROM QuizTable WHERE TeacherEmail=@teacherEmail AND ExamName=@examName";
                            using (var cmd = new SqlCommand(check, con))
                            {
                                cmd.Parameters.AddWithValue("@teacherEmail", teacherEmail);
                                cmd.Parameters.AddWithValue("@examName", _examName ?? string.Empty);
                                object found = cmd.ExecuteScalar();
                                if (found != null && found != DBNull.Value)
                                {
                                    _quizID = Convert.ToInt32(found);
                                }
                            }

                            if (_quizID <= 0)
                            {
                                // insert new quiz and return identity
                                string insert = @"
INSERT INTO QuizTable
(TeacherID, TeacherEmail, ExamName, DurationMinutes, AllowedQuestion, Features, QuizPassword, Questions, Status, CreatedAt, Course, Section, StartTime)
VALUES
((SELECT TeacherID FROM Teachers WHERE TeacherEmail=@teacherEmail), @teacherEmail, @examName, @duration, @allowedQuestion, @features, @password, @questions, @status, @createdAt, @course, @section, @startTime);
SELECT CAST(SCOPE_IDENTITY() AS INT);";

                                int durationVal = 0;
                                int allowedQ = 0;
                                int.TryParse(_duration, out durationVal);
                                int.TryParse(_totalMarks, out allowedQ);

                                using (var cmd = new SqlCommand(insert, con))
                                {
                                    cmd.Parameters.AddWithValue("@teacherEmail", teacherEmail);
                                    cmd.Parameters.AddWithValue("@examName", _examName ?? string.Empty);
                                    cmd.Parameters.AddWithValue("@duration", durationVal);
                                    cmd.Parameters.AddWithValue("@allowedQuestion", allowedQ);
                                    cmd.Parameters.AddWithValue("@features", _features ?? string.Empty);
                                    cmd.Parameters.AddWithValue("@password", _password ?? string.Empty);
                                    cmd.Parameters.AddWithValue("@questions", _questionsJson ?? string.Empty);
                                    cmd.Parameters.AddWithValue("@status", "Running");
                                    cmd.Parameters.AddWithValue("@createdAt", DateTime.Now);
                                    cmd.Parameters.AddWithValue("@course", _course ?? string.Empty);
                                    cmd.Parameters.AddWithValue("@section", _section ?? string.Empty);
                                    cmd.Parameters.AddWithValue("@startTime", DateTime.Now);

                                    object idobj = cmd.ExecuteScalar();
                                    if (idobj != null && idobj != DBNull.Value)
                                    {
                                        _quizID = Convert.ToInt32(idobj);
                                    }
                                    else
                                    {
                                        MessageBox.Show("Failed to save quiz before hosting.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return;
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error saving quiz: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                // ensure duration parsed
                if (_durationMinutesParsed <= 0)
                    int.TryParse(_duration, out _durationMinutesParsed);

                // teacher-facing countdown should be the quiz duration + extra minutes for server stay
                remainingSeconds = Math.Max(0, _durationMinutesParsed * 60 + _extraMinutes * 60);

                 // set server start time and end time (duration + extra)
                 _startTime = DateTime.Now;
                 _serverEndTime = _startTime.Value.AddMinutes(_durationMinutesParsed + _extraMinutes);

                 // update DB StartTime and Status for this quiz (best-effort)
                 UpdateQuizStatus("Running", setStartTime: true, setEndTime: false);
 
                 // Start server
                 StartWebServer(port);
 
                 // ===== START COUNTDOWN TIMER for teacher UI =====
                 timerLbl.Visible = true;
                 timerLbl.ForeColor = Color.SeaGreen;
 
                 quizTimer = new System.Windows.Forms.Timer();
                 quizTimer.Interval = 1000; // 1 second
                 quizTimer.Tick += (s, ev) =>
                 {
                     // decrement first
                     if (remainingSeconds > 0)
                         remainingSeconds--;
 
                     int min = remainingSeconds / 60;
                     int sec = remainingSeconds % 60;
                     timerLbl.Text = $"Time Left: {min:D2}:{sec:D2}";
 
                     // change color if last minute
                     if (remainingSeconds <= 60)
                         timerLbl.ForeColor = Color.Red;
                     else
                         timerLbl.ForeColor = Color.SeaGreen;
 
                     // when reached zero, stop and trigger server closure flow
                     if (remainingSeconds <= 0)
                     {
                         try { quizTimer.Stop(); } catch { }
                         timerLbl.Text = "Time Left: 00:00";
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
                // confirm manual stop
                var res = MessageBox.Show("Stop the server? This will mark the quiz as Completed.", "Stop Server", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (res != DialogResult.Yes) return;

                // manual stop: stop server and timer immediately
                try { quizTimer?.Stop(); } catch { }
                timerLbl.Visible = false;
                StopWebServer();
                if (linkLbl != null) { linkLbl.Text = "Quiz Stopped!"; linkLbl.ForeColor = Color.Red; }
                startBtn.Text = "Start Quiz";
                startBtn.BackColor = Color.Green;
                serverRunning = false;

                // Mark quiz as completed and set EndTime
                UpdateQuizStatus("Completed", setStartTime: false, setEndTime: true);

                MessageBox.Show("Server stopped and quiz marked as Completed.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            if (serverRunning)
            {
                var dr = MessageBox.Show("Server is running. Closing will stop the server and mark the quiz as Completed. Continue?", "Confirm Close", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dr != DialogResult.Yes)
                {
                    e.Cancel = true;
                    return;
                }

                try { quizTimer?.Stop(); } catch { }
                try { StopWebServer(); } catch { }
                serverRunning = false;
                UpdateQuizStatus("Completed", setStartTime: false, setEndTime: true);
            }
            else
            {
                StopWebServer();
            }
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
                html.AppendLine("      // set verified student name in page header if provided");
                html.AppendLine("      if(j.studentName) document.getElementById('studentName').textContent = j.studentName;");
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
                html.AppendLine("  var html = '';    ");
                html.AppendLine("  for(var qi=0; qi<questions.length; qi++){ ");
                html.AppendLine("    var q = questions[qi];");
                html.AppendLine("    var marks = (q.Marks !== undefined ? q.Marks : 0);");
                html.AppendLine("    var qText = q.Question || q.QuestionText || q.question || '';" +
                    " html += '<div class=\"question-box\"><p><strong>Q'+(qi+1)+': '+qText+'</strong> <span style=\"color:#888;font-size:0.9em\">(Marks: '+marks+')</span><br/>';" +
                    "    for(var oi=0; oi< (q.Options||[]).length; oi++) {" +
                    "      html += '<label><input type=\"radio\" name=\"q'+(qi+1)+'\" value=\"'+oi+'\" required> '+(q.Options[oi]||'')+'</label><br/>';" +
                    "    }" +
                    "    html += '</p></div>'; " +
                    "  }" +
                    "  document.getElementById('questionsContainer').innerHTML = html;" +
                    "}");

                html.AppendLine("</script>");
                html.AppendLine("</head><body style='font-family:Arial;text-align:center'>");
                // student name placeholder (will be set after validation)
                html.AppendLine("<div id='studentName' style='position:absolute;right:20px;top:10px;font-weight:bold'></div>");
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
                string studentId = (form["studentId"] ?? "").Trim();
                string password = (form["password"] ?? "").Trim();
                string section = (form["section"] ?? _section ?? "").Trim();
                string course = (form["course"] ?? _course ?? "").Trim();

                // verify student exists in Students table for the course & section
                bool studentExists = false;
                string studentName = string.Empty;
                // use the quiz password provided when the quiz was created/opened
                string quizPassword = _password ?? string.Empty;

                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    // include TeacherEmail to ensure we are checking the correct teacher's students
                    using (SqlCommand cmd = new SqlCommand("SELECT COUNT(1) FROM Students WHERE LTRIM(RTRIM(StudentID)) = @sid AND LTRIM(RTRIM(Section)) = @sec AND LTRIM(RTRIM(Course)) = @course AND TeacherEmail = @teacherEmail", conn))
                    {
                        cmd.Parameters.AddWithValue("@sid", studentId);
                        cmd.Parameters.AddWithValue("@sec", section);
                        cmd.Parameters.AddWithValue("@course", course);
                        cmd.Parameters.AddWithValue("@teacherEmail", LoggedInUser.Email ?? string.Empty);
                        var cnt = Convert.ToInt32(cmd.ExecuteScalar() ?? 0);
                        studentExists = cnt > 0;
                    }

                    if (studentExists)
                    {
                        using (SqlCommand cmdName = new SqlCommand("SELECT TOP 1 StudentName FROM Students WHERE LTRIM(RTRIM(StudentID)) = @sid AND LTRIM(RTRIM(Section)) = @sec AND LTRIM(RTRIM(Course)) = @course AND TeacherEmail = @teacherEmail", conn))
                        {
                            cmdName.Parameters.AddWithValue("@sid", studentId);
                            cmdName.Parameters.AddWithValue("@sec", section);
                            cmdName.Parameters.AddWithValue("@course", course);
                            cmdName.Parameters.AddWithValue("@teacherEmail", LoggedInUser.Email ?? string.Empty);
                            var sname = cmdName.ExecuteScalar();
                            if (sname != null && sname != DBNull.Value) studentName = sname.ToString() ?? string.Empty;
                        }
                    }

                    // fetch actual quiz password if not available
                    if (string.IsNullOrEmpty(quizPassword))
                    {
                        using (SqlCommand cmd3 = new SqlCommand("SELECT QuizPassword FROM QuizTable WHERE QuizID=@qId", conn))
                        {
                            cmd3.Parameters.AddWithValue("@qId", _quizID);
                            var res = cmd3.ExecuteScalar();
                            if (res != null) quizPassword = (res.ToString() ?? "").Trim();
                        }
                    }
                }

                if (!studentExists)
                {
                    var resp = new { success = false, message = "Student ID not found for this course/section for the logged-in teacher" };
                    await ctx.SendStringAsync(JsonConvert.SerializeObject(resp), "application/json", Encoding.UTF8);
                    return;
                }

                if (!string.Equals(quizPassword?.Trim(), password, StringComparison.Ordinal))
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
                    // give students 10 seconds less than teacher to allow for network/server delays
                    studentSeconds = (int)Math.Max(0, (durationEnd - DateTime.Now).TotalSeconds) - 10;
                    if (studentSeconds < 0) studentSeconds = 0;
                }
                else
                {
                    // if no start time set, allow full duration minus 10 seconds
                    studentSeconds = Math.Max(0, _durationMinutesParsed * 60 - 10);
                }

                if (studentSeconds <= 0)
                {
                    var resp = new { success = false, message = "Quiz duration has ended" };
                    await ctx.SendStringAsync(JsonConvert.SerializeObject(resp), "application/json", Encoding.UTF8);
                    return;
                }

                // parse features flags
                bool shuffleQuestions = false;
                bool shuffleOptions = false;
                bool showResult = false;
                try
                {
                    if (!string.IsNullOrWhiteSpace(_features))
                    {
                        var fobj = JObject.Parse(_features);
                        shuffleQuestions = fobj.Property("ShuffleQuestions", StringComparison.OrdinalIgnoreCase) != null && (bool?)fobj["ShuffleQuestions"] == true;
                        shuffleOptions = fobj.Property("ShuffleOptions", StringComparison.OrdinalIgnoreCase) != null && (bool?)fobj["ShuffleOptions"] == true;
                        showResult = fobj.Property("ShowCorrectAnswersAfterSubmission", StringComparison.OrdinalIgnoreCase) != null && (bool?)fobj["ShowCorrectAnswersAfterSubmission"] == true;
                    }
                }
                catch { /* ignore parse errors, default false */ }

                // prepare questions: support both 'Question' and 'QuestionText' keys saved by different code paths
                var questionsList = new List<QuizQuestion>();
                try
                {
                    var jarr = JArray.Parse(_questionsJson ?? "[]");
                    foreach (var j in jarr)
                    {
                        if (j is not JObject jo) continue;
                        string qtext = jo.Value<string>("Question") ?? jo.Value<string>("QuestionText") ?? jo.Value<string>("question") ?? string.Empty;
                        var optsToken = jo["Options"] ?? jo["options"];
                        var opts = new List<string>();
                        if (optsToken is JArray oa)
                        {
                            foreach (var o in oa)
                                opts.Add(o.ToString());
                        }
                        int corr = jo.Value<int?>("CorrectIndex") ?? jo.Value<int?>("correctIndex") ?? 0;
                        int marks = jo.Value<int?>("Marks") ?? jo.Value<int?>("marks") ?? 0;

                        var qq = new QuizQuestion
                        {
                            Question = qtext ?? string.Empty,
                            Options = opts,
                            CorrectIndex = corr,
                            Marks = marks
                        };
                        questionsList.Add(qq);
                    }
                }
                catch
                {
                    questionsList = new List<QuizQuestion>();
                }

                var rnd = new Random();

                // fetch AllowedQuestion from DB if available
                int allowedQuestion = 0;
                try
                {
                    using (var con = new SqlConnection(connStr))
                    {
                        con.Open();
                        using (var cmd = new SqlCommand("SELECT ISNULL(AllowedQuestion,0) FROM QuizTable WHERE QuizID=@qId", con))
                        {
                            cmd.Parameters.AddWithValue("@qId", _quizID);
                            allowedQuestion = Convert.ToInt32(cmd.ExecuteScalar() ?? 0);
                        }
                    }
                }
                catch { /* ignore */ }

                // per-student randomization according to features
                if (shuffleQuestions)
                {
                    // If AllowedQuestion represents allowed total marks (not count), attempt to select subset summing to that value.
                    if (allowedQuestion > 0)
                    {
                        int totalMarksAvailable = questionsList.Sum(q => q.Marks);
                        if (allowedQuestion > totalMarksAvailable)
                        {
                            // Notify teacher/admin on the UI thread
                            try
                            {
                                this?.BeginInvoke(new Action(() =>
                                {
                                    MessageBox.Show("Not enough total marks in this quiz. Please add more questions or increase marks.", "Quiz Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }));
                            }
                            catch { }

                            // Return a generic message to the student so internals are not exposed
                            var resp = new { success = false, message = "Quiz not available. Please contact your instructor." };
                            await ctx.SendStringAsync(JsonConvert.SerializeObject(resp), "application/json", Encoding.UTF8);
                            return;
                        }

                        // Try randomized greedy selection multiple times to find a subset whose marks sum to allowedQuestion
                        List<QuizQuestion>? chosen = null;
                        const int maxAttempts = 2000;
                        for (int attempt = 0; attempt < maxAttempts && chosen == null; attempt++)
                        {
                            var shuffled = questionsList.OrderBy(x => rnd.Next()).ToList();
                            var sel = new List<QuizQuestion>();
                            int sum = 0;
                            foreach (var q in shuffled)
                            {
                                if (sum + q.Marks <= allowedQuestion)
                                {
                                    sel.Add(q);
                                    sum += q.Marks;
                                    if (sum == allowedQuestion) break;
                                }
                            }

                            if (sum == allowedQuestion)
                                chosen = sel;
                        }

                        if (chosen == null)
                        {
                            try
                            {
                                this?.BeginInvoke(new Action(() =>
                                {
                                    MessageBox.Show("Cannot select questions matching allowed marks. Please adjust AllowedQuestion or add questions with appropriate marks.", "Quiz Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }));
                            }
                            catch { }

                            var resp = new { success = false, message = "Quiz not available. Please contact your instructor." };
                            await ctx.SendStringAsync(JsonConvert.SerializeObject(resp), "application/json", Encoding.UTF8);
                            return;
                        }

                        questionsList = chosen;
                    }
                    else
                    {
                        // simple shuffle of all questions
                        questionsList = questionsList.OrderBy(x => rnd.Next()).ToList();
                    }
                }

                // shuffle options per question only if enabled
                if (shuffleOptions)
                {
                    foreach (var q in questionsList)
                    {
                        if (q.Options == null || q.Options.Count <= 1) continue;
                        string correctText = q.Options.ElementAtOrDefault(q.CorrectIndex) ?? string.Empty;
                        q.Options = q.Options.OrderBy(x => rnd.Next()).ToList();
                        q.CorrectIndex = q.Options.IndexOf(correctText);
                    }
                }

                // When serializing for client, include Question and Marks etc.
                string shuffledJson = JsonConvert.SerializeObject(questionsList);

                var ok = new { success = true, shuffledQuestions = shuffledJson, studentSeconds = studentSeconds, studentName = studentName };
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

                // decide whether to show result based on features
                bool showResult = false;
                try
                {
                    if (!string.IsNullOrWhiteSpace(_features))
                    {
                        var fobj = JObject.Parse(_features);
                        showResult = fobj.Property("ShowCorrectAnswersAfterSubmission", StringComparison.OrdinalIgnoreCase) != null && (bool?)fobj["ShowCorrectAnswersAfterSubmission"] == true;
                    }
                }
                catch { }

                if (showResult)
                {
                    string resultHtml =
                        $"<html><body style='text-align:center;font-family:Arial'>" +
                        $"<h1>Result</h1>" +
                        $"<p>Total Marks: {totalMarks}</p>" +
                        $"<p>Your Score: {score}</p>" +
                        $"</body></html>";

                    await ctx.SendStringAsync(resultHtml, "text/html", Encoding.UTF8);
                }
                else
                {
                    await ctx.SendStringAsync("<html><body><h1>Your quiz is saved</h1></body></html>", "text/html", Encoding.UTF8);
                }
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
