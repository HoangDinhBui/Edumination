using Edumination.WinForms.UI.Admin.TestManager;
using IELTS.DTO;
using IELTS.UI.Admin.AccountManager;
using IELTS.UI.Admin.CoursesManager;
using IELTS.UI.Admin.CourseStudents;
using IELTS.UI.Admin.DashBoard;
using IELTS.UI.Admin.ReportManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Shapes;


namespace Edumination.WinForms.UI.Admin
{
    public partial class AdminMainForm : Form
    {
        //private readonly LoginForm _loginForm;
        //private readonly string _token;\
        private AdminDashboardControl adminDashboardControl;
        public AdminDashboardControl AdminDashboardControl
        {
            get => adminDashboardControl;
            set => adminDashboardControl = value;
        }

        private TestManagerControl testManagerControl;
        public TestManagerControl TestManagerControl
        {
            get => testManagerControl;
            set => testManagerControl = value;
        }

        private readonly string _fullName;
        private readonly string _role;
        private int id;
        private UserDTO user;
        // 👉 Constructor đúng: nhận loginForm + token
        public AdminMainForm(int id)
        {
            InitializeComponent();
            this.id = id;
        }
        public AdminMainForm(UserDTO user)
        {
            InitializeComponent();
            this.user = user;
            _fullName = user.FullName;
            _role = user.Role;

            testManagerControl = new TestManagerControl();
            testManagerControl.UserId= user.Id;
            
            // ===== TẠO NAVBAR DUY NHẤT =====
            var navBar = new AdminNavBarPanel
            {
                Dock = DockStyle.Fill
            };

            navBar.OnMenuClicked += NavBar_OnMenuClicked;
            pnlNavBar.Controls.Clear();
            pnlNavBar.Controls.Add(navBar);

            // ===== LOAD DASHBOARD MẶC ĐỊNH =====
            LoadContent(new AdminDashboardControl());

            this.FormClosed += AdminMainForm_FormClosed;
        }


        public AdminMainForm(string fullName, string role)
        {
            InitializeComponent();

            _fullName = fullName;
            _role = role;

            // ===== TẠO NAVBAR DUY NHẤT =====
            var navBar = new AdminNavBarPanel
            {
                Dock = DockStyle.Fill
            };

            navBar.OnMenuClicked += NavBar_OnMenuClicked;
            pnlNavBar.Controls.Clear();
            pnlNavBar.Controls.Add(navBar);

            // ===== LOAD DASHBOARD MẶC ĐỊNH =====
            LoadContent(new AdminDashboardControl());

            this.FormClosed += AdminMainForm_FormClosed;
        }


        private void AdminMainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //_loginForm.Show();
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
                    LoadContent(new AdminDashboardControl());
                    break;

                case "courses":
                    LoadContent(new CoursesPanel());
                    break;

                case "tests":
                    LoadContent(testManagerControl);
                    break;

                case "students":
                    LoadContent(new CourseStudentViewerPanel());
                    break;

                case "teachers":
                    //LoadContent(new TeachersPanel());
                    break;

                case "accounts":
                    LoadContent(new AccountsPanel());
                    break;

                case "reports":
                    LoadContent(new ReportsPanel());
                    break;

                case "settings":
                    //LoadContent(new SettingsPanel());
                    break;

                case "logout":
                    this.Close(); // sẽ kích hoạt FormClosed → hiện lại login
                    break;
                default:
                    LoadContent(new AdminDashboardControl());
                    break;
            }
        }

        private void AdminMainForm_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }
    }

}
