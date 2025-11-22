namespace IELTS.UI.User.TestTaking.SpeakingTest
{
    partial class SpeakingTest
    {
        private System.ComponentModel.IContainer components = null;

        private Controls.TestNavBarPanel testNavBar;
        private Controls.TestFooterPanel testFooter;

        private Sunny.UI.UILabel lblPart;
        private Sunny.UI.UILabel lblTitle;
        private Sunny.UI.UILabel lblQuestion;

        private Controls.AudioRecorderPanel audioPanel;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            testNavBar = new IELTS.UI.User.TestTaking.Controls.TestNavBarPanel();
            testFooter = new IELTS.UI.User.TestTaking.Controls.TestFooterPanel();
            lblPart = new Sunny.UI.UILabel();
            lblTitle = new Sunny.UI.UILabel();
            lblQuestion = new Sunny.UI.UILabel();
            audioPanel = new IELTS.UI.User.TestTaking.Controls.AudioRecorderPanel();
            SuspendLayout();
            // 
            // testNavBar
            // 
            testNavBar.BackColor = Color.White;
            testNavBar.Dock = DockStyle.Top;
            testNavBar.Location = new Point(0, 0);
            testNavBar.Name = "testNavBar";
            testNavBar.Size = new Size(1920, 90);
            testNavBar.TabIndex = 0;
            // 
            // testFooter
            // 
            testFooter.BackColor = Color.White;
            testFooter.Dock = DockStyle.Bottom;
            testFooter.Location = new Point(0, 965);
            testFooter.Name = "testFooter";
            testFooter.Size = new Size(1920, 90);
            testFooter.TabIndex = 5;
            // 
            // lblPart
            // 
            lblPart.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            lblPart.ForeColor = Color.FromArgb(48, 48, 48);
            lblPart.Location = new Point(0, 120);
            lblPart.Name = "lblPart";
            lblPart.Size = new Size(1920, 50);
            lblPart.TabIndex = 1;
            lblPart.Text = "PART 1";
            lblPart.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblTitle
            // 
            lblTitle.Font = new Font("Segoe UI", 16F);
            lblTitle.ForeColor = Color.FromArgb(70, 70, 70);
            lblTitle.Location = new Point(0, 180);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(1920, 40);
            lblTitle.TabIndex = 2;
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblQuestion
            // 
            lblQuestion.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
            lblQuestion.ForeColor = Color.FromArgb(60, 60, 60);
            lblQuestion.Location = new Point(200, 260);
            lblQuestion.Name = "lblQuestion";
            lblQuestion.Size = new Size(1520, 90);
            lblQuestion.TabIndex = 3;
            lblQuestion.Text = "Question text goes here...";
            lblQuestion.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // audioPanel
            // 
            audioPanel.BackColor = Color.White;
            audioPanel.Location = new Point(865, 402);
            audioPanel.Name = "audioPanel";
            audioPanel.Size = new Size(164, 200);
            audioPanel.TabIndex = 4;
            // 
            // SpeakingTest
            // 
            BackColor = Color.White;
            ClientSize = new Size(1920, 1055);
            Controls.Add(testNavBar);
            Controls.Add(lblPart);
            Controls.Add(lblTitle);
            Controls.Add(lblQuestion);
            Controls.Add(audioPanel);
            Controls.Add(testFooter);
            Name = "SpeakingTest";
            Load += SpeakingTest_Load;
            ResumeLayout(false);
        }
    }
}
