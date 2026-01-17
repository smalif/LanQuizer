namespace LanQuizer
{
    partial class Settings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Settings));
            Logo = new Panel();
            minbtn = new Label();
            tag = new Label();
            exitBtn = new Label();
            label1 = new Label();
            cngPassLbl = new Label();
            oldLbl = new Label();
            NewPassLbl = new Label();
            confirmPassLbl = new Label();
            oldPassBox = new TextBox();
            NewPassBox = new TextBox();
            confirmBox = new TextBox();
            Viewold = new PictureBox();
            viewNew = new PictureBox();
            viewCon = new PictureBox();
            PassCon = new Button();
            matchWar = new Label();
            Logo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)Viewold).BeginInit();
            ((System.ComponentModel.ISupportInitialize)viewNew).BeginInit();
            ((System.ComponentModel.ISupportInitialize)viewCon).BeginInit();
            SuspendLayout();
            // 
            // Logo
            // 
            Logo.BackColor = Color.DarkBlue;
            Logo.BackgroundImageLayout = ImageLayout.Zoom;
            Logo.BorderStyle = BorderStyle.FixedSingle;
            Logo.Controls.Add(minbtn);
            Logo.Controls.Add(tag);
            Logo.Controls.Add(exitBtn);
            Logo.Controls.Add(label1);
            Logo.Dock = DockStyle.Top;
            Logo.ForeColor = SystemColors.Control;
            Logo.Location = new Point(0, 0);
            Logo.Margin = new Padding(10);
            Logo.Name = "Logo";
            Logo.Padding = new Padding(50);
            Logo.Size = new Size(429, 93);
            Logo.TabIndex = 2;
            // 
            // minbtn
            // 
            minbtn.AutoSize = true;
            minbtn.Font = new Font("ISOCPEUR", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            minbtn.ForeColor = Color.RosyBrown;
            minbtn.Location = new Point(354, 12);
            minbtn.Name = "minbtn";
            minbtn.Size = new Size(22, 26);
            minbtn.TabIndex = 24;
            minbtn.Text = "-";
            minbtn.Click += minbtn_Click;
            // 
            // tag
            // 
            tag.AutoSize = true;
            tag.Font = new Font("Monotype Corsiva", 10.8F, FontStyle.Italic, GraphicsUnit.Point, 0);
            tag.Location = new Point(11, 51);
            tag.Name = "tag";
            tag.Size = new Size(266, 21);
            tag.TabIndex = 4;
            tag.Text = "A Lan based Quiz Management System";
            // 
            // exitBtn
            // 
            exitBtn.AutoSize = true;
            exitBtn.Font = new Font("ISOCPEUR", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            exitBtn.ForeColor = Color.RosyBrown;
            exitBtn.Location = new Point(388, 14);
            exitBtn.Name = "exitBtn";
            exitBtn.Size = new Size(25, 26);
            exitBtn.TabIndex = 23;
            exitBtn.Text = "X";
            exitBtn.Click += exitBtn_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("BankGothic Lt BT", 22.2F, FontStyle.Italic, GraphicsUnit.Point, 0);
            label1.Location = new Point(7, 12);
            label1.Name = "label1";
            label1.Size = new Size(223, 39);
            label1.TabIndex = 1;
            label1.Text = "LanQuizer";
            // 
            // cngPassLbl
            // 
            cngPassLbl.AutoSize = true;
            cngPassLbl.Font = new Font("Times New Roman", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            cngPassLbl.Location = new Point(12, 103);
            cngPassLbl.Name = "cngPassLbl";
            cngPassLbl.Size = new Size(211, 23);
            cngPassLbl.TabIndex = 3;
            cngPassLbl.Text = "CHANGE PASSWORD";
            // 
            // oldLbl
            // 
            oldLbl.AutoSize = true;
            oldLbl.Location = new Point(12, 142);
            oldLbl.Name = "oldLbl";
            oldLbl.Size = new Size(136, 20);
            oldLbl.TabIndex = 4;
            oldLbl.Text = "Enter Old Password";
            // 
            // NewPassLbl
            // 
            NewPassLbl.AutoSize = true;
            NewPassLbl.Location = new Point(12, 176);
            NewPassLbl.Name = "NewPassLbl";
            NewPassLbl.Size = new Size(142, 20);
            NewPassLbl.TabIndex = 5;
            NewPassLbl.Text = "Enter New Password";
            // 
            // confirmPassLbl
            // 
            confirmPassLbl.AutoSize = true;
            confirmPassLbl.Location = new Point(12, 209);
            confirmPassLbl.Name = "confirmPassLbl";
            confirmPassLbl.Size = new Size(161, 20);
            confirmPassLbl.TabIndex = 6;
            confirmPassLbl.Text = "Confirm New Password";
            // 
            // oldPassBox
            // 
            oldPassBox.Location = new Point(181, 135);
            oldPassBox.Name = "oldPassBox";
            oldPassBox.PlaceholderText = "Enter Old Password";
            oldPassBox.Size = new Size(191, 27);
            oldPassBox.TabIndex = 7;
            oldPassBox.TextAlign = HorizontalAlignment.Center;
            oldPassBox.UseSystemPasswordChar = true;
            // 
            // NewPassBox
            // 
            NewPassBox.Location = new Point(181, 169);
            NewPassBox.Name = "NewPassBox";
            NewPassBox.PlaceholderText = "Enter New Password";
            NewPassBox.Size = new Size(191, 27);
            NewPassBox.TabIndex = 8;
            NewPassBox.TextAlign = HorizontalAlignment.Center;
            NewPassBox.UseSystemPasswordChar = true;
            // 
            // confirmBox
            // 
            confirmBox.Location = new Point(181, 202);
            confirmBox.Name = "confirmBox";
            confirmBox.PlaceholderText = "Confirm New Password";
            confirmBox.Size = new Size(191, 27);
            confirmBox.TabIndex = 9;
            confirmBox.TextAlign = HorizontalAlignment.Center;
            confirmBox.UseSystemPasswordChar = true;
            // 
            // Viewold
            // 
            Viewold.Image = (Image)resources.GetObject("Viewold.Image");
            Viewold.Location = new Point(378, 137);
            Viewold.Name = "Viewold";
            Viewold.Size = new Size(36, 21);
            Viewold.SizeMode = PictureBoxSizeMode.Zoom;
            Viewold.TabIndex = 10;
            Viewold.TabStop = false;
            Viewold.Click += Viewold_Click;
            // 
            // viewNew
            // 
            viewNew.Image = (Image)resources.GetObject("viewNew.Image");
            viewNew.Location = new Point(378, 172);
            viewNew.Name = "viewNew";
            viewNew.Size = new Size(36, 21);
            viewNew.SizeMode = PictureBoxSizeMode.Zoom;
            viewNew.TabIndex = 11;
            viewNew.TabStop = false;
            viewNew.Click += viewNew_Click;
            // 
            // viewCon
            // 
            viewCon.Image = (Image)resources.GetObject("viewCon.Image");
            viewCon.Location = new Point(378, 207);
            viewCon.Name = "viewCon";
            viewCon.Size = new Size(36, 21);
            viewCon.SizeMode = PictureBoxSizeMode.Zoom;
            viewCon.TabIndex = 12;
            viewCon.TabStop = false;
            viewCon.Click += viewCon_Click;
            // 
            // PassCon
            // 
            PassCon.BackColor = Color.FromArgb(0, 64, 64);
            PassCon.Font = new Font("Times New Roman", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            PassCon.ForeColor = SystemColors.Control;
            PassCon.Location = new Point(139, 271);
            PassCon.Name = "PassCon";
            PassCon.Size = new Size(122, 41);
            PassCon.TabIndex = 13;
            PassCon.Text = "CONFIRM";
            PassCon.UseVisualStyleBackColor = false;
            PassCon.Click += PassCon_Click;
            // 
            // matchWar
            // 
            matchWar.AutoSize = true;
            matchWar.ForeColor = Color.Red;
            matchWar.Location = new Point(114, 242);
            matchWar.Name = "matchWar";
            matchWar.Size = new Size(177, 20);
            matchWar.TabIndex = 14;
            matchWar.Text = "Password doesnot Match!";
            // 
            // Settings
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.LightGray;
            ClientSize = new Size(429, 334);
            Controls.Add(matchWar);
            Controls.Add(PassCon);
            Controls.Add(viewCon);
            Controls.Add(viewNew);
            Controls.Add(Viewold);
            Controls.Add(confirmBox);
            Controls.Add(NewPassBox);
            Controls.Add(oldPassBox);
            Controls.Add(confirmPassLbl);
            Controls.Add(NewPassLbl);
            Controls.Add(oldLbl);
            Controls.Add(cngPassLbl);
            Controls.Add(Logo);
            FormBorderStyle = FormBorderStyle.None;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Settings";
            StartPosition = FormStartPosition.CenterScreen;
            Logo.ResumeLayout(false);
            Logo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)Viewold).EndInit();
            ((System.ComponentModel.ISupportInitialize)viewNew).EndInit();
            ((System.ComponentModel.ISupportInitialize)viewCon).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel Logo;
        private Label tag;
        private Label label1;
        private Label minbtn;
        private Label exitBtn;
        private Label cngPassLbl;
        private Label oldLbl;
        private Label NewPassLbl;
        private Label confirmPassLbl;
        private TextBox oldPassBox;
        private TextBox NewPassBox;
        private TextBox confirmBox;
        private PictureBox Viewold;
        private PictureBox viewNew;
        private PictureBox viewCon;
        private Button PassCon;
        private Label matchWar;
    }
}