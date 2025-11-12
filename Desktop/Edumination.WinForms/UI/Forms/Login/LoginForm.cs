using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Windows.Forms;
using Edumination.WinForms.UI.Forms.Login; // để dùng SignInPanel, ForgotPasswordPanel,...

namespace Edumination.WinForms
{
    public partial class LoginForm : Form
    {
        private List<Image> slides;
        private int currentSlide = 0;
        private bool showPassword = false;

        public LoginForm()
        {
            InitializeComponent();

            // Load slide ảnh
            slides = new List<Image>
            {
                Image.FromFile("assets/img/adv.jpg"),
                Image.FromFile("assets/img/adv1.jpg"),
                Image.FromFile("assets/img/adv2.jpg")
            };

            pictureBoxSlide.Image = slides[0];
            timerSlide.Start();
        }

        // 🧩 Hàm hiển thị UserControl trong panelLeft (dùng chung cho SignIn, ForgotPassword,...)
        public void ShowPanel(UserControl panel)
        {
            panelLeft.Controls.Clear(); // Xóa control cũ
            panel.Dock = DockStyle.Fill; // Choán toàn bộ panelLeft
            panelLeft.Controls.Add(panel);
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            // Căn form vừa khít vùng làm việc (không che taskbar)
            var workingArea = Screen.PrimaryScreen.WorkingArea;
            this.Location = workingArea.Location;
            this.Size = workingArea.Size;

            // Tỷ lệ panel trái/phải 40/60
            panelLeft.Width = (int)(workingArea.Width * 0.4);
            panelRight.Width = (int)(workingArea.Width * 0.6);

            // ✅ Hiển thị SignInPanel mặc định khi mở form
            ShowPanel(new SignInPanel(this)); // Truyền LoginForm vào để SignInPanel có thể gọi lại ShowPanel
        }

        private void TimerSlide_Tick(object sender, EventArgs e)
        {
            currentSlide = (currentSlide + 1) % slides.Count;
            pictureBoxSlide.Image = slides[currentSlide];
        }

        // Các event khác giữ nguyên (BtnTogglePassword_Click, BtnSignIn_Click, ...)
        // 👉 Nhưng nên di chuyển logic SignIn, ForgotPassword,... sang từng panel riêng (SignInPanel.cs, ForgotPasswordPanel.cs)
    }
}
