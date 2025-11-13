//using System;
//using System.Drawing;
//using System.Windows.Forms;
//using ReaLTaiizor.Controls;

//namespace Edumination.WinForms.UI.Forms.DashBoard
//{
//    partial class MainDashboardForm : Form
//    {
//        private System.ComponentModel.IContainer components = null;

//        private HopeTabPage tabControlMain;
//        private System.Windows.Forms.TabPage tabPageTestManagement;
//        private System.Windows.Forms.TabPage tabPageQuestionManagement;
//        private System.Windows.Forms.TabPage tabPageSubmissions;
//        private System.Windows.Forms.TabPage tabPageUserManagement;
//        private System.Windows.Forms.TabPage tabPageReport;

//        private HopeButton btnLogout;

//        // Controls cho tab “Quản lý Bài Test”
//        private HopeRoundPanel panelTestHeader;
//        private HopeLabel lblTitle;
//        private HopeRoundPanel panelSkillFilters;
//        private HopeButton btnAllSkills;
//        private HopeButton btnListening;
//        private HopeButton btnReading;
//        private HopeButton btnWriting;
//        private HopeButton btnSpeaking;
//        private HopeTextBox txtSearch;
//        private HopeButton btnSearch;
//        private HopeComboBox comboLatest;
//        private HopeRoundPanel panelMockTests;
//        private HopeLabel lblMockTest2025;
//        private HopeButton btnQuarter1;
//        private HopeButton btnQuarter2;
//        private HopeButton btnQuarter3;
//        private HopeButton btnQuarter4;
//        private HopeButton btnViewMore;
//        private HopeDataGridView dgvTests;

//        protected override void Dispose(bool disposing)
//        {
//            if (disposing && components != null)
//                components.Dispose();
//            base.Dispose(disposing);
//        }

//        private void InitializeComponent()
//        {
//            this.components = new System.ComponentModel.Container();

//            // --- Main TabControl ---
//            this.tabControlMain = new HopeTabPage();
//            //this.tabPageTestManagement = new TabPage();
//            //this.tabPageQuestionManagement = new TabPage();
//            //this.tabPageSubmissions = new TabPage();
//            //this.tabPageUserManagement = new TabPage();
//            //this.tabPageReport = new TabPage();

//            this.btnLogout = new HopeButton();

//            // --- TabControl Config ---
//            this.tabControlMain.Controls.Add(this.tabPageTestManagement);
//            this.tabControlMain.Controls.Add(this.tabPageQuestionManagement);
//            this.tabControlMain.Controls.Add(this.tabPageSubmissions);
//            this.tabControlMain.Controls.Add(this.tabPageUserManagement);
//            this.tabControlMain.Controls.Add(this.tabPageReport);
//            this.tabControlMain.Dock = DockStyle.Fill;
//            this.tabControlMain.Location = new Point(0, 0);
//            this.tabControlMain.SelectedIndex = 0;
//            this.tabControlMain.Size = new Size(1000, 700);
//            this.tabControlMain.BaseColor = Color.FromArgb(242, 245, 250);
//            //this.tabControlMain.ActiveColor = Color.FromArgb(85, 153, 255);
//            this.tabControlMain.Font = new Font("Segoe UI", 10F);

//            // --- Tab Names ---
//            this.tabPageTestManagement.Text = "Quản lý Bài Test";
//            this.tabPageQuestionManagement.Text = "Quản lý Câu Hỏi";
//            this.tabPageSubmissions.Text = "Bài Nộp Học Viên";
//            this.tabPageUserManagement.Text = "Quản lý User (Admin)";
//            this.tabPageReport.Text = "Thống kê / Report";

//            // --- TabPage TestManagement ---
//            this.tabPageTestManagement.BackColor = Color.FromArgb(242, 245, 250);
//            this.tabPageTestManagement.SuspendLayout();

//            // Header panel
//            this.panelTestHeader = new HopeRoundPanel();
//            this.panelTestHeader.BaseColor = Color.White;
//            this.panelTestHeader.Size = new Size(992, 120);
//            this.panelTestHeader.Location = new Point(0, 0);

//            this.lblTitle = new HopeLabel();
//            this.lblTitle.Text = "IELTS Test Papers Library";
//            this.lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
//            this.lblTitle.Location = new Point(30, 40);
//            this.panelTestHeader.Controls.Add(this.lblTitle);

//            this.tabPageTestManagement.Controls.Add(this.panelTestHeader);

//            // Panel filters
//            this.panelSkillFilters = new HopeRoundPanel();
//            this.panelSkillFilters.BaseColor = Color.White;
//            this.panelSkillFilters.Size = new Size(950, 60);
//            this.panelSkillFilters.Location = new Point(20, 140);
//            this.panelSkillFilters.Radius = 8;

//            this.btnAllSkills = CreateFilterButton("All Skills", new Point(10, 15), true);
//            this.btnListening = CreateFilterButton("Listening", new Point(140, 15));
//            this.btnReading = CreateFilterButton("Reading", new Point(270, 15));
//            this.btnWriting = CreateFilterButton("Writing", new Point(400, 15));
//            this.btnSpeaking = CreateFilterButton("Speaking", new Point(530, 15));

//            this.panelSkillFilters.Controls.AddRange(new Control[]
//            {
//                this.btnAllSkills,
//                this.btnListening,
//                this.btnReading,
//                this.btnWriting,
//                this.btnSpeaking
//            });
//            this.tabPageTestManagement.Controls.Add(this.panelSkillFilters);

//            // Search box
//            this.txtSearch = new HopeTextBox();
//            this.txtSearch.Location = new Point(20, 220);
//            this.txtSearch.Size = new Size(400, 35);
//            this.txtSearch.Hint = "Search...";
//            this.tabPageTestManagement.Controls.Add(this.txtSearch);

//            this.btnSearch = new HopeButton();
//            this.btnSearch.Text = "🔎";
//            this.btnSearch.Location = new Point(430, 220);
//            this.btnSearch.Size = new Size(40, 35);
//            this.tabPageTestManagement.Controls.Add(this.btnSearch);

//            this.comboLatest = new HopeComboBox();
//            this.comboLatest.Items.AddRange(new object[] { "Latest", "Oldest", "Popular" });
//            this.comboLatest.SelectedIndex = 0;
//            this.comboLatest.Location = new Point(820, 220);
//            this.comboLatest.Size = new Size(150, 35);
//            this.tabPageTestManagement.Controls.Add(this.comboLatest);

//            // Panel mock tests
//            this.panelMockTests = new HopeRoundPanel();
//            this.panelMockTests.BaseColor = Color.White;
//            this.panelMockTests.Radius = 8;
//            this.panelMockTests.Size = new Size(950, 300);
//            this.panelMockTests.Location = new Point(20, 280);

//            this.lblMockTest2025 = new HopeLabel();
//            this.lblMockTest2025.Text = "IELTS Mock Test 2025";
//            this.lblMockTest2025.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
//            this.lblMockTest2025.Location = new Point(20, 20);
//            this.panelMockTests.Controls.Add(this.lblMockTest2025);

//            this.btnQuarter1 = CreateQuarterButton("Quarter 1\n⚡ 951,605 tests taken", new Point(20, 70));
//            this.btnQuarter2 = CreateQuarterButton("Quarter 2\n⚡ 851,320 tests taken", new Point(250, 70));
//            this.btnQuarter3 = CreateQuarterButton("Quarter 3\n⚡ 703,412 tests taken", new Point(480, 70));
//            this.btnQuarter4 = CreateQuarterButton("Quarter 4\n⚡ 622,899 tests taken", new Point(710, 70));
//            this.btnViewMore = new HopeButton()
//            {
//                Text = "View more 2 tests",
//                Location = new Point(400, 260),
//                Size = new Size(150, 30),
//                PrimaryColor = Color.Transparent,
//                ForeColor = Color.FromArgb(85, 153, 255),
//                Font = new Font("Segoe UI", 9F, FontStyle.Underline)
//            };

//            this.panelMockTests.Controls.AddRange(new Control[]
//            {
//                this.btnQuarter1, this.btnQuarter2,
//                this.btnQuarter3, this.btnQuarter4,
//                this.btnViewMore
//            });
//            this.tabPageTestManagement.Controls.Add(this.panelMockTests);

//            this.tabPageTestManagement.ResumeLayout(false);

//            // Logout button
//            this.btnLogout.Text = "Logout";
//            this.btnLogout.Location = new Point(880, 10);
//            this.btnLogout.Size = new Size(100, 35);
//            this.btnLogout.PrimaryColor = Color.FromArgb(255, 100, 100);
//            this.btnLogout.ForeColor = Color.White;
//            this.btnLogout.Click += new EventHandler(this.realButtonLogout_Click);

//            // --- Form Config ---
//            this.ClientSize = new Size(1000, 700);
//            this.Controls.Add(this.btnLogout);
//            this.Controls.Add(this.tabControlMain);
//            this.Text = "EDM IELTS Admin Dashboard";
//            this.Load += new EventHandler(this.MainDashboardForm_Load);
//        }

//        private HopeButton CreateFilterButton(string text, Point loc, bool active = false)
//        {
//            var btn = new HopeButton
//            {
//                Text = text,
//                Location = loc,
//                Size = new Size(120, 30),
//                PrimaryColor = active ? Color.FromArgb(85, 153, 255) : Color.White,
//                ForeColor = active ? Color.White : Color.Gray,
//                Font = new Font("Segoe UI", 9F)
//            };
//            return btn;
//        }

//        private HopeButton CreateQuarterButton(string text, Point loc)
//        {
//            var btn = new HopeButton
//            {
//                Text = text,
//                Location = loc,
//                Size = new Size(220, 100),
//                PrimaryColor = Color.FromArgb(242, 245, 250),
//                ForeColor = Color.Black,
//                Font = new Font("Segoe UI", 10F, FontStyle.Regular),
//                //TextAlign = ContentAlignment.TopLeft
//            };
//            return btn;
//        }
//    }
//}
