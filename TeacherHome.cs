using Microsoft.Data.SqlClient;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace LanQuizer
{
    public partial class TeacherHome : Form
    {
        public TeacherHome()
        {
            InitializeComponent();
        }

        public TeacherHome(string teacherName)
        {
            InitializeComponent();
            WelcomeTeacher.Text = "Welcome Back, " + teacherName;
        }

        private void TeacherHome_Load(object sender, EventArgs e)
        {
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
            draftlbl.Visible = false;
            quizLbl.Visible = false;
            groupBox2.Visible = false;
            label8.Visible = false;
            groupBox1.Visible = false;
            addSection.Visible = true;

            Panel panelSections = this.Controls["panelSections"] as Panel;
            if (panelSections != null)
            {
                panelSections.Visible = true; // Make sure panel is visible
            }

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
                string query = "SELECT DISTINCT Section, Course FROM Students ORDER BY Section, Course";
                using (SqlCommand cmd = new SqlCommand(query, connect))
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


                        MessageBox.Show(
    Properties.Resources.ResourceManager.GetObject("edit") == null
    ? "Image NOT found"
    : "Image FOUND"
);
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
                            Cursor = Cursors.Hand
                        };
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
                        modifyBtn.FlatAppearance.BorderSize = 0;
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
                            Cursor = Cursors.Hand
                        };
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
                            xPos = xStart;         // reset to first column
                            yPos += gbHeight + gapY; // move to next row
                        }
                    }

                    if (!hasSections)
                    {
                        Label noSectionLbl = new Label
                        {
                            Text = "NO SECTIONS ADDED YET",
                            Top = yStart + 30,
                            Left = xStart +400,
                            AutoSize = true,
                            ForeColor = Color.Red,
                            Font = new Font("Times New Roman", 18, FontStyle.Bold)
                        };
                        panelSections.Controls.Add(noSectionLbl);
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
                this.Close();
            }
        }

        private void myQuizBtn_Click(object sender, EventArgs e)
        {
            draftlbl.Visible = true;
            quizLbl.Visible = true;
            groupBox2.Visible = true;
            label8.Visible = true;
            groupBox1.Visible = true;
            addSection.Visible = false;

            // Hide the section panel when viewing quizzes
            Panel panelSections = this.Controls["panelSections"] as Panel;
            if (panelSections != null)
                panelSections.Visible = false;
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

    }
}
