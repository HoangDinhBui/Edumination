using IELTS.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IELTS.BLL
{
    public class TestPaperBLL
    {
        private TestPaperDAL paperDAL = new TestPaperDAL();

        public DataTable GetAllPublishedPapers()
        {
            return paperDAL.GetAllPublishedPapers();
        }

        public DataTable GetPaperById(long paperId)
        {
            if (paperId <= 0)
                throw new Exception("Paper ID không hợp lệ!");

            return paperDAL.GetPaperById(paperId);
        }

        public long CreatePaper(string code, string title, string description, long createdBy)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new Exception("Tiêu đề không được để trống!");

            if (createdBy <= 0)
                throw new Exception("Người tạo không hợp lệ!");

            return paperDAL.CreatePaper(code, title, description, createdBy);
        }

        public bool PublishPaper(long paperId)
        {
            if (paperId <= 0)
                throw new Exception("Paper ID không hợp lệ!");

            return paperDAL.PublishPaper(paperId);
        }

        public bool DeletePaper(long paperId)
        {
            if (paperId <= 0)
                throw new Exception("Paper ID không hợp lệ!");

            return paperDAL.DeletePaper(paperId);
        }

        public bool CreateTestPaper(string title, string description, long createdBy, string pdfFullPath)
        {
            if (!File.Exists(pdfFullPath))
                throw new FileNotFoundException("File PDF không tồn tại.", pdfFullPath);

            // Tạo code đề tự động: TP + timestamp
            string code = "TP" + DateTime.Now.ToString("yyyyMMddHHmmss");

            // Tên file
            string pdfFileName = Path.GetFileName(pdfFullPath);

            // Đường dẫn lưu trong assets
            string uploadFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UI", "assets");
            if (!Directory.Exists(uploadFolder))
                Directory.CreateDirectory(uploadFolder);

            string pdfFilePath = Path.Combine(uploadFolder, pdfFileName);

            // Copy file vào assets
            File.Copy(pdfFullPath, pdfFilePath, true);

            // Gọi DAL lưu vào database
            return paperDAL.CreateTestPaper(code, title, description, createdBy, pdfFileName, pdfFilePath);
        }
    }
}
