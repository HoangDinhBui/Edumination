using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Edumination.WinForms.UI.Forms.LoginForm
{
    public partial class SignInPanel : UserControl
    {
        private bool showPassword = false;
        private string[] slideImages = { "assets/img/adv.jpg", "assets/img/adv1.jpg", "assets/img/adv2.jpg" };
        private int currentSlide = 0;
        private System.Windows.Forms.Timer slideTimer;

        public SignInPanel()
        {
            InitializeComponent();
            InitializeSlides();

            btnTogglePassword.Click += BtnTogglePassword_Click;
            btnSignIn.Click += BtnSignIn_Click;
            btnGoogleLogin.Click += BtnGoogleLogin_Click;
            lblForgotPassword.Click += LblForgotPassword_Click;
            lblRegister.Click += LblRegister_Click;
        }

        private void InitializeSlides()
        {
            if (slideImages.Length == 0) return;

            pictureBoxSlide.Image = Image.FromFile(slideImages[0]);
            slideTimer = new System.Windows.Forms.Timer { Interval = 4000 };
            slideTimer.Tick += (s, e) =>
            {
                currentSlide = (currentSlide + 1) % slideImages.Length;
                pictureBoxSlide.Image = Image.FromFile(slideImages[currentSlide]);
            };
            slideTimer.Start();
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
                if (resp onse.IsSuccessStatusCode)
                {
                    var respBody = await response.Content.ReadAsStringAsync();
                    var data = JsonSerializer.Deserialize<JsonElement>(respBody);
                    if (data.TryGetProperty("Token", out var token))
                    {
                        //Properties.Settings.Default["Token"] = token.GetString();
                        //Properties.Settings.Default.Save();
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

        private void BtnGoogleLogin_Click(object sender, EventArgs e) => MessageBox.Show("Google login not implemented.");
        private void LblForgotPassword_Click(object sender, EventArgs e) => MessageBox.Show("Forgot password clicked.");
        private void LblRegister_Click(object sender, EventArgs e) => MessageBox.Show("Register clicked.");
    }
}