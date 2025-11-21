using Sunny.UI;
using System.Drawing;
using System.Windows.Forms;

namespace Edumination.WinForms.UI.Forms.TestTaking.Controls
{
    partial class TestFooterPanel
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            SuspendLayout();

            BackColor = Color.White;
            Name = "TestFooterPanel";
            Size = new Size(1920, 90);
            Resize += TestFooterPanel_Resize;

            ResumeLayout(false);
        }
    }

}
