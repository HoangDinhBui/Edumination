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
            SessionManager.ClearSession();
            MessageBox.Show("You have been logged out.");

            this.FindForm().Hide();  // Ẩn Home

            LoginForm login = new LoginForm();
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
            Form parent = this.FindForm();

            // nếu đang ở Home thì thôi
            if (parent is Home)
                return;

            // mở Home
            Home home = new Home();
            home.Show();

            // KHÔNG close form cũ → chỉ hide
            parent.Hide();
        }

        private void btnLibrary_Click(object sender, EventArgs e)
        {
            Form parent = this.FindForm();

            // Nếu form hiện tại đã là TestLibrary → không làm gì
            if (parent is IELTS.UI.User.TestLibrary.TestLibrary)
                return;

            // Mở TestLibrary mới
            var library = new IELTS.UI.User.TestLibrary.TestLibrary();
            library.Show();

            // Ẩn form cũ, KHÔNG Close tránh crash hoặc tắt app
            parent.Hide();
        }

        private void UserNavbarPanel_Load(object sender, EventArgs e)
        {

        }
    }
}
