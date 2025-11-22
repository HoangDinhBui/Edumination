using IELTS.UI.User.TestTaking.Controls;

namespace IELTS.UI.User.TestTaking.WritingTest
{
    partial class WritingTest
    {
        private System.ComponentModel.IContainer components = null;

        private TestNavBarPanel testNavBar;
        private SplitContainer splitMain;
        private WritingPromptPanel promptPanel;
        private WritingAnswerPanel writingAnswerPanel;
        private TestFooterPanel testFooter;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            testNavBar = new TestNavBarPanel();
            splitMain = new SplitContainer();
            promptPanel = new WritingPromptPanel();
            writingAnswerPanel = new WritingAnswerPanel();
            testFooter = new TestFooterPanel();
            ((System.ComponentModel.ISupportInitialize)splitMain).BeginInit();
            splitMain.Panel1.SuspendLayout();
            splitMain.Panel2.SuspendLayout();
            splitMain.SuspendLayout();
            SuspendLayout();
            // 
            // testNavBar
            // 
            testNavBar.BackColor = Color.White;
            testNavBar.Dock = DockStyle.Top;
            testNavBar.Location = new Point(0, 0);
            testNavBar.Name = "testNavBar";
            testNavBar.Size = new Size(1920, 80);
            testNavBar.TabIndex = 0;
            // 
            // splitMain
            // 
            splitMain.Dock = DockStyle.Fill;
            splitMain.Location = new Point(0, 80);
            splitMain.Name = "splitMain";
            // 
            // splitMain.Panel1
            // 
            splitMain.Panel1.Controls.Add(promptPanel);
            // 
            // splitMain.Panel2
            // 
            splitMain.Panel2.Controls.Add(writingAnswerPanel);
            splitMain.Size = new Size(1920, 850);
            splitMain.SplitterDistance = 1548;
            splitMain.TabIndex = 1;
            // 
            // promptPanel
            // 
            promptPanel.BackColor = Color.White;
            promptPanel.Dock = DockStyle.Fill;
            promptPanel.Location = new Point(0, 0);
            promptPanel.Name = "promptPanel";
            promptPanel.Size = new Size(1548, 850);
            promptPanel.TabIndex = 0;
            // 
            // writingAnswerPanel
            // 
            writingAnswerPanel.Dock = DockStyle.Fill;
            writingAnswerPanel.Location = new Point(0, 0);
            writingAnswerPanel.Name = "writingAnswerPanel";
            writingAnswerPanel.Size = new Size(368, 850);
            writingAnswerPanel.TabIndex = 0;
            // 
            // testFooter
            // 
            testFooter.BackColor = Color.White;
            testFooter.Dock = DockStyle.Bottom;
            testFooter.Location = new Point(0, 930);
            testFooter.Name = "testFooter";
            testFooter.Size = new Size(1920, 90);
            testFooter.TabIndex = 2;
            // 
            // WritingTest
            // 
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(1920, 1020);
            Controls.Add(splitMain);
            Controls.Add(testFooter);
            Controls.Add(testNavBar);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "WritingTest";
            Text = "IELTS Writing Test (Mock)";
            Load += WritingTest_Load;
            splitMain.Panel1.ResumeLayout(false);
            splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitMain).EndInit();
            splitMain.ResumeLayout(false);
            ResumeLayout(false);
        }
    }
}