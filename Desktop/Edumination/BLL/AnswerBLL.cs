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
    }
}
