using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace IELTS.UI.User.TestTaking.Controls
{
    public partial class PdfViewerPanel : UserControl
    {
        public PdfViewerPanel()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Hiển thị PDF nếu có, nếu không thì hiển thị text fallback.
        /// </summary>
        public void ShowPdf(string pdfPath, string title, string fallbackText)
        {
            lblTitle.Text = title;

            if (!string.IsNullOrWhiteSpace(pdfPath) && File.Exists(pdfPath))
            {
                panelScroll.Visible = false;
                axPdfViewer.Visible = true;

                try
                {
                    axPdfViewer.LoadFile(pdfPath);
                    axPdfViewer.setView("FitH");
                }
                catch
                {
                    axPdfViewer.Visible = false;
                    panelScroll.Visible = true;
                    lblPassage.Text = "PDF cannot be opened.";
                }
            }
            else
            {
                axPdfViewer.Visible = false;
                panelScroll.Visible = true;

                lblPassage.Text = fallbackText;
            }

            ResizeLayout();
        }

        /// <summary>
        /// Chia bố cục PDF = 40% và Text = 55%
        /// </summary>
        private void ResizeLayout()
        {
            int pdfWidth = (int)(this.Width * 0.90);
            int textWidth = (int)(this.Width * 0.95);

            // PDF
            axPdfViewer.Width = pdfWidth;
            axPdfViewer.Height = this.Height - 80;
            axPdfViewer.Location = new Point(10, 60);

            // TEXT
            panelScroll.Width = textWidth;
            panelScroll.Height = this.Height - 80;
            panelScroll.Location = new Point(axPdfViewer.Right + 20, 60);
        }

        private void PdfViewerPanel_Resize(object sender, EventArgs e)
        {
            ResizeLayout();
        }
    }
}
