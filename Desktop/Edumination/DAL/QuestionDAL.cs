using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
namespace IELTS.DAL
{
    public class QuestionDAL
    {
        public DataTable GetQuestionsBySectionId(long sectionId)
        {
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                string query = @"SELECT q.*, p.Title as PassageTitle 
                                FROM Questions q
                                LEFT JOIN Passages p ON q.PassageId = p.Id
                                WHERE q.SectionId = @SectionId
                                ORDER BY q.Position";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@SectionId", sectionId);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }

        public long CreateQuestion(long sectionId, long? passageId, string questionType,
                                   string questionText, decimal points, int position)
        {
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                string query = @"INSERT INTO Questions (SectionId, PassageId, QuestionType, QuestionText, Points, Position) 
                                OUTPUT INSERTED.Id
                                VALUES (@SectionId, @PassageId, @QuestionType, @QuestionText, @Points, @Position)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@SectionId", sectionId);
                cmd.Parameters.AddWithValue("@PassageId", passageId ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@QuestionType", questionType);
                cmd.Parameters.AddWithValue("@QuestionText", questionText);
                cmd.Parameters.AddWithValue("@Points", points);
                cmd.Parameters.AddWithValue("@Position", position);

                conn.Open();
                return (long)cmd.ExecuteScalar();
            }
        }

        public DataTable GetChoicesByQuestionId(long questionId)
        {
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                string query = "SELECT * FROM QuestionChoices WHERE QuestionId = @QuestionId ORDER BY Position";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@QuestionId", questionId);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }

        public bool CreateChoice(long questionId, string choiceText, bool isCorrect, int position)
        {
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                string query = @"INSERT INTO QuestionChoices (QuestionId, ChoiceText, IsCorrect, Position) 
                                VALUES (@QuestionId, @ChoiceText, @IsCorrect, @Position)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@QuestionId", questionId);
                cmd.Parameters.AddWithValue("@ChoiceText", choiceText);
                cmd.Parameters.AddWithValue("@IsCorrect", isCorrect);
                cmd.Parameters.AddWithValue("@Position", position);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}
