using Sunny.UI;
using System.Drawing;
using System.Windows.Forms;
using AxAcroPDFLib;

namespace IELTS.UI.User.TestTaking.Controls
{
    partial class PdfViewerPanel
    {
        private System.ComponentModel.IContainer components = null;

        private UIPanel panelWrapper;
        private UILabel lblTitle;
        private UIPanel panelScroll;
        private UILabel lblPassage;
        private AxAcroPDF axPdfViewer;

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

            // ========== PDF VIEWER ==========
            axPdfViewer.Enabled = true;
            axPdfViewer.OcxState = (AxHost.State)resources.GetObject("axPdfViewer.OcxState");
            axPdfViewer.Location = new Point(10, 60);  // sẽ auto Resize sau
            axPdfViewer.Name = "axPdfViewer";
            axPdfViewer.Visible = false;
            axPdfViewer.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;

            // ========== PANEL WRAPPER ==========
            panelWrapper.Dock = DockStyle.Fill;
            panelWrapper.Padding = new Padding(20);
            panelWrapper.RectColor = Color.LightGray;
            panelWrapper.FillColor = Color.White;
            panelWrapper.Controls.Add(lblTitle);
            panelWrapper.Controls.Add(axPdfViewer);
            panelWrapper.Controls.Add(panelScroll);

            // ========== LABEL TITLE ==========
            lblTitle.Text = "Passage Title";
            lblTitle.Font = new Font("Noto Serif SC", 18F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(41, 69, 99);
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(10, 10);

            // ========== PANEL TEXT SCROLL ==========
            panelScroll.FillColor = Color.White;
            panelScroll.RectColor = Color.White;
            panelScroll.AutoScroll = true;
            panelScroll.Padding = new Padding(5);
            panelScroll.Location = new Point(10, 60);
            panelScroll.Controls.Add(lblPassage);

            // ========== TEXT LABEL ==========
            lblPassage.AutoSize = true;
            lblPassage.Font = new Font("Segoe UI", 12F);
            lblPassage.ForeColor = Color.Black;
            lblPassage.MaximumSize = new Size(900, 0);

            // ========== ADD CONTROLS ==========
            Controls.Add(panelWrapper);
            this.Resize += PdfViewerPanel_Resize;

            Name = "PdfViewerPanel";
            Size = new Size(1200, 850);

            ((System.ComponentModel.ISupportInitialize)axPdfViewer).EndInit();
            panelWrapper.ResumeLayout(false);
            panelWrapper.PerformLayout();
            panelScroll.ResumeLayout(false);
            panelScroll.PerformLayout();
            ResumeLayout(false);
        }
    }
}
