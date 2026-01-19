using EmbedIO;
using Microsoft.Data.SqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Web;
using System.Windows.Forms;

namespace LanQuizer
{
    public partial class StartQuiz : Form
    {
        /*==================Variables========================*/
        private string _examName;
        private string _duration;
        private string _marks;
        private string _features;

        // ====== Data passed from TeacherHome ======
        private string _course;
        private string _section;
        public void LoadQuestionsJson(string questionsJson)
        {
            _questionsJson = questionsJson;
        }

        public void SetMeta(string course, string section)
        {
            _course = course;
            _section = section;
        }


        private string _questionsJson; // NEW: store questions JSON
        private DateTime? _startTime;  // NEW: store quiz scheduled start time (nullable)


        private bool serverRunning = false;
        private WebServer webServer;
        private int port;
        private Label linkLbl;

        /*==================Database connection================*/
        string connStr = ConfigurationManager.ConnectionStrings["LanQuizerDB"].ConnectionString;

        public StartQuiz()
        {
            InitializeComponent();
            this.Load += StartQuiz_Load;
            this.FormClosing += StartQuiz_FormClosing;
        }

        public StartQuiz(string examName, string duration, string marks, string features)
        {
            InitializeComponent();
            _examName = examName;
            _duration = duration;
            _marks = marks;
            _features = features;
            this.Load += StartQuiz_Load;
            this.FormClosing += StartQuiz_FormClosing;

        }

        /*==================For Test: Search Method========================*/
        public bool LoadQuiz(string examName)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    string query = @"
                        SELECT TOP 1 ExamName, DurationMinutes, TotalMarks, Features
                        FROM QuizTable
                        WHERE ExamName = @examName";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@examName", examName);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                _examName = reader["ExamName"].ToString();
                                _duration = reader["DurationMinutes"].ToString();
                                _marks = reader["TotalMarks"].ToString();
                                _features = reader["Features"].ToString();
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
        private void StartQuiz_Load(object sender, EventArgs e)
        {
            // Display exam info
            examLbl.Text = "Exam Name: " + _examName;
            timeLbl.Text = "Duration: " + _duration + " Minutes";
            tmLbl.Text = "Total Marks: " + _marks;
            featureOn.Text = string.IsNullOrEmpty(_features) ? "No feature selected" : _features;

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

            port = GetOrCreatePort();
            if (port == -1)
            {
                portLbl.Text = "No available port";
                portLbl.ForeColor = Color.Red;
                return;
            }

            portLbl.Text = "Port: " + port;
            portLbl.ForeColor = Color.Green;

            // Open firewall for this port
            OpenFirewallPort(port);

            // Start server
            StartWebServer(port);

            // Make sure linkLbl exists
            if (linkLbl == null)
            {
                linkLbl = new Label
                {
                    Location = new Point(20, portLbl.Bottom + 10),
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

            linkLbl.Text = $"Visit this link: http://{ip}:{port}/";
            linkLbl.BringToFront();
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

        /*=================Web Server Methods========================*/
        private void StartWebServer(int port)
        {
            StopWebServer();

            string url = $"http://+:{port}/";

            webServer = new WebServer(url)
                .WithAction("/", HttpVerbs.Get, async ctx =>
                {
                    // Fetch questions from database
                    DataTable dtQuestions = new DataTable();
                    try
                    {
                        using (SqlConnection conn = new SqlConnection(connStr))
                        {
                            conn.Open();
                            string query = "SELECT QuestionID, QuestionText, Option1, Option2, Option3, Option4, CorrectOption FROM QuizTable WHERE ExamName=@examName";
                            using (SqlCommand cmd = new SqlCommand(query, conn))
                            {
                                cmd.Parameters.AddWithValue("@examName", _examName);
                                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                                adapter.Fill(dtQuestions);
                            }
                        }
                    }
                    catch
                    {
                        dtQuestions.Columns.Add("QuestionText");
                        dtQuestions.Columns.Add("Option1");
                        dtQuestions.Columns.Add("Option2");
                    }

                    // Build HTML form
                    StringBuilder html = new StringBuilder();
                    html.Append("<html><head><title>LAN Quiz</title></head><body style='font-family:Arial; text-align:center;'>");
                    html.Append($"<h1>Exam: {_examName}</h1>");
                    html.Append($"<p>Duration: {_duration} minutes | Total Marks: {_marks}</p>");
                    html.Append("<form method='POST' action='/submit'>");

                    int qNo = 1;
                    foreach (DataRow row in dtQuestions.Rows)
                    {
                        html.Append($"<div style='margin-bottom:20px; text-align:left; padding-left:30px;'>");
                        html.Append($"<p><strong>Q{qNo}: {row["QuestionText"]}</strong></p>");
                        for (int i = 1; i <= 4; i++)
                        {
                            string optionCol = "Option" + i;
                            if (row[optionCol] == DBNull.Value || string.IsNullOrWhiteSpace(row[optionCol].ToString()))
                                continue;
                            html.Append($"<input type='radio' name='q{row["QuestionID"]}' value='{i}' required> {row[optionCol]}<br>");
                        }
                        html.Append("</div>");
                        qNo++;
                    }

                    // Student info fields
                    html.Append("<div style='margin-bottom:20px; text-align:left; padding-left:30px;'>");
                    html.Append("Student ID: <input type='text' name='studentId' required><br>");
                    html.Append("Section: <input type='text' name='section' required><br>");
                    html.Append("</div>");

                    html.Append("<button type='submit' style='padding:10px 20px; font-size:16px;'>Submit</button>");
                    html.Append("</form></body></html>");

                    await ctx.SendStringAsync(html.ToString(), "text/html", Encoding.UTF8);
                })
                .WithAction("/submit", HttpVerbs.Post, async ctx =>
                {
                    string body;
                    using (var reader = new StreamReader(ctx.Request.InputStream, ctx.Request.ContentEncoding))
                    {
                        body = await reader.ReadToEndAsync();
                    }

                    var form = HttpUtility.ParseQueryString(body);
                    string studentId = form["studentId"];
                    string section = form["section"];
                    DateTime submitTime = DateTime.Now;

                    int totalQuestions = 0;
                    int correctAnswers = 0;

                    using (SqlConnection conn = new SqlConnection(connStr))
                    {
                        conn.Open();

                        // Get QuizSetID
                        int _quizSetID = 0;
                        string quizIdQuery = "SELECT QuizSetID FROM QuizSet WHERE ExamName=@examName";
                        using (SqlCommand cmd = new SqlCommand(quizIdQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@examName", _examName);
                            object val = cmd.ExecuteScalar();
                            _quizSetID = val != null ? Convert.ToInt32(val) : 0;
                        }

                        foreach (string key in form.Keys)
                        {
                            if (!key.StartsWith("q")) continue;

                            int questionId = int.Parse(key.Substring(1));
                            int selectedOption = int.Parse(form[key]);

                            int correctOption = 0;
                            string correctQuery = "SELECT CorrectOption FROM QuizTable WHERE QuestionID=@qid";
                            using (SqlCommand cmd = new SqlCommand(correctQuery, conn))
                            {
                                cmd.Parameters.AddWithValue("@qid", questionId);
                                object val2 = cmd.ExecuteScalar();
                                correctOption = val2 != null ? Convert.ToInt32(val2) : 0;
                            }

                            bool isCorrect = (selectedOption == correctOption);
                            if (isCorrect) correctAnswers++;
                            totalQuestions++;

                            string insertAttempt = @"
                                INSERT INTO StudentAttempts
                                (QuizSetID, StudentID, QuestionID, SelectedOptionID, IsCorrect, LoginTime, SubmitTime, Section)
                                VALUES
                                (@quizId, @studentId, @questionId, @selectedOption, @isCorrect, @loginTime, @submitTime, @section)";
                            using (SqlCommand cmd = new SqlCommand(insertAttempt, conn))
                            {
                                cmd.Parameters.AddWithValue("@quizId", _quizSetID);
                                cmd.Parameters.AddWithValue("@studentId", studentId);
                                cmd.Parameters.AddWithValue("@questionId", questionId);
                                cmd.Parameters.AddWithValue("@selectedOption", selectedOption);
                                cmd.Parameters.AddWithValue("@isCorrect", isCorrect);
                                cmd.Parameters.AddWithValue("@loginTime", DateTime.Now);
                                cmd.Parameters.AddWithValue("@submitTime", submitTime);
                                cmd.Parameters.AddWithValue("@section", section);
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }

                    string resultHtml = $"<html><body style='text-align:center; font-family:Arial;'>" +
                                        $"<h1>Result</h1>" +
                                        $"<p>Total Questions: {totalQuestions}</p>" +
                                        $"<p>Correct Answers: {correctAnswers}</p>" +
                                        $"<p>Score: {correctAnswers}/{totalQuestions}</p>" +
                                        $"</body></html>";

                    await ctx.SendStringAsync(resultHtml, "text/html", Encoding.UTF8);
                });

            webServer.RunAsync();
        }

        private void StopWebServer()
        {
            if (webServer != null)
            {
                webServer.Dispose();
                webServer = null;
            }
        }

        private void OpenFirewallPort(int port)
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo("netsh")
                {
                    Arguments = $"advfirewall firewall add rule name=\"LanQuiz_{port}\" dir=in action=allow protocol=TCP localport={port}",
                    Verb = "runas",
                    WindowStyle = ProcessWindowStyle.Hidden,
                    CreateNoWindow = true,
                    UseShellExecute = true
                };
                Process.Start(psi);
            }
            catch { }
        }

        private void refreshIcon_Click(object sender, EventArgs e)
        {
            UpdateConnectionStatus();
            RefreshNetworkInfo();
        }

        private void connected_Click(object sender, EventArgs e)
        {
            UpdateConnectionStatus();
        }

        private void exitBtn_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Are you sure you want to exit? This will close the quiz.",
                "Confirm Exit",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
                this.Close();
        }

        private void StartQuiz_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopWebServer();
        }

        private void examLbl_Click(object sender, EventArgs e) { }

        private void startBtn_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;

            string examName = searchExam.Text.Trim();
            if (string.IsNullOrEmpty(examName))
            {
                MessageBox.Show("Please enter an exam name.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            bool examExists = false;
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    string query = "SELECT COUNT(1) FROM QuizTable WHERE ExamName = @examName";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@examName", examName);
                        examExists = (int)cmd.ExecuteScalar() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error checking exam: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!examExists)
            {
                MessageBox.Show("Exam not found in database.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!serverRunning)
            {
                port = GetOrCreatePort();
                if (port == -1)
                {
                    MessageBox.Show("No available port to start the server.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                StartWebServer(port);


                if (linkLbl == null)
                {
                    linkLbl = new Label
                    {
                        Location = new Point(20, btn.Bottom + 10),
                        AutoSize = true,
                        ForeColor = Color.Blue,
                        Cursor = Cursors.Hand
                    };
                    linkLbl.Click += (s, e2) =>
                    {
                        string url = linkLbl.Text.Replace("Visit this link: ", "");
                        try
                        {
                            Process.Start(new ProcessStartInfo
                            {
                                FileName = url,
                                UseShellExecute = true
                            });
                        }
                        catch { }
                    };
                    this.Controls.Add(linkLbl);
                }

                string localIp = GetLocalIPAddress();
                linkLbl.Text = $"Visit this link: http://{localIp}:{port}/";
                linkLbl.BringToFront();

                btn.Text = "Stop";
                serverRunning = true;
            }
            else
            {
                StopWebServer();
                btn.Text = "Start";
                serverRunning = false;
                if (linkLbl != null) linkLbl.Text = "";
            }
        }

        /*========================Store IP=========================*/
        private int GetOrCreatePort()
        {
            int savedPort = 0;

            int.TryParse(ConfigurationManager.AppSettings["LanQuizPort"], out savedPort);

            if (savedPort > 0 && IsPortAvailable(savedPort))
                return savedPort;

            // Otherwise pick a new free port
            int newPort = GetDynamicPort();
            if (newPort == -1) return -1;

            SavePortToConfig(newPort);
            OpenFirewallPort(newPort);

            return newPort;
        }


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



        private void startLbl_Click(object sender, EventArgs e) { }

        private void StartQuiz_Load_1(object sender, EventArgs e)
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
                                _examName = reader["ExamName"].ToString();
                                _duration = reader["DurationMinutes"].ToString();
                                _questionsJson = reader["Questions"].ToString(); // store JSON string in a new variable
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

    }
}
