using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Windows.Forms;

namespace Edumination.WinForms.UI.Forms.Login
{
    public partial class ForgotPasswordPanel : UserControl
    {
        private readonly LoginForm _parentForm;

        public ForgotPasswordPanel(LoginForm parent)
        {
            InitializeComponent();
            _parentForm = parent;
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
                lblError.Text = "Please enter your email address.";
                return;
            }

            // Kiểm tra format email đơn giản
            if (!email.Contains("@") || !email.Contains("."))
            {
                lblError.Text = "Please enter a valid email address.";
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

                var response = await client.PostAsync(
                    "http://localhost:8081/api/v1/auth/password/forgot",
                    content
                );

                if (response.IsSuccessStatusCode)
                {
                    // Chuyển sang EnterOtpPanel để nhập OTP
                    if (_parentForm != null)
                        _parentForm.ShowPanel(new EnterOtpPanel(_parentForm, email));
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    lblError.Text = "Email address not found.";
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
                            lblError.Text = "Invalid request. Please try again.";
                    }
                    catch
                    {
                        lblError.Text = "Invalid request. Please try again.";
                    }
                }
                else
                {
                    lblError.Text = $"Server error: {response.StatusCode}";
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

            btnSendLink.Enabled = true;
            btnSendLink.Text = "Send reset link";
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            if (_parentForm != null)
            {
                _parentForm.ShowPanel(new SignInPanel(_parentForm));
            }
        }
    }
}