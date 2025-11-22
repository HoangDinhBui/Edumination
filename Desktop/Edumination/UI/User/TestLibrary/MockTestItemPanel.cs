using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace IELTS.UI.User.TestLibrary
{
    public partial class MockTestItemPanel : UserControl
    {
        public string Skill { get; set; }

        public MockTestItemPanel()
        {
            InitializeComponent();
        }

        public void SetData(string skill, string title, string taken)
        {
            Skill = skill;
            lblTitle.Text = title;
            lblTaken.Text = taken;
        }

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(int left, int top, int right, int bottom,
            int width, int height);
    }
}
