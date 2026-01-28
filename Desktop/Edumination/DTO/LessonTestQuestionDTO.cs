using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IELTS.DTO
{
	public class LessonTestQuestionDTO
	{
		public long Id { get; set; }
		public long LessonTestId { get; set; }
		public string QuestionText { get; set; }
		public string ChoiceA { get; set; }
		public string ChoiceB { get; set; }
		public string ChoiceC { get; set; }
		public string ChoiceD { get; set; }
		public string CorrectAnswer { get; set; } // 'A', 'B', 'C', 'D'
		public string ExplanationText { get; set; }
		public decimal Points { get; set; }
		public int Position { get; set; }
	}
}
