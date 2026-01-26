using System.Collections.Generic;

namespace IELTS.UI.User.Results
{
    public class QuestionReview
    {
        public int Number { get; set; }
        public string UserAnswer { get; set; }
        public string CorrectAnswer { get; set; }
        public bool IsCorrect { get; set; }
    }

    public class PartReview
    {
        public string PartName { get; set; }
        public List<QuestionReview> Questions { get; set; } = new List<QuestionReview>();
    }

    public class ExamResult
    {
        public string Skill { get; set; }
        public string UserName { get; set; }
        public string AvatarPath { get; set; }
        public int CorrectCount { get; set; }
        public int TotalQuestions { get; set; }
        public int TimeTakenSeconds { get; set; }
        public double Band { get; set; } // THÊM ĐỂ HẾT LỖI CS1061
        public List<PartReview> Parts { get; set; } = new List<PartReview>();
    }
}