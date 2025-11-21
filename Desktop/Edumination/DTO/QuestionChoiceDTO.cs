using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IELTS.DTO
{
    public class QuestionChoiceDTO
    {
        public long Id { get; set; }
        public long QuestionId { get; set; }
        public string ChoiceText { get; set; }
        public bool IsCorrect { get; set; }
        public int Position { get; set; }

        public string GetChoiceLetter()
        {
            return ((char)('A' + Position - 1)).ToString();
        }

        public override string ToString()
        {
            return $"{GetChoiceLetter()}. {ChoiceText}";
        }
    }
}
