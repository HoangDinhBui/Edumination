using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IELTS.DTO
{
    public class WritingSubmissionDTO
    {
        public long Id { get; set; }
        public long SectionAttemptId { get; set; }
        public string PromptText { get; set; }
        public string ContentText { get; set; }
        public int? WordCount { get; set; }
        public DateTime SubmittedAt { get; set; }

        // Navigation property
        public AIEvaluationDTO AIEvaluation { get; set; }

        public string GetWordCountStatus()
        {
            if (!WordCount.HasValue)
                return "N/A";

            // Giả sử yêu cầu tối thiểu 250 từ cho Writing Task 2
            if (WordCount.Value < 250)
                return $"{WordCount} từ ⚠️ (Chưa đủ)";
            return $"{WordCount} từ ✅";
        }

        public override string ToString()
        {
            return $"Writing submission - {WordCount} words";
        }
    }
}
