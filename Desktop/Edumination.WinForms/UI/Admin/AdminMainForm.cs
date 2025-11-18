using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Edumination.WinForms.UI.Admin
{
    public partial class AdminMainForm : Form
    {
        private readonly LoginForm _loginForm;
        private readonly string _token;

        // 👉 Constructor đúng: nhận loginForm + token
        public AdminMainForm(LoginForm loginForm, string token)
        {
            InitializeComponent();

            // Lưu lại form login + token
            _loginForm = loginForm;
            _token = token;

            // Load Navbar
            this.pnlNavBar.Controls.Add(new AdminNavBarPanel() { Dock = DockStyle.Fill });

            // Khi admin form bị tắt → hiện lại login
            this.FormClosed += AdminMainForm_FormClosed;
        }

        private void AdminMainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _loginForm.Show();
        }
    }

}
