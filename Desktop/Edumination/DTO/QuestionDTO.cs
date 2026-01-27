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
        public string QuestionType { get; set; } // MCQ, MULTI_SELECT, FILL_BLANK, MATCHING, ORDERING, SHORT_ANSWER, ESSAY, SPEAK_PROMPT
        public string QuestionText { get; set; }
        public decimal Points { get; set; }
        public int Position { get; set; }

        // Đáp án cho các loại câu hỏi
        public string AnswerData { get; set; } // JSON format

        // Navigation
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

        public int StartIndex { get; set; }
        public int EndIndex { get; set; }

        public List<QuestionOptionDTO> Options { get; set; } = new();
        public List<QuestionMatchDTO> MatchPairs { get; set; } = new();
        public List<QuestionChoiceDTO> NewChoices { get; set; } = new();


        public class QuestionOptionDTO
        {
            public string OptionKey { get; set; }
            public string OptionText { get; set; }
        }

        public class QuestionMatchDTO
        {
            public string LeftKey { get; set; }
            public string LeftText { get; set; }
            public string RightKey { get; set; }
        }
    }
}
