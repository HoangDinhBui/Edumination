using IELTS.BLL;
using System;
using System.Windows.Forms;

namespace IELTS.UI
{
    public partial class frmHienThiPdf : Form
    {
        private string pdfPath;

        public frmHienThiPdf(string path)
        {
            pdfPath = path;
            InitializeComponent();
            this.Load += FrmHienThiPdf_Load;
        }

        private void FrmHienThiPdf_Load(object sender, EventArgs e)
        {
            LoadPdf();
        }

        private void LoadPdf()
        {
            if (string.IsNullOrEmpty(pdfPath)) return;

            bool ok = axAcroPDF.LoadFile(pdfPath);
            MessageBox.Show("Loaded: " + ok);

            axAcroPDF.setShowToolbar(true);
            axAcroPDF.setView("Fit");
        }

        private void frmHienThiPdf_Load_1(object sender, EventArgs e)
        {

        }
        private string selectedPdfPath = "";
        private void btnChooseFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "PDF Files (*.pdf)|*.pdf";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                selectedPdfPath = ofd.FileName;
                txtFileName.Text = Path.GetFileName(selectedPdfPath); // Hiển thị tên file
            }
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
        }

       

        //private void btnCreateTestPaper_Click(object sender, EventArgs e)
        //{
        //    string title = txtTitle.Text.Trim();
        //    string description = txtDescription.Text.Trim();
        //    string selectedPdfPath = txtPdfPath.Text.Trim(); // đường dẫn file PDF từ OpenFileDialog

        //    // Kiểm tra title
        //    if (string.IsNullOrWhiteSpace(title))
        //    {
        //        MessageBox.Show("Tiêu đề đề thi không được để trống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        return;
        //    }

        //    // Kiểm tra file PDF
        //    if (string.IsNullOrWhiteSpace(selectedPdfPath) || !File.Exists(selectedPdfPath))
        //    {
        //        MessageBox.Show("Vui lòng chọn file PDF hợp lệ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        return;
        //    }

        //    try
        //    {
        //        // Thư mục assets
        //        string assetsFolder = Path.Combine(Application.StartupPath, "..", "..", "UI", "assets");

        //        if (!Directory.Exists(assetsFolder))
        //            Directory.CreateDirectory(assetsFolder);

        //        // Tạo tên file mới để tránh trùng
        //        string newFileName = Guid.NewGuid().ToString() + ".pdf";
        //        string destPath = Path.Combine(assetsFolder, newFileName);

        //        // Copy file vào assets
        //        File.Copy(selectedPdfPath, destPath, true);

        //        // Lưu vào DB
        //        bool success = testPaperBLL.CreateTestPaper(
        //            code: GenerateTestPaperCode(), // hàm sinh mã đề thi
        //            title: title,
        //            description: description,
        //            createdBy: currentUserId,
        //            pdfFileName: newFileName,
        //            pdfFilePath: destPath
        //        );

        //        if (success)
        //            MessageBox.Show("Tạo đề thi thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        else
        //            MessageBox.Show("Tạo đề thi thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}

        private string GenerateTestPaperCode()
        {
            // Cách 1: Dựa trên timestamp
            return "TP" + DateTime.Now.ToString("yyyyMMddHHmmss");

            // Cách 2: Nếu muốn ngắn hơn, chỉ lấy giờ-phút-giây + random
            // Random rnd = new Random();
            // return "TP" + DateTime.Now.ToString("HHmmss") + rnd.Next(100, 999).ToString();
        }

    }
}