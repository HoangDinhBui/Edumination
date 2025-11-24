using Edumination.Api.Domain.Entities;
using Edumination.WinForms.UI.Admin.TestManager;
using IELTS.BLL;
using IELTS.UI.User.TestTaking.ReadingTest;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IELTS.UI.Admin.TestManager
{
    public partial class CreateTestPaperControl : UserControl
    {
        private TestManagerControl testManagerControl;
        public TestManagerControl GetTestManagerControl() { return testManagerControl; }

        public void SetManagerControl(TestManagerControl testManagerControl)
        {
            this.testManagerControl = testManagerControl;
        }

        public CreateTestPaperControl()
        {
            InitializeComponent();
        }

        private string selectedPdfPath = "";
        private void btnChooseFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "PDF Files|*.pdf";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    selectedPdfPath = ofd.FileName;
                    txtFileName.Text = Path.GetFileName(selectedPdfPath);
                    LoadPdf(selectedPdfPath);
                }
            }
        }


        private void LoadPdf(string path)
        {
            if (string.IsNullOrEmpty(path)) return;

            try
            {
                bool ok = axAcroPDFViewer.LoadFile(path);

                if (ok)
                {
                    axAcroPDFViewer.setShowToolbar(true);
                    axAcroPDFViewer.setView("Fit");
                    MessageBox.Show("PDF loaded successfully!");
                }
                else
                {
                    MessageBox.Show("Failed to load PDF!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading PDF: {ex.Message}");
            }
        }

        private void ClearPdf()
        {
            // cách gọn nhất
            axAcroPDFViewer.src = "about:blank";
        }

        private TestPaperBLL testPaperBLL = new TestPaperBLL();

        private void btnUpload_Click(object sender, EventArgs e)
        {
            string title = txtTitle.Text.Trim();
            string description = txtDescription.Text.Trim();
            // đường dẫn file PDF từ OpenFileDialog
            long createdBy = SessionManager.CurrentUserId;

            // Kiểm tra title
            if (string.IsNullOrWhiteSpace(title))
            {
                MessageBox.Show("Tiêu đề đề thi không được để trống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kiểm tra file PDF
            if (string.IsNullOrWhiteSpace(selectedPdfPath) || !File.Exists(selectedPdfPath))
            {
                MessageBox.Show("Vui lòng chọn file PDF hợp lệ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Thư mục assets trong project
                string assetsFolder = Path.Combine(Application.StartupPath, "..", "..", "UI", "assets");

                if (!Directory.Exists(assetsFolder))
                    Directory.CreateDirectory(assetsFolder);

                // Tạo tên file mới để tránh trùng lặp
                string newFileName = Guid.NewGuid().ToString() + ".pdf";
                string destPath = Path.Combine(assetsFolder, newFileName);

                // Copy file PDF vào folder assets
                File.Copy(selectedPdfPath, destPath, true);

                // Lưu thông tin đề thi vào DB
                bool success = testPaperBLL.CreateTestPaper(
                    // sinh mã đề thi tự động
                    title: title,
                    description: description,
                    createdBy: 2,
                    pdfFullPath: destPath
                );

                if (success)
                    MessageBox.Show("Upload PDF và tạo đề thi thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show("Tạo đề thi thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            int newPaperId= testPaperBLL.GetLatestTestPaperId();
            testManagerControl.GetAddSectionButtonControl().SetTestPaperId(newPaperId);
            MessageBox.Show(" " + newPaperId);
            testManagerControl.ShowPanel(testManagerControl.GetAddSectionButtonControl());

        }
        public void ResetForm()
        {
            txtFileName.Text = "";
            txtTitle.Text = "";
            txtDescription.Text = "";
            ClearPdf();
            
        }

        internal void SetTestPaperControl(TestManagerControl testManagerControl)
        {
            this.testManagerControl = testManagerControl;
        }
    }
}
