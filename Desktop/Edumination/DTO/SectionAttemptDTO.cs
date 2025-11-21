using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IELTS.DTO
{
    public class SectionAttemptDTO
    {
        public long Id { get; set; }
        public long TestAttemptId { get; set; }
        public long SectionId { get; set; }
        public string Skill { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? FinishedAt { get; set; }
        public decimal? RawScore { get; set; }
        public decimal? BandScore { get; set; }
        public string Status { get; set; }

        // Navigation properties
        public List<AnswerDTO> Answers { get; set; }

        public SectionAttemptDTO()
        {
            Answers = new List<AnswerDTO>();
        }

        // Helper methods
        public bool IsInProgress => Status == "IN_PROGRESS";
        public bool IsGraded => Status == "GRADED";

        public TimeSpan? GetDuration()
        {
            if (FinishedAt.HasValue)
                return FinishedAt.Value - StartedAt;
            return null;
        }

        public string GetSkillIcon()
        {
            return Skill switch
            {
                "LISTENING" => "🎧",
                "READING" => "📖",
                "WRITING" => "✍️",
                "SPEAKING" => "🗣️",
                _ => "📝"
            };
        }

        public override string ToString()
        {
            return $"{GetSkillIcon()} {Skill} - Band {BandScore?.ToString() ?? "N/A"}";
        }
    }
}
