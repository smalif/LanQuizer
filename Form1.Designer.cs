namespace LanQuizer
{
    partial class Student
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Student));
            Logo = new Panel();
            teacherBtn = new Button();
            label1 = new Label();
            pictureBox1 = new PictureBox();
            exitBtn = new Label();
            label3 = new Label();
            label4 = new Label();
            showPassChkBox = new CheckBox();
            userIdBox = new TextBox();
            passBox = new TextBox();
            logInBtn = new Button();
            label2 = new Label();
            textBox1 = new TextBox();
            pictureBox2 = new PictureBox();
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
            Logo.Size = new Size(253, 600);
            Logo.TabIndex = 0;
            // 
            // teacherBtn
            // 
            teacherBtn.Font = new Font("Times New Roman", 12F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            teacherBtn.ForeColor = SystemColors.ActiveCaptionText;
            teacherBtn.Location = new Point(42, 543);
            teacherBtn.Name = "teacherBtn";
            teacherBtn.Size = new Size(158, 29);
            teacherBtn.TabIndex = 2;
            teacherBtn.Text = "I'm a Teacher!";
            teacherBtn.UseVisualStyleBackColor = true;
            teacherBtn.Click += teacherBtn_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("BankGothic Lt BT", 22.2F, FontStyle.Italic, GraphicsUnit.Point, 0);
            label1.Location = new Point(11, 346);
            label1.Name = "label1";
            label1.Size = new Size(223, 39);
            label1.TabIndex = 1;
            label1.Text = "LanQuizer";
            label1.Click += label1_Click;
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
            pictureBox1.Click += pictureBox1_Click;
            // 
            // exitBtn
            // 
            exitBtn.AutoSize = true;
            exitBtn.Font = new Font("ISOCPEUR", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            exitBtn.ForeColor = Color.RosyBrown;
            exitBtn.Location = new Point(562, 9);
            exitBtn.Name = "exitBtn";
            exitBtn.Size = new Size(25, 26);
            exitBtn.TabIndex = 1;
            exitBtn.Text = "X";
            exitBtn.Click += exitBtn_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Times New Roman", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.Location = new Point(286, 307);
            label3.Name = "label3";
            label3.Size = new Size(36, 26);
            label3.TabIndex = 2;
            label3.Text = "ID";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Times New Roman", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label4.Location = new Point(286, 369);
            label4.Name = "label4";
            label4.Size = new Size(134, 26);
            label4.TabIndex = 3;
            label4.Text = "PASSWORD";
            // 
            // showPassChkBox
            // 
            showPassChkBox.AutoSize = true;
            showPassChkBox.Location = new Point(443, 438);
            showPassChkBox.Name = "showPassChkBox";
            showPassChkBox.Size = new Size(132, 24);
            showPassChkBox.TabIndex = 4;
            showPassChkBox.Text = "Show Password";
            showPassChkBox.UseVisualStyleBackColor = true;
            showPassChkBox.CheckedChanged += showPassChkBox_CheckedChanged;
            // 
            // userIdBox
            // 
            userIdBox.BackColor = SystemColors.Window;
            userIdBox.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            userIdBox.Location = new Point(286, 336);
            userIdBox.MaxLength = 10;
            userIdBox.Name = "userIdBox";
            userIdBox.PlaceholderText = "Enter Your 10 Digit ID";
            userIdBox.Size = new Size(289, 30);
            userIdBox.TabIndex = 5;
            userIdBox.TextAlign = HorizontalAlignment.Center;
            userIdBox.WordWrap = false;
            userIdBox.TextChanged += userIdBox_TextChanged;
            // 
            // passBox
            // 
            passBox.Font = new Font("Times New Roman", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            passBox.Location = new Point(286, 398);
            passBox.Name = "passBox";
            passBox.PasswordChar = '*';
            passBox.PlaceholderText = "Enter Password";
            passBox.Size = new Size(289, 34);
            passBox.TabIndex = 6;
            passBox.TextAlign = HorizontalAlignment.Center;
            // 
            // logInBtn
            // 
            logInBtn.BackColor = Color.SteelBlue;
            logInBtn.BackgroundImageLayout = ImageLayout.Center;
            logInBtn.FlatAppearance.BorderSize = 0;
            logInBtn.FlatStyle = FlatStyle.Flat;
            logInBtn.Font = new Font("Times New Roman", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            logInBtn.ForeColor = SystemColors.ControlLightLight;
            logInBtn.Location = new Point(360, 481);
            logInBtn.Name = "logInBtn";
            logInBtn.Size = new Size(125, 40);
            logInBtn.TabIndex = 7;
            logInBtn.Text = "Start Quiz";
            logInBtn.UseVisualStyleBackColor = false;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Times New Roman", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(286, 245);
            label2.Name = "label2";
            label2.Size = new Size(168, 26);
            label2.TabIndex = 8;
            label2.Text = "Enter IP Address";
            // 
            // textBox1
            // 
            textBox1.BackColor = SystemColors.Window;
            textBox1.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            textBox1.Location = new Point(286, 274);
            textBox1.MaxLength = 10;
            textBox1.Name = "textBox1";
            textBox1.PlaceholderText = "Enter IP Address";
            textBox1.Size = new Size(289, 30);
            textBox1.TabIndex = 9;
            textBox1.TextAlign = HorizontalAlignment.Center;
            textBox1.WordWrap = false;
            // 
            // pictureBox2
            // 
            pictureBox2.Image = Properties.Resources.graduation;
            pictureBox2.Location = new Point(360, 99);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(143, 124);
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.TabIndex = 10;
            pictureBox2.TabStop = false;
            // 
            // Student
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(600, 600);
            Controls.Add(pictureBox2);
            Controls.Add(textBox1);
            Controls.Add(label2);
            Controls.Add(logInBtn);
            Controls.Add(passBox);
            Controls.Add(userIdBox);
            Controls.Add(showPassChkBox);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(exitBtn);
            Controls.Add(Logo);
            FormBorderStyle = FormBorderStyle.None;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Student";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Student-login";
            Load += Form1_Load;
            Click += Student_Click;
            Move += Form1_Load;
            Logo.ResumeLayout(false);
            Logo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel Logo;
        private PictureBox pictureBox1;
        private Label label1;
        private Label exitBtn;
        private Label label3;
        private Label label4;
        private CheckBox showPassChkBox;
        private TextBox userIdBox;
        private TextBox passBox;
        private Button logInBtn;
        private Label label2;
        private TextBox textBox1;
        private Button teacherBtn;
        private PictureBox pictureBox2;
    }
}
