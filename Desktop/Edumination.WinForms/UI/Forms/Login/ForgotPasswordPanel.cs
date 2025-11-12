using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Windows.Forms;


namespace Edumination.WinForms.UI.Forms.Login
{
    public partial class ForgotPasswordPanel : UserControl
    {
        private LoginForm parentForm;


        // ✅ Hàm khởi tạo nhận LoginForm
        public ForgotPasswordPanel(LoginForm parent)
        {
            InitializeComponent();
            parentForm = parent;

            btnSendLink.Click += BtnSendLink_Click;
            btnBack.Click += BtnBack_Click;
        }
        public ForgotPasswordPanel() : this(null!) { }

        private async void BtnSendLink_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            lblMessage.Text = "";

            var email = txtEmail.Text.Trim();
            if (string.IsNullOrEmpty(email))
            {
                lblError.Text = "Vui lòng nhập địa chỉ email.";
                return;
            }

            btnSendLink.Enabled = false;
            btnSendLink.Text = "Sending...";

            using var client = new HttpClient();
            try
            {
                var payload = new { email };
                var json = JsonSerializer.Serialize(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("http://localhost:8081/api/v1/auth/password/forgot", content);

                if (response.IsSuccessStatusCode)
                {
                    lblMessage.Text = "Nếu email tồn tại, hướng dẫn đặt lại mật khẩu đã được gửi.";
                    txtEmail.Text = "";
                }
                else
                {
                    lblError.Text = $"Server error: {response.StatusCode}";
                }
            }
            catch (Exception ex)
            {
                lblError.Text = "Network error: " + ex.Message;
            }

            btnSendLink.Enabled = true;
            btnSendLink.Text = "Send reset link";
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            // Thay panelLeft của LoginForm bằng SignInPanel
            if (parentForm != null)
            {
                parentForm.ShowPanel(new SignInPanel(parentForm));
            }
        }

       
    }
}
