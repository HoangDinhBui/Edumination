using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Windows.Forms;

namespace Edumination.WinForms.UI.Forms.Login
{
    public partial class EnterOtpPanel : UserControl
    {
        private readonly LoginForm _parentForm;
        private readonly string _email;

        public EnterOtpPanel(LoginForm parent, string email)
        {
            InitializeComponent();
            _parentForm = parent;
            _email = email;

            // Gán sự kiện
            btnVerify.Click += BtnVerify_Click;
            btnBack.Click += BtnBack_Click;

            // Tự động focus và chuyển giữa các ô OTP
            for (int i = 0; i < txtOtp.Length; i++)
            {
                int index = i;
                txtOtp[i].KeyPress += (s, e) =>
                {
                    // Chỉ cho phép nhập số
                    if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                        e.Handled = true;
                };

                txtOtp[i].TextChanged += (s, e) =>
                {
                    // Tự động chuyển sang ô tiếp theo khi nhập xong
                    if (txtOtp[index].Text.Length == 1 && index < 5)
                        txtOtp[index + 1].Focus();
                };

                txtOtp[i].KeyDown += (s, e) =>
                {
                    // Backspace quay lại ô trước
                    if (e.KeyCode == Keys.Back && txtOtp[index].Text.Length == 0 && index > 0)
                    {
                        txtOtp[index - 1].Focus();
                        txtOtp[index - 1].SelectAll();
                    }
                };
            }

            // Focus vào ô đầu tiên
            txtOtp[0].Focus();
        }

        private async void BtnVerify_Click(object sender, EventArgs e)
        {
            lblError.Text = "";

            // Lấy mã OTP từ 6 textbox
            string otp = "";
            foreach (var txt in txtOtp)
            {
                if (string.IsNullOrWhiteSpace(txt.Text))
                {
                    lblError.Text = "Please enter all 6 digits of OTP code.";
                    return;
                }
                otp += txt.Text;
            }

            btnVerify.Enabled = false;
            btnVerify.Text = "Verifying...";

            using var client = new HttpClient();
            try
            {
                var payload = new { email = _email, otp };
                var json = JsonSerializer.Serialize(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(
                    "http://localhost:8081/api/v1/auth/verify-otp",
                    content
                );

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Email verified successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    if (_parentForm != null)
                        _parentForm.ShowPanel(new SignInPanel(_parentForm));
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    try
                    {
                        using var doc = JsonDocument.Parse(responseBody);
                        if (doc.RootElement.TryGetProperty("message", out var msg))
                            lblError.Text = msg.GetString();
                        else
                            lblError.Text = "Invalid or expired OTP code.";
                    }
                    catch
                    {
                        lblError.Text = "Invalid or expired OTP code.";
                    }

                    // Clear các ô OTP để nhập lại
                    foreach (var txt in txtOtp)
                        txt.Text = "";
                    txtOtp[0].Focus();
                }
                else
                {
                    lblError.Text = $"Verification failed: {response.StatusCode}";
                }
            }
            catch (HttpRequestException)
            {
                lblError.Text = "Cannot connect to server. Please check your connection.";
            }
            catch (Exception ex)
            {
                lblError.Text = "Error: " + ex.Message;
            }

            btnVerify.Enabled = true;
            btnVerify.Text = "Verify OTP";
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            if (_parentForm != null)
                _parentForm.ShowPanel(new ForgotPasswordPanel(_parentForm));
        }
    }
}