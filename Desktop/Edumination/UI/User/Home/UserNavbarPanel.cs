using ReaLTaiizor.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IELTS.BLL;
using IELTS.DAL;
using IELTS.UI.Login;

namespace IELTS.UI.User.Home
{
    public partial class UserNavbarPanel : UserControl
    {
        private Form _parentForm;

        // Constructor mặc định để designer dùng
        public UserNavbarPanel() : this(null)
        {
        }

        // Constructor thật sự (có truyền form cha)
        public UserNavbarPanel(Form parentForm)
        {
            InitializeComponent();
            _parentForm = parentForm;
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            SessionManager.Logout();
            MessageBox.Show("You have been logged out.");

            this.FindForm().Hide();  // Ẩn Home

            SignIn login = new SignIn();
            if (login.ShowDialog() == DialogResult.OK)
            {
                // login lại thành công
                this.FindForm().Show();  // hiện Home lại
            }
            else
            {
                this.FindForm().Close(); // người dùng thoát login
            }
        }



        private void btnHome_Click(object sender, EventArgs e)
        {
            try
            {
                Form parent = this.FindForm();
                if (parent == null)
                {
                    MessageBox.Show("Parent form not found!", "Error");
                    return;
                }

                // nếu đang ở Home thì thôi
                if (parent is Home)
                    return;

                // mở Home
                Home home = new Home();
                home.Show();

                // KHÔNG close form cũ → chỉ hide
                // parent.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error navigating to Home: {ex.Message}\n{ex.StackTrace}", "Error");
            }
        }

        private void btnLibrary_Click(object sender, EventArgs e)
        {
            try
            {
                Form parent = this.FindForm();
                if (parent == null)
                {
                    MessageBox.Show("Parent form not found!", "Error");
                    return;
                }

                // Nếu form hiện tại đã là TestLibrary → không làm gì
                if (parent is IELTS.UI.User.TestLibrary.TestLibrary)
                    return;

                // Mở TestLibrary mới
                var library = new IELTS.UI.User.TestLibrary.TestLibrary();
                library.Show();

                // Ẩn form cũ, KHÔNG Close tránh crash hoặc tắt app
                // parent.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error navigating to Library: {ex.Message}\n{ex.StackTrace}", "Error");
            }
        }

        private void UserNavbarPanel_Load(object sender, EventArgs e)
        {

        }

        private void btnCourse_Click(object sender, EventArgs e)
        {
            try
            {
                Form parent = this.FindForm();
                if (parent == null)
                {
                    MessageBox.Show("Parent form not found!", "Error");
                    return;
                }

                // Nếu đang ở CoursesForm thì thôi
                if (parent is IELTS.UI.User.Courses.CoursesForm)
                    return;

                var f = new IELTS.UI.User.Courses.CoursesForm();
                f.Show();
                // parent.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error navigating to Courses: {ex.Message}\n{ex.StackTrace}", "Error");
            }
        }
        public void SetActive(string key)
        {
            // reset colors
            btnHome.ForeColor = Color.FromArgb(100, 100, 100);
            btnLibrary.ForeColor = Color.FromArgb(100, 100, 100);
            btnCourse.ForeColor = Color.FromArgb(100, 100, 100);
            btnAvt.ForeColor = Color.FromArgb(100, 100, 100);

            // highlight active
            switch (key.ToLower())
            {
                case "home":
                    btnHome.ForeColor = Color.FromArgb(25, 41, 88);
                    break;
                case "library":
                    btnLibrary.ForeColor = Color.FromArgb(25, 41, 88);
                    break;
                case "courses":
                    btnCourse.ForeColor = Color.FromArgb(25, 41, 88);
                    break;
                case "profile":
                    btnAvt.ForeColor = Color.FromArgb(25, 41, 88);
                    break;
            }
        }

    }
}
