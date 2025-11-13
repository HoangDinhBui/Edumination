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
using System.Drawing;
using System.Net.Http;
using System.Text.Json;

namespace Edumination.WinForms.UI.Forms.Login

{
    public partial class SignUpPanel : UserControl
    {
        private bool showPassword = false;
        private bool showConfirmPassword = false;
        private readonly Form parentForm;

        public SignUpPanel(Form parent)
        {
            InitializeComponent();
            parentForm = parent;

            btnTogglePassword.Click += BtnTogglePassword_Click;
            btnToggleConfirmPassword.Click += BtnToggleConfirmPassword_Click;
            btnSignUp.Click += BtnSignUp_Click;
            btnGoogle.Click += BtnGoogle_Click;
            lblSignInLink.Click += LblSignInLink_Click;
        }

        private void BtnTogglePassword_Click(object? sender, EventArgs e)
        {
            showPassword = !showPassword;
            txtPassword.UseSystemPasswordChar = !showPassword;
        }

        private void BtnToggleConfirmPassword_Click(object? sender, EventArgs e)
        {
            showConfirmPassword = !showConfirmPassword;
            txtConfirmPassword.UseSystemPasswordChar = !showConfirmPassword;
        }

        private async void BtnSignUp_Click(object? sender, EventArgs e)
        {
            lblError.Text = "";
            btnSignUp.Enabled = false;
            btnSignUp.Text = "Signing up...";

            if (txtPassword.Text != txtConfirmPassword.Text)
            {
                lblError.Text = "Passwords do not match!";
                btnSignUp.Enabled = true;
                btnSignUp.Text = "Sign Up";
                return;
            }

            var apiUrl = "http://localhost:8081/api/v1/auth/register";
            var payload = new
            {
                Full_Name = $"{txtFirstName.Text} {txtLastName.Text}".Trim(),
                Email = txtEmail.Text,
                Password = txtPassword.Text,
                ConfirmPassword = txtConfirmPassword.Text
            };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            using var client = new HttpClient();
            try
            {
                var response = await client.PostAsync(apiUrl, content);
                var responseBody = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Registration successful!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    if (parentForm is LoginForm loginForm)
                        loginForm.ShowPanel(new SignInPanel(loginForm));
                }
                else
                {
                    using var doc = JsonDocument.Parse(responseBody);
                    if (doc.RootElement.TryGetProperty("message", out var msg))
                        lblError.Text = msg.GetString();
                    else if (doc.RootElement.TryGetProperty("errors", out var errors))
                    {
                        foreach (var errProp in errors.EnumerateObject())
                        {
                            lblError.Text = errProp.Value[0].GetString();
                            break;
                        }
                    }
                    else
                        lblError.Text = $"Server error: {response.StatusCode}";
                }
            }
            catch (Exception ex)
            {
                lblError.Text = "Network error: " + ex.Message;
            }

            btnSignUp.Enabled = true;
            btnSignUp.Text = "Sign Up";
        }

        private void BtnGoogle_Click(object? sender, EventArgs e)
        {
            MessageBox.Show("Google login not implemented yet.");
        }

        private void LblSignInLink_Click(object? sender, EventArgs e)
        {
            if (parentForm is LoginForm loginForm)
                loginForm.ShowPanel(new SignInPanel(loginForm));
        }

        private void SignUpPanel_Load(object sender, EventArgs e)
        {
            // Nếu chưa cần xử lý gì, để trống cũng được
        }
    }
}

