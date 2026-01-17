namespace LanQuizer
{
    partial class NewRegistration_Teacher
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewRegistration_Teacher));
            exitBtn = new Label();
            pictureBox1 = new PictureBox();
            Logo = new Panel();
            BacktoT_log = new Button();
            label1 = new Label();
            Reg_lbl = new Label();
            T_name = new TextBox();
            nameLbl = new Label();
            TID_lbl = new Label();
            email_lbl = new Label();
            newPassLbl = new Label();
            confirmPassLbl = new Label();
            T_ID = new TextBox();
            T_email = new TextBox();
            new_pass = new TextBox();
            confirm_pass = new TextBox();
            showPass = new CheckBox();
            registerBtn = new Button();
            matchWar = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            Logo.SuspendLayout();
            SuspendLayout();
            // 
            // exitBtn
            // 
            exitBtn.AutoSize = true;
            exitBtn.Font = new Font("ISOCPEUR", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            exitBtn.ForeColor = Color.RosyBrown;
            exitBtn.Location = new Point(604, 9);
            exitBtn.Name = "exitBtn";
            exitBtn.Size = new Size(25, 26);
            exitBtn.TabIndex = 23;
            exitBtn.Text = "X";
            exitBtn.Click += exitBtn_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(23, 173);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(200, 200);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            pictureBox1.Click += pictureBox1_Click;
            // 
            // Logo
            // 
            Logo.BackColor = Color.DarkBlue;
            Logo.BackgroundImageLayout = ImageLayout.Zoom;
            Logo.BorderStyle = BorderStyle.FixedSingle;
            Logo.Controls.Add(BacktoT_log);
            Logo.Controls.Add(label1);
            Logo.Controls.Add(pictureBox1);
            Logo.Dock = DockStyle.Left;
            Logo.ForeColor = SystemColors.Control;
            Logo.Location = new Point(0, 0);
            Logo.Margin = new Padding(10);
            Logo.Name = "Logo";
            Logo.Padding = new Padding(50);
            Logo.Size = new Size(253, 620);
            Logo.TabIndex = 22;
            // 
            // BacktoT_log
            // 
            BacktoT_log.Font = new Font("Times New Roman", 12F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            BacktoT_log.ForeColor = SystemColors.ActiveCaptionText;
            BacktoT_log.Location = new Point(40, 554);
            BacktoT_log.Name = "BacktoT_log";
            BacktoT_log.Size = new Size(158, 29);
            BacktoT_log.TabIndex = 2;
            BacktoT_log.Text = "Back To Login";
            BacktoT_log.UseVisualStyleBackColor = true;
            BacktoT_log.Click += BacktoT_log_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("BankGothic Lt BT", 22.2F, FontStyle.Italic, GraphicsUnit.Point, 0);
            label1.Location = new Point(11, 376);
            label1.Name = "label1";
            label1.Size = new Size(223, 39);
            label1.TabIndex = 1;
            label1.Text = "LanQuizer";
            label1.Click += label1_Click;
            // 
            // Reg_lbl
            // 
            Reg_lbl.AutoSize = true;
            Reg_lbl.Font = new Font("Times New Roman", 19.8000011F, FontStyle.Bold, GraphicsUnit.Point, 0);
            Reg_lbl.Location = new Point(358, 67);
            Reg_lbl.Name = "Reg_lbl";
            Reg_lbl.Size = new Size(194, 38);
            Reg_lbl.TabIndex = 24;
            Reg_lbl.Text = "REGISTER";
            Reg_lbl.TextAlign = ContentAlignment.TopCenter;
            // 
            // T_name
            // 
            T_name.Location = new Point(266, 154);
            T_name.Name = "T_name";
            T_name.PlaceholderText = "Enter your full name";
            T_name.Size = new Size(363, 27);
            T_name.TabIndex = 25;
            T_name.TextAlign = HorizontalAlignment.Center;
            // 
            // nameLbl
            // 
            nameLbl.AutoSize = true;
            nameLbl.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            nameLbl.Location = new Point(266, 129);
            nameLbl.Name = "nameLbl";
            nameLbl.Size = new Size(182, 22);
            nameLbl.TabIndex = 27;
            nameLbl.Text = "Enter Your Full Name";
            nameLbl.TextAlign = ContentAlignment.TopCenter;
            // 
            // TID_lbl
            // 
            TID_lbl.AutoSize = true;
            TID_lbl.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            TID_lbl.Location = new Point(266, 192);
            TID_lbl.Name = "TID_lbl";
            TID_lbl.Size = new Size(187, 22);
            TID_lbl.TabIndex = 28;
            TID_lbl.Text = "Enter Your Teacher ID";
            TID_lbl.TextAlign = ContentAlignment.TopCenter;
            // 
            // email_lbl
            // 
            email_lbl.AutoSize = true;
            email_lbl.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            email_lbl.Location = new Point(266, 252);
            email_lbl.Name = "email_lbl";
            email_lbl.Size = new Size(146, 22);
            email_lbl.TabIndex = 29;
            email_lbl.Text = "Enter Your Email";
            email_lbl.TextAlign = ContentAlignment.TopCenter;
            // 
            // newPassLbl
            // 
            newPassLbl.AutoSize = true;
            newPassLbl.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            newPassLbl.Location = new Point(266, 314);
            newPassLbl.Name = "newPassLbl";
            newPassLbl.Size = new Size(178, 22);
            newPassLbl.TabIndex = 30;
            newPassLbl.Text = "Enter New Password";
            newPassLbl.TextAlign = ContentAlignment.TopCenter;
            // 
            // confirmPassLbl
            // 
            confirmPassLbl.AutoSize = true;
            confirmPassLbl.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            confirmPassLbl.Location = new Point(266, 376);
            confirmPassLbl.Name = "confirmPassLbl";
            confirmPassLbl.Size = new Size(158, 22);
            confirmPassLbl.TabIndex = 31;
            confirmPassLbl.Text = "Confirm Password";
            confirmPassLbl.TextAlign = ContentAlignment.TopCenter;
            // 
            // T_ID
            // 
            T_ID.Location = new Point(266, 217);
            T_ID.Name = "T_ID";
            T_ID.PlaceholderText = "Enter your teacher ID";
            T_ID.Size = new Size(363, 27);
            T_ID.TabIndex = 32;
            T_ID.TextAlign = HorizontalAlignment.Center;
            // 
            // T_email
            // 
            T_email.Location = new Point(266, 277);
            T_email.Name = "T_email";
            T_email.PlaceholderText = "Enter your email";
            T_email.Size = new Size(363, 27);
            T_email.TabIndex = 33;
            T_email.TextAlign = HorizontalAlignment.Center;
            // 
            // new_pass
            // 
            new_pass.Location = new Point(266, 339);
            new_pass.Name = "new_pass";
            new_pass.PasswordChar = '*';
            new_pass.PlaceholderText = "Enter New Password";
            new_pass.Size = new Size(363, 27);
            new_pass.TabIndex = 34;
            new_pass.TextAlign = HorizontalAlignment.Center;
            // 
            // confirm_pass
            // 
            confirm_pass.Location = new Point(266, 401);
            confirm_pass.Name = "confirm_pass";
            confirm_pass.PasswordChar = '*';
            confirm_pass.PlaceholderText = "Confirm Password";
            confirm_pass.Size = new Size(363, 27);
            confirm_pass.TabIndex = 35;
            confirm_pass.TextAlign = HorizontalAlignment.Center;
            // 
            // showPass
            // 
            showPass.AutoSize = true;
            showPass.Location = new Point(497, 438);
            showPass.Name = "showPass";
            showPass.Size = new Size(132, 24);
            showPass.TabIndex = 36;
            showPass.Text = "Show Password";
            showPass.UseVisualStyleBackColor = true;
            showPass.CheckedChanged += showPass_CheckedChanged;
            // 
            // registerBtn
            // 
            registerBtn.BackColor = Color.SteelBlue;
            registerBtn.BackgroundImageLayout = ImageLayout.Center;
            registerBtn.FlatAppearance.BorderSize = 0;
            registerBtn.FlatStyle = FlatStyle.Flat;
            registerBtn.Font = new Font("Times New Roman", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            registerBtn.ForeColor = SystemColors.ControlLightLight;
            registerBtn.Location = new Point(387, 488);
            registerBtn.Name = "registerBtn";
            registerBtn.Size = new Size(125, 40);
            registerBtn.TabIndex = 37;
            registerBtn.Text = "Register";
            registerBtn.UseVisualStyleBackColor = false;
            registerBtn.Click += registerBtn_Click;
            // 
            // matchWar
            // 
            matchWar.AutoSize = true;
            matchWar.ForeColor = Color.Red;
            matchWar.Location = new Point(360, 462);
            matchWar.Name = "matchWar";
            matchWar.Size = new Size(177, 20);
            matchWar.TabIndex = 38;
            matchWar.Text = "Password doesnot Match!";
            // 
            // NewRegistration_Teacher
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(641, 620);
            Controls.Add(matchWar);
            Controls.Add(registerBtn);
            Controls.Add(showPass);
            Controls.Add(confirm_pass);
            Controls.Add(new_pass);
            Controls.Add(T_email);
            Controls.Add(T_ID);
            Controls.Add(confirmPassLbl);
            Controls.Add(newPassLbl);
            Controls.Add(email_lbl);
            Controls.Add(TID_lbl);
            Controls.Add(nameLbl);
            Controls.Add(T_name);
            Controls.Add(Reg_lbl);
            Controls.Add(exitBtn);
            Controls.Add(Logo);
            FormBorderStyle = FormBorderStyle.None;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "NewRegistration_Teacher";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "NewRegistration_Teacher";
            Load += NewRegistration_Teacher_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            Logo.ResumeLayout(false);
            Logo.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label exitBtn;
        private PictureBox pictureBox1;
        private Panel Logo;
        private Button BacktoT_log;
        private Label label1;
        private Label Reg_lbl;
        private TextBox T_name;
        private Button button1;
        private Label nameLbl;
        private Label TID_lbl;
        private Label email_lbl;
        private Label newPassLbl;
        private Label confirmPassLbl;
        private TextBox T_ID;
        private TextBox T_email;
        private TextBox new_pass;
        private TextBox confirm_pass;
        private CheckBox showPass;
        private Button registerBtn;
        private Label matchWar;
    }
}