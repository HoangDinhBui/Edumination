//using IELTS.UI.User.TestTaking.Controls;

//namespace IELTS.UI.User.TestTaking.ReadingTest
//{
//    partial class ReadingTest
//    {
//        private System.ComponentModel.IContainer components = null;

//        private TestNavBarPanel testNavBar;
//        private TestFooterPanel testFooter;
//        private SplitContainer splitMain;
//        private PdfViewerPanel pdfViewer;
//        private AnswerPanel answerPanel;

  
//        protected override void Dispose(bool disposing)
//        {
//            if (disposing && components != null)
//                components.Dispose();

//            base.Dispose(disposing);
//        }

//        private void InitializeComponent()
//        {
//            components = new System.ComponentModel.Container();
//            testNavBar = new TestNavBarPanel();
//            testFooter = new TestFooterPanel();
//            splitMain = new SplitContainer();
//            pdfViewer = new PdfViewerPanel();
//            answerPanel = new AnswerPanel();

//            ((System.ComponentModel.ISupportInitialize)splitMain).BeginInit();
//            splitMain.Panel1.SuspendLayout();
//            splitMain.Panel2.SuspendLayout();
//            splitMain.SuspendLayout();
//            SuspendLayout();
//            // 
//            // ReadingTest
//            // 
//            AutoScaleMode = AutoScaleMode.None;
//            BackColor = Color.White;
//            ClientSize = new Size(1920, 1020);
//            FormBorderStyle = FormBorderStyle.FixedSingle;
//            MaximizeBox = false;
//            MinimizeBox = false;
//            Name = "ReadingTest";
//            StartPosition = FormStartPosition.CenterScreen;
//            Text = "IELTS Mock Reading Test";
//            Load += ReadingTest_Load;
//            // 
//            // testNavBar
//            // 
//            testNavBar.Dock = DockStyle.Top;
//            testNavBar.Location = new Point(0, 0);
//            testNavBar.Name = "testNavBar";
//            testNavBar.Size = new Size(1920, 80);
//            testNavBar.TabIndex = 0;
//            // 
//            // testFooter
//            // 
//            testFooter.Dock = DockStyle.Bottom;
//            testFooter.Location = new Point(0, 930);
//            testFooter.Name = "testFooter";
//            testFooter.Size = new Size(1920, 90);
//            testFooter.TabIndex = 2;
//            // 
//            // splitMain
//            // 
//            splitMain.Dock = DockStyle.Fill;
//            splitMain.Location = new Point(0, 80);
//            splitMain.Name = "splitMain";
//            splitMain.Size = new Size(1920, 850);
//            splitMain.SplitterDistance = 960;
//            splitMain.SplitterWidth = 8;
//            splitMain.TabIndex = 1;
//            // 
//            // splitMain.Panel1 (PDF)
//            // 
//            splitMain.Panel1.Controls.Add(pdfViewer);
//            // 
//            // splitMain.Panel2 (Answer)
//            // 
//            splitMain.Panel2.Controls.Add(answerPanel);
//            // 
//            // pdfViewer
//            // 
//            pdfViewer.Dock = DockStyle.Fill;
//            pdfViewer.Name = "pdfViewer";
//            pdfViewer.TabIndex = 0;
//            // 
//            // answerPanel
//            // 
//            answerPanel.Dock = DockStyle.Fill;
//            answerPanel.Name = "answerPanel";
//            answerPanel.TabIndex = 0;
//            // 
//            // Add controls
//            // 
//            Controls.Add(splitMain);
//            Controls.Add(testFooter);
//            Controls.Add(testNavBar);

//            splitMain.Panel1.ResumeLayout(false);
//            splitMain.Panel2.ResumeLayout(false);
//            ((System.ComponentModel.ISupportInitialize)splitMain).EndInit();
//            splitMain.ResumeLayout(false);
//            ResumeLayout(false);
//        }
//    }
//}