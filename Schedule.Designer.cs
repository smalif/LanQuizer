namespace LanQuizer
{
    partial class Schedule
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Schedule));
            label2 = new Label();
            label1 = new Label();
            label3 = new Label();
            ScheduleQuiz = new GroupBox();
            ScheduleSec = new ComboBox();
            scheduleCourse = new ComboBox();
            scheduleTime = new DateTimePicker();
            scheduleDate = new DateTimePicker();
            scheduleBtn = new Button();
            label5 = new Label();
            label4 = new Label();
            StartNowGrp = new GroupBox();
            StartSection = new ComboBox();
            StartCourse = new ComboBox();
            startNowBtn = new Button();
            label6 = new Label();
            label7 = new Label();
            scheduleCheck = new RadioButton();
            startNowChk = new RadioButton();
            ScheduleQuiz.SuspendLayout();
            StartNowGrp.SuspendLayout();
            SuspendLayout();
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("BankGothic Lt BT", 22.2F, FontStyle.Italic, GraphicsUnit.Point, 0);
            label2.Location = new Point(80, 23);
            label2.Name = "label2";
            label2.Size = new Size(237, 39);
            label2.TabIndex = 6;
            label2.Text = "Save/Start";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Times New Roman", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(6, 36);
            label1.Name = "label1";
            label1.Size = new Size(105, 20);
            label1.TabIndex = 8;
            label1.Text = "Select Date :";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Times New Roman", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(6, 70);
            label3.Name = "label3";
            label3.Size = new Size(108, 20);
            label3.TabIndex = 9;
            label3.Text = "Select Time :";
            // 
            // ScheduleQuiz
            // 
            ScheduleQuiz.Controls.Add(ScheduleSec);
            ScheduleQuiz.Controls.Add(scheduleCourse);
            ScheduleQuiz.Controls.Add(scheduleTime);
            ScheduleQuiz.Controls.Add(scheduleDate);
            ScheduleQuiz.Controls.Add(scheduleBtn);
            ScheduleQuiz.Controls.Add(label5);
            ScheduleQuiz.Controls.Add(label4);
            ScheduleQuiz.Controls.Add(label1);
            ScheduleQuiz.Controls.Add(label3);
            ScheduleQuiz.Font = new Font("Times New Roman", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            ScheduleQuiz.Location = new Point(12, 108);
            ScheduleQuiz.Name = "ScheduleQuiz";
            ScheduleQuiz.Size = new Size(386, 186);
            ScheduleQuiz.TabIndex = 10;
            ScheduleQuiz.TabStop = false;
            ScheduleQuiz.Text = "Schedule Quiz";
            // 
            // ScheduleSec
            // 
            ScheduleSec.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            ScheduleSec.AutoCompleteSource = AutoCompleteSource.ListItems;
            ScheduleSec.FormattingEnabled = true;
            ScheduleSec.Location = new Point(315, 104);
            ScheduleSec.Name = "ScheduleSec";
            ScheduleSec.Size = new Size(65, 28);
            ScheduleSec.TabIndex = 16;
            ScheduleSec.SelectedIndexChanged += scheduleSec_SelectedIndexChanged;
            // 
            // scheduleCourse
            // 
            scheduleCourse.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            scheduleCourse.AutoCompleteSource = AutoCompleteSource.ListItems;
            scheduleCourse.FormattingEnabled = true;
            scheduleCourse.Location = new Point(80, 104);
            scheduleCourse.Name = "scheduleCourse";
            scheduleCourse.Size = new Size(156, 28);
            scheduleCourse.TabIndex = 15;
            scheduleCourse.SelectedIndexChanged += scheduleCourse_SelectedIndexChanged;
            // 
            // scheduleTime
            // 
            scheduleTime.CustomFormat = "hh:mm tt";
            scheduleTime.Format = DateTimePickerFormat.Custom;
            scheduleTime.Location = new Point(111, 66);
            scheduleTime.Name = "scheduleTime";
            scheduleTime.ShowUpDown = true;
            scheduleTime.Size = new Size(250, 28);
            scheduleTime.TabIndex = 14;
            // 
            // scheduleDate
            // 
            scheduleDate.Location = new Point(111, 32);
            scheduleDate.Name = "scheduleDate";
            scheduleDate.Size = new Size(250, 28);
            scheduleDate.TabIndex = 13;
            // 
            // scheduleBtn
            // 
            scheduleBtn.BackColor = Color.Yellow;
            scheduleBtn.ForeColor = SystemColors.ActiveCaptionText;
            scheduleBtn.Location = new Point(143, 140);
            scheduleBtn.Name = "scheduleBtn";
            scheduleBtn.Size = new Size(94, 29);
            scheduleBtn.TabIndex = 12;
            scheduleBtn.Text = "Schedule";
            scheduleBtn.UseVisualStyleBackColor = false;
            scheduleBtn.Click += scheduleBtn_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Times New Roman", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.Location = new Point(240, 107);
            label5.Name = "label5";
            label5.Size = new Size(76, 20);
            label5.TabIndex = 11;
            label5.Text = "Section :";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Times New Roman", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.Location = new Point(6, 107);
            label4.Name = "label4";
            label4.Size = new Size(74, 20);
            label4.TabIndex = 10;
            label4.Text = "Course :";
            // 
            // StartNowGrp
            // 
            StartNowGrp.Controls.Add(StartSection);
            StartNowGrp.Controls.Add(StartCourse);
            StartNowGrp.Controls.Add(startNowBtn);
            StartNowGrp.Controls.Add(label6);
            StartNowGrp.Controls.Add(label7);
            StartNowGrp.Font = new Font("Times New Roman", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            StartNowGrp.Location = new Point(12, 125);
            StartNowGrp.Name = "StartNowGrp";
            StartNowGrp.Size = new Size(386, 127);
            StartNowGrp.TabIndex = 17;
            StartNowGrp.TabStop = false;
            StartNowGrp.Text = "Start Now";
            // 
            // StartSection
            // 
            StartSection.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            StartSection.AutoCompleteSource = AutoCompleteSource.ListItems;
            StartSection.FormattingEnabled = true;
            StartSection.Location = new Point(315, 39);
            StartSection.Name = "StartSection";
            StartSection.Size = new Size(65, 28);
            StartSection.TabIndex = 16;
            StartSection.SelectedIndexChanged += StartSection_SelectedIndexChanged;
            // 
            // StartCourse
            // 
            StartCourse.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            StartCourse.AutoCompleteSource = AutoCompleteSource.ListItems;
            StartCourse.FormattingEnabled = true;
            StartCourse.Location = new Point(80, 39);
            StartCourse.Name = "StartCourse";
            StartCourse.Size = new Size(156, 28);
            StartCourse.TabIndex = 15;
            StartCourse.SelectedIndexChanged += StartCourse_SelectedIndexChanged;
            // 
            // startNowBtn
            // 
            startNowBtn.BackColor = Color.SeaGreen;
            startNowBtn.ForeColor = SystemColors.Control;
            startNowBtn.Location = new Point(143, 75);
            startNowBtn.Name = "startNowBtn";
            startNowBtn.Size = new Size(94, 29);
            startNowBtn.TabIndex = 12;
            startNowBtn.Text = "Start Now";
            startNowBtn.UseVisualStyleBackColor = false;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Times New Roman", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label6.Location = new Point(240, 42);
            label6.Name = "label6";
            label6.Size = new Size(76, 20);
            label6.TabIndex = 11;
            label6.Text = "Section :";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Times New Roman", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label7.Location = new Point(6, 42);
            label7.Name = "label7";
            label7.Size = new Size(74, 20);
            label7.TabIndex = 10;
            label7.Text = "Course :";
            // 
            // scheduleCheck
            // 
            scheduleCheck.AutoSize = true;
            scheduleCheck.Location = new Point(252, 78);
            scheduleCheck.Name = "scheduleCheck";
            scheduleCheck.Size = new Size(90, 24);
            scheduleCheck.TabIndex = 11;
            scheduleCheck.TabStop = true;
            scheduleCheck.Text = "Schedule";
            scheduleCheck.UseVisualStyleBackColor = true;
            scheduleCheck.CheckedChanged += scheduleCheck_CheckedChanged;
            // 
            // startNowChk
            // 
            startNowChk.AutoSize = true;
            startNowChk.Location = new Point(56, 78);
            startNowChk.Name = "startNowChk";
            startNowChk.Size = new Size(96, 24);
            startNowChk.TabIndex = 12;
            startNowChk.TabStop = true;
            startNowChk.Text = "Start Now";
            startNowChk.UseVisualStyleBackColor = true;
            startNowChk.CheckedChanged += startNowChk_CheckedChanged;
            // 
            // Schedule
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(410, 299);
            Controls.Add(StartNowGrp);
            Controls.Add(startNowChk);
            Controls.Add(scheduleCheck);
            Controls.Add(ScheduleQuiz);
            Controls.Add(label2);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Schedule";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Start Quiz";
            Load += Schedule_Load;
            ScheduleQuiz.ResumeLayout(false);
            ScheduleQuiz.PerformLayout();
            StartNowGrp.ResumeLayout(false);
            StartNowGrp.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label2;
        private Label label1;
        private Label label3;
        private GroupBox ScheduleQuiz;
        private Label label5;
        private Label label4;
        private DateTimePicker scheduleDate;
        private Button scheduleBtn;
        private RadioButton scheduleCheck;
        private RadioButton startNowChk;
        private DateTimePicker scheduleTime;
        private ComboBox ScheduleSec;
        private ComboBox scheduleCourse;
        private GroupBox StartNowGrp;
        private ComboBox StartSection;
        private ComboBox StartCourse;
        private Button startNowBtn;
        private Label label6;
        private Label label7;
    }
}