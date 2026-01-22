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

        private async void btnUpload_Click(object sender, EventArgs e)
        {
            string title = txtTitle.Text.Trim();
            string description = txtDescription.Text.Trim();

            // Lấy ID người dùng thực tế từ Session
            long currentUserId = SessionManager.CurrentUserId;

            // 1. Kiểm tra đầu vào
            if (string.IsNullOrWhiteSpace(title))
            {
                MessageBox.Show("Tiêu đề đề thi không được để trống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(selectedPdfPath) || !File.Exists(selectedPdfPath))
            {
                MessageBox.Show("Vui lòng chọn file PDF hợp lệ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // 2. Khóa UI để tránh người dùng bấm lung tung khi AI đang chạy
                btnUpload.Enabled = false;
                this.Cursor = Cursors.WaitCursor;

                // 3. Xử lý file vật lý
                string assetsFolder = Path.Combine(Application.StartupPath, "..", "..", "UI", "assets");
                if (!Directory.Exists(assetsFolder)) Directory.CreateDirectory(assetsFolder);

                string newFileName = Guid.NewGuid().ToString() + ".pdf";
                string destPath = Path.Combine(assetsFolder, newFileName);

                // Đọc và copy file (có thể dùng Task.Run nếu file quá nặng)
                File.Copy(selectedPdfPath, destPath, true);

                // 4. Lưu TestPaper vào SQL Server
                bool success = testPaperBLL.CreateTestPaper(
                    title: title,
                    description: description,
                    createdBy: currentUserId, // ĐÃ SỬA: Dùng ID thực tế thay vì số 4
                    pdfFullPath: destPath
                );

                if (!success)
                {
                    MessageBox.Show("Tạo đề thi thất bại! Vui lòng kiểm tra lại Database.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int newPaperId = testPaperBLL.GetLatestTestPaperId();

                // 5. Gọi AI phân tích (Giai đoạn này tốn 5-15 giây)
                var aiService = new AIQuestionAnalysisService();
                bool aiSuccess = await aiService.AnalyzePdfAndSaveQuestions(destPath, newPaperId);

                if (aiSuccess)
                {
                    MessageBox.Show("✅ Thành công: AI đã phân tích và lưu câu hỏi!", "Thông báo");
                }
                else
                {
                    MessageBox.Show("⚠️ AI phân tích thất bại. Hệ thống sẽ để trống danh sách câu hỏi.", "Cảnh báo");
                }

                // 6. Chuyển sang bước tiếp theo
                testManagerControl.GetAddSectionButtonControl().SetTestPaperId(newPaperId);
                testManagerControl.ShowPanel(testManagerControl.GetAddSectionButtonControl());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi hệ thống: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // 7. Luôn luôn mở lại UI cho dù có lỗi xảy ra
                btnUpload.Enabled = true;
                this.Cursor = Cursors.Default;
            }
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
