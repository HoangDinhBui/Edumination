using Edumination.WinForms.UI.Admin.TestManager;
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

            var navBar = new AdminNavBarPanel() { Dock = DockStyle.Fill };

            // Bắt sự kiện từ NavBar
            navBar.OnMenuClicked += NavBar_OnMenuClicked;

            pnlNavBar.Controls.Add(navBar);

            this.FormClosed += AdminMainForm_FormClosed;
            // Load Navbar
            this.pnlNavBar.Controls.Add(new AdminNavBarPanel() { Dock = DockStyle.Fill });

            // Khi admin form bị tắt → hiện lại login
            this.FormClosed += AdminMainForm_FormClosed;
        }

        private void AdminMainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _loginForm.Show();
        }

        private void LoadContent(Control control)
        {
            pnlContent.Controls.Clear();
            control.Dock = DockStyle.Fill;
            pnlContent.Controls.Add(control);
        }

        private void NavBar_OnMenuClicked(string menu)
        {
            switch (menu)
            {
                case "dashboard":
                    //LoadContent(new DashboardPanel());
                    break;

                case "courses":
                    //LoadContent(new CoursesPanel());
                    break;

                case "tests":
                    LoadContent(new pnlTestManager());
                    break;

                case "students":
                    //LoadContent(new StudentsPanel());
                    break;

                case "teachers":
                    //LoadContent(new TeachersPanel());
                    break;

                case "accounts":
                    //LoadContent(new AccountsPanel());
                    break;

                case "reports":
                    //LoadContent(new ReportsPanel());
                    break;

                case "settings":
                    //LoadContent(new SettingsPanel());
                    break;

                case "logout":
                    this.Close(); // sẽ kích hoạt FormClosed → hiện lại login
                    break;
            }
        }

    }

}
