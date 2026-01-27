using IELTS.DAL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IELTS.BLL
{
    public class AnswerBLL
    {
        private AnswerDAL answerDAL = new AnswerDAL();
        private QuestionDAL questionDAL = new QuestionDAL();

        public bool SaveAnswer(long sectionAttemptId, long questionId, string answerData)
        {
            if (sectionAttemptId <= 0 || questionId <= 0)
                throw new Exception("Section Attempt ID hoặc Question ID không hợp lệ!");

            // Auto-grading cho objective questions (MCQ)
            bool? isCorrect = null;
            decimal? score = null;

            // Lấy thông tin question
            DataTable question = questionDAL.GetQuestionsBySectionId(0); // Cần optimize

            // Logic chấm điểm tự động (đơn giản hóa)
            // Thực tế cần check questionType và so sánh với answer key

            return answerDAL.SaveAnswer(sectionAttemptId, questionId, answerData, isCorrect, score);
        }

        public bool SaveAndGradeAnswer(long sectionAttemptId, long questionId, string answerData,
                                      string questionType, string correctAnswerJson)
        {
            if (sectionAttemptId <= 0 || questionId <= 0)
                throw new Exception("Section Attempt ID hoặc Question ID không hợp lệ!");

            bool? isCorrect = null;
            decimal? score = null;

            // Auto-grading logic
            if (questionType == "MCQ")
            {
                // answerData là choice_id, correctAnswerJson là choice_id đúng
                isCorrect = answerData == correctAnswerJson;
                score = isCorrect.Value ? 1.0m : 0.0m;
            }
            else if (questionType == "FILL_BLANK")
            {
                // So sánh text (ignore case)
                var correctAnswers = JsonConvert.DeserializeObject<string[]>(correctAnswerJson);
                isCorrect = Array.Exists(correctAnswers, a => a.Trim().Equals(answerData?.Trim(), StringComparison.OrdinalIgnoreCase));
                score = isCorrect.Value ? 1.0m : 0.0m;
            }

            return answerDAL.SaveAnswer(sectionAttemptId, questionId, answerData, isCorrect, score);
        }

        public DataTable GetAnswersBySectionAttemptId(long sectionAttemptId)
        {
            if (sectionAttemptId <= 0)
                throw new Exception("Section Attempt ID không hợp lệ!");

            return answerDAL.GetAnswersBySectionAttemptId(sectionAttemptId);
        }

		public bool CheckAnswer(string userAnswer, string correctAnswerJson, string questionType)
		{
			if (string.IsNullOrWhiteSpace(userAnswer))
				return false;

			try
			{
				// ✅ Parse JSON để lấy giá trị thực tế
				dynamic answerData = JsonConvert.DeserializeObject(correctAnswerJson);

				switch (questionType?.ToUpperInvariant())
				{
					case "MCQ":
					case "FILL_BLANK":
					case "SHORT_ANSWER":
					case "TRUE_FALSE_NOT_GIVEN":
						// Lấy giá trị "answer" từ JSON
						string correctAnswer = answerData.answer?.ToString() ?? "";

						// So sánh không phân biệt hoa thường và trim khoảng trắng
						return userAnswer.Trim().Equals(correctAnswer.Trim(), StringComparison.OrdinalIgnoreCase);

					case "MULTI_SELECT":
						// JSON format: {"answers":["A","C"]}
						// Thay vì var, hãy viết rõ List<string>
						List<string> correctAnswers = answerData.answers.ToObject<List<string>>();
						var userAnswers = userAnswer.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
												   .Select(s => s.Trim())
												   .ToList();

						// Kiểm tra tất cả đáp án đúng và không thừa
						return correctAnswers.Count == userAnswers.Count &&
							   correctAnswers.All(a => userAnswers.Contains(a, StringComparer.OrdinalIgnoreCase));

					case "MATCHING":
						// JSON format: {"1":"A","2":"B"} hoặc {"answer":"1:A;2:B"}
						if (answerData.answer != null)
						{
							string correctMatch = answerData.answer.ToString();
							return userAnswer.Trim().Equals(correctMatch.Trim(), StringComparison.OrdinalIgnoreCase);
						}
						// Handle dictionary format nếu cần
						break;

					case "ORDERING":
						// JSON format: {"order":["A","B","C"]}
						var correctOrder = answerData.order.ToObject<List<string>>();
						var userOrder = userAnswer.Split(new[] { ',', ';', '|' }, StringSplitOptions.RemoveEmptyEntries)
												 .Select(s => s.Trim())
												 .ToList();

						return correctOrder.Count == userOrder.Count &&
							   correctOrder.SequenceEqual(userOrder, StringComparer.OrdinalIgnoreCase);

					default:
						// Fallback: so sánh trực tiếp
						string fallbackAnswer = answerData.answer?.ToString() ?? "";
						return userAnswer.Trim().Equals(fallbackAnswer.Trim(), StringComparison.OrdinalIgnoreCase);
				}

				return false;
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"Error checking answer: {ex.Message}");

				// Fallback: so sánh trực tiếp nếu không parse được JSON
				return userAnswer.Trim().Equals(correctAnswerJson.Trim(), StringComparison.OrdinalIgnoreCase);
			}
		}

		// Trong code hiển thị kết quả
		public string FormatCorrectAnswer(string answerJson, string questionType)
		{
			try
			{
				dynamic answerData = JsonConvert.DeserializeObject(answerJson);

				switch (questionType?.ToUpperInvariant())
				{
					case "MCQ":
					case "FILL_BLANK":
					case "SHORT_ANSWER":
					case "TRUE_FALSE_NOT_GIVEN":
						return answerData.answer?.ToString() ?? answerJson;

					case "MULTI_SELECT":
						var answers = answerData.answers.ToObject<List<string>>();
						return string.Join(", ", answers);

					case "MATCHING":
						if (answerData.answer != null)
							return answerData.answer.ToString();
						// Handle dictionary
						return answerJson;

					case "ORDERING":
						var order = answerData.order.ToObject<List<string>>();
						return string.Join(" → ", order);

					default:
						return answerData.answer?.ToString() ?? answerJson;
				}
			}
			catch
			{
				return answerJson; // Fallback nếu không parse được
			}
		}

	}
}
