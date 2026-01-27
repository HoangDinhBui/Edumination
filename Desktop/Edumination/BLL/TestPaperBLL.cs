using IELTS.DAL;
using IELTS.DTO;
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
        private readonly UserDAL _userDal= new UserDAL();

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
			if (string.IsNullOrWhiteSpace(title))
				throw new Exception("Tiêu đề không được để trống!");

			if (createdBy <= 0)
				throw new Exception("Người tạo không hợp lệ!");

			if (!File.Exists(pdfFullPath))
				throw new FileNotFoundException("File PDF không tồn tại.", pdfFullPath);

			// ✅ Tạo code đề tự động
			string code = "TP" + DateTime.Now.ToString("yyyyMMddHHmmss");

			// ✅ Lấy tên file từ path (file đã được copy bởi UI)
			string pdfFileName = Path.GetFileName(pdfFullPath);

			// ✅ KHÔNG copy file nữa, vì UI đã copy rồi
			// Chỉ cần lưu thông tin vào database

			System.Diagnostics.Debug.WriteLine($"=== CreateTestPaper ===");
			System.Diagnostics.Debug.WriteLine($"Code: {code}");
			System.Diagnostics.Debug.WriteLine($"Title: {title}");
			System.Diagnostics.Debug.WriteLine($"PdfFileName: {pdfFileName}");
			System.Diagnostics.Debug.WriteLine($"PdfFilePath: {pdfFullPath}");
			System.Diagnostics.Debug.WriteLine($"CreatedBy: {createdBy}");

			// ✅ Gọi DAL lưu vào database
			bool result = paperDAL.CreateTestPaper(code, title, description, createdBy, pdfFileName, pdfFullPath);

			System.Diagnostics.Debug.WriteLine($"Result: {result}");
			System.Diagnostics.Debug.WriteLine($"======================");

			return result;
		}

		public List<TestPaperDTO> GetAll()
        {
            var papers = paperDAL.GetAllTestPapers();

            foreach (var p in papers)
            {
                var user = _userDal.GetUserById(p.CreatedBy);
                p.CreatorFullName = user?.FullName ?? "Unknown";
            }

            return papers;
        }

        public int GetLatestTestPaperId()
        {
            return paperDAL.GetMaxTestPaperId();
        }
    }
}
