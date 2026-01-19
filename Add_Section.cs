using OfficeOpenXml;
using System;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Configuration;
using Microsoft.Data.SqlClient;



namespace LanQuizer
{
    public partial class Add_Section : Form
    {

        DataTable studentTable = new DataTable();
        string idPattern = @"^\d{2}-\d{5}-\d{1}$";
        // For pre-filling Section and Course when modifying
        public string PreFillSection { get; set; } = "";
        public string PreFillCourse { get; set; } = "";
        private string connStr = ConfigurationManager.ConnectionStrings["LanQuizerDB"].ConnectionString;

        public Add_Section()
        {
            InitializeComponent();
            LoadExistingSectionsCourses();
        }

        private void sectionNameLbl_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Add_Section_Load(object sender, EventArgs e)
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            studentTable.Columns.Add("StudentID");
            studentTable.Columns.Add("StudentName");
            dataGridView1.Visible = false;
            DataClose.Visible = false;

            // Pre-fill section/course if set (for modification)
            if (!string.IsNullOrEmpty(PreFillSection))
                SectionCombo.Text = PreFillSection;

            if (!string.IsNullOrEmpty(PreFillCourse))
                courseCombo.Text = PreFillCourse;

            LoadExistingSectionsCourses(); // populate ComboBox suggestions
        }

        private void ReadExcel(string path)
        {
            studentTable.Rows.Clear();

            FileInfo file = new FileInfo(path);

            using (ExcelPackage package = new ExcelPackage(file))
            {
                ExcelWorksheet ws = package.Workbook.Worksheets[0];
                int rowCount = ws.Dimension.Rows;
                int colCount = ws.Dimension.Columns;

                for (int row = 2; row <= rowCount; row++)
                {
                    string foundID = "";
                    string foundName = "";

                    for (int col = 1; col <= colCount; col++)
                    {
                        string cellValue = ws.Cells[row, col].Text.Trim();

                        if (Regex.IsMatch(cellValue, idPattern))
                        {
                            foundID = cellValue;
                        }
                        else if (Regex.IsMatch(cellValue, @"^[a-zA-Z\s\.]+$") && cellValue.Length > 3)
                        {
                            foundName = cellValue;
                        }
                    }

                    if (!string.IsNullOrEmpty(foundID) && !string.IsNullOrEmpty(foundName))
                    {
                        studentTable.Rows.Add(foundID, foundName);
                    }
                }
            }

            dataView.Text = studentTable.Rows.Count + " students found";
        }




        private void uploadBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Excel Files|*.xlsx;*.xls";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                ReadExcel(ofd.FileName);
            }
        }

        private void ResizeDataGridView()
        {
            int totalHeight = dataGridView1.ColumnHeadersHeight; // header

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                totalHeight += row.Height;
            }

            // Maximum height for the DataGridView
            int maxHeight = 300; // adjust based on your form layout

            dataGridView1.Height = Math.Min(totalHeight, maxHeight);
        }

        private void viewBtn_Click(object sender, EventArgs e)
        {
            string selectedSection = SectionCombo.Text.Trim();
            string selectedCourse = courseCombo.Text.Trim();

            if (string.IsNullOrEmpty(selectedSection) || string.IsNullOrEmpty(selectedCourse))
            {
                MessageBox.Show("Please select Section and Course first or Upload Excel File.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 1️⃣ Check if Excel data exists
            if (studentTable != null && studentTable.Rows.Count > 0)
            {
                dataGridView1.DataSource = studentTable;
                ShowDataGrid();
                return;
            }

            // 2️⃣ If no Excel, fetch from database
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    string query = @"
                SELECT StudentID, StudentName
                FROM Students
                WHERE Section = @section 
                  AND Course = @course
                  AND TeacherEmail = @teacherEmail
                  AND TeacherID = @teacherID";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@section", selectedSection);
                        cmd.Parameters.AddWithValue("@course", selectedCourse);
                        cmd.Parameters.AddWithValue("@teacherEmail", LoggedInUser.Email);
                        cmd.Parameters.AddWithValue("@teacherID", LoggedInUser.ID);

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dbTable = new DataTable();
                        adapter.Fill(dbTable);

                        if (dbTable.Rows.Count == 0)
                        {
                            MessageBox.Show("No students found for the selected section and course.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }

                        dataGridView1.DataSource = dbTable;
                        ShowDataGrid();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error fetching students from database: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Helper function to display the grid
        private void ShowDataGrid()
        {
            dataGridView1.Visible = true;
            DataClose.Visible = true;
            dataGridView1.BringToFront();
            DataClose.BringToFront();
            ResizeDataGridView();
        
        }

        private void DataClose_Click(object sender, EventArgs e)
        {
            dataGridView1.Visible = false;
            DataClose.Visible = false;
        }


        private void saveBtn_Click(object sender, EventArgs e)
        {
            if (studentTable.Rows.Count == 0)
            {
                MessageBox.Show("No students uploaded. Please select an Excel file first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string sectionName = SectionCombo.Text.Trim();
            string courseName = courseCombo.Text.Trim();

            if (string.IsNullOrEmpty(sectionName) || string.IsNullOrEmpty(courseName))
            {
                MessageBox.Show("Please enter both Section and Course name.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string connStr = ConfigurationManager.ConnectionStrings["LanQuizerDB"].ConnectionString;

            using (SqlConnection connect = new SqlConnection(connStr))
            {
                try
                {
                    connect.Open();

                    // Get current logged-in teacher info
                    string teacherEmail = LoggedInUser.Email;
                    string teacherID = LoggedInUser.ID;

                    // 1️⃣ Check if Section+Course already exists for this teacher
                    string checkQuery = "SELECT COUNT(*) FROM Students WHERE Section=@section AND Course=@course AND TeacherEmail=@teacherEmail AND TeacherID=@teacherID";
                    int existingCount = 0;

                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, connect))
                    {
                        checkCmd.Parameters.AddWithValue("@section", sectionName);
                        checkCmd.Parameters.AddWithValue("@course", courseName);
                        checkCmd.Parameters.AddWithValue("@teacherEmail", teacherEmail);
                        checkCmd.Parameters.AddWithValue("@teacherID", teacherID);

                        existingCount = (int)checkCmd.ExecuteScalar();
                    }

                    if (existingCount > 0)
                    {
                        // Warn user about previous data
                        string warnMessage = $"{existingCount} student(s) already exist for Section \"{sectionName}\" and Course \"{courseName}\".\n" +
                                             $"Your new file contains {studentTable.Rows.Count} student(s).\n" +
                                             "Do you want to update the previous data with the new file?";
                        DialogResult updateResult = MessageBox.Show(warnMessage, "Update Section?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (updateResult == DialogResult.No)
                        {
                            MessageBox.Show("Save operation cancelled.", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }

                        // Delete old data for this teacher + Section + Course
                        string deleteQuery = "DELETE FROM Students WHERE Section=@section AND Course=@course AND TeacherEmail=@teacherEmail AND TeacherID=@teacherID";
                        using (SqlCommand deleteCmd = new SqlCommand(deleteQuery, connect))
                        {
                            deleteCmd.Parameters.AddWithValue("@section", sectionName);
                            deleteCmd.Parameters.AddWithValue("@course", courseName);
                            deleteCmd.Parameters.AddWithValue("@teacherEmail", teacherEmail);
                            deleteCmd.Parameters.AddWithValue("@teacherID", teacherID);
                            deleteCmd.ExecuteNonQuery();
                        }

                        // Reset identity only for this teacher's students (optional, can reset globally too)
                        using (SqlCommand resetCmd = new SqlCommand("DBCC CHECKIDENT ('Students', RESEED, 0)", connect))
                        {
                            resetCmd.ExecuteNonQuery();
                        }
                    }

                    // 2️⃣ Insert new students with teacher info
                    foreach (DataRow row in studentTable.Rows)
                    {
                        string insertQuery = "INSERT INTO Students (StudentID, StudentName, Section, Course, TeacherEmail, TeacherID) " +
                                             "VALUES (@id, @name, @section, @course, @teacherEmail, @teacherID)";

                        using (SqlCommand cmd = new SqlCommand(insertQuery, connect))
                        {
                            cmd.Parameters.AddWithValue("@id", row["StudentID"].ToString());
                            cmd.Parameters.AddWithValue("@name", row["StudentName"].ToString());
                            cmd.Parameters.AddWithValue("@section", sectionName);
                            cmd.Parameters.AddWithValue("@course", courseName);
                            cmd.Parameters.AddWithValue("@teacherEmail", teacherEmail);
                            cmd.Parameters.AddWithValue("@teacherID", teacherID);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show($"Successfully saved {studentTable.Rows.Count} student(s) " +
                                    $"for Section: \"{sectionName}\" and Course: \"{courseName}\".",
                                    "Save Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Clear uploaded data
                    studentTable.Rows.Clear();
                    dataGridView1.DataSource = null;
                    dataView.Text = "0 students found";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error saving data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    this.Close(); // Always close Add_Section form after operation
                }
            }

        }


        private void LoadExistingSectionsCourses()
        {
            SectionCombo.Items.Clear();
            courseCombo.Items.Clear();

            string connStr = ConfigurationManager.ConnectionStrings["LanQuizerDB"].ConnectionString;

            using (SqlConnection connect = new SqlConnection(connStr))
            {
                connect.Open();

                string query = "SELECT DISTINCT Section, Course FROM Students";
                using (SqlCommand cmd = new SqlCommand(query, connect))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string section = reader["Section"].ToString();
                        string course = reader["Course"].ToString();

                        if (!SectionCombo.Items.Contains(section))
                            SectionCombo.Items.Add(section);

                        if (!courseCombo.Items.Contains(course))
                            courseCombo.Items.Add(course);
                    }
                }
            }

        }

        private void SectionexitBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
