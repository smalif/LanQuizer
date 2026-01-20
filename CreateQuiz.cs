using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace LanQuizer
{
    public partial class CreateQuiz : Form
    {
        private string loggedInTeacherEmail = LoggedInUser.Email;
        private List<Panel> questionContainers = new List<Panel>();
        private Button addNewBtn;
        private Button saveBtn;
        private bool questionsInitialized = false;

        public CreateQuiz()
        {
            InitializeComponent();

            // Set up initial state
            SetupForm();
            // Auto-realign questions when form resizes
            this.Resize += (s, e) =>
            {
                if (questionsInitialized)
                    ReorderQuestions();
            };
        }

        private void SetupForm()
        {
            // Ensure panels are properly set up
            questionBox.Visible = true;
            questionPanel.Visible = false;
            questionPanel.AutoScrollMargin = new Size(0, 20);
            questionPanel.HorizontalScroll.Enabled = false;   // disable horizontal scrolling
            questionPanel.HorizontalScroll.Visible = false;   // hide scrollbar
            questionPanel.AutoScroll = true;                  // keep vertical scrolling
            questionPanel.AutoScrollMargin = new Size(0, 20);
            questionPanel.AutoScrollMinSize = new Size(0, 0);
            // Style buttons initially
            myQuizBtn.BackColor = Color.SeaGreen;
            myQuizBtn.ForeColor = Color.White;
            myQuizBtn.Font = new Font(myQuizBtn.Font, FontStyle.Bold);

            questionBtn.BackColor = Color.White;
            questionBtn.ForeColor = Color.Black;
            questionBtn.Font = new Font(questionBtn.Font, FontStyle.Regular);
        }

        private void CreateQuiz_Load(object sender, EventArgs e)
        {
            // Show settings panel by default
            questionBox.Visible = true;
            questionPanel.Visible = false;
            questionBox.BringToFront();
        }

        // ================= SWITCH PANELS =================
        private void questionBtn_Click_1(object sender, EventArgs e)
        {
            // FORCE VISIBILITY
            questionBox.Hide();

            questionPanel.Parent = this;              // 🔥 FORCE correct parent
            questionPanel.Location = new Point(12, 206);
            questionPanel.Size = new Size(1238, 567);

            questionPanel.Visible = true;
            questionPanel.Enabled = true;

            questionPanel.BringToFront();
            questionPanel.Show();
            questionPanel.Refresh();
            questionPanel.Invalidate();
            this.Refresh();

            // Button styling
            questionBtn.BackColor = Color.SeaGreen;
            questionBtn.ForeColor = Color.White;
            questionBtn.Font = new Font(questionBtn.Font, FontStyle.Bold);

            myQuizBtn.BackColor = Color.White;
            myQuizBtn.ForeColor = Color.Black;
            myQuizBtn.Font = new Font(myQuizBtn.Font, FontStyle.Regular);

            // INIT QUESTIONS UI ONCE
            if (!questionsInitialized)
            {
                InitializeQuestionPanel();
                AddQuestionContainer();
            }
        }



        private void myQuizBtn_Click(object sender, EventArgs e)
        {
            // Switch to settings panel
            questionPanel.Visible = false;
            questionBox.Visible = true;
            questionBox.BringToFront();

            // Update button styles
            myQuizBtn.BackColor = Color.SeaGreen;
            myQuizBtn.ForeColor = Color.White;
            myQuizBtn.Font = new Font(myQuizBtn.Font, FontStyle.Bold);

            questionBtn.BackColor = Color.White;
            questionBtn.ForeColor = Color.Black;
            questionBtn.Font = new Font(questionBtn.Font, FontStyle.Regular);
        }

        //==========Start QUESTION PANEL METHODS==========

        // ================= INITIALIZE QUESTIONS PANEL =================
        private void InitializeQuestionPanel()
        {
            questionsInitialized = true;

            questionPanel.SuspendLayout();
            questionPanel.Controls.Clear();

            questionPanel.AutoScroll = true;
            questionPanel.BackColor = Color.White;

            // ===== SAVE BUTTON =====
            saveBtn = new Button
            {
                Text = "💾 Save Questions",
                Size = new Size(160, 35),
                Location = new Point(questionPanel.Width - 180, 15),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                BackColor = Color.SeaGreen,
                ForeColor = Color.White
            };

            saveBtn.Click += (s, e) =>
            {
                // MessageBox.Show($"{questionContainers.Count} Questions saved to Database.");
                SaveQuizToDatabase(isDraft: false);
            };

            // ===== ADD QUESTION BUTTON =====
            addNewBtn = new Button
            {
                Text = "➕ Add New Question",
                Size = new Size(220, 40),
                Location = new Point((questionPanel.Width - 220) / 2, 65),
                BackColor = Color.LightSkyBlue,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Anchor = AnchorStyles.Top
            };


            addNewBtn.Click += (s, e) => AddQuestionContainer();

            questionPanel.Controls.Add(saveBtn);
            questionPanel.Controls.Add(addNewBtn);

            questionPanel.ResumeLayout();
        }


        // ================= ADD QUESTION =================
        private void AddQuestionContainer()
        {
            int width = questionPanel.ClientSize.Width - 40;

            int top = 0; // temporary, real position set by ReorderQuestions



            Panel container = new Panel
            {
                Width = width,
                Height = 280,
                Location = new Point(20, top),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.WhiteSmoke
            };

            // ===== TITLE =====
            Label title = new Label
            {
                Text = $"Question {questionContainers.Count + 1}",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(10, 10)
            };

            // ===== QUESTION TEXT =====
            TextBox questionText = new TextBox
            {
                Multiline = true,
                Size = new Size(width - 40, 50),
                Location = new Point(10, 35),
                PlaceholderText = "Enter your question here..."
            };

            // ===== OPTIONS =====
            List<TextBox> optionBoxes = new List<TextBox>();
            int optTop = questionText.Bottom + 10;

            for (int i = 0; i < 4; i++)
            {
                Label optLabel = new Label
                {
                    Text = $"Option {i + 1}:",
                    Location = new Point(10, optTop + 4),
                    Width = 70
                };

                TextBox optBox = new TextBox
                {
                    Size = new Size(width - 120, 25),
                    Location = new Point(90, optTop)
                };

                optionBoxes.Add(optBox);
                container.Controls.Add(optLabel);
                container.Controls.Add(optBox);

                optTop += 30;
            }

            // ===== CORRECT ANSWER =====
            Label correctLbl = new Label
            {
                Text = "Correct Answer:",
                Location = new Point(10, optTop + 4)
            };

            ComboBox correctAnswer = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Size = new Size(220, 25),
                Location = new Point(130, optTop)
            };

            // Update correct answer list dynamically
            foreach (var opt in optionBoxes)
            {
                opt.TextChanged += (s, e) =>
                {
                    correctAnswer.Items.Clear();
                    foreach (var o in optionBoxes.Where(x => !string.IsNullOrWhiteSpace(x.Text)))
                        correctAnswer.Items.Add(o.Text);
                };
            }
            // ===== New Marks label and textbox =====
            Label marksLbl = new Label
            {
                Text = "Marks:",
                Location = new Point(correctAnswer.Right + 15, optTop + 4), // 15px space after combobox
                AutoSize = true
            };

            TextBox marksTextBox = new TextBox
            {
                Size = new Size(80, 25),
                Location = new Point(marksLbl.Right + 5, optTop) // small gap after label
            };

            // ===== DELETE BUTTON =====
            Button deleteBtn = new Button
            {
                Text = "Delete",
                Size = new Size(80, 28),
                Location = new Point(width - 170, optTop + 10),
                BackColor = Color.IndianRed,
                ForeColor = Color.White
            };

            deleteBtn.Click += (s, e) => DeleteQuestion(container);

            // ===== ADD CONTROLS =====
            container.Controls.Add(title);
            container.Controls.Add(questionText);
            container.Controls.Add(correctLbl);
            container.Controls.Add(correctAnswer);
            container.Controls.Add(deleteBtn);
            container.Controls.Add(marksLbl);
            container.Controls.Add(marksTextBox);

            questionPanel.Controls.Add(container);
            questionContainers.Add(container);

            ReorderQuestions();
            questionPanel.ScrollControlIntoView(container);
        }

        // ================= DELETE QUESTION =================
        private void DeleteQuestion(Panel container)
        {
            if (MessageBox.Show("Delete this question?", "Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            questionPanel.Controls.Remove(container);
            questionContainers.Remove(container);
            container.Dispose();

            ReorderQuestions();
        }

        private void ReorderQuestions()
        {
            int top = addNewBtn.Bottom + 15;
            int fixedWidth = questionPanel.ClientSize.Width - 40;

            for (int i = 0; i < questionContainers.Count; i++)
            {
                Panel p = questionContainers[i];

                p.Location = new Point(20, top);
                p.Width = fixedWidth;

                var lbl = p.Controls
                           .OfType<Label>()
                           .First(l => l.Text.StartsWith("Question"));

                lbl.Text = $"Question {i + 1}";

                top = p.Bottom + 15;
            }
        }


        //========================Save Process Methods========================
        // ================= GET TEACHER ID =================
        private string GetCurrentTeacherId()
        {
            string teacherId = null;

            string connStr = System.Configuration.ConfigurationManager
                .ConnectionStrings["LanQuizerDB"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();
                string q = "SELECT TeacherID FROM Teachers WHERE TeacherEmail=@email";

                using (SqlCommand cmd = new SqlCommand(q, con))
                {
                    cmd.Parameters.AddWithValue("@email", loggedInTeacherEmail);
                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                        teacherId = result.ToString();
                }
            }

            return teacherId;
        }

        // ================= SAVE QUIZ FUNCTION =================
        int quizId = 0;
        private void SaveQuizToDatabase(bool isDraft)
        {
            try
            {
                // ---------- SETTINGS ----------
                string examName = examNameTxt.Text.Trim();
                string durationStr = durationTxt.Text.Trim();
                string allowedMarksStr = allowedMarksTxt.Text.Trim();
                string password = passwordTxt.Text.Trim();
                bool shuffleQuestions = chkShuffleQuestions.Checked;
                bool shuffleOptions = chkShuffleOptions.Checked;
                bool showAnswers = chkShowAnswers.Checked;

                if (!int.TryParse(durationStr, out int duration)) duration = 0;
                if (!int.TryParse(allowedMarksStr, out int allowedMarks)) allowedMarks = 0;

                // ---------- VALIDATE MANDATORY ----------
                if (string.IsNullOrWhiteSpace(examName) ||
                    duration <= 0 || allowedMarks <= 0 ||
                    string.IsNullOrWhiteSpace(password))
                {
                    MessageBox.Show("Please fill Exam Name, Duration, Allowed Marks, and Password.",
                        "Cannot Save", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // ---------- QUESTIONS ----------
                var questionList = new List<object>();
                bool hasAtLeastOneCompleteQuestion = false;

                foreach (var container in questionContainers)
                {
                    var qBox = container.Controls.OfType<TextBox>().FirstOrDefault(t => t.Multiline);
                    var optionBoxes = container.Controls.OfType<TextBox>()
                                        .Where(t => !t.Multiline && t.Width > 100).ToList();
                    var correctCombo = container.Controls.OfType<ComboBox>().FirstOrDefault();
                    var marksBox = container.Controls.OfType<TextBox>()
                                        .FirstOrDefault(t => !t.Multiline && t.Width <= 80);

                    int.TryParse(marksBox?.Text.Trim(), out int marks);

                    var options = optionBoxes
                        .Where(t => !string.IsNullOrWhiteSpace(t.Text))
                        .Select(t => t.Text.Trim()).ToList();

                    bool completeQuestion = !string.IsNullOrWhiteSpace(qBox?.Text) &&
                                            options.Count >= 2 &&
                                            correctCombo != null &&
                                            correctCombo.SelectedIndex >= 0 &&
                                            marks > 0;

                    if (completeQuestion)
                    {
                        hasAtLeastOneCompleteQuestion = true;

                        questionList.Add(new
                        {
                            QuestionText = qBox.Text.Trim(),
                            Options = options,
                            CorrectIndex = correctCombo.SelectedIndex,
                            Marks = marks
                        });
                    }
                }

                if (!hasAtLeastOneCompleteQuestion)
                {
                    MessageBox.Show("Please add at least one complete question (text, 2 options, correct answer, marks).",
                        "Cannot Save", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // ---------- FEATURES JSON ----------
                var featuresObj = new
                {
                    ShuffleQuestions = shuffleQuestions,
                    ShuffleOptions = shuffleOptions,
                    ShowCorrectAnswersAfterSubmission = showAnswers
                };

                string featuresJson = System.Text.Json.JsonSerializer.Serialize(featuresObj);
                string questionsJson = System.Text.Json.JsonSerializer.Serialize(questionList);

                string status = "Draft"; // always save as draft for now

                string connStr = System.Configuration.ConfigurationManager
                    .ConnectionStrings["LanQuizerDB"].ConnectionString;

                using (SqlConnection con = new SqlConnection(connStr))
                {
                    con.Open();

                    string teacherId = GetCurrentTeacherId();
                    if (string.IsNullOrWhiteSpace(teacherId))
                    {
                        MessageBox.Show("Invalid teacher session. Please login again.",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // ---------- CHECK DUPLICATE EXAM ----------
                    string checkQuery = @"SELECT QuizID FROM QuizTable 
                                  WHERE TeacherEmail=@email AND ExamName=@examName";

                    int existingQuizId = 0;
                    using (SqlCommand cmd = new SqlCommand(checkQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@email", loggedInTeacherEmail);
                        cmd.Parameters.AddWithValue("@examName", examName);

                        object result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                            existingQuizId = Convert.ToInt32(result);
                    }

                    if (existingQuizId > 0)
                    {
                        if (MessageBox.Show("A quiz with this Exam Name already exists. Do you want to overwrite it?",
                            "Overwrite Quiz?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        {
                            return;
                        }
                    }
                    // ---------- INSERT / UPDATE ----------
                    if (existingQuizId == 0)
                    {
                        // use SCOPE_IDENTITY() and cast to int
string insertQuery = @"
INSERT INTO QuizTable
(TeacherID, TeacherEmail, ExamName, DurationMinutes, AllowedQuestion,
 Features, QuizPassword, Questions, Status, CreatedAt)
VALUES
(@teacherId, @email, @examName, @duration, @allowedMarks,
 @features, @password, @questions, @status, @createdAt);
SELECT CAST(SCOPE_IDENTITY() AS int);";

using (SqlCommand cmd = new SqlCommand(insertQuery, con))
{
    cmd.Parameters.AddWithValue("@teacherId", teacherId);
    cmd.Parameters.AddWithValue("@email", loggedInTeacherEmail);
    cmd.Parameters.AddWithValue("@examName", examName);
    cmd.Parameters.AddWithValue("@duration", duration);
    cmd.Parameters.AddWithValue("@allowedMarks", allowedMarks);
    cmd.Parameters.AddWithValue("@features", featuresJson);
    cmd.Parameters.AddWithValue("@password", password);
    cmd.Parameters.AddWithValue("@questions", questionsJson);
    cmd.Parameters.AddWithValue("@status", status);
    cmd.Parameters.AddWithValue("@createdAt", DateTime.Now);

    object idObj = cmd.ExecuteScalar(); // now returns the identity
    if (idObj != null && idObj != DBNull.Value)
        quizId = Convert.ToInt32(idObj);
    // no ExecuteNonQuery() here
}
                    }
                    else
                    {
                        string updateQuery = @"
UPDATE QuizTable SET
    DurationMinutes=@duration,
    AllowedQuestion=@allowedMarks,
    Features=@features,
    QuizPassword=@password,
    Questions=@questions,
    Status=@status,
    CreatedAt=@createdAt
WHERE QuizID=@quizId";

                        using (SqlCommand cmd = new SqlCommand(updateQuery, con))
                        {
                            cmd.Parameters.AddWithValue("@quizId", existingQuizId);
                            cmd.Parameters.AddWithValue("@duration", duration);
                            cmd.Parameters.AddWithValue("@allowedMarks", allowedMarks);
                            cmd.Parameters.AddWithValue("@features", featuresJson);
                            cmd.Parameters.AddWithValue("@password", password);
                            cmd.Parameters.AddWithValue("@questions", questionsJson);
                            cmd.Parameters.AddWithValue("@status", status);
                            cmd.Parameters.AddWithValue("@createdAt", DateTime.Now);

                            cmd.ExecuteNonQuery();
                        }
                    }
                }

                // ---------- SUCCESS ----------
                MessageBox.Show("Quiz saved successfully as Draft!", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving quiz: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //=================End of save Process=================


        //======================Start Quiz Button========================
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // ---------- SETTINGS ----------
                string examName = examNameTxt.Text.Trim();
                string durationStr = durationTxt.Text.Trim();
                string allowedMarksStr = allowedMarksTxt.Text.Trim();
                string password = passwordTxt.Text.Trim();

                bool shuffleQuestions = chkShuffleQuestions.Checked;
                bool shuffleOptions = chkShuffleOptions.Checked;
                bool showAnswers = chkShowAnswers.Checked;

                if (!int.TryParse(durationStr, out int duration)) duration = 0;
                if (!int.TryParse(allowedMarksStr, out int allowedMarks)) allowedMarks = 0;

                // ---------- VALIDATE MANDATORY ----------
                if (string.IsNullOrWhiteSpace(examName) || duration <= 0 || allowedMarks <= 0 || string.IsNullOrWhiteSpace(password))
                {
                    MessageBox.Show("Please fill Exam Name, Duration, Allowed Marks, and Password.",
                        "Cannot Start Quiz", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // ---------- QUESTIONS ----------
                var questionList = new List<object>();
                int totalQuestionMarks = 0;
                bool hasAtLeastOneCompleteQuestion = false;

                foreach (var container in questionContainers)
                {
                    var qBox = container.Controls.OfType<TextBox>().FirstOrDefault(t => t.Multiline);
                    var optionBoxes = container.Controls.OfType<TextBox>()
                                        .Where(t => !t.Multiline && t.Width > 100).ToList();
                    var correctCombo = container.Controls.OfType<ComboBox>().FirstOrDefault();
                    var marksBox = container.Controls.OfType<TextBox>()
                                        .FirstOrDefault(t => !t.Multiline && t.Width <= 80);

                    int.TryParse(marksBox?.Text.Trim(), out int marks);

                    var options = optionBoxes
                        .Where(t => !string.IsNullOrWhiteSpace(t.Text))
                        .Select(t => t.Text.Trim()).ToList();

                    bool completeQuestion = !string.IsNullOrWhiteSpace(qBox?.Text) &&
                                            options.Count >= 2 &&
                                            correctCombo != null &&
                                            correctCombo.SelectedIndex >= 0 &&
                                            marks > 0;

                    if (completeQuestion)
                    {
                        hasAtLeastOneCompleteQuestion = true;
                        totalQuestionMarks += marks;

                        questionList.Add(new
                        {
                            QuestionText = qBox.Text.Trim(),
                            Options = options,
                            CorrectIndex = correctCombo.SelectedIndex,
                            Marks = marks
                        });
                    }
                }

                if (!hasAtLeastOneCompleteQuestion)
                {
                    MessageBox.Show("Please add at least one complete question (text, 2 options, correct answer, marks).",
                        "Cannot Start Quiz", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // ---------- MARKS VALIDATION ----------
                if (totalQuestionMarks < allowedMarks)
                {
                    MessageBox.Show($"Total question marks ({totalQuestionMarks}) are less than allowed marks ({allowedMarks}). Add more questions.",
                        "Cannot Start Quiz", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else if (totalQuestionMarks > allowedMarks && !shuffleQuestions)
                {
                    MessageBox.Show($"Total question marks ({totalQuestionMarks}) exceed allowed marks ({allowedMarks}). Enable Shuffle Questions or adjust marks.",
                        "Cannot Start Quiz", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // ---------- OPEN SCHEDULE FORM ----------
                // Pass all necessary info to the Schedule form
                Schedule scheduleForm = new Schedule(
                    examName: examName,
                    duration: duration,
                    allowedMarks: allowedMarks,
                    password: password,
                    questionsJson: System.Text.Json.JsonSerializer.Serialize(questionList),
                    featuresJson: System.Text.Json.JsonSerializer.Serialize(new
                    {
                        ShuffleQuestions = shuffleQuestions,
                        ShuffleOptions = shuffleOptions,
                        ShowCorrectAnswersAfterSubmission = showAnswers
                    }),
                    teacherEmail: loggedInTeacherEmail
                );

                scheduleForm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error opening schedule: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }





        //==================Start Quiz Button End======================
        // ================= OTHER BUTTONS =================
        private void exitBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void minbtn_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void backbtn_Click(object sender, EventArgs e)
        {
            // Go back to teacher home
            TeacherHome home = new TeacherHome(loggedInTeacherEmail);
            home.Show();
            this.Close();
        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {
            backbtn_Click(sender, e);
        }

        private void label1_Click(object sender, EventArgs e)
        {
            backbtn_Click(sender, e);
        }

        // This method is from your original code, keeping it as is


        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Schedule schedule = new Schedule(quizId.ToString());
            schedule.Show();
        }

        private void CreateQuiz_Load_1(object sender, EventArgs e)
        {

        }

        private void saveDraft_Click(object sender, EventArgs e)
        {
            SaveQuizToDatabase(isDraft: true);
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            SaveQuizToDatabase(isDraft: true);
        }
    }
}