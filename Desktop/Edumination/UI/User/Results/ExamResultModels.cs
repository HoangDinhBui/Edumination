using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IELTS.UI.User.Results
{
    internal class ExamResultModels
    {
    }
    // Kết quả từng câu
    public class QuestionReview
    {
        public int Number { get; set; }
        public string PartName { get; set; }
        public string QuestionText { get; set; }  // optional, dùng cho Listening nếu muốn
        public string CorrectAnswer { get; set; }
        public string UserAnswer { get; set; }
        public bool IsCorrect { get; set; }
    }

    // Kết quả theo Part (Part 1, Part 2...)
    public class PartReview
    {
        public string PartName { get; set; }
        public List<QuestionReview> Questions { get; set; } = new();
    }

    // Kết quả chung 1 bài thi (Reading / Listening)
    public class ExamResult
    {
        public string Skill { get; set; }              // "Reading" / "Listening"
        public string UserName { get; set; }           // tên học viên
        public string AvatarPath { get; set; }         // đường dẫn ảnh avatar (có thể null)
        public int CorrectCount { get; set; }
        public int TotalQuestions { get; set; }
        public int TimeTakenSeconds { get; set; }

        public List<PartReview> Parts { get; set; } = new();
    }
}
