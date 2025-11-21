using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IELTS.DTO
{
    public class SpeakingSubmissionDTO
    {
        public long Id { get; set; }
        public long SectionAttemptId { get; set; }
        public string PromptText { get; set; }
        public string AudioFilePath { get; set; }
        public string TranscriptText { get; set; }
        public int? DurationSeconds { get; set; }
        public DateTime SubmittedAt { get; set; }

        // Navigation property
        public AIEvaluationDTO AIEvaluation { get; set; }

        public string GetFormattedDuration()
        {
            if (!DurationSeconds.HasValue)
                return "N/A";

            int minutes = DurationSeconds.Value / 60;
            int seconds = DurationSeconds.Value % 60;
            return $"{minutes:D2}:{seconds:D2}";
        }

        public override string ToString()
        {
            return $"Speaking submission - {GetFormattedDuration()}";
        }
    }
}
