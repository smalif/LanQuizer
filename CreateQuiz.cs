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
                MessageBox.Show($"Questions: {questionContainers.Count}");
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





        // ================= REORDER =================



        //=================End QUESTION PANEL METHODS=================
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
        private void button1_Click(object sender, EventArgs e)
        {
            Schedule schedule = new Schedule();
            schedule.Show();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Schedule schedule = new Schedule();
            schedule.Show();
        }

        private void CreateQuiz_Load_1(object sender, EventArgs e)
        {

        }
    }
}