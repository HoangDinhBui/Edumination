using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IELTS.DTO
{
    public class TestAttemptDTO
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long PaperId { get; set; }
        public string PaperTitle { get; set; }
        public string PaperCode { get; set; }
        public int AttemptNumber { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? FinishedAt { get; set; }
        public string Status { get; set; } // IN_PROGRESS, SUBMITTED, GRADED
        public decimal? OverallBand { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation properties
        public List<SectionAttemptDTO> SectionAttempts { get; set; }

        public TestAttemptDTO()
        {
            SectionAttempts = new List<SectionAttemptDTO>();
        }

        // Helper methods
        public bool IsInProgress => Status == "IN_PROGRESS";
        public bool IsSubmitted => Status == "SUBMITTED";
        public bool IsGraded => Status == "GRADED";

        public TimeSpan? GetDuration()
        {
            if (FinishedAt.HasValue)
                return FinishedAt.Value - StartedAt;
            return null;
        }

        public string GetFormattedDuration()
        {
            var duration = GetDuration();
            if (duration.HasValue)
                return $"{duration.Value.Hours}h {duration.Value.Minutes}m";
            return "N/A";
        }

        public string GetStatusText()
        {
            return Status switch
            {
                "IN_PROGRESS" => "Đang làm",
                "SUBMITTED" => "Đã nộp",
                "GRADED" => "Đã chấm",
                _ => "N/A"
            };
        }

        public override string ToString()
        {
            return $"{PaperTitle} - Lần {AttemptNumber} ({GetStatusText()})";
        }
    }
}
