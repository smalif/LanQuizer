using Microsoft.Data.SqlClient;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text.Json.Serialization;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;

namespace LanQuizer
{
    public partial class TeacherHome : Form
    {
        private string teacherEmail;
        private string teacherID;
        private string teacherName;

        public TeacherHome()
        {
            InitializeComponent();
        }

        public TeacherHome(string teacherName)
        {
            InitializeComponent();
            connectImg.Visible = false;
            WelcomeTeacher.Text = "Welcome Back, " + LoggedInUser.Name;
        }

        public TeacherHome(string email, string id)
        {
            InitializeComponent();
            teacherEmail = email;
            teacherID = id;
        }

        private void TeacherHome_Load(object sender, EventArgs e)
        {
            QuizPanel.HorizontalScroll.Enabled = false;
            QuizPanel.HorizontalScroll.Visible = false;
            QuizPanel.AutoScroll = true;
            LoadQuizzes();
            RefreshNetworkStatus();
            addSection.Visible = false;

            if (this.Controls["panelSections"] == null)
            {
                Panel panelSections = new Panel
                {
                    Name = "panelSections",
                    Top = 144 + 70,
                    Left = 10,
                    Width = this.ClientSize.Width - 20,
                    Height = this.ClientSize.Height - 20,
                    AutoScroll = true,
                    Visible = false
                };
                this.Controls.Add(panelSections);
            }
        }

        private void sectionBtn_Click(object sender, EventArgs e)
        {
            sectionBtn.BackColor = Color.SeaGreen;
            sectionBtn.ForeColor = Color.White;
            sectionBtn.Font = new Font(sectionBtn.Font, FontStyle.Bold);

            myQuizBtn.BackColor = Color.White;
            myQuizBtn.ForeColor = Color.Black;
            myQuizBtn.Font = new Font(myQuizBtn.Font, FontStyle.Regular);

            Panel panelSections = this.Controls["panelSections"] as Panel;
            if (panelSections != null)
                panelSections.Visible = true;

            QuizPanel.Visible = false;
            draftlbl.Visible = false;
            quizLbl.Visible = false;
            groupBox2.Visible = false;
            label8.Visible = false;
            groupBox1.Visible = false;
            addSection.Visible = true;

            LoadSections();
        }

        private int GetStudentCount(string section, string course)
        {
            int count = 0;
            string connStr = ConfigurationManager.ConnectionStrings["LanQuizerDB"].ConnectionString;

            using (SqlConnection connect = new SqlConnection(connStr))
            {
                connect.Open();
                string query = "SELECT COUNT(*) FROM Students WHERE Section=@section AND Course=@course";
                using (SqlCommand cmd = new SqlCommand(query, connect))
                {
                    cmd.Parameters.AddWithValue("@section", section);
                    cmd.Parameters.AddWithValue("@course", course);
                    count = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }

            return count;
        }

        private void LoadSections()
        {
            Panel panelSections = this.Controls["panelSections"] as Panel;
            panelSections.Controls.Clear();

            int xStart = 10;
            int yStart = 10;
            int gbWidth = 300;
            int gbHeight = 140;
            int gapX = 10;
            int gapY = 10;

            int xPos = xStart;
            int yPos = yStart;

            string connStr = ConfigurationManager.ConnectionStrings["LanQuizerDB"].ConnectionString;

            using (SqlConnection connect = new SqlConnection(connStr))
            {
                connect.Open();

                string query = "SELECT DISTINCT Section, Course FROM Students " +
                               "WHERE TeacherEmail = @teacherEmail AND TeacherID = @teacherID " +
                               "ORDER BY Section, Course";

                using (SqlCommand cmd = new SqlCommand(query, connect))
                {
                    cmd.Parameters.AddWithValue("@teacherEmail", LoggedInUser.Email);
                    cmd.Parameters.AddWithValue("@teacherID", LoggedInUser.ID);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        bool hasSections = false;

                        while (reader.Read())
                        {
                            hasSections = true;

                            string sectionName = reader["Section"].ToString();
                            string courseName = reader["Course"].ToString();

                            GroupBox gb = new GroupBox
                            {
                                Text = $"Section: {sectionName}",
                                Width = gbWidth,
                                Height = gbHeight,
                                Top = yPos,
                                Left = xPos,
                                Font = new Font("Times New Roman", 14, FontStyle.Bold)
                            };

                            Label courseLbl = new Label
                            {
                                Text = courseName,
                                Font = new Font("Times New Roman", 11, FontStyle.Regular),
                                ForeColor = Color.DimGray,
                                Top = 35,
                                Left = 10,
                                AutoSize = true
                            };
                            gb.Controls.Add(courseLbl);

                            Label lblCount = new Label
                            {
                                Text = GetStudentCount(sectionName, courseName) + " students",
                                Top = courseLbl.Bottom + 8,
                                Left = 10,
                                AutoSize = true,
                                Font = new Font("Times New Roman", 11, FontStyle.Regular)
                            };
                            gb.Controls.Add(lblCount);

                            Button modifyBtn = new Button
                            {
                                Text = "Modify",
                                Width = 80,
                                Height = 40,
                                Top = lblCount.Bottom + 10,
                                Left = 50,
                                Font = new Font("Times New Roman", 11, FontStyle.Bold),
                                BackColor = Color.DarkOliveGreen,
                                ForeColor = Color.White,
                                Cursor = Cursors.Hand,
                                FlatStyle = FlatStyle.Flat
                            };
                            modifyBtn.FlatAppearance.BorderSize = 0;
                            modifyBtn.Click += (s, e) =>
                            {
                                Add_Section addForm = new Add_Section
                                {
                                    PreFillSection = sectionName,
                                    PreFillCourse = courseName
                                };
                                addForm.ShowDialog();
                                LoadSections();
                            };
                            gb.Controls.Add(modifyBtn);

                            Button deleteBtn = new Button
                            {
                                Text = "Delete",
                                Width = 80,
                                Height = 40,
                                Top = lblCount.Bottom + 10,
                                Left = 170,
                                Font = new Font("Times New Roman", 11, FontStyle.Bold),
                                BackColor = Color.FromArgb(244, 67, 54),
                                ForeColor = Color.White,
                                Cursor = Cursors.Hand,
                                FlatStyle = FlatStyle.Flat
                            };
                            deleteBtn.FlatAppearance.BorderSize = 0;
                            deleteBtn.Click += (s, e) =>
                            {
                                DeleteSection(sectionName, courseName);
                                LoadSections();
                            };
                            gb.Controls.Add(deleteBtn);

                            panelSections.Controls.Add(gb);

                            xPos += gbWidth + gapX;

                            if (xPos + gbWidth > panelSections.ClientSize.Width)
                            {
                                xPos = xStart;
                                yPos += gbHeight + gapY;
                            }
                        }

                        if (!hasSections)
                        {
                            Label noSectionLbl = new Label
                            {
                                Text = "NO SECTIONS ADDED YET",
                                Top = yStart + 30,
                                Left = xStart + 400,
                                AutoSize = true,
                                ForeColor = Color.Red,
                                Font = new Font("Times New Roman", 18, FontStyle.Bold)
                            };
                            panelSections.Controls.Add(noSectionLbl);
                        }
                    }
                }
            }
        }

        private void AddNewSection()
        {
            Add_Section addForm = new Add_Section();
            addForm.ShowDialog();
            LoadSections();
        }

        private void ModifySection(string sectionName, string courseName)
        {
            Add_Section addForm = new Add_Section
            {
                PreFillSection = sectionName,
                PreFillCourse = courseName
            };
            addForm.ShowDialog();
            LoadSections();
        }

        private void DeleteSection(string sectionName, string courseName)
        {
            DialogResult dr = MessageBox.Show(
                $"Are you sure you want to delete section '{sectionName}' for course '{courseName}'?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (dr == DialogResult.Yes)
            {
                string connStr = ConfigurationManager.ConnectionStrings["LanQuizerDB"].ConnectionString;
                using (SqlConnection connect = new SqlConnection(connStr))
                {
                    connect.Open();
                    string deleteQuery = "DELETE FROM Students WHERE Section=@section AND Course=@course";
                    using (SqlCommand cmd = new SqlCommand(deleteQuery, connect))
                    {
                        cmd.Parameters.AddWithValue("@section", sectionName);
                        cmd.Parameters.AddWithValue("@course", courseName);
                        cmd.ExecuteNonQuery();
                    }
                }
                LoadSections();
            }
        }

        private void logoutBtn_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Are you sure you want to Log Out",
                "Confirm Log Out",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result == DialogResult.Yes)
            {
                this.Close();
                Teacher_Reg_Form form = new Teacher_Reg_Form();
                form.Show();
            }
        }

        private void minbtn_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void exitBtn_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Are you sure you want to exit? It needs to Login again.",
                "Confirm Exit",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result == DialogResult.Yes)
            {
                System.Windows.Forms.Application.Exit();
            }
        }

        private void myQuizBtn_Click(object sender, EventArgs e)
        {
            myQuizBtn.BackColor = Color.SeaGreen;
            myQuizBtn.ForeColor = Color.White;
            myQuizBtn.Font = new Font(myQuizBtn.Font, FontStyle.Bold);

            sectionBtn.BackColor = Color.White;
            sectionBtn.ForeColor = Color.Black;
            sectionBtn.Font = new Font(sectionBtn.Font, FontStyle.Regular);

            QuizPanel.Visible = true;
            draftlbl.Visible = true;
            quizLbl.Visible = true;
            groupBox2.Visible = true;
            label8.Visible = true;
            groupBox1.Visible = true;
            addSection.Visible = false;

            Panel panelSections = this.Controls["panelSections"] as Panel;
            if (panelSections != null)
                panelSections.Visible = false;

            LoadQuizzes();
        }

        private void createQuizBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            CreateQuiz quiz = new CreateQuiz();
            quiz.Show();
        }

        private void addSection_Click(object sender, EventArgs e)
        {
            Add_Section addForm = new Add_Section();
            addForm.ShowDialog();
            LoadSections();
        }

        private void settingsBtn_Click(object sender, EventArgs e)
        {
            string loggedInEmail = this.teacherEmail;
            string loggedInID = this.teacherID;
            Settings settingsForm = new Settings(loggedInEmail, loggedInID);
            settingsForm.ShowDialog();
        }

        private void connected_Click(object sender, EventArgs e)
        {
            RefreshNetworkStatus();
        }

        private void RefreshNetworkStatus()
        {
            if (IsNetworkConnected())
            {
                string ip = GetLocalIPAddress();
                connected.Text = "CONNECTED\nIP Address: " + ip;
                connected.ForeColor = Color.Green;
                connectImg.Visible = true;
                disconnected.Visible = false;
            }
            else
            {
                connected.Text = "NOT CONNECTED";
                connected.ForeColor = Color.Red;
                connectImg.Visible = false;
                disconnected.Visible = true;
            }
        }

        private bool IsNetworkConnected()
        {
            return NetworkInterface.GetIsNetworkAvailable();
        }

        private string GetLocalIPAddress()
        {
            foreach (var ip in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return "IP Not Found";
        }

        /*================ Load Quizzes ==================== */
        private void LoadQuizzes()
        {
            QuizPanel.Controls.Clear();
            QuizPanel.AutoScroll = true;

            int xStart = 10;
            int yStart = 10;
            int gbWidth = 327;
            int gbHeight = 252;
            int gapX = 10;
            int gapY = 10;
            int sectionGapY = 30;

            int xPos = xStart;
            int yPos = yStart;
            int rowMaxY = yStart;

            string connStr = ConfigurationManager.ConnectionStrings["LanQuizerDB"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();

                string query = @"
            SELECT QuizID, ExamName, Course, Section, DurationMinutes, CreatedAt, StartTime, ISNULL(Status, 'Draft') AS Status, EndTime
            FROM QuizTable
            WHERE TeacherEmail = @teacherEmail
            ORDER BY 
                CASE 
                    WHEN ISNULL(Status, 'Draft') = 'Scheduled' THEN 1
                    WHEN ISNULL(Status, 'Draft') = 'Draft' THEN 2
                    WHEN ISNULL(Status, 'Draft') = 'Completed' THEN 3
                    ELSE 4
                END,
                CASE
                    WHEN ISNULL(Status, 'Draft') = 'Scheduled' THEN StartTime
                    WHEN ISNULL(Status, 'Draft') = 'Draft' THEN CreatedAt
                    WHEN ISNULL(Status, 'Draft') = 'Completed' THEN EndTime
                    ELSE CreatedAt
                END DESC";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@teacherEmail", LoggedInUser.Email);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        bool hasQuizzes = false;
                        string currentStatus = "";

                        while (reader.Read())
                        {
                            hasQuizzes = true;

                            int quizID = Convert.ToInt32(reader["QuizID"]);
                            string examName = reader["ExamName"].ToString();
                            string courseName = reader["Course"] == DBNull.Value ? "N/A" : reader["Course"].ToString();
                            string sectionName = reader["Section"] == DBNull.Value ? "N/A" : reader["Section"].ToString();
                            string status = reader["Status"].ToString();
                            string duration = reader["DurationMinutes"] == DBNull.Value ? "N/A" : reader["DurationMinutes"].ToString();
                            string createdAt = Convert.ToDateTime(reader["CreatedAt"]).ToString("yyyy-MM-dd HH:mm");
                            string startTime = reader["StartTime"] == DBNull.Value ? "N/A" : Convert.ToDateTime(reader["StartTime"]).ToString("yyyy-MM-dd HH:mm");
                            string endTime = reader["EndTime"] == DBNull.Value ? "N/A" : Convert.ToDateTime(reader["EndTime"]).ToString("yyyy-MM-dd HH:mm");

                            if (status != currentStatus)
                            {
                                yPos = rowMaxY + sectionGapY;
                                xPos = xStart;

                                Label statusLbl = new Label
                                {
                                    Text = status.ToUpper() + " QUIZZES",
                                    Top = yPos,
                                    Left = xStart,
                                    AutoSize = true,
                                    Font = new Font("Times New Roman", 14, FontStyle.Bold),
                                    ForeColor = Color.DarkBlue,
                                };
                                QuizPanel.Controls.Add(statusLbl);

                                yPos += statusLbl.Height + gapY;
                                rowMaxY = yPos;
                                currentStatus = status;
                            }

                            GroupBox gb = new GroupBox
                            {
                                Width = gbWidth,
                                Height = gbHeight,
                                Top = yPos,
                                Left = xPos,
                                Font = new Font("Times New Roman", 12, FontStyle.Bold),
                                BackColor = Color.White
                            };

                            Label lblCourse = new Label { Text = "Course: " + courseName, Top = 20, Left = 15, AutoSize = true, Font = new Font("Times New Roman", 11, FontStyle.Regular) };
                            Label lblExam = new Label { Text = "Exam: " + examName, Top = lblCourse.Bottom + 5, Left = 15, AutoSize = true, Font = new Font("Times New Roman", 11, FontStyle.Regular) };
                            Label lblSection = new Label { Text = "Section: " + sectionName, Top = lblExam.Bottom + 5, Left = 15, AutoSize = true, Font = new Font("Times New Roman", 11, FontStyle.Regular) };
                            Label lblDuration = new Label { Text = "Duration: " + duration + " mins", Top = lblSection.Bottom + 5, Left = 15, AutoSize = true, Font = new Font("Times New Roman", 11, FontStyle.Regular) };
                            Label lblCreated = new Label { Text = "Created: " + createdAt, Top = lblDuration.Bottom + 5, Left = 15, AutoSize = true, Font = new Font("Times New Roman", 11, FontStyle.Regular) };

                            Label lblTime = new Label
                            {
                                Text = status == "Scheduled" ? "StartTime: " + startTime :
                                       status == "Completed" ? "Held: " + endTime :
                                       "StartTime: " + startTime,
                                Top = lblCreated.Bottom + 5,
                                Left = 15,
                                AutoSize = true,
                                Font = new Font("Times New Roman", 11, FontStyle.Regular)
                            };

                            Label lblStatus = new Label
                            {
                                Text = status,
                                Top = 0,
                                Left = gbWidth - 100,
                                Width = 100,
                                Height = 25,
                                Font = new Font("Times New Roman", 11, FontStyle.Bold),
                                TextAlign = ContentAlignment.MiddleCenter,
                                ForeColor = Color.White,
                                BackColor = status == "Completed" ? Color.Green : status == "Scheduled" ? Color.Orange : Color.Red
                            };

                            gb.Controls.Add(lblCourse);
                            gb.Controls.Add(lblExam);
                            gb.Controls.Add(lblSection);
                            gb.Controls.Add(lblDuration);
                            gb.Controls.Add(lblCreated);
                            gb.Controls.Add(lblTime);
                            gb.Controls.Add(lblStatus);

                            Button startBtn = new Button { Text = "Start Quiz", Width = 90, Height = 35, Top = gbHeight - 55, Left = 10, BackColor = Color.Green, ForeColor = Color.White, Cursor = Cursors.Hand, FlatStyle = FlatStyle.Flat };
                            startBtn.FlatAppearance.BorderSize = 0;
                            // single handler that branches based on button text
                            startBtn.Click += (s, e) =>
                            {
                                Button b = s as Button;
                                if (b != null && b.Text == "Start Quiz")
                                {
                                    StartQuiz_Click(quizID);
                                }
                                else if (b != null && b.Text == "Start Again")
                                {
                                    // Completed quiz: confirm before opening editor
                                    var dr = MessageBox.Show(
                                        "This quiz has already been completed. Do you want to start it again (open editor)?",
                                        "Start Again",
                                        MessageBoxButtons.YesNo,
                                        MessageBoxIcon.Question);

                                    if (dr == DialogResult.Yes)
                                    {
                                        OpenEditorForQuiz(quizID, connStr);
                                    }
                                }
                                else
                                {
                                    // Acts like edit when labeled differently
                                    OpenEditorForQuiz(quizID, connStr);
                                }
                            };

                            Button editBtn = new Button { Text = "Edit", Width = 90, Height = 35, Top = gbHeight - 55, Left = 115, BackColor = Color.Orange, ForeColor = Color.White, Cursor = Cursors.Hand, FlatStyle = FlatStyle.Flat };
                            editBtn.FlatAppearance.BorderSize = 0;
                            // single handler that branches based on button text
                            editBtn.Click += (s, e) =>
                            {
                                Button b = s as Button;
                                if (b != null && b.Text == "View Result")
                                {
                                    ShowResultsPanel(quizID, examName, courseName, sectionName);
                                }
                                else
                                {
                                    OpenEditorForQuiz(quizID, connStr);
                                }
                            };

                            Button deleteBtn = new Button { Text = "Delete", Width = 90, Height = 35, Top = gbHeight - 55, Left = 220, BackColor = Color.Red, ForeColor = Color.White, Cursor = Cursors.Hand, FlatStyle = FlatStyle.Flat };
                            deleteBtn.FlatAppearance.BorderSize = 0;
                            deleteBtn.Click += (s, e) => { DeleteQuiz(quizID); LoadQuizzes(); };

                            // If quiz completed, change buttons: Start -> Start Again (acts like edit), Edit -> View Result
                            if (status == "Completed")
                            {
                                startBtn.Text = "Start Again";
                                startBtn.BackColor = Color.DarkBlue;

                                editBtn.Text = "View Result";
                                editBtn.BackColor = Color.Teal;
                            }

                            gb.Controls.Add(startBtn);
                            gb.Controls.Add(editBtn);
                            gb.Controls.Add(deleteBtn);

                            QuizPanel.Controls.Add(gb);

                            int gbBottom = gb.Bottom;
                            if (gbBottom > rowMaxY) rowMaxY = gbBottom;

                            xPos += gbWidth + gapX;
                            if (xPos + gbWidth > QuizPanel.ClientSize.Width)
                            {
                                xPos = xStart;
                                yPos = rowMaxY + gapY;
                            }
                        }

                        if (!hasQuizzes)
                        {
                            Label noQuizLbl = new Label
                            {
                                Text = "NO QUIZZES ADDED YET",
                                Top = yStart + 30,
                                Left = xStart + 350,
                                AutoSize = true,
                                ForeColor = Color.Red,
                                Font = new Font("Times New Roman", 18, FontStyle.Bold)
                            };
                            QuizPanel.Controls.Add(noQuizLbl);
                        }
                    }
                }
            }
        }

        /*================ Start Quiz ==================== */
        private void StartQuiz_Click(int quizID)
        {
            string connStr = ConfigurationManager.ConnectionStrings["LanQuizerDB"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();

                string query = @"
        SELECT 
            ExamName, Course, Section, DurationMinutes, Questions, 
            StartTime, Status, Features, QuizMark, AllowedQuestion
        FROM QuizTable
        WHERE QuizID = @quizID";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@quizID", quizID);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.Read())
                        {
                            MessageBox.Show("Quiz not found.", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        string examName = reader["ExamName"]?.ToString();
                        string course = reader["Course"]?.ToString();
                        string section = reader["Section"]?.ToString();
                        string allowedQuestion = reader["AllowedQuestion"] == DBNull.Value ? "N/A" : reader["AllowedQuestion"].ToString();
                        string durationStr = reader["DurationMinutes"]?.ToString();
                        string questionsJson = reader["Questions"]?.ToString();
                        string status = reader["Status"] == DBNull.Value ? "Draft" : reader["Status"].ToString();
                        string startTimeStr = reader["StartTime"] == DBNull.Value ? null : reader["StartTime"].ToString();
                        string featuresJson = reader["Features"] == DBNull.Value ? "{}" : reader["Features"].ToString();
                        int quizMark = reader["QuizMark"] == DBNull.Value ? 0 : Convert.ToInt32(reader["QuizMark"]);

                        if (status == "Completed")
                        {
                            MessageBox.Show("This quiz has already been completed.",
                                "Quiz Completed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }

                        bool hasMissing =
                            string.IsNullOrWhiteSpace(examName) ||
                            string.IsNullOrWhiteSpace(course) ||
                            string.IsNullOrWhiteSpace(section) ||
                            string.IsNullOrWhiteSpace(durationStr) ||
                            string.IsNullOrWhiteSpace(questionsJson) ||
                            quizMark <= 0;

                        if (hasMissing)
                        {
                            MessageBox.Show(
                                "This quiz is incomplete.\n\n" +
                                "Please make sure the following are filled:\n" +
                                "- Exam name\n- Course\n- Section\n- Duration\n" +
                                "- Questions\n- Exam mark\n- Features",
                                "Incomplete Quiz",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                            return;
                        }

                        if (status == "Scheduled" && DateTime.TryParse(startTimeStr, out DateTime scheduledTime))
                        {
                            if (DateTime.Now < scheduledTime)
                            {
                                DialogResult dr = MessageBox.Show(
                                    $"This quiz is scheduled for:\n\n{scheduledTime:yyyy-MM-dd HH:mm}\n\n" +
                                    "Do you want to start it now?",
                                    "Scheduled Quiz",
                                    MessageBoxButtons.YesNo,
                                    MessageBoxIcon.Question);

                                if (dr != DialogResult.Yes)
                                    return;
                            }
                        }

                        int duration = int.TryParse(durationStr, out int d) ? d : 0;

                        StartQuiz startForm = new StartQuiz(
                            examName,
                            duration.ToString(),
                            quizMark.ToString(),
                            featuresJson,
                            allowedQuestion,
                            quizID,
                            course,      // <-- Added course argument
                            section      // <-- Added section argument
                        );

                        startForm.LoadQuestionsJson(questionsJson);
                        startForm.SetMeta(course, section);
                        // hide teacher home while StartQuiz is open and restore after it closes
                        startForm.FormClosed += (s, e) =>
                        {
                            try { this.Show(); } catch { }
                            try { LoadQuizzes(); } catch { }
                        };
                        try { this.Hide(); } catch { }
                        startForm.Show();
                    }
                }
            }
        }
    



/*================ Edit Quiz Button Event ==================== */

        private void EditQuiz_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            int quizId = (int)btn.Tag;

            MessageBox.Show("Edit quiz later. QuizID: " + quizId);
        }

        private void DeleteQuiz_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            int quizId = (int)btn.Tag;

            if (MessageBox.Show("Delete this quiz?", "Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                DeleteQuiz(quizId);
                LoadQuizzes();
            }
        }

        /*================ Delete Quiz from DB ==================== */
        private void DeleteQuiz(int quizID)
        {
            string connStr = ConfigurationManager.ConnectionStrings["LanQuizerDB"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();
                string query = "DELETE FROM QuizTable WHERE QuizID=@quizID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@quizID", quizID);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void Schedule_Load(object sender, EventArgs e)
        {
            LoadQuizzes();
        }

        private void OpenEditorForQuiz(int quizID, string connStr)
        {
            // open CreateQuiz and load data for editing
            CreateQuiz editor = new CreateQuiz();

            // When editor closes, show this form again and refresh quizzes
            editor.FormClosed += (s, e) =>
            {
                try { this.Show(); } catch { }
                try { LoadQuizzes(); } catch { }
            };

            // hide the teacher home while editing
            try { this.Hide(); } catch { }
            editor.Show();

            // load details into editor
            int qid = quizID;
            try
            {
                using (SqlConnection c2 = new SqlConnection(connStr))
                {
                    c2.Open();
                    using (SqlCommand cmd2 = new SqlCommand("SELECT ExamName, DurationMinutes, AllowedQuestion, QuizPassword, Features, Questions, QuizMark FROM QuizTable WHERE QuizID=@qid", c2))
                    {
                        cmd2.Parameters.AddWithValue("@qid", qid);
                        using (var r2 = cmd2.ExecuteReader())
                        {
                            if (r2.Read())
                            {
                                string en = r2["ExamName"]?.ToString() ?? string.Empty;
                                int dur = r2["DurationMinutes"] == DBNull.Value ? 0 : Convert.ToInt32(r2["DurationMinutes"]);
                                int allowed = r2["AllowedQuestion"] == DBNull.Value ? 0 : Convert.ToInt32(r2["AllowedQuestion"]);
                                string pwd = r2["QuizPassword"]?.ToString() ?? string.Empty;
                                string fjson = r2["Features"]?.ToString() ?? "{}";
                                string qjson = r2["Questions"]?.ToString() ?? "[]";
                                editor.LoadQuizForEdit(qid, en, dur, allowed, pwd, fjson, qjson);
                            }
                        }
                    }
                }
            }
            catch { }
        }

        private void ShowResultsPanel(int quizID, string examName, string courseName, string sectionName)
        {
            // Check database for attempts for this quiz
            string connStr = ConfigurationManager.ConnectionStrings["LanQuizerDB"].ConnectionString;
            DataTable attempts = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(connStr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT AttemptID, StudentID, Section, Course, Score, LoginTime, SubmitTime FROM StudentAttempts WHERE QuizID=@quizId", con))
                    {
                        cmd.Parameters.AddWithValue("@quizId", quizID);
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(attempts);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load attempts: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (attempts.Rows.Count == 0)
            {
                MessageBox.Show("No attempts found for this quiz.", "Results", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Ensure metadata columns are present and populated so they appear in the grid and saved file
            if (!attempts.Columns.Contains("ExamName"))
                attempts.Columns.Add("ExamName", typeof(string));
            if (!attempts.Columns.Contains("Course") && !attempts.Columns.Contains("course"))
            {
                // if original column missing, add with capitalized name
                attempts.Columns.Add("Course", typeof(string));
            }
            if (!attempts.Columns.Contains("Section") && !attempts.Columns.Contains("section"))
            {
                attempts.Columns.Add("Section", typeof(string));
            }

            foreach (DataRow r in attempts.Rows)
            {
                try { r["ExamName"] = examName ?? string.Empty; } catch { }
                try { if (string.IsNullOrWhiteSpace(r["Course"]?.ToString())) r["Course"] = courseName ?? string.Empty; } catch { }
                try { if (string.IsNullOrWhiteSpace(r["Section"]?.ToString())) r["Section"] = sectionName ?? string.Empty; } catch { }
            }

            // Show results in a modal form with DataGridView and Save button
            Form resultsForm = new Form();
            resultsForm.Text = $"Results - {examName}";
            resultsForm.StartPosition = FormStartPosition.CenterParent;
            resultsForm.Size = new Size(900, 600);

            // Save button
            Button saveBtn = new Button();
            saveBtn.Text = "Save Excel";
            saveBtn.Width = 110;
            saveBtn.Height = 32;
            saveBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            saveBtn.Cursor = Cursors.Hand;

            // Info label
            Label infoLbl = new Label();
            infoLbl.AutoSize = false;
            infoLbl.TextAlign = ContentAlignment.MiddleLeft;
            infoLbl.Dock = DockStyle.Left;
            infoLbl.Width = 600;
            infoLbl.Text = $"Exam: {examName}    Course: {courseName}    Section: {sectionName}";
            infoLbl.Font = new Font(infoLbl.Font.FontFamily, 10, FontStyle.Bold);

            saveBtn.Click += (s, e) =>
            {
                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.Filter = "CSV files (*.csv)|*.csv|Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
                    sfd.FileName = $"{SanitizeFileName(examName)}_{SanitizeFileName(courseName)}_{SanitizeFileName(sectionName)}_results.csv";
                    if (sfd.ShowDialog(resultsForm) == DialogResult.OK)
                    {
                        try
                        {
                            // Save as CSV (Excel can open CSV). If user selects .xlsx we still write CSV content.
                            SaveDataTableAsCsv(attempts, sfd.FileName);
                            MessageBox.Show("Results saved successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Failed to save file: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            };

            // Layout: use TableLayoutPanel so top info panel sizes dynamically and grid fills remaining space
            TableLayoutPanel layout = new TableLayoutPanel();
            layout.Dock = DockStyle.Fill;
            layout.ColumnCount = 1;
            layout.RowCount = 2;
            layout.RowStyles.Clear();
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // top panel auto-sized
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F)); // grid takes remaining

            // Top panel: contains info label and save button
            Panel topPanel = new Panel();
            topPanel.Height = 56;
            topPanel.Dock = DockStyle.Fill;
            topPanel.Padding = new Padding(8);

            // place controls using docking inside the panel
            infoLbl.Dock = DockStyle.Fill;
            saveBtn.Dock = DockStyle.Right;
            saveBtn.Margin = new Padding(8, 8, 8, 8);

            topPanel.Controls.Add(infoLbl);
            topPanel.Controls.Add(saveBtn);

            DataGridView dgv = new DataGridView();
            dgv.Dock = DockStyle.Fill;
            dgv.ReadOnly = true;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.DataSource = attempts;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = false;
            dgv.AutoGenerateColumns = true;

            // add controls to layout
            layout.Controls.Add(topPanel, 0, 0);
            layout.Controls.Add(dgv, 0, 1);

            resultsForm.Controls.Add(layout);

            // make sure form doesn't hide rows and allows resizing
            resultsForm.AutoScroll = false;
            resultsForm.MinimumSize = new Size(600, 400);

            resultsForm.ShowDialog(this);
        }

        private static string SanitizeFileName(string name)
        {
            foreach (var c in Path.GetInvalidFileNameChars())
                name = name.Replace(c, '_');
            return string.IsNullOrWhiteSpace(name) ? "results" : name;
        }

        private void SaveDataTableAsCsv(DataTable table, string path)
        {
            using (var sw = new StreamWriter(path, false, Encoding.UTF8))
            {
                // header
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    if (i > 0) sw.Write(',');
                    sw.Write('"');
                    sw.Write(table.Columns[i].ColumnName.Replace("\"", "\"\""));
                    sw.Write('"');
                }
                sw.WriteLine();

                // rows
                foreach (DataRow row in table.Rows)
                {
                    for (int i = 0; i < table.Columns.Count; i++)
                    {
                        if (i > 0) sw.Write(',');
                        var val = row[i]?.ToString() ?? string.Empty;
                        // escape quotes
                        val = val.Replace("\"", "\"\"");
                        sw.Write('"');
                        sw.Write(val);
                        sw.Write('"');
                    }
                    sw.WriteLine();
                }
            }
        }
    }
}
