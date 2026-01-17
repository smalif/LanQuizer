using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Windows.Forms;
using Microsoft.Data.SqlClient; 
namespace LanQuizer
{
    public partial class Settings : Form
    {
        private string loggedInEmail;
        private string loggedInID;
        private string connStr;
        private SqlConnection connect;

        // Constructor with logged-in user info
        public Settings(string email, string teacherID)
        {
            InitializeComponent();

            loggedInEmail = email;
            loggedInID = teacherID;

            // Database connection
            connStr = ConfigurationManager.ConnectionStrings["LanQuizerDB"].ConnectionString;
            connect = new SqlConnection(connStr);

            // Initialize UI
            PassCon.Enabled = false;       // Disable button initially
            matchWar.Visible = false;      // Hide warning label

            // Ensure password boxes are masked initially
            oldPassBox.UseSystemPasswordChar = true;
            NewPassBox.UseSystemPasswordChar = true;
            confirmBox.UseSystemPasswordChar = true;

            // Wire TextChanged events to check password match
            NewPassBox.TextChanged += PasswordBoxes_TextChanged;
            confirmBox.TextChanged += PasswordBoxes_TextChanged;

            // Optional: make buttons have hand cursor
            ApplyHandCursor(this);
        }

        // Universal method to apply hand cursor to buttons
        private void ApplyHandCursor(Control parent)
        {
            foreach (Control ctrl in parent.Controls)
            {
                if (ctrl is Button)
                    ctrl.Cursor = Cursors.Hand;

                if (ctrl.HasChildren)
                    ApplyHandCursor(ctrl);
            }
        }

        // Toggle password visibility
        private void TogglePasswordVisibility(TextBox textBox)
        {
            textBox.UseSystemPasswordChar = !textBox.UseSystemPasswordChar;
        }

        private void Viewold_Click(object sender, EventArgs e)
        {
            TogglePasswordVisibility(oldPassBox);
        }

        private void viewNew_Click(object sender, EventArgs e)
        {
            TogglePasswordVisibility(NewPassBox);
        }

        private void viewCon_Click(object sender, EventArgs e)
        {
            TogglePasswordVisibility(confirmBox);
        }

        // TextChanged event for both New and Confirm password
        private void PasswordBoxes_TextChanged(object sender, EventArgs e)
        {
            CheckPasswordsMatch();
        }

        // Enable/disable button and show warning
        private void CheckPasswordsMatch()
        {
            string newPass = NewPassBox.Text.Trim();
            string confirmPass = confirmBox.Text.Trim();

            if (!string.IsNullOrEmpty(newPass) && newPass == confirmPass)
            {
                PassCon.Enabled = true;
                matchWar.Visible = false;
            }
            else
            {
                PassCon.Enabled = false;
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

        // Change password button click
        private void PassCon_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(oldPassBox.Text.Trim()) || string.IsNullOrEmpty(NewPassBox.Text.Trim()))
            {
                MessageBox.Show("Please fill all password fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Open connection
                connect.Open();

                // Step 1: Verify old password using the logged-in user's email/ID
                string selectQuery = "SELECT * FROM Teachers WHERE teacherEmail = @email AND TeacherID = @teacherID AND TeacherpassBox = @oldPassword";

                using (SqlCommand cmd = new SqlCommand(selectQuery, connect))
                {
                    // Use LoggedInUser to avoid null/unsupplied parameters
                    cmd.Parameters.Add("@email", SqlDbType.NVarChar, 400).Value = LoggedInUser.Email;
                    cmd.Parameters.Add("@teacherID", SqlDbType.NVarChar, 50).Value = LoggedInUser.ID;
                    cmd.Parameters.Add("@oldPassword", SqlDbType.NVarChar, 50).Value = oldPassBox.Text.Trim();

                    // Fill DataTable to check if user exists and old password matches
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    if (table.Rows.Count > 0)
                    {
                        // Step 2: Old password correct → update to new password
                        string updateQuery = "UPDATE Teachers SET TeacherpassBox = @newPassword WHERE teacherEmail = @email AND TeacherID = @teacherID";

                        using (SqlCommand updateCmd = new SqlCommand(updateQuery, connect))
                        {
                            updateCmd.Parameters.Add("@newPassword", SqlDbType.NVarChar, 50).Value = NewPassBox.Text.Trim();
                            updateCmd.Parameters.Add("@email", SqlDbType.NVarChar, 400).Value = LoggedInUser.Email;
                            updateCmd.Parameters.Add("@teacherID", SqlDbType.NVarChar, 50).Value = LoggedInUser.ID;

                            int rowsAffected = updateCmd.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Password changed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                // Optionally clear password fields
                                oldPassBox.Clear();
                                NewPassBox.Clear();
                                confirmBox.Clear();

                                this.Close(); // Close the form after success
                            }
                            else
                            {
                                MessageBox.Show("Failed to change password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Old password is incorrect!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (connect.State == ConnectionState.Open)
                    connect.Close();
            }
        }

        // Minimize and close buttons
        private void exitBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void minbtn_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}
