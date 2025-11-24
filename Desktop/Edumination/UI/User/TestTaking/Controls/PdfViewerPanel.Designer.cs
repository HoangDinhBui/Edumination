using AxAcroPDFLib;
using Sunny.UI;

namespace IELTS.UI.User.TestTaking.Controls
{
    partial class PdfViewerPanel
    {
        private System.ComponentModel.IContainer components = null;

        private UIPanel panelWrapper;
        private UILabel lblTitle;
        private AxAcroPDF axPdfViewer;
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
            System.ComponentModel.ComponentResourceManager resources =
                new System.ComponentModel.ComponentResourceManager(typeof(PdfViewerPanel));

            axPdfViewer = new AxAcroPDF();
            panelWrapper = new UIPanel();
            lblTitle = new UILabel();
            panelScroll = new UIPanel();
            lblPassage = new UILabel();

            ((System.ComponentModel.ISupportInitialize)axPdfViewer).BeginInit();
            panelWrapper.SuspendLayout();
            panelScroll.SuspendLayout();
            SuspendLayout();

            // axPdfViewer
            axPdfViewer.Enabled = true;
            axPdfViewer.Name = "axPdfViewer";
            axPdfViewer.OcxState = (AxHost.State)resources.GetObject("axPdfViewer.OcxState");

            axPdfViewer.Location = new Point(10, 60);
            axPdfViewer.Size = new Size(400, 740);

            axPdfViewer.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            axPdfViewer.Visible = false;

            // panelWrapper
            panelWrapper.Controls.Add(lblTitle);
            panelWrapper.Controls.Add(panelScroll);
            panelWrapper.Controls.Add(axPdfViewer);

            panelWrapper.Dock = DockStyle.Fill;
            panelWrapper.FillColor = Color.White;
            panelWrapper.Padding = new Padding(20);
            panelWrapper.RectColor = Color.LightGray;
            panelWrapper.Name = "panelWrapper";
            panelWrapper.Size = new Size(900, 850);

            // lblTitle
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Noto Serif SC", 18F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(41, 69, 99);
            lblTitle.Location = new Point(10, 10);
            lblTitle.Name = "lblTitle";
            lblTitle.Text = "Passage Title";

            // panelScroll
            panelScroll.AutoScroll = true;
            panelScroll.Controls.Add(lblPassage);
            panelScroll.FillColor = Color.White;

            panelScroll.Location = new Point(10, 60);
            panelScroll.Size = new Size(400, 740);

            panelScroll.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            panelScroll.RectColor = Color.White;
            panelScroll.Name = "panelScroll";

            // lblPassage
            lblPassage.AutoSize = true;
            lblPassage.Font = new Font("Segoe UI", 12F);
            lblPassage.ForeColor = Color.Black;
            lblPassage.MaximumSize = new Size(380, 0);
            lblPassage.Location = new Point(0, 0);
            lblPassage.Name = "lblPassage";

            // PdfViewerPanel
            Controls.Add(panelWrapper);
            Name = "PdfViewerPanel";
            Size = new Size(900, 850);

            ((System.ComponentModel.ISupportInitialize)axPdfViewer).EndInit();
            panelWrapper.ResumeLayout(false);
            panelWrapper.PerformLayout();
            panelScroll.ResumeLayout(false);
            panelScroll.PerformLayout();
            ResumeLayout(false);
        }
    }
}
