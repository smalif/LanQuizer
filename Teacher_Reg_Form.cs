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
using System.Configuration;
using Microsoft.Data.SqlClient;

namespace LanQuizer
{
    public partial class Teacher_Reg_Form : Form
    {
        private string connStr;
        private SqlConnection connect;

        private void ApplyHandCursor(Control parent)
        {
            foreach (Control ctrl in parent.Controls)
            {
                if (ctrl is Button)
                {
                    ctrl.Cursor = Cursors.Hand;
                }

                // Recursively apply to nested controls (GroupBox, Panel, etc.)
                if (ctrl.HasChildren)
                {
                    ApplyHandCursor(ctrl);
                }
            }
        }

        public Teacher_Reg_Form()
        {
            InitializeComponent();
            ApplyHandCursor(this);
            connStr = ConfigurationManager
                      .ConnectionStrings["LanQuizerDB"]
                      .ConnectionString;

            connect = new SqlConnection(connStr);
        }

        private void teacherBtn_Click(object sender, EventArgs e)
        {
            Student student = new Student();
            ApplyHandCursor(this);
            student.Show();
            this.Hide();
        }

        private void exitBtn_Click(object sender, EventArgs e)
        {
            ApplyHandCursor(this);
            Application.Exit();
        }

        private void TeacherpassBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            ApplyHandCursor(this);
            TeacherpassBox.PasswordChar = checkBox1.Checked ? '\0' : '*';
        }

        private void teacherEmail_TextChanged(object sender, EventArgs e)
        {

        }

        private void logInBtn_Click(object sender, EventArgs e)
        {
            ApplyHandCursor(this);
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

                    string selectData = "SELECT * FROM Teachers " +
                     "WHERE teacherEmail = @email " +
                     "AND TeacherID = @teacherID " +
                     "AND TeacherpassBox = @password";

                    using (SqlCommand cmd = new SqlCommand(selectData, connect))
                    {
                        // Add parameters
                        cmd.Parameters.AddWithValue("@email", teacherEmail.Text.Trim());
                        cmd.Parameters.AddWithValue("@teacherID", TeacherID.Text.Trim());
                        cmd.Parameters.AddWithValue("@password", TeacherpassBox.Text.Trim());

                        // Execute query and fill DataTable
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable table = new DataTable();
                        adapter.Fill(table);

                        // Check if login is successful
                        if (table.Rows.Count > 0)
                        {
                            // ✅ Read teacher name from database
                            string teacherName = table.Rows[0]["teacherName"].ToString();

                            MessageBox.Show("Login Successful!", "Information Message",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // ✅ Pass name to TeacherHome form
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

        private void Teacher_Reg_Form_Load(object sender, EventArgs e)
        {
            ApplyHandCursor(this);
        }
    }
}
