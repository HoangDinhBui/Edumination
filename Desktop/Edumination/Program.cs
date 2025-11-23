
using Edumination.WinForms.UI.Admin;
using Edumination.WinForms.UI.Admin.TestManager;
using IELTS.BLL;
using IELTS.UI;
using IELTS.UI.Admin.DashBoard;
using IELTS.UI.Admin.TestManager;

//using IELTS.UI.IELTS.UI;
using IELTS.UI.Login;

namespace Edumination.WinForms;

static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.

        string pdfName = "6451071039_DamHoangLam.pdf"; // lấy từ DB sau này

        ApplicationConfiguration.Initialize();
        string fullPath = Path.Combine(Application.StartupPath, "UI", "assets", pdfName);
        //Application.Run(new frmHienThiPdf(fullPath));
        //Application.Run(new SignIn());
        //Application.Run(new AddTestSectionForm(5));
        //Application.Run(new TestForm());
        Application.Run(new UpdateReadingTestSectionForm(5));

        //AdminDashboardForm dashboard = new AdminDashboardForm();
        //dashboard.ShowDialog();

        //Application.Run(new AdminMainForm("vbhg", "fafa"));
    }
}