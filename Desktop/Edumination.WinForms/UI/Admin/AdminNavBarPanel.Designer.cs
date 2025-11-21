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
            btnDashboard = new Button();
            btnCourses = new Button();
            btnTests = new Button();
            btnStudents = new Button();
            btnTeachers = new Button();
            btnAccounts = new Button();
            btnReports = new Button();
            btnSettings = new Button();
            btnLogout = new Button();
            lblBrand = new Label();
            flowMenu.SuspendLayout();
            SuspendLayout();
            // 
            // flowMenu
            // 
            flowMenu.AutoScroll = true;
            flowMenu.BackColor = Color.FromArgb(41, 69, 99);
            flowMenu.Controls.Add(btnDashboard);
            flowMenu.Controls.Add(btnCourses);
            flowMenu.Controls.Add(btnTests);
            flowMenu.Controls.Add(btnStudents);
            flowMenu.Controls.Add(btnTeachers);
            flowMenu.Controls.Add(btnAccounts);
            flowMenu.Controls.Add(btnReports);
            flowMenu.Controls.Add(btnSettings);
            flowMenu.Controls.Add(btnLogout);
            flowMenu.Dock = DockStyle.Fill;
            flowMenu.FlowDirection = FlowDirection.TopDown;
            flowMenu.Location = new Point(0, 0);
            flowMenu.Name = "flowMenu";
            flowMenu.Padding = new Padding(0, 120, 0, 0);
            flowMenu.Size = new Size(250, 1000);
            flowMenu.TabIndex = 0;
            flowMenu.WrapContents = false;
            flowMenu.Paint += flowMenu_Paint;
            // 
            // btnDashboard
            // 
            btnDashboard.Location = new Point(3, 123);
            btnDashboard.Name = "btnDashboard";
            btnDashboard.Size = new Size(75, 23);
            btnDashboard.TabIndex = 0;
            // 
            // btnCourses
            // 
            btnCourses.Location = new Point(3, 152);
            btnCourses.Name = "btnCourses";
            btnCourses.Size = new Size(75, 23);
            btnCourses.TabIndex = 1;
            // 
            // btnTests
            // 
            btnTests.Location = new Point(3, 181);
            btnTests.Name = "btnTests";
            btnTests.Size = new Size(75, 23);
            btnTests.TabIndex = 2;
            // 
            // btnStudents
            // 
            btnStudents.Location = new Point(3, 210);
            btnStudents.Name = "btnStudents";
            btnStudents.Size = new Size(75, 23);
            btnStudents.TabIndex = 3;
            // 
            // btnTeachers
            // 
            btnTeachers.Location = new Point(3, 239);
            btnTeachers.Name = "btnTeachers";
            btnTeachers.Size = new Size(75, 23);
            btnTeachers.TabIndex = 4;
            // 
            // btnAccounts
            // 
            btnAccounts.Location = new Point(3, 268);
            btnAccounts.Name = "btnAccounts";
            btnAccounts.Size = new Size(75, 23);
            btnAccounts.TabIndex = 5;
            // 
            // btnReports
            // 
            btnReports.Location = new Point(3, 297);
            btnReports.Name = "btnReports";
            btnReports.Size = new Size(75, 23);
            btnReports.TabIndex = 6;
            // 
            // btnSettings
            // 
            btnSettings.Location = new Point(3, 326);
            btnSettings.Name = "btnSettings";
            btnSettings.Size = new Size(75, 23);
            btnSettings.TabIndex = 7;
            // 
            // btnLogout
            // 
            btnLogout.Location = new Point(3, 355);
            btnLogout.Name = "btnLogout";
            btnLogout.Size = new Size(75, 23);
            btnLogout.TabIndex = 8;
            // 
            // lblBrand
            // 
            lblBrand.AutoSize = true;
            lblBrand.Font = new Font("Microsoft Sans Serif", 19F, FontStyle.Bold);
            lblBrand.ForeColor = Color.White;
            lblBrand.Location = new Point(20, 20);
            lblBrand.Name = "lblBrand";
            lblBrand.Size = new Size(207, 37);
            lblBrand.TabIndex = 1;
            lblBrand.Text = "Edumination";
            // 
            // AdminNavBarPanel
            // 
            Controls.Add(flowMenu);
            Controls.Add(lblBrand);
            Name = "AdminNavBarPanel";
            Size = new Size(250, 1000);
            flowMenu.ResumeLayout(false);
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
