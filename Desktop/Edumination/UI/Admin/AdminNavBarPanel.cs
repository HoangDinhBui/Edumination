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
    public partial class AdminNavBarPanel : UserControl
    {
        public event Action<string> OnMenuClicked;
        public AdminNavBarPanel()
        {
            InitializeComponent();

            btnDashboard.Click += (s, e) => OnMenuClicked?.Invoke("dashboard");
            btnCourses.Click += (s, e) => OnMenuClicked?.Invoke("courses");
            btnTests.Click += (s, e) => OnMenuClicked?.Invoke("tests");
            btnStudents.Click += (s, e) => OnMenuClicked?.Invoke("students");
            btnTeachers.Click += (s, e) => OnMenuClicked?.Invoke("teachers");
            btnAccounts.Click += (s, e) => OnMenuClicked?.Invoke("accounts");
            btnReports.Click += (s, e) => OnMenuClicked?.Invoke("reports");
            btnSettings.Click += (s, e) => OnMenuClicked?.Invoke("settings");
            btnLogout.Click += (s, e) => OnMenuClicked?.Invoke("logout");
        }
    }
}
