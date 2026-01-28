namespace IELTS.UI.Admin.TestManager
{
    partial class AddSectionButtonControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            uiLabelTitle = new Sunny.UI.UILabel();
            btnAddSpeakingSection = new Sunny.UI.UIButton();
            btnAddWritingSection = new Sunny.UI.UIButton();
            btnAddReadingSection = new Sunny.UI.UIButton();
            btnAddListeningSection = new Sunny.UI.UIButton();
            label5 = new Label();
            cboMockTest = new ComboBox();
            label1 = new Label();
            cmbTestMonth = new ComboBox();
            SuspendLayout();
            // 
            // uiLabelTitle
            // 
            uiLabelTitle.BackColor = SystemColors.GradientInactiveCaption;
            uiLabelTitle.Font = new Font("Microsoft Sans Serif", 24F, FontStyle.Bold, GraphicsUnit.Point, 0);
            uiLabelTitle.ForeColor = SystemColors.Highlight;
            uiLabelTitle.Location = new Point(0, 0);
            uiLabelTitle.Name = "uiLabelTitle";
            uiLabelTitle.Size = new Size(1430, 70);
            uiLabelTitle.Style = Sunny.UI.UIStyle.Custom;
            uiLabelTitle.TabIndex = 12;
            uiLabelTitle.Text = "Add These Section For Test Paper";
            uiLabelTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnAddSpeakingSection
            // 
            btnAddSpeakingSection.BackColor = SystemColors.Control;
            btnAddSpeakingSection.Cursor = Cursors.Hand;
            btnAddSpeakingSection.FillColor = Color.White;
            btnAddSpeakingSection.FillHoverColor = Color.FromArgb(230, 240, 255);
            btnAddSpeakingSection.FillPressColor = Color.DodgerBlue;
            btnAddSpeakingSection.Font = new Font("Segoe UI", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnAddSpeakingSection.ForeColor = Color.Black;
            btnAddSpeakingSection.Location = new Point(980, 124);
            btnAddSpeakingSection.MinimumSize = new Size(1, 1);
            btnAddSpeakingSection.Name = "btnAddSpeakingSection";
            btnAddSpeakingSection.Radius = 20;
            btnAddSpeakingSection.RectColor = Color.LightGray;
            btnAddSpeakingSection.Size = new Size(230, 69);
            btnAddSpeakingSection.TabIndex = 16;
            btnAddSpeakingSection.Text = "🗣️  Speaking";
            btnAddSpeakingSection.TipsFont = new Font("Microsoft Sans Serif", 9F);
            btnAddSpeakingSection.TipsForeColor = Color.Azure;
            // 
            // btnAddWritingSection
            // 
            btnAddWritingSection.BackColor = SystemColors.Control;
            btnAddWritingSection.Cursor = Cursors.Hand;
            btnAddWritingSection.FillColor = Color.White;
            btnAddWritingSection.FillHoverColor = Color.FromArgb(230, 240, 255);
            btnAddWritingSection.FillPressColor = Color.DodgerBlue;
            btnAddWritingSection.Font = new Font("Segoe UI", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnAddWritingSection.ForeColor = Color.Black;
            btnAddWritingSection.Location = new Point(695, 124);
            btnAddWritingSection.MinimumSize = new Size(1, 1);
            btnAddWritingSection.Name = "btnAddWritingSection";
            btnAddWritingSection.Radius = 20;
            btnAddWritingSection.RectColor = Color.LightGray;
            btnAddWritingSection.Size = new Size(230, 69);
            btnAddWritingSection.TabIndex = 15;
            btnAddWritingSection.Text = "✍️  Writing";
            btnAddWritingSection.TipsFont = new Font("Microsoft Sans Serif", 9F);
            btnAddWritingSection.TipsForeColor = Color.Azure;
            // 
            // btnAddReadingSection
            // 
            btnAddReadingSection.BackColor = SystemColors.Control;
            btnAddReadingSection.Cursor = Cursors.Hand;
            btnAddReadingSection.FillColor = Color.White;
            btnAddReadingSection.FillHoverColor = Color.FromArgb(230, 240, 255);
            btnAddReadingSection.FillPressColor = Color.DodgerBlue;
            btnAddReadingSection.Font = new Font("Segoe UI", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnAddReadingSection.ForeColor = Color.Black;
            btnAddReadingSection.Location = new Point(126, 124);
            btnAddReadingSection.MinimumSize = new Size(1, 1);
            btnAddReadingSection.Name = "btnAddReadingSection";
            btnAddReadingSection.Radius = 20;
            btnAddReadingSection.RectColor = Color.LightGray;
            btnAddReadingSection.Size = new Size(230, 69);
            btnAddReadingSection.TabIndex = 14;
            btnAddReadingSection.Text = "📖  Reading";
            btnAddReadingSection.TipsFont = new Font("Microsoft Sans Serif", 9F);
            btnAddReadingSection.TipsForeColor = Color.Azure;
            btnAddReadingSection.Click += btnAddReadingSection_Click;
            // 
            // btnAddListeningSection
            // 
            btnAddListeningSection.BackColor = SystemColors.Control;
            btnAddListeningSection.Cursor = Cursors.Hand;
            btnAddListeningSection.FillColor = Color.White;
            btnAddListeningSection.FillHoverColor = Color.FromArgb(230, 240, 255);
            btnAddListeningSection.FillPressColor = Color.DodgerBlue;
            btnAddListeningSection.Font = new Font("Segoe UI", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnAddListeningSection.ForeColor = Color.Black;
            btnAddListeningSection.Location = new Point(411, 124);
            btnAddListeningSection.MinimumSize = new Size(1, 1);
            btnAddListeningSection.Name = "btnAddListeningSection";
            btnAddListeningSection.Radius = 20;
            btnAddListeningSection.RectColor = Color.LightGray;
            btnAddListeningSection.Size = new Size(230, 69);
            btnAddListeningSection.TabIndex = 13;
            btnAddListeningSection.Text = "🎧  Listening";
            btnAddListeningSection.TipsFont = new Font("Microsoft Sans Serif", 9F);
            btnAddListeningSection.TipsForeColor = Color.Azure;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(18, 76);
            label5.Name = "label5";
            label5.Size = new Size(78, 20);
            label5.TabIndex = 112;
            label5.Text = "Mock Test:";
            // 
            // cboMockTest
            // 
            cboMockTest.FormattingEnabled = true;
            cboMockTest.Location = new Point(153, 73);
            cboMockTest.Name = "cboMockTest";
            cboMockTest.Size = new Size(275, 28);
            cboMockTest.TabIndex = 111;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(989, 79);
            label1.Name = "label1";
            label1.Size = new Size(85, 20);
            label1.TabIndex = 114;
            label1.Text = "Test Month:";
            // 
            // cmbTestMonth
            // 
            cmbTestMonth.FormattingEnabled = true;
            cmbTestMonth.Location = new Point(1124, 76);
            cmbTestMonth.Name = "cmbTestMonth";
            cmbTestMonth.Size = new Size(275, 28);
            cmbTestMonth.TabIndex = 113;
            // 
            // AddSectionButtonControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(label1);
            Controls.Add(cmbTestMonth);
            Controls.Add(label5);
            Controls.Add(cboMockTest);
            Controls.Add(btnAddSpeakingSection);
            Controls.Add(btnAddWritingSection);
            Controls.Add(btnAddReadingSection);
            Controls.Add(btnAddListeningSection);
            Controls.Add(uiLabelTitle);
            Name = "AddSectionButtonControl";
            Size = new Size(1430, 710);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Sunny.UI.UILabel uiLabelTitle;
        private Sunny.UI.UIButton btnAddSpeakingSection;
        private Sunny.UI.UIButton btnAddWritingSection;
        private Sunny.UI.UIButton btnAddReadingSection;
        private Sunny.UI.UIButton btnAddListeningSection;
        private Label label5;
        private ComboBox cboMockTest;
        private Label label1;
        private ComboBox cmbTestMonth;
    }
}
