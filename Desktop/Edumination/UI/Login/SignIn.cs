using Edumination.WinForms.UI.Admin;
using IELTS.BLL;
//using IELTS.UI.IELTS.UI;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IELTS.UI.Login
{
    public partial class SignIn : Form
    {
        private UserBLL userBLL = new UserBLL();
        public SignIn()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text;

            DataTable result = userBLL.Login(email, password);

            if (result.Rows.Count > 0)
            {
                // Lấy dữ liệu
                string fullName = result.Rows[0]["FullName"].ToString();
                string role = result.Rows[0]["Role"].ToString();

                // Lưu session
                SessionManager.Login(result.Rows[0]);
                UIMessageBox.ShowSuccess($"Chào mừng {fullName}!");

                // Ẩn form hiện tại
                this.Hide();

                // Mở form theo role
                Form nextForm;

                switch (role.ToUpper())
                {
                    case "ADMIN":
                        nextForm = new AdminMainForm(fullName, role);
                        nextForm.ShowDialog();
                        break;

                    case "STUDENT":
                        MessageBox.Show("Đã đăng nhập tư cách STUDENT");
                        //nextForm = new frmStudent(fullName, role);
                        break;

                    case "TEACHER":
                        MessageBox.Show("Đã đăng nhập tư cách TEACHER");
                        //nextForm = new frmTeacher(fullName, role);
                        break;

                    default:
                        UIMessageBox.ShowError("Không xác định được quyền người dùng!");
                        this.Show();
                        return;
                }

                // Hiển thị form
                

                // Sau khi đóng form tiếp theo thì đóng form login
                this.Close();
            }
            else
            {
                UIMessageBox.ShowError("Email hoặc mật khẩu không đúng!");
            }
        }


        private void btnRegister_Click(object sender, EventArgs e)
        {
            Register registerForm = new Register();
            registerForm.ShowDialog();
        }

    }
}
