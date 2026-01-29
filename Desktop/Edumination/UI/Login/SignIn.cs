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
        private List<Image> slides;
        private int currentSlide = 0;
        public SignIn()
        {
            InitializeComponent();
            slides = new List<Image>
            {
                Image.FromFile("assets/img/adv.jpg"),
                Image.FromFile("assets/img/adv1.jpg"),
                Image.FromFile("assets/img/adv2.jpg")
            };

            pictureBoxSlide.Image = slides[0];
            timerSlide.Start();

            txtEmail.Text= "admin3@edumination.com";
            txtPassword.Text = "admin123";

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
                    // Kiểm tra tên trong đối tượng UserDTO trước khi lưu vào Session
                    Console.WriteLine($"[DEBUG LOGIN] Tên từ Response: {response.User.FullName}");
                }

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
            DataTable dt = new DataTable();
            dt.Columns.Add("Id", typeof(long));
            dt.Columns.Add("Email", typeof(string));
            dt.Columns.Add("FullName", typeof(string));
            dt.Columns.Add("Role", typeof(string));
            dt.Columns.Add("IsActive", typeof(bool));
            dt.Columns.Add("Token", typeof(string));

            DataRow row = dt.NewRow();
            row["Id"] = response.User.Id;
            row["Email"] = response.User.Email;
            row["FullName"] = response.User.FullName;
            row["Role"] = response.User.Role;
            row["IsActive"] = response.User.IsActive;
            row["Token"] = response.Token;

            dt.Rows.Add(row);
            SessionManager.Login(row);
        }

        /// <summary>
        /// Mở form tương ứng với role
        /// </summary>
        private void OpenFormByRole(UserDTO user)
        {
			this.Hide(); // 1. Ẩn form Login hiện tại đi

			Form nextForm = null;

			// 2. Khởi tạo form tương ứng dựa trên Role
			switch (user.Role.ToUpper())
			{
				case "ADMIN":
					nextForm = new AdminMainForm(user);
					break;

				case "STUDENT":
				case "TEACHER":
					nextForm = new IELTS.UI.User.Home.Home();
					break;

				default:
					UIMessageBox.ShowError("Không xác định được quyền người dùng!");
					this.Show(); // Hiện lại login nếu lỗi role
					return;
			}

			if (nextForm != null)
			{
				// 3. Mở form mới bằng ShowDialog
				// Code sẽ "đứng" tại dòng này cho đến khi form 'nextForm' bị đóng (Logout)
				nextForm.ShowDialog();

				// 4. SAU KHI LOGOUT (Form chính đóng lại)
				// Code chạy tiếp xuống đây:
				this.Show();            // Hiện lại form Login
				txtPassword.Clear();    // Xóa mật khẩu cũ cho bảo mật
				txtPassword.Focus();    // Đưa con trỏ vào ô mật khẩu
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

        private void TimerSlide_Tick(object sender, EventArgs e)
        {
            currentSlide = (currentSlide + 1) % slides.Count;
            pictureBoxSlide.Image = slides[currentSlide];
        }

        private void picEmail_Click(object sender, EventArgs e)
        {

        }

        private void panelLogin_Click(object sender, EventArgs e)
        {

        }

        private void SignIn_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }
    }
}
