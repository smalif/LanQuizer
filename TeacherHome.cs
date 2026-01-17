using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LanQuizer
{
    public partial class TeacherHome : Form
    {
        public TeacherHome()
        {
            InitializeComponent();
            WelcomeTeacher.Text = "Welcome Back, " + WelcomeTeacher;
        }

        public TeacherHome(string teacherName)
        {
            InitializeComponent();
            WelcomeTeacher.Text = "Welcome Back, " + teacherName;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }


        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void TeacherHome_Load(object sender, EventArgs e)
        {
            addSection.Visible = false;
        }

        private void createQuizBtn_Click(object sender, EventArgs e)
        {
            CreateQuiz q1 = new CreateQuiz();
            q1.Show();
            this.Hide();
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

        private void label25_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

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

        private void sectionBtn_Click(object sender, EventArgs e)
        {
            draftlbl.Visible = false;
            quizLbl.Visible = false;
            groupBox2.Visible = false;
            label8.Visible = false;
            groupBox1.Visible = false;
            addSection.Visible = true;
        }

        private void myQuizBtn_Click(object sender, EventArgs e)
        {
            draftlbl.Visible = true;
            quizLbl.Visible = true;
            groupBox2.Visible = true;
            label8.Visible = true;
            groupBox1.Visible = true;
            addSection.Visible = false;
        }

        private void addSection_Click(object sender, EventArgs e)
        {
            Add_Section add_Section = new Add_Section();
            add_Section.Show();
        }
    }
}
