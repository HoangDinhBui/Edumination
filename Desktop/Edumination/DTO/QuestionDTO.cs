using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IELTS.DTO
{
    public class QuestionDTO
    {
        public long Id { get; set; }
        public long SectionId { get; set; }
        public long? PassageId { get; set; }
        public string PassageTitle { get; set; }
        public string QuestionType { get; set; } // MCQ, FILL_BLANK, MATCHING, ESSAY, SPEAKING
        public string QuestionText { get; set; }
        public decimal Points { get; set; }
        public int Position { get; set; }

        // Navigation properties
        public List<QuestionChoiceDTO> Choices { get; set; }
        public QuestionAnswerKeyDTO AnswerKey { get; set; }

        public QuestionDTO()
        {
            Choices = new List<QuestionChoiceDTO>();
        }

        // Helper methods
        public bool IsMCQ => QuestionType == "MCQ";
        public bool IsFillBlank => QuestionType == "FILL_BLANK";
        public bool IsMatching => QuestionType == "MATCHING";
        public bool IsEssay => QuestionType == "ESSAY";
        public bool IsSpeaking => QuestionType == "SPEAKING";
        public bool IsObjective => IsMCQ || IsFillBlank || IsMatching;

        public override string ToString()
        {
            return $"Q{Position}: {QuestionText.Substring(0, Math.Min(50, QuestionText.Length))}...";
        }
    }
}
