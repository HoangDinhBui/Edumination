using IELTS.BLL;
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
    public partial class Register : Form
    {
        private UserBLL userBLL = new UserBLL();
        private List<Image> slides;
        private int currentSlide = 0;
        public Register()
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
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            try
            {
                string fullName = txtFullName.Text.Trim();
                string email = txtEmail.Text.Trim();
                string password = txtPassword.Text.Trim();
                string confirmPassword = txtConfirmPassword.Text.Trim();

                // Gọi UserBLL để đăng ký
                bool success = userBLL.Register(email, password, confirmPassword, fullName);

                if (success)
                {
                    MessageBox.Show("Đăng ký thành công! Vui lòng đăng nhập.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close(); // Đóng form đăng ký
                }
                else
                {
                    MessageBox.Show("Đăng ký thất bại! Email có thể đã tồn tại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                ; MessageBox.Show($"Đăng ký thất bại!\n{ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TimerSlide_Tick(object sender, EventArgs e)
        {
            currentSlide = (currentSlide + 1) % slides.Count;
            pictureBoxSlide.Image = slides[currentSlide];
        }

        private void Register_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }

        private void lnkBackToLogin_Click(object sender, EventArgs e)
        {
            this.Hide();
            new SignIn().Show();
            this.Close();
        }

    }


}
