using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IELTS.DTO
{
    public class ListeningGradeResult
    {
        public int TotalCorrect { get; set; }   // Số câu đúng
        public int TotalQuestions { get; set; } // Tổng số câu
        public double BandScore { get; set; }   // Điểm Band (ví dụ 6.5)
        public string Feedback { get; set; }    // Nhận xét chung
        public List<DetailResult> Details { get; set; } // Chi tiết từng câu
    }

    public class DetailResult
    {
        public int QuestionNumber { get; set; }
        public string UserAnswer { get; set; }
        public string CorrectKey { get; set; }
        public bool IsCorrect { get; set; }
        public string Explanation { get; set; } // Giải thích ngắn gọn
    }
}
