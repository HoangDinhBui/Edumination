using IELTS.BLL;
using IELTS.DTO;
using Sunny.UI;
using System;
using System.Windows.Forms;

namespace IELTS.UI.Login
{
    public partial class ForgotPasswordForm : Form
    {
        private UserBLL userBLL = new UserBLL();
        private string currentEmail = "";
        private bool otpSent = false;

        public ForgotPasswordForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gửi OTP qua email
        /// </summary>
        private void btnSendOtp_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();

            // Validate
            if (string.IsNullOrWhiteSpace(email))
            {
                UIMessageBox.ShowError("Vui lòng nhập email!");
                txtEmail.Focus();
                return;
            }

            if (!email.Contains("@"))
            {
                UIMessageBox.ShowError("Email không hợp lệ!");
                txtEmail.Focus();
                return;
            }

            // Hiển thị loading
            btnSendOtp.Enabled = false;
            btnSendOtp.Text = "Đang gửi...";
            this.Cursor = Cursors.WaitCursor;

            try
            {
                // Gọi API gửi OTP
                var response = userBLL.SendForgotPasswordOtp(email);

                if (response.Success)
                {
                    UIMessageBox.ShowSuccess(response.Message);
                    
                    // Lưu email và chuyển sang bước nhập OTP
                    currentEmail = email;
                    otpSent = true;

                    // Enable các field OTP và password
                    txtOtp.Enabled = true;
                    txtNewPassword.Enabled = true;
                    txtConfirmPassword.Enabled = true;
                    btnResetPassword.Enabled = true;

                    // Disable email field
                    txtEmail.Enabled = false;
                    btnSendOtp.Text = "Gửi lại OTP";

                    // Focus vào OTP field
                    txtOtp.Focus();
                }
                else
                {
                    UIMessageBox.ShowError(response.Message);
                }
            }
            catch (Exception ex)
            {
                UIMessageBox.ShowError($"Lỗi: {ex.Message}");
            }
            finally
            {
                btnSendOtp.Enabled = true;
                if (!otpSent)
                    btnSendOtp.Text = "Gửi mã OTP";
                this.Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// Reset password sau khi nhập OTP
        /// </summary>
        private void btnResetPassword_Click(object sender, EventArgs e)
        {
            string otpCode = txtOtp.Text.Trim();
            string newPassword = txtNewPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;

            // Validate
            if (string.IsNullOrWhiteSpace(otpCode))
            {
                UIMessageBox.ShowError("Vui lòng nhập mã OTP!");
                txtOtp.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(newPassword))
            {
                UIMessageBox.ShowError("Vui lòng nhập mật khẩu mới!");
                txtNewPassword.Focus();
                return;
            }

            if (newPassword.Length < 6)
            {
                UIMessageBox.ShowError("Mật khẩu phải có ít nhất 6 ký tự!");
                txtNewPassword.Focus();
                return;
            }

            if (newPassword != confirmPassword)
            {
                UIMessageBox.ShowError("Mật khẩu xác nhận không khớp!");
                txtConfirmPassword.Focus();
                return;
            }

            // Hiển thị loading
            btnResetPassword.Enabled = false;
            btnResetPassword.Text = "Đang xử lý...";
            this.Cursor = Cursors.WaitCursor;

            try
            {
                // Gọi API reset password
                var response = userBLL.ResetPassword(currentEmail, otpCode, newPassword, confirmPassword);

                if (response.Success)
                {
                    UIMessageBox.ShowSuccess(response.Message);
                    
                    // Đóng form
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    UIMessageBox.ShowError(response.Message);
                }
            }
            catch (Exception ex)
            {
                UIMessageBox.ShowError($"Lỗi: {ex.Message}");
            }
            finally
            {
                btnResetPassword.Enabled = true;
                btnResetPassword.Text = "Đặt lại mật khẩu";
                this.Cursor = Cursors.Default;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
