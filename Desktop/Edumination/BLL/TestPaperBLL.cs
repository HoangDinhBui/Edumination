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

        public long Insert(TestPaperDTO paper)
        {
            if (paper == null)
                throw new Exception("TestPaper không hợp lệ");

            if (string.IsNullOrWhiteSpace(paper.Title))
                throw new Exception("Title không được để trống");

            if (string.IsNullOrWhiteSpace(paper.PdfFilePath))
                throw new Exception("Chưa chọn file PDF");

            if (!File.Exists(paper.PdfFilePath))
                throw new FileNotFoundException("File PDF không tồn tại");

            // 1️⃣ Tạo Code
            string code = "TP" + DateTime.Now.ToString("yyyyMMddHHmmss");

            // 2️⃣ Thư mục assets
            string solutionRoot = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)!
                                .Parent!
                                .Parent!
                                .Parent!
                                .FullName;

            string assetsFolder = Path.Combine(solutionRoot, "UI", "assets");

            if (!Directory.Exists(assetsFolder))
                Directory.CreateDirectory(assetsFolder);


            // 3️⃣ Tên file PDF mới
            string newPdfFileName = code + ".pdf";
            string destPath = Path.Combine(assetsFolder, newPdfFileName);

            // 4️⃣ Copy file
            File.Copy(paper.PdfFilePath, destPath, true);

            // gán lại cho DTO
            paper.PdfFileName = newPdfFileName;
            paper.PdfFilePath = destPath;

            // 5️⃣ Gán lại dữ liệu
            paper.Code = code;
            paper.PdfFileName = newPdfFileName;
            paper.PdfFilePath = destPath;
            paper.CreatedAt = DateTime.Now;

            // 6️⃣ Gọi DAL
            return paperDAL.Insert(paper);
        }

        // ===== TEST SECTION =====
        public List<TestSectionDTO> GetSectionsByPaper(long paperId)
        {
            return paperDAL.GetSectionsByPaper(paperId);
        }

        public TestSectionDTO GetSectionById(long id)
        {
            return paperDAL.GetSectionById(id);
        }

        public long CreateSection(TestSectionDTO section)
        {
            ValidateSection(section);
            return paperDAL.CreateSection(section);
        }

        public bool UpdateSection(TestSectionDTO section)
        {
            ValidateSection(section);
            return paperDAL.UpdateSection(section);
        }

        public bool DeleteSection(long id)
        {
            return paperDAL.DeleteSection(id);
        }

        private void ValidateSection(TestSectionDTO section)
        {
            if (string.IsNullOrWhiteSpace(section.Skill))
                throw new ArgumentException("Skill is required");

            var validSkills = new[] { "LISTENING", "READING", "WRITING", "SPEAKING" };
            if (Array.IndexOf(validSkills, section.Skill.ToUpper()) == -1)
                throw new ArgumentException("Invalid skill. Must be LISTENING, READING, WRITING, or SPEAKING");
        }

        // ===== PASSAGE =====
        public List<PassageDTO> GetPassagesBySection(long sectionId)
        {
            return paperDAL.GetPassagesBySection(sectionId);
        }

        public long CreatePassage(PassageDTO passage)
        {
            return paperDAL.CreatePassage(passage);
        }

        public bool UpdatePassage(PassageDTO passage)
        {
            return paperDAL.UpdatePassage(passage);
        }

        public bool DeletePassage(long id)
        {
            return paperDAL.DeletePassage(id);
        }

        // ===== QUESTION =====
        public List<QuestionDTO> GetQuestionsBySection(long sectionId)
        {
            return paperDAL.GetQuestionsBySection(sectionId);
        }

        public QuestionDTO GetQuestionById(long id)
        {
            return paperDAL.GetQuestionById(id);
        }

        public long CreateQuestion(QuestionDTO question)
        {
            ValidateQuestion(question);
            return paperDAL.CreateQuestion(question);
        }

        public bool UpdateQuestion(QuestionDTO question)
        {
            ValidateQuestion(question);
            return paperDAL.UpdateQuestion(question);
        }

        public bool DeleteQuestion(long id)
        {
            return paperDAL.DeleteQuestion(id);
        }

        private void ValidateQuestion(QuestionDTO question)
        {
            if (string.IsNullOrWhiteSpace(question.QuestionText))
                throw new ArgumentException("Question text is required");

            if (string.IsNullOrWhiteSpace(question.QuestionType))
                throw new ArgumentException("Question type is required");

            if (question.Points <= 0)
                throw new ArgumentException("Points must be greater than 0");
        }

        // ===== QUESTION CHOICE =====
        public bool SaveQuestionChoices(long questionId, List<QuestionChoiceDTO> choices)
        {
            return paperDAL.SaveQuestionChoices(questionId, choices);
        }

        // ===== ANSWER KEY =====
        public bool SaveAnswerKey(long questionId, string answerData)
        {
            if (string.IsNullOrWhiteSpace(answerData))
                throw new ArgumentException("Answer data is required");

            return paperDAL.SaveAnswerKey(questionId, answerData);
        }

        public string GetAnswerKey(long questionId)
        {
            return paperDAL.GetAnswerKey(questionId);
        }
    }
}
