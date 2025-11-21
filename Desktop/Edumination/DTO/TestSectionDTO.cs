using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IELTS.DTO
{
    public class TestSectionDTO
    {
        public long Id { get; set; }
        public long PaperId { get; set; }
        public string Skill { get; set; } // LISTENING, READING, WRITING, SPEAKING
        public int? TimeLimitMinutes { get; set; }
        public string AudioFilePath { get; set; }

        // Navigation properties
        public List<PassageDTO> Passages { get; set; }
        public List<QuestionDTO> Questions { get; set; }

        public TestSectionDTO()
        {
            Passages = new List<PassageDTO>();
            Questions = new List<QuestionDTO>();
        }

        // Helper methods
        public bool IsListening => Skill == "LISTENING";
        public bool IsReading => Skill == "READING";
        public bool IsWriting => Skill == "WRITING";
        public bool IsSpeaking => Skill == "SPEAKING";

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
            return $"{GetSkillIcon()} {Skill}";
        }
    }
}
