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

        public CreateQuiz()
        {
            InitializeComponent();

            // Set up initial state
            SetupForm();
        }

        private void SetupForm()
        {
            // Ensure panels are properly set up
            questionBox.Visible = true;
            questionPanel.Visible = false;
            questionPanel.AutoScroll = true;
            questionPanel.AutoScrollMargin = new Size(0, 20);

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
            // Switch to questions panel
            questionBox.Visible = false;
            questionPanel.Visible = true;
            questionPanel.BringToFront();

            // Update button styles
            questionBtn.BackColor = Color.SeaGreen;
            questionBtn.ForeColor = Color.White;
            questionBtn.Font = new Font(questionBtn.Font, FontStyle.Bold);

            myQuizBtn.BackColor = Color.White;
            myQuizBtn.ForeColor = Color.Black;
            myQuizBtn.Font = new Font(myQuizBtn.Font, FontStyle.Regular);

            // Initialize question panel if not already done
            if (!questionPanel.Controls.ContainsKey("initialized"))
            {
                InitializeQuestionPanel();
            }

            // Add first question if none exist
            if (questionContainers.Count == 0)
            {
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

        // ================= QUESTION PANEL INITIALIZATION =================
        private void InitializeQuestionPanel()
        {
            // Clear existing controls
            questionPanel.Controls.Clear();

            // Create and add Save button
            saveBtn = new Button
            {
                Name = "saveBtn",
                Text = "💾 Save Questions",
                Width = 150,
                Height = 35,
                Top = 10,
                Left = questionPanel.Width - 160,
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                BackColor = Color.SeaGreen,
                ForeColor = Color.White
            };
            saveBtn.Click += (s, e) =>
            {
                MessageBox.Show($"Saving {questionContainers.Count} question(s)", "Save",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            };
            questionPanel.Controls.Add(saveBtn);

            // Create and add Add New Question button
            addNewBtn = new Button
            {
                Name = "addNewBtn",
                Text = "➕ Add New Question",
                Width = 180,
                Height = 35,
                Top = 60,
                Left = (questionPanel.Width - 180) / 2,
                BackColor = Color.LightBlue,
                ForeColor = Color.Black
            };
            addNewBtn.Click += (s, e) => AddQuestionContainer();
            questionPanel.Controls.Add(addNewBtn);

            // Mark panel as initialized
            questionPanel.Controls.Add(new Control { Name = "initialized", Visible = false });
        }

        // ================= ADD QUESTION CONTAINER =================
        private void AddQuestionContainer()
        {
            int containerWidth = questionPanel.Width - 60; // Leave margins
            int containerHeight = 300; // Fixed height for each question

            // Create container panel
            Panel container = new Panel
            {
                Width = containerWidth,
                Height = containerHeight,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.WhiteSmoke,
                Padding = new Padding(10)
            };

            // Position container
            if (questionContainers.Count > 0)
            {
                Panel lastContainer = questionContainers.Last();
                container.Top = lastContainer.Bottom + 10;
            }
            else
            {
                container.Top = 100; // Start below buttons
            }
            container.Left = 20;

            // Question number label
            Label questionNumberLabel = new Label
            {
                Text = $"Question #{questionContainers.Count + 1}",
                Font = new Font("Arial", 10, FontStyle.Bold),
                Top = 10,
                Left = 10,
                Width = 120
            };
            container.Controls.Add(questionNumberLabel);

            // Question textbox
            TextBox questionTextBox = new TextBox
            {
                Name = "questionText",
                Top = questionNumberLabel.Bottom + 10,
                Left = 10,
                Width = containerWidth - 40,
                Height = 50,
                Multiline = true,
                PlaceholderText = "Enter your question here..."
            };
            container.Controls.Add(questionTextBox);

            int optionTop = questionTextBox.Bottom + 20;
            List<TextBox> optionTextBoxes = new List<TextBox>();

            // Create 4 option textboxes
            for (int i = 0; i < 4; i++)
            {
                Label optionLabel = new Label
                {
                    Text = $"Option {i + 1}:",
                    Top = optionTop,
                    Left = 10,
                    Width = 60
                };
                container.Controls.Add(optionLabel);

                TextBox optionTextBox = new TextBox
                {
                    Name = $"option{i + 1}",
                    Top = optionTop,
                    Left = 80,
                    Width = containerWidth - 100,
                    PlaceholderText = $"Enter option {i + 1}..."
                };
                container.Controls.Add(optionTextBox);
                optionTextBoxes.Add(optionTextBox);

                optionTop += 35;
            }

            // Correct answer dropdown
            Label correctAnswerLabel = new Label
            {
                Text = "Correct Answer:",
                Top = optionTop + 5,
                Left = 10,
                Width = 100
            };
            container.Controls.Add(correctAnswerLabel);

            ComboBox correctAnswerCombo = new ComboBox
            {
                Name = "correctAnswer",
                Top = optionTop,
                Left = 120,
                Width = 200,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            container.Controls.Add(correctAnswerCombo);

            // Update combo box when option text changes
            foreach (var optionBox in optionTextBoxes)
            {
                optionBox.TextChanged += (s, e) =>
                {
                    correctAnswerCombo.Items.Clear();
                    foreach (var opt in optionTextBoxes.Where(o => !string.IsNullOrWhiteSpace(o.Text)))
                    {
                        correctAnswerCombo.Items.Add(opt.Text);
                    }
                };
            }

            // Marks
            Label marksLabel = new Label
            {
                Text = "Marks:",
                Top = optionTop + 5,
                Left = 340,
                Width = 50
            };
            container.Controls.Add(marksLabel);

            TextBox marksTextBox = new TextBox
            {
                Name = "marks",
                Top = optionTop,
                Left = 390,
                Width = 50,
                Text = "1"
            };
            container.Controls.Add(marksTextBox);

            // Delete button
            Button deleteButton = new Button
            {
                Text = "🗑️ Delete",
                Top = optionTop,
                Left = containerWidth - 100,
                Width = 80,
                Height = 25,
                BackColor = Color.IndianRed,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            deleteButton.Click += (s, e) => DeleteQuestionContainer(container);
            container.Controls.Add(deleteButton);

            // Add container to panel and list
            questionPanel.Controls.Add(container);
            questionContainers.Add(container);

            // Reorder containers
            ReorderContainers();

            // Scroll to show new container
            questionPanel.ScrollControlIntoView(container);
        }

        // ================= DELETE QUESTION CONTAINER =================
        private void DeleteQuestionContainer(Panel container)
        {
            if (MessageBox.Show("Are you sure you want to delete this question?",
                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // Remove from panel and list
                questionPanel.Controls.Remove(container);
                questionContainers.Remove(container);

                // Dispose resources
                container.Dispose();

                // Reorder remaining containers
                ReorderContainers();
            }
        }

        // ================= REORDER CONTAINERS =================
        private void ReorderContainers()
        {
            int topPosition = 100; // Start below buttons

            for (int i = 0; i < questionContainers.Count; i++)
            {
                Panel container = questionContainers[i];
                container.Top = topPosition;

                // Update question number
                foreach (Control ctrl in container.Controls)
                {
                    if (ctrl is Label label && label.Text.StartsWith("Question #"))
                    {
                        label.Text = $"Question #{i + 1}";
                        break;
                    }
                }

                topPosition = container.Bottom + 10;
            }

            // Position Add New button at the bottom
            if (addNewBtn != null)
            {
                if (questionContainers.Count > 0)
                {
                    addNewBtn.Top = questionContainers.Last().Bottom + 20;
                }
                else
                {
                    addNewBtn.Top = 100;
                }
                addNewBtn.Left = (questionPanel.Width - addNewBtn.Width) / 2;
            }
        }

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
    }
}