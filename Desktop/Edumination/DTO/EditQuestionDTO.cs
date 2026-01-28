using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IELTS.DTO
{
    public class EditQuestionDTO
    {
        public long QuestionId { get; set; }
        public string QuestionType { get; set; }
        public string QuestionText { get; set; }

        public List<QuestionChoiceDTO> Choices { get; set; } = new();
        public string AnswerKey { get; set; }
    }

}
