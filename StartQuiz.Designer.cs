namespace LanQuizer
{
    partial class StartQuiz
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StartQuiz));
            Logo = new Panel();
            disconnectImg = new PictureBox();
            connectImg = new PictureBox();
            connected = new Label();
            minbtn = new Label();
            tag = new Label();
            exitBtn = new Label();
            label1 = new Label();
            pictureBox1 = new PictureBox();
            startLbl = new Label();
            examLbl = new Label();
            timeLbl = new Label();
            tmLbl = new Label();
            featureOn = new Label();
            IpLbl = new Label();
            portLbl = new Label();
            startBtn = new Button();
            refreshIcon = new PictureBox();
            questionLbl = new Label();
            searchExam = new TextBox();
            Logo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)disconnectImg).BeginInit();
            ((System.ComponentModel.ISupportInitialize)connectImg).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)refreshIcon).BeginInit();
            SuspendLayout();
            // 
            // Logo
            // 
            Logo.BackColor = Color.DarkBlue;
            Logo.BackgroundImageLayout = ImageLayout.Zoom;
            Logo.BorderStyle = BorderStyle.FixedSingle;
            Logo.Controls.Add(disconnectImg);
            Logo.Controls.Add(connectImg);
            Logo.Controls.Add(connected);
            Logo.Controls.Add(minbtn);
            Logo.Controls.Add(tag);
            Logo.Controls.Add(exitBtn);
            Logo.Controls.Add(label1);
            Logo.Controls.Add(pictureBox1);
            Logo.Dock = DockStyle.Top;
            Logo.ForeColor = SystemColors.Control;
            Logo.Location = new Point(0, 0);
            Logo.Margin = new Padding(10);
            Logo.Name = "Logo";
            Logo.Padding = new Padding(50);
            Logo.Size = new Size(629, 106);
            Logo.TabIndex = 22;
            Logo.Paint += Logo_Paint;
            // 
            // disconnectImg
            // 
            disconnectImg.Image = (Image)resources.GetObject("disconnectImg.Image");
            disconnectImg.Location = new Point(378, 35);
            disconnectImg.Name = "disconnectImg";
            disconnectImg.Size = new Size(25, 22);
            disconnectImg.SizeMode = PictureBoxSizeMode.Zoom;
            disconnectImg.TabIndex = 29;
            disconnectImg.TabStop = false;
            // 
            // connectImg
            // 
            connectImg.Image = (Image)resources.GetObject("connectImg.Image");
            connectImg.Location = new Point(378, 35);
            connectImg.Name = "connectImg";
            connectImg.Size = new Size(25, 22);
            connectImg.SizeMode = PictureBoxSizeMode.Zoom;
            connectImg.TabIndex = 28;
            connectImg.TabStop = false;
            // 
            // connected
            // 
            connected.AutoSize = true;
            connected.Font = new Font("Times New Roman", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            connected.ForeColor = Color.Gray;
            connected.Location = new Point(401, 35);
            connected.Name = "connected";
            connected.Size = new Size(70, 19);
            connected.TabIndex = 25;
            connected.Text = "Connect";

            // 
            // minbtn
            // 
            minbtn.AutoSize = true;
            minbtn.Font = new Font("ISOCPEUR", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            minbtn.ForeColor = Color.RosyBrown;
            minbtn.Location = new Point(559, 9);
            minbtn.Name = "minbtn";
            minbtn.Size = new Size(22, 26);
            minbtn.TabIndex = 27;
            minbtn.Text = "-";
            // 
            // tag
            // 
            tag.AutoSize = true;
            tag.Font = new Font("Monotype Corsiva", 10.8F, FontStyle.Italic, GraphicsUnit.Point, 0);
            tag.Location = new Point(93, 50);
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
            exitBtn.Location = new Point(591, 11);
            exitBtn.Name = "exitBtn";
            exitBtn.Size = new Size(25, 26);
            exitBtn.TabIndex = 26;
            exitBtn.Text = "X";
            exitBtn.Click += exitBtn_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("BankGothic Lt BT", 22.2F, FontStyle.Italic, GraphicsUnit.Point, 0);
            label1.Location = new Point(88, 11);
            label1.Name = "label1";
            label1.Size = new Size(223, 39);
            label1.TabIndex = 1;
            label1.Text = "LanQuizer";
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(11, 11);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(78, 69);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // startLbl
            // 
            startLbl.AutoSize = true;
            startLbl.Font = new Font("Times New Roman", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            startLbl.ForeColor = Color.FromArgb(0, 0, 192);
            startLbl.Location = new Point(165, 116);
            startLbl.Name = "startLbl";
            startLbl.Size = new Size(296, 25);
            startLbl.TabIndex = 23;
            startLbl.Text = "You are going to start a Quiz";
            startLbl.Click += startLbl_Click;
            // 
            // examLbl
            // 
            examLbl.AutoSize = true;
            examLbl.Font = new Font("Times New Roman", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            examLbl.Location = new Point(43, 175);
            examLbl.Name = "examLbl";
            examLbl.Size = new Size(134, 23);
            examLbl.TabIndex = 24;
            examLbl.Text = "Exam Name  : ";
            // 
            // timeLbl
            // 
            timeLbl.AutoSize = true;
            timeLbl.Font = new Font("Times New Roman", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            timeLbl.Location = new Point(43, 207);
            timeLbl.Name = "timeLbl";
            timeLbl.Size = new Size(131, 23);
            timeLbl.TabIndex = 25;
            timeLbl.Text = "Duration       : ";
            // 
            // tmLbl
            // 
            tmLbl.AutoSize = true;
            tmLbl.Font = new Font("Times New Roman", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            tmLbl.Location = new Point(43, 264);
            tmLbl.Name = "tmLbl";
            tmLbl.Size = new Size(125, 23);
            tmLbl.TabIndex = 26;
            tmLbl.Text = "Total Marks :";
            // 
            // featureOn
            // 
            featureOn.AutoSize = true;
            featureOn.Font = new Font("Times New Roman", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            featureOn.Location = new Point(46, 299);
            featureOn.Name = "featureOn";
            featureOn.Size = new Size(182, 23);
            featureOn.TabIndex = 27;
            featureOn.Text = "No Feature Selected";
            // 
            // IpLbl
            // 
            IpLbl.AutoSize = true;
            IpLbl.Font = new Font("Times New Roman", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            IpLbl.Location = new Point(46, 328);
            IpLbl.Name = "IpLbl";
            IpLbl.Size = new Size(161, 23);
            IpLbl.TabIndex = 28;
            IpLbl.Text = "Your IP Address : ";
            // 
            // portLbl
            // 
            portLbl.AutoSize = true;
            portLbl.Font = new Font("Times New Roman", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            portLbl.Location = new Point(367, 328);
            portLbl.Name = "portLbl";
            portLbl.Size = new Size(59, 23);
            portLbl.TabIndex = 29;
            portLbl.Text = "Port :";
            // 
            // startBtn
            // 
            startBtn.BackColor = Color.FromArgb(192, 255, 192);
            startBtn.Font = new Font("Times New Roman", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            startBtn.ForeColor = SystemColors.ActiveCaptionText;
            startBtn.Location = new Point(239, 377);
            startBtn.Name = "startBtn";
            startBtn.Size = new Size(121, 46);
            startBtn.TabIndex = 30;
            startBtn.Text = "Start Quiz";
            startBtn.UseVisualStyleBackColor = false;
            startBtn.Click += startBtn_Click;
            // 
            // refreshIcon
            // 
            refreshIcon.Image = (Image)resources.GetObject("refreshIcon.Image");
            refreshIcon.Location = new Point(387, 383);
            refreshIcon.Name = "refreshIcon";
            refreshIcon.Size = new Size(26, 33);
            refreshIcon.SizeMode = PictureBoxSizeMode.Zoom;
            refreshIcon.TabIndex = 31;
            refreshIcon.TabStop = false;
            // 
            // questionLbl
            // 
            questionLbl.AutoSize = true;
            questionLbl.Font = new Font("Times New Roman", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            questionLbl.Location = new Point(43, 236);
            questionLbl.Name = "questionLbl";
            questionLbl.Size = new Size(126, 23);
            questionLbl.TabIndex = 32;
            questionLbl.Text = "Questions     :";
            // 
            // searchExam
            // 
            searchExam.Location = new Point(336, 175);
            searchExam.Name = "searchExam";
            searchExam.Size = new Size(254, 27);
            searchExam.TabIndex = 33;
            // 
            // StartQuiz
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(629, 450);
            Controls.Add(searchExam);
            Controls.Add(questionLbl);
            Controls.Add(refreshIcon);
            Controls.Add(startBtn);
            Controls.Add(portLbl);
            Controls.Add(IpLbl);
            Controls.Add(featureOn);
            Controls.Add(tmLbl);
            Controls.Add(timeLbl);
            Controls.Add(examLbl);
            Controls.Add(startLbl);
            Controls.Add(Logo);
            FormBorderStyle = FormBorderStyle.None;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "StartQuiz";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Start Quiz";
            Load += StartQuiz_Load_1;
            Logo.ResumeLayout(false);
            Logo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)disconnectImg).EndInit();
            ((System.ComponentModel.ISupportInitialize)connectImg).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)refreshIcon).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel Logo;
        private Label tag;
        private Label label1;
        private PictureBox pictureBox1;
        private Label minbtn;
        private Label exitBtn;
        private Label connected;
        private Label startLbl;
        private Label examLbl;
        private Label timeLbl;
        private Label tmLbl;
        private Label featureOn;
        private Label IpLbl;
        private Label portLbl;
        private Button startBtn;
        private PictureBox refreshIcon;
        private PictureBox connectImg;
        private PictureBox disconnectImg;
        private Label questionLbl;
        private TextBox searchExam;
    }
}