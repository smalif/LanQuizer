namespace LanQuizer
{
    partial class Teacher_Reg_Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Teacher_Reg_Form));
            Logo = new Panel();
            teacherBtn = new Button();
            label1 = new Label();
            pictureBox1 = new PictureBox();
            pictureBox2 = new PictureBox();
            TeacherpassBox = new TextBox();
            TeacherID = new TextBox();
            checkBox1 = new CheckBox();
            label4 = new Label();
            label3 = new Label();
            teacherEmail = new TextBox();
            label2 = new Label();
            logInBtn = new Button();
            exitBtn = new Label();
            Logo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            SuspendLayout();
            // 
            // Logo
            // 
            Logo.BackColor = Color.DarkBlue;
            Logo.BackgroundImageLayout = ImageLayout.Zoom;
            Logo.BorderStyle = BorderStyle.FixedSingle;
            Logo.Controls.Add(teacherBtn);
            Logo.Controls.Add(label1);
            Logo.Controls.Add(pictureBox1);
            Logo.Dock = DockStyle.Left;
            Logo.ForeColor = SystemColors.Control;
            Logo.Location = new Point(0, 0);
            Logo.Margin = new Padding(10);
            Logo.Name = "Logo";
            Logo.Padding = new Padding(50);
            Logo.Size = new Size(253, 620);
            Logo.TabIndex = 11;
            // 
            // teacherBtn
            // 
            teacherBtn.Font = new Font("Times New Roman", 12F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            teacherBtn.ForeColor = SystemColors.ActiveCaptionText;
            teacherBtn.Location = new Point(42, 543);
            teacherBtn.Name = "teacherBtn";
            teacherBtn.Size = new Size(158, 29);
            teacherBtn.TabIndex = 2;
            teacherBtn.Text = "I'm a Student!";
            teacherBtn.UseVisualStyleBackColor = true;
            teacherBtn.Click += teacherBtn_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("BankGothic Lt BT", 22.2F, FontStyle.Italic, GraphicsUnit.Point, 0);
            label1.Location = new Point(11, 355);
            label1.Name = "label1";
            label1.Size = new Size(223, 39);
            label1.TabIndex = 1;
            label1.Text = "LanQuizer";
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(24, 143);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(200, 200);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            pictureBox2.Image = Properties.Resources.teachers_day;
            pictureBox2.Location = new Point(374, 121);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(143, 124);
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.TabIndex = 20;
            pictureBox2.TabStop = false;
            // 
            // TeacherpassBox
            // 
            TeacherpassBox.Font = new Font("Times New Roman", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            TeacherpassBox.Location = new Point(306, 421);
            TeacherpassBox.Name = "TeacherpassBox";
            TeacherpassBox.PasswordChar = '*';
            TeacherpassBox.PlaceholderText = "Enter Password";
            TeacherpassBox.Size = new Size(289, 34);
            TeacherpassBox.TabIndex = 16;
            TeacherpassBox.TextAlign = HorizontalAlignment.Center;
            TeacherpassBox.TextChanged += TeacherpassBox_TextChanged;
            // 
            // TeacherID
            // 
            TeacherID.BackColor = SystemColors.Window;
            TeacherID.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            TeacherID.Location = new Point(306, 359);
            TeacherID.MaxLength = 12;
            TeacherID.Name = "TeacherID";
            TeacherID.PlaceholderText = "Enter Your ID";
            TeacherID.Size = new Size(289, 30);
            TeacherID.TabIndex = 15;
            TeacherID.TextAlign = HorizontalAlignment.Center;
            TeacherID.WordWrap = false;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(463, 461);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(132, 24);
            checkBox1.TabIndex = 14;
            checkBox1.Text = "Show Password";
            checkBox1.UseVisualStyleBackColor = true;
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Times New Roman", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label4.Location = new Point(306, 392);
            label4.Name = "label4";
            label4.Size = new Size(134, 26);
            label4.TabIndex = 13;
            label4.Text = "PASSWORD";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Times New Roman", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.Location = new Point(306, 330);
            label3.Name = "label3";
            label3.Size = new Size(114, 26);
            label3.TabIndex = 12;
            label3.Text = "Teacher ID";
            // 
            // teacherEmail
            // 
            teacherEmail.BackColor = SystemColors.Window;
            teacherEmail.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            teacherEmail.Location = new Point(306, 297);
            teacherEmail.MaxLength = 100;
            teacherEmail.Name = "teacherEmail";
            teacherEmail.PlaceholderText = "Enter Your Email";
            teacherEmail.Size = new Size(289, 30);
            teacherEmail.TabIndex = 19;
            teacherEmail.TextAlign = HorizontalAlignment.Center;
            teacherEmail.WordWrap = false;
            teacherEmail.TextChanged += teacherEmail_TextChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Times New Roman", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(306, 268);
            label2.Name = "label2";
            label2.Size = new Size(171, 26);
            label2.TabIndex = 18;
            label2.Text = "Enter Your Email";
            // 
            // logInBtn
            // 
            logInBtn.BackColor = Color.SteelBlue;
            logInBtn.BackgroundImageLayout = ImageLayout.Center;
            logInBtn.FlatAppearance.BorderSize = 0;
            logInBtn.FlatStyle = FlatStyle.Flat;
            logInBtn.Font = new Font("Times New Roman", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            logInBtn.ForeColor = SystemColors.ControlLightLight;
            logInBtn.Location = new Point(387, 511);
            logInBtn.Name = "logInBtn";
            logInBtn.Size = new Size(125, 40);
            logInBtn.TabIndex = 17;
            logInBtn.Text = "Log In";
            logInBtn.UseVisualStyleBackColor = false;
            logInBtn.Click += logInBtn_Click;
            // 
            // exitBtn
            // 
            exitBtn.AutoSize = true;
            exitBtn.Font = new Font("ISOCPEUR", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            exitBtn.ForeColor = Color.RosyBrown;
            exitBtn.Location = new Point(604, 9);
            exitBtn.Name = "exitBtn";
            exitBtn.Size = new Size(25, 26);
            exitBtn.TabIndex = 21;
            exitBtn.Text = "X";
            exitBtn.Click += exitBtn_Click;
            // 
            // Teacher_Reg_Form
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(641, 620);
            Controls.Add(exitBtn);
            Controls.Add(Logo);
            Controls.Add(pictureBox2);
            Controls.Add(TeacherpassBox);
            Controls.Add(TeacherID);
            Controls.Add(checkBox1);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(teacherEmail);
            Controls.Add(label2);
            Controls.Add(logInBtn);
            FormBorderStyle = FormBorderStyle.None;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Teacher_Reg_Form";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Teacher_Reg_Form";
            Load += Teacher_Reg_Form_Load;
            Logo.ResumeLayout(false);
            Logo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel Logo;
        private Button teacherBtn;
        private Label label1;
        private PictureBox pictureBox1;
        private PictureBox pictureBox2;
        private TextBox TeacherpassBox;
        private TextBox TeacherID;
        private CheckBox checkBox1;
        private Label label4;
        private Label label3;
        private TextBox teacherEmail;
        private Label label2;
        private Button logInBtn;
        private Label exitBtn;
    }
}