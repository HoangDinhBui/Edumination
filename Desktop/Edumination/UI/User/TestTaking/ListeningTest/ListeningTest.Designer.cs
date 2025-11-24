using System.Windows.Forms;
using IELTS.UI.User.TestTaking.Controls;

namespace IELTS.UI.User.TestTaking.ListeningTest
{
    partial class ListeningTest
    {
        private System.ComponentModel.IContainer components = null;

        private TestNavBarPanel testNavBar;
        private TestFooterPanel testFooter;
        private SplitContainer splitMain;       // giống Reading
        private SplitContainer splitLeft;       // Panel1 chứa AUDIO + PDF
        private AudioPlayerPanel audioPanel;
        private PdfViewerPanel pdfViewer;
        private AnswerPanel answerPanel;        // giống Reading

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            testNavBar = new TestNavBarPanel();
            testFooter = new TestFooterPanel();
            splitMain = new SplitContainer();
            splitLeft = new SplitContainer();
            audioPanel = new AudioPlayerPanel();
            pdfViewer = new PdfViewerPanel();
            answerPanel = new AnswerPanel();
            ((System.ComponentModel.ISupportInitialize)splitMain).BeginInit();
            splitMain.Panel1.SuspendLayout();
            splitMain.Panel2.SuspendLayout();
            splitMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitLeft).BeginInit();
            splitLeft.Panel1.SuspendLayout();
            splitLeft.Panel2.SuspendLayout();
            splitLeft.SuspendLayout();
            SuspendLayout();
            // 
            // testNavBar
            // 
            testNavBar.BackColor = Color.White;
            testNavBar.Dock = DockStyle.Top;
            testNavBar.Location = new Point(0, 0);
            testNavBar.Name = "testNavBar";
            testNavBar.Size = new Size(1920, 80);
            testNavBar.TabIndex = 2;
            // 
            // testFooter
            // 
            testFooter.BackColor = Color.White;
            testFooter.Dock = DockStyle.Bottom;
            testFooter.Location = new Point(0, 930);
            testFooter.Name = "testFooter";
            testFooter.Size = new Size(1920, 90);
            testFooter.TabIndex = 1;
            // 
            // splitMain
            // 
            splitMain.Dock = DockStyle.Fill;
            splitMain.Location = new Point(0, 80);
            splitMain.Name = "splitMain";
            // 
            // splitMain.Panel1
            // 
            splitMain.Panel1.Controls.Add(splitLeft);
            // 
            // splitMain.Panel2
            // 
            splitMain.Panel2.Controls.Add(answerPanel);
            splitMain.Size = new Size(1920, 850);
            splitMain.SplitterDistance = 960;
            splitMain.TabIndex = 0;
            // 
            // splitLeft
            // 
            splitLeft.Dock = DockStyle.Fill;
            splitLeft.Location = new Point(0, 0);
            splitLeft.Name = "splitLeft";
            splitLeft.Orientation = Orientation.Horizontal;
            // 
            // splitLeft.Panel1
            // 
            splitLeft.Panel1.Controls.Add(audioPanel);
            // 
            // splitLeft.Panel2
            // 
            splitLeft.Panel2.Controls.Add(pdfViewer);
            splitLeft.Size = new Size(960, 850);
            splitLeft.SplitterDistance = 603;
            splitLeft.TabIndex = 0;
            // 
            // audioPanel
            // 
            audioPanel.Location = new Point(0, 0);
            audioPanel.Name = "audioPanel";
            audioPanel.Size = new Size(900, 150);
            audioPanel.TabIndex = 0;
            // 
            // pdfViewer
            // 
            pdfViewer.Location = new Point(0, 0);
            pdfViewer.Name = "pdfViewer";
            pdfViewer.Size = new Size(960, 850);
            pdfViewer.TabIndex = 0;
            // 
            // answerPanel
            // 
            answerPanel.BackColor = Color.White;
            answerPanel.Dock = DockStyle.Fill;
            answerPanel.Location = new Point(0, 0);
            answerPanel.Name = "answerPanel";
            answerPanel.Size = new Size(956, 850);
            answerPanel.TabIndex = 0;
            // 
            // ListeningTest
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.White;
            ClientSize = new Size(1920, 1020);
            Controls.Add(splitMain);
            Controls.Add(testFooter);
            Controls.Add(testNavBar);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "ListeningTest";
            Text = "IELTS Listening Test";
            Load += ListeningTest_Load;
            splitMain.Panel1.ResumeLayout(false);
            splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitMain).EndInit();
            splitMain.ResumeLayout(false);
            splitLeft.Panel1.ResumeLayout(false);
            splitLeft.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitLeft).EndInit();
            splitLeft.ResumeLayout(false);
            ResumeLayout(false);
        }
    }
}
