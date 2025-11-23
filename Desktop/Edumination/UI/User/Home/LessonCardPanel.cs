using System;
using System.Drawing;
using System.Windows.Forms;

namespace IELTS.UI.User.Home
{
    public partial class LessonCardPanel : UserControl
    {
        public LessonCardPanel()
        {
            InitializeComponent();

            this.Click += OpenCourses;
            foreach (Control c in this.Controls)
                c.Click += OpenCourses;
        }

        public Image Thumbnail { get => picThumb.Image; set => picThumb.Image = value; }
        public string Category { get => lblCategory.Text; set => lblCategory.Text = value; }
        public string TitleText { get => lblTitle.Text; set => lblTitle.Text = value; }
        public string TimeText { get => lblTime.Text; set => lblTime.Text = value; }
        public string Attending { get => lblAttending.Text; set => lblAttending.Text = value; }

        private void OpenCourses(object sender, EventArgs e)
        {
            var form = new IELTS.UI.User.Courses.CoursesForm();
            form.Show();

            Form parent = this.FindForm();
            parent?.Hide();
        }
    }
}
