using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IELTS.DTO
{
    public class QuestionAnswerKeyDTO
    {
        public long Id { get; set; }
        public long QuestionId { get; set; }
        public string AnswerData { get; set; } // JSON format

        // Helper methods for common answer types
        public List<string> GetFillBlankAnswers()
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(AnswerData);
        }

        public Dictionary<string, string> GetMatchingAnswers()
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(AnswerData);
        }

        public long GetMCQCorrectChoiceId()
        {
            return long.Parse(AnswerData);
        }
    }
}
