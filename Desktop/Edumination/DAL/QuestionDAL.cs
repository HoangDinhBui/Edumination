using IELTS.DTO;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static IELTS.DTO.QuestionDTO;
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

        public List<QuestionDTO> GetQuestionsByPassage(long passageId)
        {
            var questions = new List<QuestionDTO>();

            using var conn = DatabaseConnection.GetConnection();
            conn.Open();

            // 1️⃣ Load questions
            using (var cmdQ = new SqlCommand(
                "SELECT Id, QuestionType, QuestionText FROM Questions WHERE PassageId=@pid ORDER BY Position",
                conn))
            {
                cmdQ.Parameters.AddWithValue("@pid", passageId);

                using var r = cmdQ.ExecuteReader();
                while (r.Read())
                {
                    questions.Add(new QuestionDTO
                    {
                        Id = (long)r["Id"],
                        QuestionType = r["QuestionType"].ToString(),
                        QuestionText = r["QuestionText"].ToString()
                    });
                }
            }

            // 2️⃣ Load detail cho từng question
            foreach (var q in questions)
            {
                // ===== MCQ / MULTI / ORDER / MATCHING (RIGHT COLUMN) =====
                if (q.QuestionType is "MCQ" or "MULTI_SELECT" or "ORDER" or "MATCHING")
                {
                    using var cmdOpt = new SqlCommand(
                        "SELECT OptionKey, OptionText FROM QuestionOptions WHERE QuestionId=@qid ORDER BY OptionKey",
                        conn);
                    cmdOpt.Parameters.AddWithValue("@qid", q.Id);

                    using var rr = cmdOpt.ExecuteReader();
                    while (rr.Read())
                    {
                        q.Options.Add(new QuestionOptionDTO
                        {
                            OptionKey = rr["OptionKey"].ToString(),
                            OptionText = rr["OptionText"].ToString()
                        });
                    }
                }

                // ===== MATCHING (LEFT COLUMN) =====
                if (q.QuestionType == "MATCHING")
                {
                    using var cmdMatch = new SqlCommand(
                        "SELECT LeftKey, LeftText FROM QuestionMatchPairs WHERE QuestionId=@qid ORDER BY LeftKey",
                        conn);
                    cmdMatch.Parameters.AddWithValue("@qid", q.Id);

                    using var rr = cmdMatch.ExecuteReader();
                    while (rr.Read())
                    {
                        q.MatchPairs.Add(new QuestionMatchDTO
                        {
                            LeftKey = rr["LeftKey"].ToString(),
                            LeftText = rr["LeftText"].ToString()
                        });
                    }
                }
            }

            return questions;
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

        public List<int> GetExistingQuestionPositions(long passageId)
        {
            List<int> list = new();

            using SqlConnection conn = DatabaseConnection.GetConnection();
            conn.Open();

            string sql = @"
            SELECT DISTINCT Position
            FROM Questions
            WHERE PassageId = @PassageId";

            using SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@PassageId", passageId);

            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(reader.GetInt32(0));
            }

            return list;
        }

        // Lấy chi tiết 1 câu hỏi theo position
        public QuestionDTO GetQuestionByPosition(long passageId, int position)
        {
            using SqlConnection conn = DatabaseConnection.GetConnection();
            conn.Open();

            string sql = @"
            SELECT Id, Position, QuestionType, QuestionText, EndIndex
            FROM Questions
            WHERE PassageId = @PassageId AND Position = @Position";

            using SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@PassageId", passageId);
            cmd.Parameters.AddWithValue("@Position", position);

            using SqlDataReader r = cmd.ExecuteReader();
            if (!r.Read()) return null;

            return new QuestionDTO
            {
                Id = r.GetInt64(0),
                Position = r.GetInt32(1),
                QuestionType = r.GetString(2),
                QuestionText = r.GetString(3),
                EndIndex = r.GetInt32(1)
            };
        }

        public List<QuestionChoiceDTO> NewGetChoicesByQuestionId(long questionId)
        {
            List<QuestionChoiceDTO> list = new();

            using SqlConnection conn = DatabaseConnection.GetConnection();
            conn.Open();

            string sql = @"
        SELECT Id, ChoiceText, IsCorrect, Position
        FROM QuestionChoices
        WHERE QuestionId = @QuestionId
        ORDER BY Position";

            using SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@QuestionId", questionId);

            using SqlDataReader r = cmd.ExecuteReader();
            while (r.Read())
            {
                list.Add(new QuestionChoiceDTO
                {
                    Id = r.GetInt64(0),
                    ChoiceText = r.GetString(1),
                    IsCorrect = r.GetBoolean(2),
                    Position = r.GetInt32(3)
                });
            }

            return list;
        }

        public string GetAnswerKey(long questionId)
        {
            using SqlConnection conn = DatabaseConnection.GetConnection();
            conn.Open();

            string sql = @"
        SELECT AnswerData
        FROM QuestionAnswerKeys
        WHERE QuestionId = @QuestionId";

            using SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@QuestionId", questionId);

            return cmd.ExecuteScalar() as string;
        }

        public void NewDeleteQuestion(long questionId)
        {
            using SqlConnection conn = DatabaseConnection.GetConnection();
            conn.Open();

            string sql = @"DELETE FROM Questions WHERE Id = @Id";

            using SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", questionId);
            cmd.ExecuteNonQuery();
        }

        public long InsertQuestion(
    long passageId,
    int position,
    string type,
    string text)
        {
            using SqlConnection conn = DatabaseConnection.GetConnection();
            conn.Open();

            string sql = @"
        INSERT INTO Questions (PassageId, Position, QuestionType, QuestionText)
        OUTPUT INSERTED.Id
        VALUES (@PassageId, @Position, @Type, @Text)";

            using SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@PassageId", passageId);
            cmd.Parameters.AddWithValue("@Position", position);
            cmd.Parameters.AddWithValue("@Type", type);
            cmd.Parameters.AddWithValue("@Text", text);

            return (long)cmd.ExecuteScalar();
        }

    public void InsertAnswerKey(long questionId, string answer)
        {
            using SqlConnection conn = DatabaseConnection.GetConnection();
            conn.Open();

            string sql = @"
        INSERT INTO QuestionAnswerKeys (QuestionId, AnswerData)
        VALUES (@QId, @Answer)";

            using SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@QId", questionId);
            cmd.Parameters.AddWithValue("@Answer", answer);

            cmd.ExecuteNonQuery();
        }

        public List<int> GetQuestionPositionsByPassageId(long passageId)
        {
            var list = new List<int>();

            using var conn = DatabaseConnection.GetConnection();
            conn.Open();

            string sql = @"
        SELECT Position
        FROM Questions
        WHERE PassageId = @PassageId";

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@PassageId", passageId);

            using var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                list.Add((int)rd["Position"]);
            }

            return list;
        }

        public long GetSectionIdByQuestionId(long questionId)
        {
            using SqlConnection conn = DatabaseConnection.GetConnection();
            conn.Open();

            string sql = "SELECT SectionId FROM Questions WHERE Id = @Id";

            using SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", questionId);

            return (long)cmd.ExecuteScalar();
        }

        public void DeleteChoicesByQuestionId(long questionId)
        {
            using var conn = DatabaseConnection.GetConnection();
            conn.Open();

            using var cmd = new SqlCommand(
                "DELETE FROM QuestionChoices WHERE QuestionId = @QuestionId", conn);

            cmd.Parameters.AddWithValue("@QuestionId", questionId);
            cmd.ExecuteNonQuery();
        }


        public void InsertChoice(long questionId, string text, bool isCorrect, int position)
        {
            using var conn = DatabaseConnection.GetConnection();
            conn.Open();

            using var cmd = new SqlCommand(@"
        INSERT INTO QuestionChoices (QuestionId, ChoiceText, IsCorrect, Position)
        VALUES (@QuestionId, @Text, @IsCorrect, @Position)", conn);

            cmd.Parameters.AddWithValue("@QuestionId", questionId);
            cmd.Parameters.AddWithValue("@Text", text);
            cmd.Parameters.AddWithValue("@IsCorrect", isCorrect);
            cmd.Parameters.AddWithValue("@Position", position);

            cmd.ExecuteNonQuery();
        }


        public void SaveAnswerKey(long questionId, string answer)
        {
            using var conn = DatabaseConnection.GetConnection();
            conn.Open();

            using var cmd = new SqlCommand(@"
        IF EXISTS (SELECT 1 FROM QuestionAnswerKeys WHERE QuestionId = @QuestionId)
            UPDATE QuestionAnswerKeys 
            SET AnswerData = @Answer 
            WHERE QuestionId = @QuestionId
        ELSE
            INSERT INTO QuestionAnswerKeys (QuestionId, AnswerData)
            VALUES (@QuestionId, @Answer)
    ", conn);

            cmd.Parameters.AddWithValue("@QuestionId", questionId);
            cmd.Parameters.AddWithValue("@Answer", answer);

            cmd.ExecuteNonQuery();
        }



    }

}