using Sunny.UI;
using System.Drawing;
using System.Windows.Forms;

namespace IELTS.UI.User.TestTaking.Controls
{
    partial class PdfViewerPanel
    {
        private System.ComponentModel.IContainer components = null;

        private UIPanel panelWrapper;
        private UILabel lblTitle;
        private UIPanel panelScroll;
        private UILabel lblPassage;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            panelWrapper = new UIPanel();
            lblTitle = new UILabel();
            panelScroll = new UIPanel();
            lblPassage = new UILabel();

            SuspendLayout();

            // panelWrapper
            panelWrapper.Dock = DockStyle.Fill;
            panelWrapper.FillColor = Color.White;
            panelWrapper.RectColor = Color.LightGray;
            panelWrapper.RectSize = 1;
            panelWrapper.Padding = new Padding(20);
            panelWrapper.Controls.Add(lblTitle);
            panelWrapper.Controls.Add(panelScroll);
            panelWrapper.Name = "panelWrapper";
            panelWrapper.Size = new Size(900, 850);

            // lblTitle
            lblTitle.Font = new Font("Noto Serif SC", 18F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(41, 69, 99);
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(10, 10);
            lblTitle.Text = "Passage Title";

            // panelScroll
            panelScroll.Location = new Point(10, 60);
            panelScroll.Size = new Size(860, 760);
            panelScroll.FillColor = Color.White;
            panelScroll.RectColor = Color.White;
            panelScroll.AutoScroll = true;
            panelScroll.Padding = new Padding(5);
            panelScroll.Controls.Add(lblPassage);

            // lblPassage
            lblPassage.AutoSize = true;
            lblPassage.MaximumSize = new Size(820, 0);
            lblPassage.Font = new Font("Segoe UI", 12F);
            lblPassage.ForeColor = Color.Black;
            lblPassage.Text = "Passage text goes here...";

            // PdfViewerPanel
            Controls.Add(panelWrapper);
            Name = "PdfViewerPanel";
            Size = new Size(900, 850);

            ResumeLayout(false);
        }
    }
}
