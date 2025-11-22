using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
namespace IELTS.DAL
{
    public class AnswerDAL
    {
        public bool SaveAnswer(long sectionAttemptId, long questionId, string answerData,
                              bool? isCorrect, decimal? score)
        {
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                string query = @"IF EXISTS (SELECT 1 FROM Answers WHERE SectionAttemptId = @SectionAttemptId AND QuestionId = @QuestionId)
                                    UPDATE Answers SET AnswerData = @AnswerData, IsCorrect = @IsCorrect, Score = @Score
                                    WHERE SectionAttemptId = @SectionAttemptId AND QuestionId = @QuestionId
                                ELSE
                                    INSERT INTO Answers (SectionAttemptId, QuestionId, AnswerData, IsCorrect, Score) 
                                    VALUES (@SectionAttemptId, @QuestionId, @AnswerData, @IsCorrect, @Score)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@SectionAttemptId", sectionAttemptId);
                cmd.Parameters.AddWithValue("@QuestionId", questionId);
                cmd.Parameters.AddWithValue("@AnswerData", answerData ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@IsCorrect", isCorrect ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Score", score ?? (object)DBNull.Value);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public DataTable GetAnswersBySectionAttemptId(long sectionAttemptId)
        {
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                string query = @"SELECT a.*, q.QuestionText, q.QuestionType
                                FROM Answers a
                                JOIN Questions q ON a.QuestionId = q.Id
                                WHERE a.SectionAttemptId = @SectionAttemptId
                                ORDER BY q.Position";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@SectionAttemptId", sectionAttemptId);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }
    }
}
