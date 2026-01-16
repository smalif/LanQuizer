using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;

namespace LanQuizer
{
    public partial class Teacher_Reg_Form : Form
    {
        SqlConnection connect = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\alif0\OneDrive\Dokumen\teacher.mdf;Integrated Security=True;Connect Timeout=30");
        public Teacher_Reg_Form()
        {
            InitializeComponent();
        }

        private void teacherBtn_Click(object sender, EventArgs e)
        {
            Student student = new Student();
            student.Show();
            this.Hide();
        }

        private void exitBtn_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void TeacherpassBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            TeacherpassBox.PasswordChar = checkBox1.Checked ? '\0' : '*';
        }

        private void teacherEmail_TextChanged(object sender, EventArgs e)
        {

        }

        private void logInBtn_Click(object sender, EventArgs e)
        {
            if (teacherEmail.Text == "" || TeacherID.Text == "" || TeacherpassBox.Text == "")
            {
                MessageBox.Show("Please fill up all fields!", "Error Message",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (connect.State == ConnectionState.Closed)
            {
                try
                {
                    connect.Open();

                    string selectData = "SELECT * FROM users " +
                                        "WHERE email = @email " +
                                        "AND teacher_id = @teacherID " +
                                        "AND password = @password";

                    using (SqlCommand cmd = new SqlCommand(selectData, connect))
                    {
                        cmd.Parameters.AddWithValue("@email", teacherEmail.Text.Trim());
                        cmd.Parameters.AddWithValue("@teacherID", TeacherID.Text.Trim());
                        cmd.Parameters.AddWithValue("@password", TeacherpassBox.Text.Trim());

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable table = new DataTable();
                        adapter.Fill(table);

                        if (table.Rows.Count > 0)
                        {
                            // ✅ READ TEACHER NAME FROM DATABASE
                            string teacherName = table.Rows[0]["teacher_name"].ToString();

                            MessageBox.Show("Login Successful!", "Information Message",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // ✅ PASS NAME TO HOME FORM
                            TeacherHome home = new TeacherHome(teacherName);
                            home.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Login Failed!", "Warning Message",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error Message",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connect.Close();
                }
            }
        }

    }
}
