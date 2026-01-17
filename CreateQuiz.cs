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
    public partial class CreateQuiz : Form
    {
        public CreateQuiz()
        {
            InitializeComponent();
        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void backBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            TeacherHome teacherHome = new TeacherHome();
            teacherHome.Show();
        }


        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void questionBtn_Click(object sender, EventArgs e)
        {
            groupBox1.Visible = false;
        }

        private void label1_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            TeacherHome te = new TeacherHome();
            te.Show();
        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {
            string teacherName = "Admin Teacher"; // from DataTable in login

            this.Hide();
            TeacherHome teach = new TeacherHome(teacherName); // ✅ pass the string
            teach.Show();
        }

        private void exitBtn_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
            "Are you sure you want to exit? It needs to Login again.",
            "Confirm Exit",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning );

            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void minbtn_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}
