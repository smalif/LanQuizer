using System.Runtime.InteropServices;


namespace LanQuizer
{
    public partial class Student : Form
    {

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
        public Student()
        {
            InitializeComponent();
            ApplyHandCursor(this);
        }

        [DllImport("user32.dll")]
        public static extern void ReleaseCapture();

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HTCAPTION = 0x2;

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void userIdBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void exitBtn_Click(object sender, EventArgs e)
        {
            ApplyHandCursor(this);
            Application.Exit();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void teacherBtn_Click(object sender, EventArgs e)
        {
            ApplyHandCursor(this);
            Teacher_Reg_Form teacher = new Teacher_Reg_Form();
            teacher.Show();
            this.Hide();
        }

        private void showPassChkBox_CheckedChanged(object sender, EventArgs e)
        {
            passBox.PasswordChar = showPassChkBox.Checked ? '\0' : '*';
        }

        private void Student_Click(object sender, EventArgs e) { 
        }
    }
}
