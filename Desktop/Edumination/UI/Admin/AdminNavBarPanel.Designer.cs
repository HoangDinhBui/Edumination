namespace Edumination.WinForms.UI.Admin
{
    partial class AdminNavBarPanel
    {
        private System.ComponentModel.IContainer components = null;

        private Label lblBrand;
        private Button btnDashboard;
        private Button btnCourses;
        private Button btnTests;
        private Button btnStudents;
        private Button btnTeachers;
        private Button btnAccounts;
        private Button btnReports;
        private Button btnSettings;
        private Button btnLogout;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private FlowLayoutPanel flowMenu;

        private void InitializeComponent()
        {
            flowMenu = new FlowLayoutPanel();
            lblBrand = new Label();
            btnDashboard = new Button();
            btnCourses = new Button();
            btnTests = new Button();
            btnStudents = new Button();
            btnTeachers = new Button();
            btnAccounts = new Button();
            btnReports = new Button();
            btnSettings = new Button();
            btnLogout = new Button();

            SuspendLayout();

            // flowMenu
            flowMenu.Dock = DockStyle.Fill;
            flowMenu.FlowDirection = FlowDirection.TopDown;
            flowMenu.WrapContents = false;
            flowMenu.AutoScroll = true;
            flowMenu.Padding = new Padding(0, 120, 0, 0);
            flowMenu.BackColor = Color.FromArgb(41, 69, 99);

            // lblBrand
            lblBrand.AutoSize = true;
            lblBrand.Font = new Font("Microsoft Sans Serif", 19F, FontStyle.Bold);
            lblBrand.ForeColor = Color.White;
            lblBrand.Location = new Point(20, 20);
            lblBrand.Text = "Edumination";

            // Add buttons
            AddMenuButton(btnDashboard, "📊  Dashboard");
            AddMenuButton(btnCourses, "📚  Quản lý khóa học");
            AddMenuButton(btnTests, "📝  Quản lý bài test");
            AddMenuButton(btnStudents, "👨‍🎓  Quản lý sinh viên");
            AddMenuButton(btnTeachers, "👨‍🏫  Quản lý giảng viên");
            AddMenuButton(btnAccounts, "👤  Quản lý tài khoản");
            AddMenuButton(btnReports, "📈  Báo cáo thống kê");
            AddMenuButton(btnSettings, "⚙️  Cài đặt");
            AddMenuButton(btnLogout, "🚪  Đăng xuất");

            // Add all to flow panel
            flowMenu.Controls.Add(btnDashboard);
            flowMenu.Controls.Add(btnCourses);
            flowMenu.Controls.Add(btnTests);
            flowMenu.Controls.Add(btnStudents);
            flowMenu.Controls.Add(btnTeachers);
            flowMenu.Controls.Add(btnAccounts);
            flowMenu.Controls.Add(btnReports);
            flowMenu.Controls.Add(btnSettings);
            flowMenu.Controls.Add(btnLogout);

            // Add to main control
            Controls.Add(flowMenu);
            Controls.Add(lblBrand);

            Name = "AdminNavBarPanel";
            Size = new Size(250, 1020);

            ResumeLayout(false);
            PerformLayout();
        }

        private void AddMenuButton(Button btn, string text)
        {
            btn.Text = text;
            btn.Width = 250;
            btn.Height = 48;
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatStyle = FlatStyle.Flat;
            btn.ForeColor = Color.White;
            btn.BackColor = Color.FromArgb(41, 69, 99);
            btn.TextAlign = ContentAlignment.MiddleLeft;
            btn.Padding = new Padding(20, 0, 0, 0);
        }


        //private void AddMenuButton(Button btn, string text)
        //{
        //    btn.Text = text;
        //    btn.Width = 250;
        //    btn.Height = 48;
        //    btn.FlatAppearance.BorderSize = 0;
        //    btn.FlatStyle = FlatStyle.Flat;
        //    btn.ForeColor = Color.White;
        //    btn.BackColor = Color.FromArgb(41, 69, 99);
        //    btn.TextAlign = ContentAlignment.MiddleLeft;
        //    btn.Padding = new Padding(20, 0, 0, 0);
        //}

    }
}
