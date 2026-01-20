using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Configuration;

namespace LanQuizer
{
    public partial class Schedule : Form
    {
        // initialize fields to safe defaults to satisfy nullable analysis
        private string connStr = string.Empty;
        private SqlConnection connect = null!;
        private string LoggedInTeacherEmail = string.Empty;
        private string quizId = string.Empty; // Quiz ID passed from CreateQuiz
        private string examName = string.Empty;
        private int duration;
        private int allowedMarks;
        private string password = string.Empty;
        private string questionsJson = string.Empty;
        private string featuresJson = string.Empty;
        private string teacherEmail = string.Empty;

        public Schedule(string quizID) // Pass QuizID from previous form
        {
            InitializeComponent();
            LoggedInTeacherEmail = LoggedInUser.Email ?? string.Empty;
            this.quizId = quizID ?? string.Empty;

            connStr = ConfigurationManager.ConnectionStrings["LanQuizerDB"]?.ConnectionString ?? string.Empty;
            connect = new SqlConnection(connStr);
        }

        public Schedule(string examName, int duration, int allowedMarks, string password,
                string questionsJson, string featuresJson, string teacherEmail)
        {
            InitializeComponent();

            // Store values in private fields
            this.examName = examName ?? string.Empty;
            this.duration = duration;
            this.allowedMarks = allowedMarks;
            this.password = password ?? string.Empty;
            this.questionsJson = questionsJson ?? string.Empty;
            this.featuresJson = featuresJson ?? string.Empty;
            this.teacherEmail = teacherEmail ?? string.Empty;

            // ensure connection string and logged-in teacher value are set for this ctor too
            connStr = ConfigurationManager.ConnectionStrings["LanQuizerDB"]?.ConnectionString ?? string.Empty;
            connect = new SqlConnection(connStr);
            LoggedInTeacherEmail = LoggedInUser.Email ?? string.Empty;

            LoadCourses();
        }

        private void Schedule_Load(object sender, EventArgs e)
        {
            // Default panel visibility
            StartNowGrp.Visible = true;
            StartNowGrp.Location = new Point(12, 126);
            StartNowGrp.BringToFront();

            ScheduleQuiz.Visible = false;

            startNowChk.Checked = true;
            scheduleCheck.Checked = false;

            LoadStartCourses();
            LoadCourses();
        }

        #region Radio Buttons Change
        private void startNowChk_CheckedChanged(object sender, EventArgs e)
        {
            if (startNowChk.Checked)
            {
                StartNowGrp.Visible = true;
                StartNowGrp.BringToFront();
                ScheduleQuiz.Visible = false;
            }
        }

        private void scheduleCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (scheduleCheck.Checked)
            {
                ScheduleQuiz.Visible = true;
                ScheduleQuiz.BringToFront();
                StartNowGrp.Visible = false;
            }
        }
        #endregion

        #region Load Courses & Sections

        private void LoadCourses()
        {
            scheduleCourse.Items.Clear();

            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();

                string query = @"
                    SELECT DISTINCT Course
                    FROM Students
                    WHERE TeacherEmail = @teacherEmail
                      AND Course IS NOT NULL
                      AND LTRIM(RTRIM(Course)) <> ''";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@teacherEmail", LoggedInTeacherEmail.Trim());
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        bool hasData = false;
                        while (dr.Read())
                        {
                            hasData = true;
                            // guard against DBNull and null
                            scheduleCourse.Items.Add(dr["Course"]?.ToString() ?? string.Empty);
                        }

                        if (!hasData)
                        {
                            MessageBox.Show("No courses found. Please add student data first.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            scheduleBtn.Enabled = false;
                        }
                    }
                }
            }
        }

        private void LoadSections(string course, ComboBox targetCombo)
        {
            targetCombo.Items.Clear();

            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();

                string query = @"
                    SELECT DISTINCT Section
                    FROM Students
                    WHERE Course = @course
                      AND Section IS NOT NULL
                      AND LTRIM(RTRIM(Section)) <> ''";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@course", course ?? string.Empty);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        bool hasData = false;
                        while (dr.Read())
                        {
                            hasData = true;
                            targetCombo.Items.Add(dr["Section"]?.ToString() ?? string.Empty);
                        }

                        if (!hasData)
                        {
                            MessageBox.Show("No sections found for this course.", "No Sections", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
        }

        private void scheduleCourse_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (scheduleCourse.SelectedIndex != -1)
                LoadSections(scheduleCourse.SelectedItem?.ToString() ?? string.Empty, ScheduleSec);

            ValidateScheduleInputs();
        }

        private void ScheduleSec_SelectedIndexChanged(object sender, EventArgs e)
        {
            ValidateScheduleInputs();
        }

        private void ValidateScheduleInputs()
        {
            // DateTimePicker.Value is never null; remove null checks
            scheduleBtn.Enabled =
                scheduleCourse.SelectedIndex != -1 &&
                ScheduleSec.SelectedIndex != -1;
        }

        #endregion

        #region Schedule Quiz Button
        private void scheduleBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (scheduleCourse.SelectedIndex == -1 || ScheduleSec.SelectedIndex == -1)
                {
                    MessageBox.Show("Please select both course and section.", "Cannot Schedule", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DateTime scheduledDateTime = scheduleDate.Value.Date + scheduleTime.Value.TimeOfDay;
                if (scheduledDateTime <= DateTime.Now.AddMinutes(10))
                {
                    MessageBox.Show("Scheduled time must be at least 10 minutes from now.", "Invalid Time", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // ---------- SAVE QUIZ AS SCHEDULED ----------
                string localConnStr = System.Configuration.ConfigurationManager.ConnectionStrings["LanQuizerDB"]?.ConnectionString ?? string.Empty;
                using (SqlConnection con = new SqlConnection(localConnStr))
                {
                    con.Open();

                    // Check if quiz already exists
                    string checkQuery = @"SELECT QuizID FROM QuizTable WHERE TeacherEmail=@teacherEmail AND ExamName=@examName";
                    using (SqlCommand cmd = new SqlCommand(checkQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@teacherEmail", teacherEmail ?? string.Empty);
                        cmd.Parameters.AddWithValue("@examName", examName ?? string.Empty);

                        object result = cmd.ExecuteScalar();
                        if (result != null) quizId = result.ToString() ?? string.Empty;
                    }

                    if (string.IsNullOrEmpty(quizId))
                    {
                        // Insert new
                        string insertQuery = @"
INSERT INTO QuizTable
(TeacherID, TeacherEmail, ExamName, DurationMinutes, AllowedQuestion,
 Features, QuizPassword, Questions, Status, CreatedAt,
 Course, Section, StartTime)
VALUES
((SELECT TeacherID FROM Teachers WHERE TeacherEmail=@teacherEmail),
 @teacherEmail, @examName, @duration, @allowedMarks,
 @features, @password, @questions, @status, @createdAt,
 @course, @section, @scheduledDateTime)";

                        using (SqlCommand cmd = new SqlCommand(insertQuery, con))
                        {
                            cmd.Parameters.AddWithValue("@teacherEmail", teacherEmail ?? string.Empty);
                            cmd.Parameters.AddWithValue("@examName", examName ?? string.Empty);
                            cmd.Parameters.AddWithValue("@duration", duration);
                            cmd.Parameters.AddWithValue("@allowedMarks", allowedMarks);
                            cmd.Parameters.AddWithValue("@features", featuresJson ?? string.Empty);
                            cmd.Parameters.AddWithValue("@password", password ?? string.Empty);
                            cmd.Parameters.AddWithValue("@questions", questionsJson ?? string.Empty);
                            cmd.Parameters.AddWithValue("@status", "Scheduled");
                            cmd.Parameters.AddWithValue("@createdAt", DateTime.Now);
                            cmd.Parameters.AddWithValue("@course", scheduleCourse.SelectedItem?.ToString() ?? string.Empty);
                            cmd.Parameters.AddWithValue("@section", ScheduleSec.SelectedItem?.ToString() ?? string.Empty);
                            cmd.Parameters.AddWithValue("@scheduledDateTime", scheduledDateTime);

                            cmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        // Update existing
                        string updateQuery = @"
UPDATE QuizTable SET
DurationMinutes=@duration,
AllowedQuestion=@allowedMarks,
Features=@features,
QuizPassword=@password,
Questions=@questions,
Status=@status,
Course=@course,
Section=@section,
StartTime=@scheduledDateTime
WHERE QuizID=@quizId";

                        using (SqlCommand cmd = new SqlCommand(updateQuery, con))
                        {
                            cmd.Parameters.AddWithValue("@quizId", quizId);
                            cmd.Parameters.AddWithValue("@duration", duration);
                            cmd.Parameters.AddWithValue("@allowedMarks", allowedMarks);
                            cmd.Parameters.AddWithValue("@features", featuresJson ?? string.Empty);
                            cmd.Parameters.AddWithValue("@password", password ?? string.Empty);
                            cmd.Parameters.AddWithValue("@questions", questionsJson ?? string.Empty);
                            cmd.Parameters.AddWithValue("@status", "Scheduled");
                            cmd.Parameters.AddWithValue("@course", scheduleCourse.SelectedItem?.ToString() ?? string.Empty);
                            cmd.Parameters.AddWithValue("@section", ScheduleSec.SelectedItem?.ToString() ?? string.Empty);
                            cmd.Parameters.AddWithValue("@scheduledDateTime", scheduledDateTime);

                            cmd.ExecuteNonQuery();
                        }
                    }
                }

                MessageBox.Show("Quiz scheduled successfully!", "Scheduled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error scheduling quiz: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Instant Start Panel
        private void LoadStartCourses(string? previousCourse = null)
        {
            StartCourse.Items.Clear();

            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();

                string query = @"
                    SELECT DISTINCT Course
                    FROM Students
                    WHERE TeacherEmail = @teacherEmail
                      AND Course IS NOT NULL
                      AND LTRIM(RTRIM(Course)) <> ''";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@teacherEmail", LoggedInTeacherEmail.Trim());
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        bool hasData = false;
                        while (dr.Read())
                        {
                            hasData = true;
                            StartCourse.Items.Add(dr["Course"]?.ToString() ?? string.Empty);
                        }

                        if (hasData && !string.IsNullOrEmpty(previousCourse))
                        {
                            int index = StartCourse.Items.IndexOf(previousCourse);
                            if (index != -1) StartCourse.SelectedIndex = index;
                        }
                    }
                }
            }
        }

        private void LoadStartSections(string course, string? previousSection = null)
        {
            StartSection.Items.Clear();

            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();

                string query = @"
                    SELECT DISTINCT Section
                    FROM Students
                    WHERE TeacherEmail = @teacherEmail
                      AND Course = @course
                      AND Section IS NOT NULL
                      AND LTRIM(RTRIM(Section)) <> ''";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@teacherEmail", LoggedInTeacherEmail.Trim());
                    cmd.Parameters.AddWithValue("@course", course.Trim());

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        bool hasData = false;
                        while (dr.Read())
                        {
                            hasData = true;
                            StartSection.Items.Add(dr["Section"]?.ToString() ?? string.Empty);
                        }

                        if (hasData && !string.IsNullOrEmpty(previousSection))
                        {
                            int index = StartSection.Items.IndexOf(previousSection);
                            if (index != -1) StartSection.SelectedIndex = index;
                        }
                    }
                }
            }
        }

        private void StartCourse_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (StartCourse.SelectedIndex != -1)
                LoadStartSections(StartCourse.SelectedItem?.ToString() ?? string.Empty);

            ValidateStartInputs();
        }

        private void StartSection_SelectedIndexChanged(object sender, EventArgs e)
        {
            ValidateStartInputs();
        }

        private void ValidateStartInputs()
        {
            startNowBtn.Enabled =
                StartCourse.SelectedIndex != -1 &&
                StartSection.SelectedIndex != -1;
        }

        private void startNowBtn_Click(object sender, EventArgs e)
        {
            string courseName = StartCourse.SelectedItem?.ToString() ?? string.Empty;
            string sectionName = StartSection.SelectedItem?.ToString() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(courseName) || string.IsNullOrWhiteSpace(sectionName))
            {
                MessageBox.Show("Please select Course and Section.", "Missing Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            QuizInfo? quiz = GetQuizInfo(quizId);
            if (quiz == null)
            {
                MessageBox.Show("Quiz data not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            StartQuiz startForm = new StartQuiz(
                quiz.QuizID ?? string.Empty,
                quiz.ExamName ?? string.Empty,
                quiz.QuestionsJson ?? string.Empty,
                quiz.Password ?? string.Empty,
                quiz.FeaturesJson ?? string.Empty,
                quiz.DurationMinutes,
                courseName,
                sectionName
            );
            startForm.Show();
            this.Close();
        }
        #endregion

        private QuizInfo? GetQuizInfo(string quizID)
        {
            QuizInfo? quiz = null;

            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();

                string query = "SELECT * FROM QuizTable WHERE QuizID=@quizId";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@quizId", quizID);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            quiz = new QuizInfo
                            {
                                QuizID = dr["QuizID"]?.ToString() ?? string.Empty,
                                ExamName = dr["ExamName"]?.ToString() ?? string.Empty,
                                DurationMinutes = Convert.ToInt32(dr["DurationMinutes"]),
                                AllowedMarks = Convert.ToInt32(dr["AllowedQuestion"]),
                                QuestionsJson = dr["Questions"]?.ToString() ?? string.Empty,
                                FeaturesJson = dr["Features"]?.ToString() ?? string.Empty,
                                Password = dr["QuizPassword"]?.ToString() ?? string.Empty
                            };
                        }
                    }
                }
            }

            return quiz;
        }

        private void startNowBtn_Click_1(object sender, EventArgs e)
        {
            try
            {
                // Validate course & section
                if (StartCourse.SelectedIndex == -1)
                {
                    MessageBox.Show("Please select a course to start the quiz.", "Missing Course", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (StartSection.SelectedIndex == -1)
                {
                    MessageBox.Show("Please select a section to start the quiz.", "Missing Section", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Selected values
                string selectedCourse = StartCourse.SelectedItem?.ToString() ?? string.Empty;
                string selectedSection = StartSection.SelectedItem?.ToString() ?? string.Empty;

                // Retrieve quiz info from database using quizId
                string localConnStr = ConfigurationManager.ConnectionStrings["LanQuizerDB"]?.ConnectionString ?? string.Empty;
                using (SqlConnection con = new SqlConnection(localConnStr))
                {
                    con.Open();
                    string query = @"SELECT ExamName, DurationMinutes, AllowedQuestion, Features, QuizPassword
                             FROM QuizTable
                             WHERE QuizID=@quizId";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@quizId", this.quizId); // quizId stored when Schedule was opened

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                string examName = dr["ExamName"]?.ToString() ?? string.Empty;
                                string duration = dr["DurationMinutes"]?.ToString() ?? "0";
                                string allowedMarks = dr["AllowedQuestion"]?.ToString() ?? "0";
                                string features = dr["Features"]?.ToString() ?? string.Empty;
                                string password = dr["QuizPassword"]?.ToString() ?? string.Empty;

                                // Open StartQuiz passing section & password
                                StartQuiz startQuizForm = new StartQuiz(
                                    examName,
                                    duration,
                                    allowedMarks,   // marks (optional, can be calculated if needed)
                                    features,
                                    allowedMarks,
                                    int.Parse(this.quizId),
                                    selectedSection,
                                    password
                                );
                                startQuizForm.Show();
                                // Optionally hide Schedule form
                                // this.Hide();
                            }
                            else
                            {
                                MessageBox.Show("Quiz data not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error starting quiz: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



    }

    public class QuizInfo
    {
        public string? QuizID { get; set; }
        public string? ExamName { get; set; }
        public int DurationMinutes { get; set; }
        public int AllowedMarks { get; set; }
        public string? QuestionsJson { get; set; }
        public string? FeaturesJson { get; set; }
        public string? Password { get; set; }
    }

 }
