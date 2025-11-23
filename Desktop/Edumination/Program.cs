
using IELTS.UI;
//using IELTS.UI.IELTS.UI;
using IELTS.UI.Login;
using IELTS.UI.User;
using IELTS.UI.User.Home;

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
        Application.Run(new SignIn());
    }
}