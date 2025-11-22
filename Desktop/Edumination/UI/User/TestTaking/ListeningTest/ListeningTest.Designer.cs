using IELTS.UI.User.TestTaking.Controls;

namespace IELTS.UI.User.TestTaking.ListeningTest
{
    partial class ListeningTest
    {
        private System.ComponentModel.IContainer components = null;

        private TestNavBarPanel testNavBar;
        private SplitContainer splitMain;
        private AudioPlayerPanel audioPanel;
        private AnswerPanel answerPanel;
        private TestFooterPanel testFooter;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            testNavBar = new TestNavBarPanel();
            splitMain = new SplitContainer();
            audioPanel = new AudioPlayerPanel();
            answerPanel = new AnswerPanel();
            testFooter = new TestFooterPanel();

            ((System.ComponentModel.ISupportInitialize)splitMain).BeginInit();
            splitMain.Panel1.SuspendLayout();
            splitMain.Panel2.SuspendLayout();
            splitMain.SuspendLayout();
            SuspendLayout();
            // 
            // testNavBar
            // 
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
            // Left
            splitMain.Panel1.Controls.Add(audioPanel);
            // Right
            splitMain.Panel2.Controls.Add(answerPanel);

            splitMain.SplitterDistance = 960;
            splitMain.TabIndex = 1;
            // 
            // audioPanel
            // 
            audioPanel.Dock = DockStyle.Fill;
            audioPanel.Name = "audioPanel";
            audioPanel.TabIndex = 0;
            // 
            // answerPanel
            // 
            answerPanel.Dock = DockStyle.Fill;
            answerPanel.Name = "answerPanel";
            answerPanel.TabIndex = 0;
            // 
            // testFooter
            // 
            testFooter.Dock = DockStyle.Bottom;
            testFooter.Location = new Point(0, 930);
            testFooter.Name = "testFooter";
            testFooter.Size = new Size(1920, 90);
            testFooter.TabIndex = 2;
            // 
            // ListeningTest
            // 
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(1920, 1020);
            Controls.Add(splitMain);
            Controls.Add(testFooter);
            Controls.Add(testNavBar);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "ListeningTest";
            Text = "IELTS Listening Test (Mock)";
            Load += ListeningTest_Load;

            splitMain.Panel1.ResumeLayout(false);
            splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitMain).EndInit();
            splitMain.ResumeLayout(false);
            ResumeLayout(false);
        }
    }
}