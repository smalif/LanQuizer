using Microsoft.Extensions.FileSystemGlobbing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using Microsoft.Data.SqlClient;
using System.Configuration;


namespace LanQuizer
{
    public partial class NewRegistration_Teacher : Form
    {

        private string connStr;
        private SqlConnection connect;
        public NewRegistration_Teacher()
        {
            InitializeComponent();
            connStr = ConfigurationManager.ConnectionStrings["LanQuizerDB"].ConnectionString;
            connect = new SqlConnection(connStr);
        }

        private void NewRegistration_Teacher_Load(object sender, EventArgs e)
        {

        }

        private void showPass_CheckedChanged(object sender, EventArgs e)
        {
            new_pass.PasswordChar = showPass.Checked ? '\0' : '*';
            confirm_pass.PasswordChar = showPass.Checked ? '\0' : '*';
        }

        private void BacktoT_log_Click(object sender, EventArgs e)
        {
            this.Close();
            Teacher_Reg_Form teacherForm = new Teacher_Reg_Form();
            teacherForm.Show();
        }

        private void exitBtn_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
            Teacher_Reg_Form teacherForm = new Teacher_Reg_Form();
            teacherForm.Show();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            this.Close();
            Teacher_Reg_Form teacherForm = new Teacher_Reg_Form();
            teacherForm.Show();
        }

        private void CheckPasswordsMatch()
        {
            string newPass = new_pass.Text.Trim();
            string confirmPass = confirm_pass.Text.Trim();

            if (!string.IsNullOrEmpty(newPass) && newPass == confirmPass)
            {
                registerBtn.Enabled = true;
                matchWar.Visible = false;
            }
            else
            {
                registerBtn.Enabled = false;
                if (!string.IsNullOrEmpty(confirmPass))
                {
                    matchWar.Text = "Passwords do not match!";
                    matchWar.Visible = true;
                }
                else
                {
                    matchWar.Visible = false;
                }
            }
        }
        private void registerBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(T_name.Text.Trim()) ||
                string.IsNullOrEmpty(T_ID.Text.Trim()) ||
                string.IsNullOrEmpty(T_email.Text.Trim()) ||
                string.IsNullOrEmpty(new_pass.Text.Trim()) ||
                string.IsNullOrEmpty(confirm_pass.Text.Trim()))
            {
                MessageBox.Show("Please fill all password fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            try
            {
                // Open connection
                connect.Open();
                // Insert new teacher record
                string insertData = "INSERT INTO Teachers (teacherName, TeacherID, teacherEmail, TeacherpassBox) " +
                                    "VALUES (@name, @teacherID, @email, @password)";
                using (SqlCommand cmd = new SqlCommand(insertData, connect))
                {
                    // Add parameters
                    cmd.Parameters.AddWithValue("@name", T_name.Text.Trim());
                    cmd.Parameters.AddWithValue("@teacherID", T_ID.Text.Trim());
                    cmd.Parameters.AddWithValue("@email", T_email.Text.Trim());
                    cmd.Parameters.AddWithValue("@password", new_pass.Text.Trim());
                    // Execute insert command
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Registration Successful! Please Log In", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                        Teacher_Reg_Form teacherForm = new Teacher_Reg_Form();
                        teacherForm.Show();
                    }
                    else
                    {
                        MessageBox.Show("Registration Failed!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Close connection
                if (connect.State == ConnectionState.Open)
                {
                    connect.Close();
                }
            }

        }
    }
}
