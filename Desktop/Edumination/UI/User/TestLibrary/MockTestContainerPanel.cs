using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace IELTS.UI.User.TestLibrary
{
    public partial class MockTestContainerPanel : UserControl
    {
        public string TitleText => lblTitle.Text;

        public MockTestContainerPanel()
        {
            InitializeComponent();
        }

        public void SetTitle(string title)
        {
            lblTitle.Text = title;
        }


        public void AddItem(string title, string taken)
        {
            var item = new MockTestItemPanel();
            item.SetData(title, taken);
            panelItems.Controls.Add(item);
        }

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(int left, int top, int right, int bottom,
            int width, int height);
    }
}
