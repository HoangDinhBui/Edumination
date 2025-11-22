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
        public Register()
        {
            InitializeComponent();
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
;                MessageBox.Show($"Đăng ký thất bại!\n{ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }


}
