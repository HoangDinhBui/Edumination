using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Edumination.WinForms.UI.Forms.TestLibrary
{
    public partial class MockTestItemPanel : UserControl
    {
        public MockTestItemPanel()
        {
            InitializeComponent();
        }

        public void SetData(string title, string taken)
        {
            lblTitle.Text = title;
            lblTaken.Text = taken;
        }

        // Rounded Rectangle API
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(int left, int top, int right, int bottom,
            int width, int height);
    }
}
