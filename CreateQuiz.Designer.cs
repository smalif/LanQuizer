namespace LanQuizer
{
    partial class CreateQuiz
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CreateQuiz));
            groupBox3 = new GroupBox();
            questionPanel = new Panel();
            questionBtn = new Button();
            myQuizBtn = new Button();
            Logo = new Panel();
            backbtn = new PictureBox();
            minbtn = new Label();
            label2 = new Label();
            exitBtn = new Label();
            tag = new Label();
            WelcomeTeacher = new Label();
            teacherBtn = new Button();
            label1 = new Label();
            pictureBox1 = new PictureBox();
            questionBox = new GroupBox();
            txtRandomCount = new TextBox();
            chkShowResult = new CheckBox();
            chkRanOption = new CheckBox();
            chkRanQuestion = new CheckBox();
            pictureBox5 = new PictureBox();
            saveDraft = new Button();
            pictureBox3 = new PictureBox();
            button1 = new Button();
            passbox = new TextBox();
            label17 = new Label();
            pictureBox4 = new PictureBox();
            label18 = new Label();
            label19 = new Label();
            label20 = new Label();
            label21 = new Label();
            label22 = new Label();
            label23 = new Label();
            label24 = new Label();
            markBox = new TextBox();
            label25 = new Label();
            durationbox = new TextBox();
            label27 = new Label();
            examNamebox = new TextBox();
            label28 = new Label();
            groupBox3.SuspendLayout();
            Logo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)backbtn).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            questionBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox5).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).BeginInit();
            SuspendLayout();
            // 
            // groupBox3
            // 
            groupBox3.BackColor = SystemColors.ButtonFace;
            groupBox3.Controls.Add(questionPanel);
            groupBox3.Controls.Add(questionBtn);
            groupBox3.Controls.Add(myQuizBtn);
            groupBox3.Location = new Point(12, 149);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(229, 61);
            groupBox3.TabIndex = 22;
            groupBox3.TabStop = false;
            // 
            // questionPanel
            // 
            questionPanel.AutoScroll = true;
            questionPanel.Location = new Point(0, 57);
            questionPanel.Name = "questionPanel";
            questionPanel.Size = new Size(1238, 567);
            questionPanel.TabIndex = 24;
            // 
            // questionBtn
            // 
            questionBtn.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            questionBtn.Location = new Point(127, 21);
            questionBtn.Name = "questionBtn";
            questionBtn.Size = new Size(96, 28);
            questionBtn.TabIndex = 0;
            questionBtn.Text = "Questions";
            questionBtn.Click += questionBtn_Click_1;
            // 
            // myQuizBtn
            // 
            myQuizBtn.BackColor = Color.SeaGreen;
            myQuizBtn.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            myQuizBtn.ForeColor = SystemColors.ButtonHighlight;
            myQuizBtn.Location = new Point(9, 19);
            myQuizBtn.Name = "myQuizBtn";
            myQuizBtn.Size = new Size(111, 32);
            myQuizBtn.TabIndex = 21;
            myQuizBtn.Text = "Settings";
            myQuizBtn.UseVisualStyleBackColor = false;
            myQuizBtn.Click += myQuizBtn_Click;
            // 
            // Logo
            // 
            Logo.BackColor = Color.DarkBlue;
            Logo.BackgroundImageLayout = ImageLayout.Zoom;
            Logo.BorderStyle = BorderStyle.FixedSingle;
            Logo.Controls.Add(backbtn);
            Logo.Controls.Add(minbtn);
            Logo.Controls.Add(label2);
            Logo.Controls.Add(exitBtn);
            Logo.Controls.Add(tag);
            Logo.Controls.Add(WelcomeTeacher);
            Logo.Controls.Add(teacherBtn);
            Logo.Controls.Add(label1);
            Logo.Controls.Add(pictureBox1);
            Logo.Dock = DockStyle.Top;
            Logo.ForeColor = SystemColors.Control;
            Logo.Location = new Point(0, 0);
            Logo.Margin = new Padding(10);
            Logo.Name = "Logo";
            Logo.Padding = new Padding(50);
            Logo.Size = new Size(1262, 144);
            Logo.TabIndex = 21;
            // 
            // backbtn
            // 
            backbtn.Image = (Image)resources.GetObject("backbtn.Image");
            backbtn.Location = new Point(1110, 53);
            backbtn.Name = "backbtn";
            backbtn.Size = new Size(50, 44);
            backbtn.SizeMode = PictureBoxSizeMode.Zoom;
            backbtn.TabIndex = 25;
            backbtn.TabStop = false;
            backbtn.Click += backbtn_Click;
            // 
            // minbtn
            // 
            minbtn.AutoSize = true;
            minbtn.Font = new Font("ISOCPEUR", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            minbtn.ForeColor = Color.RosyBrown;
            minbtn.Location = new Point(1180, 19);
            minbtn.Name = "minbtn";
            minbtn.Size = new Size(22, 26);
            minbtn.TabIndex = 25;
            minbtn.Text = "-";
            minbtn.Click += minbtn_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("BankGothic Lt BT", 22.2F, FontStyle.Italic, GraphicsUnit.Point, 0);
            label2.Location = new Point(138, 89);
            label2.Name = "label2";
            label2.Size = new Size(349, 39);
            label2.TabIndex = 5;
            label2.Text = "Create New Quiz";
            // 
            // exitBtn
            // 
            exitBtn.AutoSize = true;
            exitBtn.Font = new Font("ISOCPEUR", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            exitBtn.ForeColor = Color.RosyBrown;
            exitBtn.Location = new Point(1212, 21);
            exitBtn.Name = "exitBtn";
            exitBtn.Size = new Size(25, 26);
            exitBtn.TabIndex = 24;
            exitBtn.Text = "X";
            exitBtn.Click += exitBtn_Click;
            // 
            // tag
            // 
            tag.AutoSize = true;
            tag.Font = new Font("Monotype Corsiva", 10.8F, FontStyle.Italic, GraphicsUnit.Point, 0);
            tag.Location = new Point(138, 50);
            tag.Name = "tag";
            tag.Size = new Size(266, 21);
            tag.TabIndex = 4;
            tag.Text = "A Lan based Quiz Management System";
            // 
            // WelcomeTeacher
            // 
            WelcomeTeacher.AutoSize = true;
            WelcomeTeacher.Font = new Font("Times New Roman", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            WelcomeTeacher.Location = new Point(185, 148);
            WelcomeTeacher.Name = "WelcomeTeacher";
            WelcomeTeacher.Size = new Size(118, 20);
            WelcomeTeacher.TabIndex = 3;
            WelcomeTeacher.Text = "Welcome Back";
            // 
            // teacherBtn
            // 
            teacherBtn.Font = new Font("Times New Roman", 12F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            teacherBtn.ForeColor = SystemColors.ActiveCaptionText;
            teacherBtn.Location = new Point(42, 551);
            teacherBtn.Name = "teacherBtn";
            teacherBtn.Size = new Size(158, 29);
            teacherBtn.TabIndex = 2;
            teacherBtn.Text = "I'm a Teacher!";
            teacherBtn.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("BankGothic Lt BT", 22.2F, FontStyle.Italic, GraphicsUnit.Point, 0);
            label1.Location = new Point(135, 11);
            label1.Name = "label1";
            label1.Size = new Size(223, 39);
            label1.TabIndex = 1;
            label1.Text = "LanQuizer";
            label1.Click += label1_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(11, 11);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(118, 117);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            pictureBox1.Click += pictureBox1_Click_1;
            // 
            // questionBox
            // 
            questionBox.Controls.Add(txtRandomCount);
            questionBox.Controls.Add(chkShowResult);
            questionBox.Controls.Add(chkRanOption);
            questionBox.Controls.Add(chkRanQuestion);
            questionBox.Controls.Add(pictureBox5);
            questionBox.Controls.Add(saveDraft);
            questionBox.Controls.Add(pictureBox3);
            questionBox.Controls.Add(button1);
            questionBox.Controls.Add(passbox);
            questionBox.Controls.Add(label17);
            questionBox.Controls.Add(pictureBox4);
            questionBox.Controls.Add(label18);
            questionBox.Controls.Add(label19);
            questionBox.Controls.Add(label20);
            questionBox.Controls.Add(label21);
            questionBox.Controls.Add(label22);
            questionBox.Controls.Add(label23);
            questionBox.Controls.Add(label24);
            questionBox.Controls.Add(markBox);
            questionBox.Controls.Add(label25);
            questionBox.Controls.Add(durationbox);
            questionBox.Controls.Add(label27);
            questionBox.Controls.Add(examNamebox);
            questionBox.Controls.Add(label28);
            questionBox.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            questionBox.Location = new Point(12, 216);
            questionBox.Name = "questionBox";
            questionBox.Size = new Size(1238, 537);
            questionBox.TabIndex = 24;
            questionBox.TabStop = false;
            questionBox.Text = "Question Setting";
            // 
            // txtRandomCount
            // 
            txtRandomCount.Location = new Point(344, 201);
            txtRandomCount.Name = "txtRandomCount";
            txtRandomCount.Size = new Size(125, 31);
            txtRandomCount.TabIndex = 29;
            txtRandomCount.Visible = false;
            // 
            // chkShowResult
            // 
            chkShowResult.AutoSize = true;
            chkShowResult.Location = new Point(33, 313);
            chkShowResult.Name = "chkShowResult";
            chkShowResult.Size = new Size(18, 17);
            chkShowResult.TabIndex = 28;
            chkShowResult.UseVisualStyleBackColor = true;
            // 
            // chkRanOption
            // 
            chkRanOption.AutoSize = true;
            chkRanOption.Location = new Point(33, 261);
            chkRanOption.Name = "chkRanOption";
            chkRanOption.Size = new Size(18, 17);
            chkRanOption.TabIndex = 27;
            chkRanOption.UseVisualStyleBackColor = true;
            // 
            // chkRanQuestion
            // 
            chkRanQuestion.AutoSize = true;
            chkRanQuestion.Location = new Point(33, 207);
            chkRanQuestion.Name = "chkRanQuestion";
            chkRanQuestion.Size = new Size(18, 17);
            chkRanQuestion.TabIndex = 26;
            chkRanQuestion.UseVisualStyleBackColor = true;
            // 
            // pictureBox5
            // 
            pictureBox5.Image = (Image)resources.GetObject("pictureBox5.Image");
            pictureBox5.Location = new Point(700, 478);
            pictureBox5.Name = "pictureBox5";
            pictureBox5.Size = new Size(26, 33);
            pictureBox5.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox5.TabIndex = 25;
            pictureBox5.TabStop = false;
            // 
            // saveDraft
            // 
            saveDraft.Font = new Font("Times New Roman", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            saveDraft.Location = new Point(689, 476);
            saveDraft.Name = "saveDraft";
            saveDraft.Size = new Size(177, 38);
            saveDraft.TabIndex = 24;
            saveDraft.Text = "Save Draft";
            saveDraft.UseVisualStyleBackColor = true;
            // 
            // pictureBox3
            // 
            pictureBox3.Image = (Image)resources.GetObject("pictureBox3.Image");
            pictureBox3.Location = new Point(443, 478);
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Size = new Size(26, 33);
            pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox3.TabIndex = 23;
            pictureBox3.TabStop = false;
            pictureBox3.Click += pictureBox3_Click;
            // 
            // button1
            // 
            button1.Font = new Font("Times New Roman", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button1.Location = new Point(430, 476);
            button1.Name = "button1";
            button1.Size = new Size(177, 38);
            button1.TabIndex = 22;
            button1.Text = "Start Quiz";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // passbox
            // 
            passbox.Location = new Point(9, 430);
            passbox.Name = "passbox";
            passbox.PlaceholderText = "Enter Password";
            passbox.Size = new Size(1217, 31);
            passbox.TabIndex = 19;
            // 
            // label17
            // 
            label17.AutoSize = true;
            label17.Font = new Font("Segoe UI Semibold", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label17.Location = new Point(9, 402);
            label17.Name = "label17";
            label17.Size = new Size(141, 25);
            label17.TabIndex = 18;
            label17.Text = "Quiz Password*";
            // 
            // pictureBox4
            // 
            pictureBox4.Image = (Image)resources.GetObject("pictureBox4.Image");
            pictureBox4.Location = new Point(12, 358);
            pictureBox4.Name = "pictureBox4";
            pictureBox4.Size = new Size(19, 41);
            pictureBox4.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox4.TabIndex = 6;
            pictureBox4.TabStop = false;
            // 
            // label18
            // 
            label18.AutoSize = true;
            label18.Font = new Font("Times New Roman", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label18.Location = new Point(31, 368);
            label18.Name = "label18";
            label18.Size = new Size(198, 20);
            label18.TabIndex = 17;
            label18.Text = "Security & Access Control";
            // 
            // label19
            // 
            label19.AutoSize = true;
            label19.Font = new Font("Times New Roman", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label19.ForeColor = SystemColors.ControlDarkDark;
            label19.Location = new Point(72, 323);
            label19.Name = "label19";
            label19.Size = new Size(206, 17);
            label19.TabIndex = 13;
            label19.Text = "Display score and currect answer";
            // 
            // label20
            // 
            label20.AutoSize = true;
            label20.Font = new Font("Segoe UI Semibold", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label20.Location = new Point(71, 298);
            label20.Name = "label20";
            label20.Size = new Size(253, 25);
            label20.TabIndex = 12;
            label20.Text = "Show result after submission";
            // 
            // label21
            // 
            label21.AutoSize = true;
            label21.Font = new Font("Times New Roman", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label21.ForeColor = SystemColors.ControlDarkDark;
            label21.Location = new Point(72, 271);
            label21.Name = "label21";
            label21.Size = new Size(139, 17);
            label21.TabIndex = 11;
            label21.Text = "Shuffle answer choice";
            // 
            // label22
            // 
            label22.AutoSize = true;
            label22.Font = new Font("Segoe UI Semibold", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label22.Location = new Point(72, 246);
            label22.Name = "label22";
            label22.Size = new Size(176, 25);
            label22.TabIndex = 10;
            label22.Text = "Randomize Options";
            // 
            // label23
            // 
            label23.AutoSize = true;
            label23.Font = new Font("Times New Roman", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label23.ForeColor = SystemColors.ControlDarkDark;
            label23.Location = new Point(72, 222);
            label23.Name = "label23";
            label23.Size = new Size(229, 17);
            label23.TabIndex = 9;
            label23.Text = "Suffle question order for each student";
            // 
            // label24
            // 
            label24.AutoSize = true;
            label24.Font = new Font("Segoe UI Semibold", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label24.Location = new Point(72, 197);
            label24.Name = "label24";
            label24.Size = new Size(194, 25);
            label24.TabIndex = 8;
            label24.Text = "Randomize Questions";
            // 
            // markBox
            // 
            markBox.Location = new Point(641, 141);
            markBox.Name = "markBox";
            markBox.PlaceholderText = " e.g., 100";
            markBox.Size = new Size(585, 31);
            markBox.TabIndex = 7;
            // 
            // label25
            // 
            label25.AutoSize = true;
            label25.Font = new Font("Segoe UI Semibold", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label25.Location = new Point(641, 113);
            label25.Name = "label25";
            label25.Size = new Size(120, 25);
            label25.TabIndex = 6;
            label25.Text = "Total Marks *";
            // 
            // durationbox
            // 
            durationbox.Location = new Point(9, 141);
            durationbox.Name = "durationbox";
            durationbox.PlaceholderText = " e.g., 60";
            durationbox.Size = new Size(589, 31);
            durationbox.TabIndex = 3;
            // 
            // label27
            // 
            label27.AutoSize = true;
            label27.Font = new Font("Segoe UI Semibold", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label27.Location = new Point(9, 113);
            label27.Name = "label27";
            label27.Size = new Size(152, 25);
            label27.TabIndex = 2;
            label27.Text = "Duration(Mins) *";
            // 
            // examNamebox
            // 
            examNamebox.Location = new Point(12, 67);
            examNamebox.Name = "examNamebox";
            examNamebox.PlaceholderText = " e.g., Midterm Exam";
            examNamebox.Size = new Size(1214, 31);
            examNamebox.TabIndex = 1;
            // 
            // label28
            // 
            label28.AutoSize = true;
            label28.Font = new Font("Segoe UI Semibold", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label28.Location = new Point(11, 38);
            label28.Name = "label28";
            label28.Size = new Size(122, 25);
            label28.TabIndex = 0;
            label28.Text = "Exam Name *";
            // 
            // CreateQuiz
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoValidate = AutoValidate.EnablePreventFocusChange;
            ClientSize = new Size(1262, 785);
            Controls.Add(questionBox);
            Controls.Add(groupBox3);
            Controls.Add(Logo);
            FormBorderStyle = FormBorderStyle.None;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "CreateQuiz";
            StartPosition = FormStartPosition.CenterScreen;
            groupBox3.ResumeLayout(false);
            Logo.ResumeLayout(false);
            Logo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)backbtn).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            questionBox.ResumeLayout(false);
            questionBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox5).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox3;
        private Button questionBtn;
        private Button myQuizBtn;
        private Panel Logo;
        private Label label2;
        private Label tag;
        private Label WelcomeTeacher;
        private Button teacherBtn;
        private Label label1;
        private PictureBox pictureBox1;
        private Label minbtn;
        private Label exitBtn;
        private Panel questionPanel;
        private GroupBox questionBox;
        private TextBox txtRandomCount;
        private CheckBox chkShowResult;
        private CheckBox chkRanOption;
        private CheckBox chkRanQuestion;
        private PictureBox pictureBox5;
        private Button saveDraft;
        private PictureBox pictureBox3;
        private Button button1;
        private TextBox passbox;
        private Label label17;
        private PictureBox pictureBox4;
        private Label label18;
        private Label label19;
        private Label label20;
        private Label label21;
        private Label label22;
        private Label label23;
        private Label label24;
        private TextBox markBox;
        private Label label25;
        private TextBox durationbox;
        private Label label27;
        private TextBox examNamebox;
        private Label label28;
        private PictureBox backbtn;
    }
}