using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IELTS.DTO
{
    public class AnswerDTO
    {
        public long Id { get; set; }
        public long SectionAttemptId { get; set; }
        public long QuestionId { get; set; }
        public string QuestionText { get; set; }
        public string QuestionType { get; set; }
        public string AnswerData { get; set; } // JSON
        public bool? IsCorrect { get; set; }
        public decimal? Score { get; set; }

        // Helper methods
        public string GetAnswerText()
        {
            if (string.IsNullOrEmpty(AnswerData))
                return "Chưa trả lời";

            try
            {
                if (QuestionType == "MCQ")
                {
                    return $"Chọn đáp án ID: {AnswerData}";
                }
                else if (QuestionType == "FILL_BLANK")
                {
                    return AnswerData;
                }
                else
                {
                    return AnswerData.Length > 50 ? AnswerData.Substring(0, 50) + "..." : AnswerData;
                }
            }
            catch
            {
                return AnswerData;
            }
        }

        public string GetResultIcon()
        {
            if (!IsCorrect.HasValue)
                return "⏳";
            return IsCorrect.Value ? "✅" : "❌";
        }

        public override string ToString()
        {
            return $"{GetResultIcon()} {GetAnswerText()}";
        }
    }
}
