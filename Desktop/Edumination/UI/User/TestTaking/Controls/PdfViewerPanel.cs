using Sunny.UI;
using System;
using System.Drawing;
using System.Windows.Forms;
using AxAcroPDFLib;

namespace IELTS.UI.User.TestTaking.Controls
{
    public partial class PdfViewerPanel : UserControl
    {
        public PdfViewerPanel()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Hiển thị văn bản passage (Reading dạng text)
        /// </summary>
        public void ShowPassage(string title, string content)
        {
            lblTitle.Text = title;
            lblPassage.Text = content;

            lblPassage.Visible = true;
            panelScroll.Visible = true;

            axPdfViewer.Visible = false;
        }

        /// <summary>
        /// Hiển thị file PDF
        /// </summary>
        public void ShowPdf(string pdfPath, string title = "PDF Viewer", string fallbackText = "")
        {
            lblTitle.Text = title;

            panelScroll.Visible = false;
            lblPassage.Visible = false;

            axPdfViewer.Visible = true;

            try
            {
                if (!string.IsNullOrEmpty(pdfPath) && System.IO.File.Exists(pdfPath))
                {
                    axPdfViewer.LoadFile(pdfPath);
                    axPdfViewer.setView("FitW");
                    axPdfViewer.setShowToolbar(false);
                    axPdfViewer.setPageMode("none");
                }
                else
                {
                    MessageBox.Show("PDF file not found:\n" + pdfPath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading PDF:\n" + ex.Message);
            }
        }

        /// <summary>
        /// Cố định PDF chiếm 1/3 panel trái
        /// </summary>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            int fixedWidth = this.Width;   // ⭐ 1/3 màn hình

            axPdfViewer.Width = fixedWidth;
            axPdfViewer.Height = this.Height - 80;
            axPdfViewer.Left = 10;
            axPdfViewer.Top = 60;

            panelScroll.Width = fixedWidth;
            panelScroll.Height = this.Height - 80;
            panelScroll.Left = 10;
            panelScroll.Top = 60;
        }
    }
}
