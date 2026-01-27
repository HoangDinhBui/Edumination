using IELTS.DTO;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        /// <summary>
        /// Thêm Question mới
        /// </summary>
        public long InsertQuestion(QuestionDTO question)
        {
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                conn.Open();

                string query = @"
                    INSERT INTO Questions (SectionId, PassageId, QuestionType, QuestionText, Points, Position)
                    VALUES (@SectionId, @PassageId, @QuestionType, @QuestionText, @Points, @Position);
                    SELECT CAST(SCOPE_IDENTITY() AS BIGINT);";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@SectionId", question.SectionId);
                    cmd.Parameters.AddWithValue("@PassageId", (object)question.PassageId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@QuestionType", question.QuestionType);
                    cmd.Parameters.AddWithValue("@QuestionText", question.QuestionText);
                    cmd.Parameters.AddWithValue("@Points", question.Points);
                    cmd.Parameters.AddWithValue("@Position", question.Position);

                    return (long)cmd.ExecuteScalar();
                }
            }
        }

        /// <summary>
        /// Lấy tất cả Questions của một Section
        /// </summary>
        public List<QuestionDTO> GetQuestionsBySectionIdDTO(long sectionId)
        {
            List<QuestionDTO> questions = new List<QuestionDTO>();

            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                conn.Open();

                string query = @"
                    SELECT Id, SectionId, PassageId, QuestionType, QuestionText, Points, Position
                    FROM Questions
                    WHERE SectionId = @SectionId
                    ORDER BY Position";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@SectionId", sectionId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            questions.Add(new QuestionDTO
                            {
                                Id = reader.GetInt64(0),
                                SectionId = reader.GetInt64(1),
                                PassageId = reader.IsDBNull(2) ? (long?)null : reader.GetInt64(2),
                                QuestionType = reader.GetString(3),
                                QuestionText = reader.GetString(4),
                                Points = reader.GetDecimal(5),
                                Position = reader.GetInt32(6)
                            });
                        }
                    }
                }
            }
            return questions;
        }

        /// <summary>
        /// Xóa Question
        /// </summary>
        public bool DeleteQuestion(long id)
        {
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                conn.Open();
                string query = "DELETE FROM Questions WHERE Id = @Id";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public List<long> GetQuestionIdsBySectionId(long sectionId)
        {
            List<long> ids = new List<long>();

            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                conn.Open();

                string query = @"SELECT Id FROM Questions WHERE SectionId = @SectionId ORDER BY Position";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@SectionId", sectionId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ids.Add(reader.GetInt64(0));
                        }
                    }
                }
            }

            return ids;
        }

		public bool InsertWritingPrompt(long sectionId, int position, string questionType, string promptText)
		{
			try
			{
				using (var conn = DatabaseConnection.GetConnection())
				{
					conn.Open();
					// Chèn nội dung đề bài vào cột QuestionText
					string sql = @"INSERT INTO Questions (SectionId, Position, QuestionType, QuestionText) 
                           VALUES (@SectionId, @Position, @QuestionType, @QuestionText)";

					using (var cmd = new SqlCommand(sql, conn))
					{
						cmd.Parameters.AddWithValue("@SectionId", sectionId);
						cmd.Parameters.AddWithValue("@Position", position);
						cmd.Parameters.AddWithValue("@QuestionType", questionType);
						cmd.Parameters.AddWithValue("@QuestionText", promptText);

						return cmd.ExecuteNonQuery() > 0;
					}
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine("Error in InsertWritingPrompt: " + ex.Message);
				return false;
			}
		}

	}
}
