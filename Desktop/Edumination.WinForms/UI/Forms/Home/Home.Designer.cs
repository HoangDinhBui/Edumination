namespace Edumination.WinForms.UI.Forms.Home
{
    partial class Home
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
            components = new System.ComponentModel.Container();
            userNavbarPanel1 = new UserNavbarPanel();
            picSlide = new PictureBox();
            timerSlide = new System.Windows.Forms.Timer(components);
            panelTests = new FlowLayoutPanel();
            panelLessons = new FlowLayoutPanel();
            panelMain = new Panel();
            lblDetail = new Sunny.UI.UILabel();
            lblClass = new Sunny.UI.UILabel();
            lblTest = new Sunny.UI.UILabel();
            btnPrevTest = new Button();
            btnNextTest = new Button();
            btnPrevLesson = new Button();
            btnNextLesson = new Button();
            ((System.ComponentModel.ISupportInitialize)picSlide).BeginInit();
            panelMain.SuspendLayout();
            SuspendLayout();
            // 
            // userNavbarPanel1
            // 
            userNavbarPanel1.Location = new Point(-1, -2);
            userNavbarPanel1.Name = "userNavbarPanel1";
            userNavbarPanel1.Size = new Size(2400, 72);
            userNavbarPanel1.TabIndex = 0;
            // 
            // picSlide
            // 
            picSlide.Location = new Point(-1, 76);
            picSlide.Name = "picSlide";
            picSlide.Size = new Size(1921, 587);
            picSlide.TabIndex = 1;
            picSlide.TabStop = false;
            // 
            // timerSlide
            // 
            timerSlide.Enabled = true;
            timerSlide.Interval = 2500;
            timerSlide.Tick += timerSlide_Tick;
            // 
            // panelTests
            // 
            panelTests.Location = new Point(98, 780);
            panelTests.Name = "panelTests";
            panelTests.Size = new Size(1639, 410);
            panelTests.TabIndex = 2;
            panelTests.WrapContents = false;
            // 
            // panelLessons
            // 
            panelLessons.Location = new Point(98, 1363);
            panelLessons.Name = "panelLessons";
            panelLessons.Size = new Size(1639, 475);
            panelLessons.TabIndex = 3;
            panelLessons.WrapContents = false;
            // 
            // panelMain
            // 
            panelMain.AutoScroll = true;
            panelMain.AutoScrollMargin = new Size(0, 20);
            panelMain.Controls.Add(lblDetail);
            panelMain.Controls.Add(lblClass);
            panelMain.Controls.Add(lblTest);
            panelMain.Controls.Add(userNavbarPanel1);
            panelMain.Controls.Add(picSlide);
            panelMain.Controls.Add(panelTests);
            panelMain.Controls.Add(panelLessons);
            panelMain.Controls.Add(btnPrevTest);
            panelMain.Controls.Add(btnNextTest);
            panelMain.Controls.Add(btnPrevLesson);
            panelMain.Controls.Add(btnNextLesson);
            panelMain.Dock = DockStyle.Fill;
            panelMain.Location = new Point(0, 0);
            panelMain.Name = "panelMain";
            panelMain.Size = new Size(1902, 973);
            panelMain.TabIndex = 0;
            // 
            // lblDetail
            // 
            lblDetail.Font = new Font("Noto Serif SC", 10.1999989F, FontStyle.Bold, GraphicsUnit.Point, 163);
            lblDetail.ForeColor = Color.Gray;
            lblDetail.Location = new Point(73, 1298);
            lblDetail.Name = "lblDetail";
            lblDetail.Size = new Size(880, 29);
            lblDetail.TabIndex = 6;
            lblDetail.Text = "Build your confidence in all IELTS skills and prepare for studying abroad with our daily live lessons";
            // 
            // lblClass
            // 
            lblClass.Font = new Font("Noto Serif SC", 18F, FontStyle.Bold, GraphicsUnit.Point, 163);
            lblClass.ForeColor = Color.FromArgb(39, 56, 146);
            lblClass.Location = new Point(73, 1238);
            lblClass.Name = "lblClass";
            lblClass.Size = new Size(577, 44);
            lblClass.TabIndex = 5;
            lblClass.Text = "Join our live lessons for advice from the experts";
            // 
            // lblTest
            // 
            lblTest.Font = new Font("Noto Serif SC", 18F, FontStyle.Bold, GraphicsUnit.Point, 163);
            lblTest.ForeColor = Color.FromArgb(39, 56, 146);
            lblTest.Location = new Point(73, 704);
            lblTest.Name = "lblTest";
            lblTest.Size = new Size(440, 42);
            lblTest.TabIndex = 4;
            lblTest.Text = "Latest IELTS test releases:";

            
            // ---- btnPrevTest ----
            btnPrevTest.Text = "<";
            btnPrevTest.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            btnPrevTest.Size = new Size(50, 70);
            int testMidY = panelTests.Location.Y + (panelTests.Height / 2) - (btnPrevTest.Height / 2);
            btnPrevTest.Location = new Point(panelTests.Location.X - 60, testMidY); 
            btnPrevTest.Click += btnPrevTest_Click;

            // ---- btnNextTest ----
            btnNextTest.Text = ">";
            btnNextTest.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            btnNextTest.Size = new Size(50, 70);
            int testMidY2 = panelTests.Location.Y + (panelTests.Height / 2) - (btnNextTest.Height / 2);
            btnNextTest.Location = new Point(panelTests.Location.X + panelTests.Width + 10, testMidY2);
            btnNextTest.Click += btnNextTest_Click;

            int lessonMidY = panelLessons.Location.Y + (panelLessons.Height / 2) - (btnPrevLesson.Height / 2);

            // ---- btnPrevLesson ----
            btnPrevLesson.Text = "<";
            btnPrevLesson.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            btnPrevLesson.Size = new Size(50, 70);
            btnPrevLesson.Location = new Point(panelLessons.Location.X - 60, lessonMidY);
            btnPrevLesson.Click += btnPrevLesson_Click;

            // ---- btnNextLesson ----
            btnNextLesson.Text = ">";
            btnNextLesson.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            btnNextLesson.Size = new Size(50, 70);
            btnNextLesson.Location = new Point(panelLessons.Location.X + panelLessons.Width + 10, lessonMidY);
            btnNextLesson.Click += btnNextLesson_Click;


            // ADD CONTROLS TO MAIN PANEL
            panelMain.Controls.Add(userNavbarPanel1);
            panelMain.Controls.Add(picSlide);
            panelMain.Controls.Add(panelTests);
            panelMain.Controls.Add(panelLessons);

            panelMain.Controls.Add(btnPrevTest);
            panelMain.Controls.Add(btnNextTest);

            panelMain.Controls.Add(btnPrevLesson);
            panelMain.Controls.Add(btnNextLesson);
            // 
            // Home
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1902, 973);
            Controls.Add(panelMain);
            Name = "Home";
            Text = "Home";
            Load += Home_Load;
            ((System.ComponentModel.ISupportInitialize)picSlide).EndInit();
            panelMain.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion

        private UserNavbarPanel userNavbarPanel1;
        private FlowLayoutPanel panelTests;
        private FlowLayoutPanel panelLessons;
        private PictureBox picSlide;
        private System.Windows.Forms.Timer timerSlide;
        private Panel panelMain;
        private Sunny.UI.UILabel lblDetail;
        private Sunny.UI.UILabel lblClass;
        private Sunny.UI.UILabel lblTest;
        // buttons for Tests
        private Button btnPrevTest;
        private Button btnNextTest;

        // buttons for Lessons
        private Button btnPrevLesson;
        private Button btnNextLesson;
    }
}