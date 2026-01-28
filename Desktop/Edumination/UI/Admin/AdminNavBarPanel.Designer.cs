using Sunny.UI;
using System.Drawing;
using System.Windows.Forms;

namespace Edumination.WinForms.UI.Admin
{
    partial class AdminNavBarPanel
    {
        private System.ComponentModel.IContainer components = null;

        private UIPanel sidebar;
        private FlowLayoutPanel menu;

        private UIButton btnDashboard;
        private UIButton btnCourses;
        private UIButton btnTests;
        private UIButton btnStudents;
        private UIButton btnAccounts;
        private UIButton btnReports;
        private UIButton btnSettings;
        private UIButton btnLogout;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            sidebar = new UIPanel();
            menu = new FlowLayoutPanel();
            sidebar.SuspendLayout();
            SuspendLayout();
            // 
            // sidebar
            // 
            sidebar.Controls.Add(menu);
            sidebar.Dock = DockStyle.Left;
            sidebar.FillColor = Color.FromArgb(245, 247, 251);
            sidebar.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 163);
            sidebar.Location = new Point(0, 0);
            sidebar.Margin = new Padding(4, 5, 4, 5);
            sidebar.MinimumSize = new Size(1, 1);
            sidebar.Name = "sidebar";
            sidebar.Padding = new Padding(12, 16, 12, 16);
            sidebar.RectColor = Color.FromArgb(228, 232, 240);
            sidebar.RectSides = ToolStripStatusLabelBorderSides.Right;
            sidebar.Size = new Size(260, 1000);
            sidebar.TabIndex = 0;
            sidebar.Text = null;
            sidebar.TextAlignment = ContentAlignment.MiddleCenter;
            // 
            // menu
            // 
            menu.AutoScroll = true;
            menu.BackColor = Color.FromArgb(0, 0, 0, 0);
            menu.Dock = DockStyle.Fill;
            menu.FlowDirection = FlowDirection.TopDown;
            menu.Location = new Point(12, 16);
            menu.Name = "menu";
            menu.Padding = new Padding(0, 10, 0, 0);
            menu.Size = new Size(236, 968);
            menu.TabIndex = 0;
            menu.WrapContents = false;
            // 
            // AdminNavBarPanel
            // 
            Controls.Add(sidebar);
            Name = "AdminNavBarPanel";
            Size = new Size(260, 1000);
            sidebar.ResumeLayout(false);
            ResumeLayout(false);
        }

        
    }
}
