using Edumination.WinForms.UI.Admin;
using IELTS.BLL;
using IELTS.DTO;
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

        /// <summary>
        /// Xử lý login bằng cách gọi API
        /// </summary>
        private void btnLogin_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text;

            // Validate input
            if (string.IsNullOrWhiteSpace(email))
            {
                UIMessageBox.ShowError("Vui lòng nhập email!");
                txtEmail.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                UIMessageBox.ShowError("Vui lòng nhập mật khẩu!");
                txtPassword.Focus();
                return;
            }

            // Hiển thị loading
            btnLogin.Enabled = false;
            btnLogin.Text = "Đang đăng nhập...";
            this.Cursor = Cursors.WaitCursor;

            try
            {
                // Gọi API Login (sử dụng BLL trực tiếp)
                LoginResponseDTO response = userBLL.LoginWithToken(email, password);

                if (response.Success)
                {
                    // Lưu thông tin vào SessionManager
                    SaveToSession(response);

                    // Hiển thị thông báo thành công
                    UIMessageBox.ShowSuccess(response.Message);

                    // Log thông tin (cho debug)
                    Console.WriteLine($"Login successful!");
                    Console.WriteLine($"User: {response.User.FullName}");
                    Console.WriteLine($"Role: {response.User.Role}");
                    Console.WriteLine($"Token: {response.Token.Substring(0, 20)}...");

                    // Ẩn form hiện tại
                    this.Hide();

                    // Mở form theo role
                    OpenFormByRole(response.User);

                    // Sau khi đóng form tiếp theo thì đóng form login
                    this.Close();
                }
                else
                {
                    UIMessageBox.ShowError(response.Message);
                }
            }
            catch (Exception ex)
            {
                UIMessageBox.ShowError($"Lỗi kết nối: {ex.Message}");
            }
            finally
            {
                // Reset button
                btnLogin.Enabled = true;
                btnLogin.Text = "Đăng nhập";
                this.Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// Lưu thông tin login vào SessionManager
        /// </summary>
        private void SaveToSession(LoginResponseDTO response)
        {
            // Tạo DataRow để lưu vào SessionManager (tương thích với code cũ)
            DataTable dt = new DataTable();
            dt.Columns.Add("Id", typeof(long));
            dt.Columns.Add("Email", typeof(string));
            dt.Columns.Add("FullName", typeof(string));
            dt.Columns.Add("Role", typeof(string));
            dt.Columns.Add("IsActive", typeof(bool));
            dt.Columns.Add("Token", typeof(string)); // Thêm token

            DataRow row = dt.NewRow();
            row["Id"] = response.User.Id;
            row["Email"] = response.User.Email;
            row["FullName"] = response.User.FullName;
            row["Role"] = response.User.Role;
            row["IsActive"] = response.User.IsActive;
            row["Token"] = response.Token;

            dt.Rows.Add(row);

            // Lưu vào SessionManager
            SessionManager.Login(row);

            // Lưu token riêng (nếu cần dùng cho các API call sau này)
            SessionManager.SetToken(response.Token);
        }

        /// <summary>
        /// Mở form tương ứng với role
        /// </summary>
        private void OpenFormByRole(UserDTO user)
        {
            Form nextForm;

            switch (user.Role.ToUpper())
            {
                case "ADMIN":
                    nextForm = new AdminMainForm(user.FullName, user.Role);
                    nextForm.ShowDialog();
                    break;

                case "STUDENT":
                    // MessageBox.Show($"Đã đăng nhập với tư cách STUDENT\nEmail: {user.Email}\nID: {user.Id}");
                    nextForm = new IELTS.UI.User.Home.Home();
                    nextForm.ShowDialog();
                    break;

                case "TEACHER":
                    // MessageBox.Show($"Đã đăng nhập với tư cách TEACHER\nEmail: {user.Email}\nID: {user.Id}");
                    nextForm = new IELTS.UI.User.Home.Home();
                    nextForm.ShowDialog();
                    break;

                default:
                    UIMessageBox.ShowError("Không xác định được quyền người dùng!");
                    this.Show();
                    return;
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            Register registerForm = new Register();
            registerForm.ShowDialog();
        }

        /// <summary>
        /// Xử lý khi click vào link "Quên mật khẩu?"
        /// </summary>
        private void lnkForgotPassword_Click(object sender, EventArgs e)
        {
            ForgotPasswordForm forgotForm = new ForgotPasswordForm();
            forgotForm.ShowDialog();
        }

    }
}
