namespace LanQuizer
{
    partial class Add_Section
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Add_Section));
            Logo = new Panel();
            SectionexitBtn = new Label();
            tag = new Label();
            label1 = new Label();
            pictureBox1 = new PictureBox();
            AddQuizLbl = new Label();
            sectionNameLbl = new Label();
            label2 = new Label();
            uploadBtn = new Button();
            viewBtn = new Button();
            saveBtn = new Button();
            dataView = new Label();
            dataGridView1 = new DataGridView();
            DataClose = new Label();
            courseCombo = new ComboBox();
            SectionCombo = new ComboBox();
            Logo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // Logo
            // 
            Logo.BackColor = Color.DarkBlue;
            Logo.BackgroundImageLayout = ImageLayout.Zoom;
            Logo.BorderStyle = BorderStyle.FixedSingle;
            Logo.Controls.Add(SectionexitBtn);
            Logo.Controls.Add(tag);
            Logo.Controls.Add(label1);
            Logo.Controls.Add(pictureBox1);
            Logo.Dock = DockStyle.Top;
            Logo.ForeColor = SystemColors.Control;
            Logo.Location = new Point(0, 0);
            Logo.Margin = new Padding(10);
            Logo.Name = "Logo";
            Logo.Padding = new Padding(50);
            Logo.Size = new Size(571, 105);
            Logo.TabIndex = 2;
            // 
            // SectionexitBtn
            // 
            SectionexitBtn.AutoSize = true;
            SectionexitBtn.Font = new Font("ISOCPEUR", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            SectionexitBtn.ForeColor = Color.RosyBrown;
            SectionexitBtn.Location = new Point(531, 20);
            SectionexitBtn.Name = "SectionexitBtn";
            SectionexitBtn.Size = new Size(25, 26);
            SectionexitBtn.TabIndex = 21;
            SectionexitBtn.Text = "X";
            SectionexitBtn.Click += SectionexitBtn_Click;
            // 
            // tag
            // 
            tag.AutoSize = true;
            tag.Font = new Font("Monotype Corsiva", 10.8F, FontStyle.Italic, GraphicsUnit.Point, 0);
            tag.Location = new Point(105, 49);
            tag.Name = "tag";
            tag.Size = new Size(266, 21);
            tag.TabIndex = 4;
            tag.Text = "A Lan based Quiz Management System";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("BankGothic Lt BT", 22.2F, FontStyle.Italic, GraphicsUnit.Point, 0);
            label1.Location = new Point(105, 10);
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
            pictureBox1.Size = new Size(88, 83);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // AddQuizLbl
            // 
            AddQuizLbl.AutoSize = true;
            AddQuizLbl.Font = new Font("Times New Roman", 19.8000011F, FontStyle.Regular, GraphicsUnit.Point, 0);
            AddQuizLbl.Location = new Point(123, 115);
            AddQuizLbl.Name = "AddQuizLbl";
            AddQuizLbl.Size = new Size(331, 39);
            AddQuizLbl.TabIndex = 3;
            AddQuizLbl.Text = "ADD NEW SECTION";
            // 
            // sectionNameLbl
            // 
            sectionNameLbl.AutoSize = true;
            sectionNameLbl.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            sectionNameLbl.Location = new Point(51, 222);
            sectionNameLbl.Name = "sectionNameLbl";
            sectionNameLbl.Size = new Size(69, 22);
            sectionNameLbl.TabIndex = 4;
            sectionNameLbl.Text = "Section";
            sectionNameLbl.Click += sectionNameLbl_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(51, 181);
            label2.Name = "label2";
            label2.Size = new Size(66, 22);
            label2.TabIndex = 5;
            label2.Text = "Course";
            label2.Click += label2_Click;
            // 
            // uploadBtn
            // 
            uploadBtn.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            uploadBtn.Location = new Point(307, 286);
            uploadBtn.Name = "uploadBtn";
            uploadBtn.Size = new Size(94, 29);
            uploadBtn.TabIndex = 8;
            uploadBtn.Text = "Upload";
            uploadBtn.UseVisualStyleBackColor = true;
            uploadBtn.Click += uploadBtn_Click;
            // 
            // viewBtn
            // 
            viewBtn.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            viewBtn.Location = new Point(169, 286);
            viewBtn.Name = "viewBtn";
            viewBtn.Size = new Size(94, 29);
            viewBtn.TabIndex = 9;
            viewBtn.Text = "View";
            viewBtn.UseVisualStyleBackColor = true;
            viewBtn.Click += viewBtn_Click;
            // 
            // saveBtn
            // 
            saveBtn.BackColor = Color.FromArgb(0, 192, 0);
            saveBtn.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            saveBtn.ForeColor = Color.White;
            saveBtn.Location = new Point(235, 337);
            saveBtn.Name = "saveBtn";
            saveBtn.Size = new Size(94, 29);
            saveBtn.TabIndex = 10;
            saveBtn.Text = "Save";
            saveBtn.UseVisualStyleBackColor = false;
            saveBtn.Click += saveBtn_Click;
            // 
            // dataView
            // 
            dataView.AutoSize = true;
            dataView.Font = new Font("Times New Roman", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataView.ForeColor = Color.FromArgb(0, 64, 0);
            dataView.Location = new Point(225, 259);
            dataView.Name = "dataView";
            dataView.Size = new Size(0, 19);
            dataView.TabIndex = 11;
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(95, 118);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.ReadOnly = true;
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.ScrollBars = ScrollBars.Vertical;
            dataGridView1.Size = new Size(386, 239);
            dataGridView1.TabIndex = 12;
            // 
            // DataClose
            // 
            DataClose.AutoSize = true;
            DataClose.Font = new Font("ISOCPEUR", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            DataClose.ForeColor = Color.RosyBrown;
            DataClose.Location = new Point(456, 116);
            DataClose.Name = "DataClose";
            DataClose.Size = new Size(25, 26);
            DataClose.TabIndex = 23;
            DataClose.Text = "X";
            DataClose.Click += DataClose_Click;
            // 
            // courseCombo
            // 
            courseCombo.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            courseCombo.AutoCompleteSource = AutoCompleteSource.ListItems;
            courseCombo.FormattingEnabled = true;
            courseCombo.Location = new Point(123, 184);
            courseCombo.Name = "courseCombo";
            courseCombo.Size = new Size(371, 28);
            courseCombo.TabIndex = 24;
            // 
            // SectionCombo
            // 
            SectionCombo.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            SectionCombo.AutoCompleteSource = AutoCompleteSource.ListItems;
            SectionCombo.FormattingEnabled = true;
            SectionCombo.Location = new Point(123, 222);
            SectionCombo.Name = "SectionCombo";
            SectionCombo.Size = new Size(371, 28);
            SectionCombo.TabIndex = 25;
            // 
            // Add_Section
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(192, 192, 255);
            ClientSize = new Size(571, 397);
            Controls.Add(SectionCombo);
            Controls.Add(courseCombo);
            Controls.Add(DataClose);
            Controls.Add(dataGridView1);
            Controls.Add(dataView);
            Controls.Add(saveBtn);
            Controls.Add(viewBtn);
            Controls.Add(uploadBtn);
            Controls.Add(label2);
            Controls.Add(sectionNameLbl);
            Controls.Add(AddQuizLbl);
            Controls.Add(Logo);
            FormBorderStyle = FormBorderStyle.None;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Add_Section";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Add_Section";
            Load += Add_Section_Load;
            Logo.ResumeLayout(false);
            Logo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel Logo;
        private Label minbtn;
        private Label SectionexitBtn;
        private Label tag;
        private Label label1;
        private PictureBox pictureBox1;
        private Label AddQuizLbl;
        private Label sectionNameLbl;
        private Label label2;
        private Button uploadBtn;
        private Button viewBtn;
        private Button saveBtn;
        private Label dataView;
        private DataGridView dataGridView1;
        private Label DataClose;
        private ComboBox courseCombo;
        private ComboBox SectionCombo;
    }
}