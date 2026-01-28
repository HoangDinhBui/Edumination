using IELTS.UI;
//using IELTS.UI.IELTS.UI;
using IELTS.UI.Login;
using IELTS.UI.User;
using IELTS.UI.User.Home;
using IELTS.BLL; 
using IELTS.API; // Thêm namespace chứa SimpleApiServer
using System.Threading;
using System;
using System.Windows.Forms;
using System.IO;
using Edumination.WinForms.UI.Admin;
using IELTS.UI.Admin.TestManager;
using IELTS.UI.Admin;

namespace Edumination.WinForms;

static class Program
{
    private static IELTS.API.SimpleApiServer apiServer;

    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.

        // Start API Server in a separate thread
        Thread serverThread = new Thread(() =>
        {
            try
            {
                apiServer = new IELTS.API.SimpleApiServer();
                apiServer.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to start API Server: {ex.Message}", "Error");
            }
        });
        serverThread.IsBackground = true; // Server sẽ tắt khi app tắt
        serverThread.Start();

        string pdfName = "6451071039_DamHoangLam.pdf"; // lấy từ DB sau này

        ApplicationConfiguration.Initialize();
        string fullPath = Path.Combine(Application.StartupPath, "UI", "assets", pdfName);
        //Application.Run(new frmHienThiPdf(fullPath));
        //Application.Run(new AdminMainForm("Admin Lam", "ADMIN"));
        Application.Run(new SignIn());
        //Application.Run(new CreateQuestionForm(1));

        //Application.Run(new MainForm());
        // Stop server when app exits
        apiServer?.Stop();
    }
}