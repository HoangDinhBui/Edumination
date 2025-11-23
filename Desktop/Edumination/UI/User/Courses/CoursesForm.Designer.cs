using System.Drawing;
using System.Windows.Forms;

namespace IELTS.UI.User.Courses
{
    partial class CoursesForm
    {
        private Panel panelNavbar;
        private FlowLayoutPanel flowCourses;
        private Label lblTitle;

        private void InitializeComponent()
        {
            this.panelNavbar = new Panel();
            this.flowCourses = new FlowLayoutPanel();
            this.lblTitle = new Label();

            // panelNavbar
            panelNavbar.Dock = DockStyle.Top;
            panelNavbar.Height = 70;
            panelNavbar.BackColor = Color.White;

            // FlowPanel
            flowCourses.Dock = DockStyle.Fill;
            flowCourses.FlowDirection = FlowDirection.LeftToRight;
            flowCourses.WrapContents = true;
            flowCourses.AutoScroll = true;
            flowCourses.Padding = new Padding(40, 90, 40, 40);
            flowCourses.BackColor = Color.Transparent;

            // lblTitle
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Noto Serif SC", 22F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(25, 41, 88);
            lblTitle.Text = "IELTS Courses";
            lblTitle.Location = new Point(40, 80); // dưới navbar

            // Form
            this.Text = "IELTS Courses";
            this.BackColor = Color.FromArgb(247, 249, 252);
            this.WindowState = FormWindowState.Maximized;

            this.Controls.Add(flowCourses);
            this.Controls.Add(lblTitle);
            this.Controls.Add(panelNavbar);

            this.Load += CoursesForm_Load;
        }
    }
}
