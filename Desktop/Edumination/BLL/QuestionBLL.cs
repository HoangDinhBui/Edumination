using IELTS.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IELTS.BLL
{
    public class QuestionBLL
    {
        private QuestionDAL questionDAL = new QuestionDAL();

        public DataTable GetQuestionsBySectionId(long sectionId)
        {
            if (sectionId <= 0)
                throw new Exception("Section ID không hợp lệ!");

            return questionDAL.GetQuestionsBySectionId(sectionId);
        }

        public long CreateQuestion(long sectionId, long? passageId, string questionType,
                                   string questionText, decimal points, int position)
        {
            if (sectionId <= 0)
                throw new Exception("Section ID không hợp lệ!");

            if (string.IsNullOrWhiteSpace(questionText))
                throw new Exception("Nội dung câu hỏi không được để trống!");

            if (points <= 0)
                throw new Exception("Điểm phải lớn hơn 0!");

            return questionDAL.CreateQuestion(sectionId, passageId, questionType, questionText, points, position);
        }

        //public DataTable GetChoicesByQuestionId(long questionId)
        //{
        //    if (questionId <= 0)
        //        throw new Exception("Question ID không hợp lệ!");

        //    return questionDAL.GetChoicesByQuestionId(questionId);
        //}

        public bool CreateChoice(long questionId, string choiceText, bool isCorrect, int position)
        {
            if (questionId <= 0)
                throw new Exception("Question ID không hợp lệ!");

            if (string.IsNullOrWhiteSpace(choiceText))
                throw new Exception("Nội dung lựa chọn không được để trống!");

            return questionDAL.CreateChoice(questionId, choiceText, isCorrect, position);
        }
    }
}
