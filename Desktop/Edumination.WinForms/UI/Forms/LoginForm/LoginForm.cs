using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Windows.Forms;
namespace Edumination.WinForms
{
    public partial class LoginForm : Form
    {
        private bool showPassword = false;
        private List<Image> slides;
        private int currentSlide = 0;

        public LoginForm()
        {
            InitializeComponent();

            // Load slides
            //slides = new List<Image>
            //{
            //    Image.FromFile("assets/img/adv.jpg"),
            //    Image.FromFile("assets/img/adv1.jpg"),
            //    Image.FromFile("assets/img/adv2.jpg")
            //};

            //pictureBoxSlide.Image = slides[0];
            timerSlide.Start();
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

            var apiUrl = "http://localhost:8081/api/v1/auth/login";
            var payload = new { email = txtEmail.Text, password = txtPassword.Text };
            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                try
                {
                    var response = await client.PostAsync(apiUrl, content);
                    if (response.IsSuccessStatusCode)
                    {
                        var responseBody = await response.Content.ReadAsStringAsync();
                        using var doc = JsonDocument.Parse(responseBody);
                        if (doc.RootElement.TryGetProperty("Token", out var tokenProp))
                        {
                            string token = tokenProp.GetString()!;
                            // Store token (could be in settings, file, etc.)
                            MessageBox.Show("Login successful!\nToken: " + token);
                            // Navigate to Ranking form
                        }
                        else
                        {
                            lblError.Text = "Error: Token not found in response.";
                        }
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        lblError.Text = "Invalid email or password";
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
            }

            btnSignIn.Enabled = true;
            btnSignIn.Text = "Sign In";
        }

        private void BtnGoogleLogin_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Google login not implemented yet.");
        }

        private void TimerSlide_Tick(object sender, EventArgs e)
        {
            //currentSlide = (currentSlide + 1) % slides.Count;
            //pictureBoxSlide.Image = slides[currentSlide];
        }

        
    }
}
