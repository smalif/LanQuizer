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

            // Store logged-in user info
            teacherEmail = email;
            teacherID = id;

        }

        private void TeacherHome_Load(object sender, EventArgs e)
        {
            QuizPanel.HorizontalScroll.Enabled = false;
            QuizPanel.HorizontalScroll.Visible = false;
            QuizPanel.AutoScroll = true;
            LoadQuizzes();
            //Get IP on startup
            RefreshNetworkStatus();

            addSection.Visible = false;

            // Create dynamic panel for sections if not already added
            if (this.Controls["panelSections"] == null)
            {
                Panel panelSections = new Panel
                {
                    Name = "panelSections",
                    Top = 144 + 70, // Below top bar (height = 144)
                    Left = 10,
                    Width = this.ClientSize.Width - 20, // padding left/right
                    Height = this.ClientSize.Height - 20, // fill remaining form height
                    AutoScroll = true,
                    Visible = false
                };
                this.Controls.Add(panelSections);
            }
        }

        private void sectionBtn_Click(object sender, EventArgs e)
        {
            // Highlight the Section button
            sectionBtn.BackColor = Color.SeaGreen;
            sectionBtn.ForeColor = Color.White;
            sectionBtn.Font = new Font(sectionBtn.Font, FontStyle.Bold);

            // Reset Quiz button
            myQuizBtn.BackColor = Color.White;
            myQuizBtn.ForeColor = Color.Black;
            myQuizBtn.Font = new Font(myQuizBtn.Font, FontStyle.Regular);

            // Show the Sections panel
            Panel panelSections = this.Controls["panelSections"] as Panel;
            if (panelSections != null)
                panelSections.Visible = true;

            // Hide the Quiz panel
            QuizPanel.Visible = false;

            // Hide quiz-specific labels/groupboxes
            draftlbl.Visible = false;
            quizLbl.Visible = false;
            groupBox2.Visible = false;
            label8.Visible = false;
            groupBox1.Visible = false;
            addSection.Visible = true;

            // Optionally, reload sections dynamically
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
                    count = (int)cmd.ExecuteScalar();
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

                // ✅ Filter by logged-in teacher
                string query = "SELECT DISTINCT Section, Course " +
                               "FROM Students " +
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

                            // Modify button
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

                            // Delete button
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

                            // Add GroupBox to panel
                            panelSections.Controls.Add(gb);

                            // Move to next column
                            xPos += gbWidth + gapX;

                            // Wrap to next row if width exceeded
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
            LoadSections(); // refresh after adding
        }

        private void ModifySection(string sectionName, string courseName)
        {
            Add_Section addForm = new Add_Section
            {
                PreFillSection = sectionName,
                PreFillCourse = courseName
            };
            addForm.ShowDialog();
            LoadSections(); // refresh after modifying
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
                LoadSections(); // refresh after delete
            }
        }

        // Other buttons/events (logout, quizBtn, min/exit) remain the same
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
            // Highlight the Quiz button
            myQuizBtn.BackColor = Color.SeaGreen;
            myQuizBtn.ForeColor = Color.White;
            myQuizBtn.Font = new Font(myQuizBtn.Font, FontStyle.Bold);

            // Reset Section button
            sectionBtn.BackColor = Color.White;
            sectionBtn.ForeColor = Color.Black;
            sectionBtn.Font = new Font(sectionBtn.Font, FontStyle.Regular);

            // Show the Quiz panel
            QuizPanel.Visible = true;

            // Show relevant labels/groupboxes for quizzes
            draftlbl.Visible = true;
            quizLbl.Visible = true;
            groupBox2.Visible = true;
            label8.Visible = true;
            groupBox1.Visible = true;
            addSection.Visible = false;

            // Hide the Sections panel
            Panel panelSections = this.Controls["panelSections"] as Panel;
            if (panelSections != null)
                panelSections.Visible = false;

            // Optionally, reload quizzes dynamically
            LoadQuizzes();
        }

        private void createQuizBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            CreateQuiz quiz = new CreateQuiz();
            quiz.Show();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            // Optional, can leave empty
        }

        private void label8_Click(object sender, EventArgs e)
        {
            // Optional, can leave empty
        }

        private void addSection_Click(object sender, EventArgs e)
        {
            Add_Section addForm = new Add_Section();
            addForm.ShowDialog();
            LoadSections(); // refresh after adding
        }

        private void settingsBtn_Click(object sender, EventArgs e)
        {
            // Step 1: Get logged-in teacher info
            string loggedInEmail = this.teacherEmail; // or wherever you store email of logged-in user
            string loggedInID = this.teacherID;       // or wherever you store teacher ID

            // Open Settings form and pass the info
            Settings settingsForm = new Settings(loggedInEmail, loggedInID);
            settingsForm.ShowDialog(); // or Show()
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

        /*==============Dynamic Quiz====================    */

        public class Question
        {
            public string QuestionText { get; set; }    // map "Question"
            public List<string> Options { get; set; }   // map "Options"
            public int CorrectIndex { get; set; }       // map "CorrectIndex"
            public int Marks { get; set; }              // map "Marks"
        }
        private void LoadQuizzes()
        {
            QuizPanel.Controls.Clear();
            QuizPanel.AutoScroll = true; // enable scrolling

            int xStart = 10;
            int yStart = 10;
            int gbWidth = 327;
            int gbHeight = 252;
            int gapX = 10;
            int gapY = 10;          // gap between groupboxes
            int sectionGapY = 30;   // extra gap between status sections

            int xPos = xStart;
            int yPos = yStart;
            int rowMaxY = yStart; // track bottom of tallest groupbox in current row

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
                        string currentStatus = ""; // track current status group

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

                            // If status changes, insert a header with **extra vertical space**
                            if (status != currentStatus)
                            {
                                yPos = rowMaxY + sectionGapY; // add extra gap before new status header
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

                                yPos += statusLbl.Height + gapY; // move yPos below header
                                rowMaxY = yPos; // reset rowMaxY for new status group
                                currentStatus = status;
                            }

                            // Create the GroupBox
                            GroupBox gb = new GroupBox
                            {
                                Width = gbWidth,
                                Height = gbHeight,
                                Top = yPos,
                                Left = xPos,
                                Font = new Font("Times New Roman", 12, FontStyle.Bold),
                                BackColor = Color.White
                            };

                            // Add labels
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

                            // Status label top-right corner
                            Label lblStatus = new Label
                            {
                                Text = status,
                                Top = 0,
                                Left = gbWidth - 100, // adjust right alignment
                                Width = 100,
                                Height = 25,
                                Font = new Font("Times New Roman", 11, FontStyle.Bold),
                                TextAlign = ContentAlignment.MiddleCenter,
                                ForeColor = status == "Completed" ? Color.White : status == "Scheduled" ? Color.White : Color.White,
                                BackColor = status == "Completed" ? Color.Green : status == "Scheduled" ? Color.Orange : Color.Red
                            };

                            gb.Controls.Add(lblCourse);
                            gb.Controls.Add(lblExam);
                            gb.Controls.Add(lblSection);
                            gb.Controls.Add(lblDuration);
                            gb.Controls.Add(lblCreated);
                            gb.Controls.Add(lblTime);
                            gb.Controls.Add(lblStatus);

                            // Buttons
                            Button startBtn = new Button { Text = "Start Quiz", Width = 90, Height = 35, Top = gbHeight - 55, Left = 10, BackColor = Color.Green, ForeColor = Color.White, Cursor = Cursors.Hand, FlatStyle = FlatStyle.Flat };
                            startBtn.FlatAppearance.BorderSize = 0;
                            startBtn.Click += (s, e) => StartQuiz_Click(quizID);

                            Button editBtn = new Button { Text = "Edit", Width = 90, Height = 35, Top = gbHeight - 55, Left = 115, BackColor = Color.Orange, ForeColor = Color.White, Cursor = Cursors.Hand, FlatStyle = FlatStyle.Flat };
                            editBtn.FlatAppearance.BorderSize = 0;

                            Button deleteBtn = new Button { Text = "Delete", Width = 90, Height = 35, Top = gbHeight - 55, Left = 220, BackColor = Color.Red, ForeColor = Color.White, Cursor = Cursors.Hand, FlatStyle = FlatStyle.Flat };
                            deleteBtn.FlatAppearance.BorderSize = 0;
                            deleteBtn.Click += (s, e) => { DeleteQuiz(quizID); LoadQuizzes(); };

                            gb.Controls.Add(startBtn);
                            gb.Controls.Add(editBtn);
                            gb.Controls.Add(deleteBtn);

                            QuizPanel.Controls.Add(gb);

                            // Update rowMaxY to include this groupbox
                            int gbBottom = gb.Bottom;
                            if (gbBottom > rowMaxY)
                                rowMaxY = gbBottom;

                            // Move to next column
                            xPos += gbWidth + gapX;

                            // Wrap to next row
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







        /*================ Helper to Clone Quiz GroupBox ==================== */
        private GroupBox CloneQuizGroupBox()
        {
            GroupBox clone = new GroupBox
            {
                Width = quizLbl.Width,
                Height = quizLbl.Height,
                Font = quizLbl.Font,
                BackColor = quizLbl.BackColor
            };

            foreach (Control ctrl in quizLbl.Controls)
            {
                Control copy = (Control)Activator.CreateInstance(ctrl.GetType());
                copy.Name = ctrl.Name;
                copy.Text = ctrl.Text;
                copy.Font = ctrl.Font;
                copy.Size = ctrl.Size;
                copy.Location = ctrl.Location;
                copy.BackColor = ctrl.BackColor;
                copy.ForeColor = ctrl.ForeColor;
                copy.Visible = ctrl.Visible;

                clone.Controls.Add(copy);
            }

            return clone;
        }
        /*=================Helper to Start Quiz ===================*/
        private List<Question> PrepareRandomizedQuestions(string questionsJson, int quizMark, string featuresJson)
        {
            // Deserialize questions
            var questions = System.Text.Json.JsonSerializer.Deserialize<List<Question>>(questionsJson);
            if (questions == null || questions.Count == 0)
                throw new Exception("No questions available.");

            int totalQuestionMarks = questions.Sum(q => q.Marks);

            // Parse features
            var features = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, bool>>(featuresJson ?? "{}");
            bool randomQuestions = features != null && features.ContainsKey("RandomQuestions") && features["RandomQuestions"];

            if (randomQuestions)
            {
                if (quizMark > totalQuestionMarks)
                {
                    MessageBox.Show(
                        $"Total available question marks is {totalQuestionMarks}. Quiz mark cannot exceed this. Please update.",
                        "Invalid Quiz Mark", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return null;
                }
                else if (quizMark == totalQuestionMarks)
                {
                    MessageBox.Show(
                        "Quiz mark equals total question marks. Random selection may not work as expected. Only question sequence will be randomized.",
                        "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Shuffle questions sequence
                    return questions.OrderBy(q => Guid.NewGuid()).ToList();
                }
                else
                {
                    // Randomly select questions to match quizMark exactly
                    var randomizedList = new List<Question>();
                    int accumulatedMarks = 0;
                    var rnd = new Random();
                    var shuffledQuestions = questions.OrderBy(q => rnd.Next()).ToList();

                    foreach (var q in shuffledQuestions)
                    {
                        if (accumulatedMarks + q.Marks <= quizMark)
                        {
                            randomizedList.Add(q);
                            accumulatedMarks += q.Marks;
                        }

                        if (accumulatedMarks == quizMark)
                            break;
                    }

                    return randomizedList;
                }
            }

            // If randomization not enabled, return full questions
            return questions;
        }

        /*================ Quiz Button Events ==================== */
        private void StartQuiz_Click(int quizID)
        {
            string connStr = ConfigurationManager.ConnectionStrings["LanQuizerDB"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();

                string query = @"
        SELECT 
            ExamName, Course, Section, DurationMinutes, Questions, 
            StartTime, Status, Features, QuizMark
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

                        // ---- Read values ----
                        string examName = reader["ExamName"]?.ToString();
                        string course = reader["Course"]?.ToString();
                        string section = reader["Section"]?.ToString();
                        string durationStr = reader["DurationMinutes"]?.ToString();
                        string questionsJson = reader["Questions"]?.ToString();

                        string status = reader["Status"] == DBNull.Value ? "Draft" : reader["Status"].ToString();
                        string startTimeStr = reader["StartTime"] == DBNull.Value ? null : reader["StartTime"].ToString();
                        string featuresJson = reader["Features"] == DBNull.Value ? "{}" : reader["Features"].ToString();

                        int quizMark = reader["QuizMark"] == DBNull.Value ? 0 : Convert.ToInt32(reader["QuizMark"]);

                        // ---- Do nothing for Completed ----
                        if (status == "Completed")
                        {
                            MessageBox.Show("This quiz has already been completed.",
                                "Quiz Completed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }

                        // ---- مشترক verification: Draft + Scheduled ----
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

                        // ---- Scheduled time check ----
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

                        // ---- Pass data to StartQuiz (no randomization here) ----
                        int duration = int.TryParse(durationStr, out int d) ? d : 0;

                        StartQuiz startForm = new StartQuiz(
                            examName,
                            duration.ToString(),
                            quizMark.ToString(),
                            featuresJson
                        );

                        startForm.LoadQuestionsJson(questionsJson); // <-- you implement this
                        startForm.SetMeta(course, section);         // <-- optional helper
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

    }
}
