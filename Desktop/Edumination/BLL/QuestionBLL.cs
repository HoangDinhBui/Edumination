using IELTS.DAL;
using IELTS.DTO;
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

        public class QuestionService
        {
            private readonly QuestionDAL _dal = new QuestionDAL();

            public List<QuestionDTO> GetQuestionsByPassage(long passageId)
            {
                return _dal.GetQuestionsByPassage(passageId);
            }
        }

        public int GetTotalQuestionsByPassagePosition(int passagePosition)
        {
            return passagePosition == 3 ? 14 : 13;
        }

        public QuestionDTO GetQuestion(long passageId, int position)
        {
            return questionDAL.GetQuestionByPosition(passageId, position);
        }

        public List<QuestionChoiceDTO> GetChoices(long questionId)
        {
            return questionDAL.NewGetChoicesByQuestionId(questionId);
        }

        public string GetAnswerKey(long questionId)
        {
            return questionDAL.GetAnswerKey(questionId);
        }

        public void ReplaceQuestion(
    long oldQuestionId,
    long passageId,
    int position,
    EditQuestionDTO dto)
        {
            // 1. Xóa cũ
            questionDAL.NewDeleteQuestion(oldQuestionId);

            // 2. Insert mới
            long newQuestionId = questionDAL.InsertQuestion(
                passageId,
                position,
                dto.QuestionType,
                dto.QuestionText);

            // 3. Insert data theo type
            if (dto.QuestionType == "MCQ" || dto.QuestionType == "MULTI_CHOICES")
            {
                int pos = 1;
                foreach (var c in dto.Choices)
                {
                    questionDAL.InsertChoice(newQuestionId, c.ChoiceText, c.IsCorrect, pos++);
                }
            }
            else if (dto.QuestionType == "FILL_BLANK")
            {
                questionDAL.InsertAnswerKey(newQuestionId, dto.AnswerKey);
            }
        }

        public (int start, int end) GetQuestionRangeByPassagePosition(int passagePosition)
        {
            return passagePosition switch
            {
                1 => (1, 13),
                2 => (14, 26),
                3 => (27, 40),
                _ => throw new ArgumentException("Invalid passage position")
            };
        }

        public List<QuestionStatusDTO> GetQuestionStatuses(long passageId, int passagePosition)
        {
            var (start, end) = GetQuestionRangeByPassagePosition(passagePosition);

            // lấy các câu hỏi đã tồn tại trong passage
            var existingPositions = questionDAL
                .GetQuestionPositionsByPassageId(passageId)
                .ToHashSet();

            var result = new List<QuestionStatusDTO>();

            for (int pos = start; pos <= end; pos++)
            {
                result.Add(new QuestionStatusDTO
                {
                    Position = pos,
                    HasData = existingPositions.Contains(pos)
                });
            }

            return result;
        }

        public long GetSectionIdByQuestionId(long questionId)
        {
            return questionDAL.GetSectionIdByQuestionId(questionId);
        }

        public void DeleteQuestion(long questionId)
        {
            questionDAL.DeleteQuestion(questionId);
        }

        public void DeleteChoicesByQuestionId(long questionId)
        {
            questionDAL.DeleteChoicesByQuestionId(questionId);
        }

        public void InsertChoice(long questionId, string text, bool isCorrect, int position)
        {
            questionDAL.InsertChoice(questionId, text, isCorrect, position);
        }

        public void SaveAnswerKey(long questionId, string answer)
        {
            questionDAL.SaveAnswerKey(questionId, answer);
        }
        public long GetSectionIdByPassageId(long passageId)
        {
            using var conn = DatabaseConnection.GetConnection();
            using var cmd = conn.CreateCommand();

            cmd.CommandText = @"
        SELECT SectionId
        FROM Passages
        WHERE Id = @PassageId";

            cmd.Parameters.AddWithValue("@PassageId", passageId);

            conn.Open();
            return (long)cmd.ExecuteScalar();
        }


    }
}
