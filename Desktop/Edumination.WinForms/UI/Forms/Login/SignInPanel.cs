using System;
using System.Drawing;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Edumination.WinForms.UI.Forms.Login
{
    public partial class SignInPanel : UserControl
    {
        private bool showPassword = false;
        private readonly Edumination.WinForms.LoginForm _parentForm;

        // 🔹 Constructor mới có tham số LoginForm
        public SignInPanel(Edumination.WinForms.LoginForm parentForm)
        {
            InitializeComponent();
            _parentForm = parentForm ?? throw new ArgumentNullException(nameof(parentForm)); // lưu lại form cha

            // Gán sự kiện
            btnTogglePassword.Click += BtnTogglePassword_Click;
            btnSignIn.Click += BtnSignIn_Click;
            btnGoogleLogin.Click += BtnGoogleLogin_Click;
            lblForgotPassword.Click += LblForgotPassword_Click;
            lblRegister.Click += LblRegister_Click;

            // Căn giữa nội dung khi load và resize
            this.Load += (s, e) => CenterContent();
            this.Resize += (s, e) => CenterContent();
        }

        // Nếu muốn giữ khả năng khởi tạo trống (design mode, preview, test, v.v.)
        public SignInPanel() : this(null!) { }

        private void CenterContent()
        {
            int panelHeight = this.Height;
            int contentHeight = 440; // chiều cao ước lượng nội dung
            int topMargin = (panelHeight - contentHeight) / 2;

            lblTitle.Top = topMargin;
            lblSubtitle.Top = lblTitle.Bottom + 10;
            txtEmail.Top = lblSubtitle.Bottom + 30;
            txtPassword.Top = txtEmail.Bottom + 25;
            btnTogglePassword.Top = txtPassword.Top + 2;
            btnSignIn.Top = txtPassword.Bottom + 35;
            lblError.Top = btnSignIn.Bottom + 5;
            btnGoogleLogin.Top = lblError.Bottom + 10;
            lblForgotPassword.Top = btnGoogleLogin.Bottom + 10;
            lblRegister.Top = lblForgotPassword.Bottom + 30;
        }

        private void BtnTogglePassword_Click(object sender, EventArgs e)
        {
            showPassword = !showPassword;
            txtPassword.UseSystemPasswordChar = !showPassword;
        }

        private async void BtnSignIn_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            btnSignIn.Enabled = false;
            btnSignIn.Text = "Signing in...";

            var payload = new { email = txtEmail.Text, password = txtPassword.Text };
            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            using var client = new HttpClient();
            try
            {
                var response = await client.PostAsync("http://localhost:8081/api/v1/auth/login", content);
                if (response.IsSuccessStatusCode)
                {
                    var respBody = await response.Content.ReadAsStringAsync();
                    var data = JsonSerializer.Deserialize<JsonElement>(respBody);
                    Console.WriteLine(respBody);
                    if (data.TryGetProperty("Token", out var token))
                    {
                        MessageBox.Show("Login successful!");
                    }
                    else
                    {
                        lblError.Text = "Login failed: no token returned.";
                    }
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    lblError.Text = "Invalid email or password.";
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

            btnSignIn.Enabled = true;
            btnSignIn.Text = "Sign In";
        }

        private void BtnGoogleLogin_Click(object sender, EventArgs e)
            => MessageBox.Show("Google login not implemented.");

        // 🔹 Giờ có thể gọi _parentForm.ShowPanel(...)
        private void LblForgotPassword_Click(object sender, EventArgs e)
        {
            if (_parentForm != null)
                _parentForm.ShowPanel(new ForgotPasswordPanel(_parentForm));
        }



        private void LblRegister_Click(object sender, EventArgs e)
        {
            if (_parentForm != null)
                _parentForm.ShowPanel(new SignUpPanel(_parentForm));
        }
}
}
