using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using Microsoft.Data.SqlClient;

namespace LanQuizer
{
    public partial class Schedule : Form
    {
        private string connStr;
        private SqlConnection connect;
        private string LoggedInTeacherEmail;
        public Schedule()
        {
            InitializeComponent();
            /*================Database Connection String================*/
            LoggedInTeacherEmail = LoggedInUser.Email;

            connStr = ConfigurationManager.ConnectionStrings["LanQuizerDB"].ConnectionString;
            connect = new SqlConnection(connStr);
        }

        private void Schedule_Load(object sender, EventArgs e)
        {
            // Set default selection
            startNowChk.Checked = true;
            scheduleCheck.Checked = false;

            // Update visibility based on selected radio button
            UpdateGroupVisibility();

            // Load teacher-specific courses
            LoadCourses();
        }



        private void startNowChk_CheckedChanged(object sender, EventArgs e)
        {
            UpdateGroupVisibility();
        }

        private void scheduleCheck_CheckedChanged(object sender, EventArgs e)
        {
            UpdateGroupVisibility();
        }

        private void UpdateGroupVisibility()
        {
            // Show StartNow if startNowChk is checked
            StartNow.Visible = startNowChk.Checked;
            ScheduleQuiz.Visible = scheduleCheck.Checked;

            // Bring front to avoid overlapping
            if (StartNow.Visible) StartNow.BringToFront();
            if (ScheduleQuiz.Visible) ScheduleQuiz.BringToFront();
        }

        /*================Load Course Function================*/
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

                    SqlDataReader dr = cmd.ExecuteReader();

                    bool hasData = false;

                    while (dr.Read())
                    {
                        hasData = true;
                        scheduleCourse.Items.Add(dr["Course"].ToString());
                    }

                    dr.Close();

                    if (!hasData)
                    {
                        MessageBox.Show(
                            "No courses found.\nPlease add student data first.",
                            "No Data",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning
                        );

                        scheduleBtn.Enabled = false;
                    }
                }
            }
        }

        private void scheduleCourse_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (scheduleCourse.SelectedIndex != -1)
            {
                LoadSections(scheduleCourse.SelectedItem.ToString(), ScheduleSec);
            }
        }

        private void scheduleSec_SelectedIndexChanged(object sender, EventArgs e)
        {
            ValidateScheduleInputs();
        }

        /*================Load Section Function================*/
        private void scheduleCourse_SelectedIndex(object sender, EventArgs e)
        {
            if (scheduleCourse.SelectedIndex != -1)
            {
                LoadSections(scheduleCourse.SelectedItem.ToString(), ScheduleSec);
            }
        }

        private void scheduleSec_SelectedIndex(object sender, EventArgs e)
        {
            ValidateScheduleInputs();
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

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@course", course);

                SqlDataReader dr = cmd.ExecuteReader();

                bool hasData = false;

                while (dr.Read())
                {
                    hasData = true;
                    targetCombo.Items.Add(dr["Section"].ToString());
                }

                dr.Close();

                if (!hasData)
                {
                    MessageBox.Show(
                        "No sections found for this course.\nPlease add sections first.",
                        "No Sections",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                }
            }
        }

        private void ValidateScheduleInputs()
        {
            scheduleBtn.Enabled =
            scheduleCourse.SelectedIndex != -1 &&
            ScheduleSec.SelectedIndex != -1;
        }

        private void scheduleBtn_Click(object sender, EventArgs e)
        {
            StartQuiz scheduleQuizForm = new StartQuiz();
            scheduleQuizForm.Show();
        }
    }
}
