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
            // Force StartNowGrp to display at the designer location
            StartNowGrp.Visible = true;
            StartNowGrp.Location = new Point(12, 126);  // keep original Designer position
            StartNowGrp.BringToFront();

            // Hide ScheduleQuiz initially
            ScheduleQuiz.Visible = false;

            // Make sure StartNowChk is selected
            startNowChk.Checked = true;
            scheduleCheck.Checked = false;

            // Load teacher-specific courses
            LoadStartCourses();
            LoadCourses();
        }



        private void startNowChk_CheckedChanged(object sender, EventArgs e)
        {
            if (startNowChk.Checked)
            {
                StartNowGrp.Visible = true;
                StartNowGrp.Location = new Point(12, 126);  // force position again
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

                // StartNowGrp stays visible but behind ScheduleQuiz (or you can hide it)
                StartNowGrp.Visible = false;
            }
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


        private void scheduleCourse_Selected(object sender, EventArgs e)
        {
            if (scheduleCourse.SelectedIndex != -1)
            {
                LoadSections(scheduleCourse.SelectedItem.ToString(), ScheduleSec);
            }
        }

        private void scheduleSec_Selected(object sender, EventArgs e)
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

        /*================Start now dropdown================*/
        private void LoadStartCourses(string previousCourse = null)
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
                            StartCourse.Items.Add(dr["Course"].ToString());
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

        private void LoadStartSections(string course, string previousSection = null)
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
                            StartSection.Items.Add(dr["Section"].ToString());
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

        // When StartCourse changes, load sections for it
        private void StartCourse_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (StartCourse.SelectedIndex != -1)
            {
                LoadStartSections(StartCourse.SelectedItem.ToString());
            }
            ValidateStartInputs();
        }

        // When StartSection changes, validate inputs
        private void StartSection_SelectedIndexChanged(object sender, EventArgs e)
        {
            ValidateStartInputs();
        }
        private void ValidateStartInputs()
        {
            // Enable the schedule button only if both course and section are selected
            scheduleBtn.Enabled =
                StartCourse.SelectedIndex != -1 &&
                StartSection.SelectedIndex != -1;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
