using IELTS.DAL;
using IELTS.DTO;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace IELTS.DAL
{
    public class TestPaperDAL
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["IELTSConnection"].ConnectionString;

        public TestPaperDAL()
        {
        }

        public DataTable GetAllPublishedPapers()
        {
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                string query = @"SELECT tp.*, u.FullName as CreatedByName
                                FROM TestPapers tp
                                JOIN Users u ON tp.CreatedBy = u.Id
                                WHERE tp.IsPublished = 1
                                ORDER BY tp.CreatedAt DESC";

                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }

        public DataTable GetPaperById(long paperId)
        {
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                string query = "SELECT * FROM TestPapers WHERE Id = @PaperId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@PaperId", paperId);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }

        public long CreatePaper(string code, string title, string description, long createdBy)
        {
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                string query = @"INSERT INTO TestPapers (Code, Title, Description, CreatedBy) 
                                OUTPUT INSERTED.Id
                                VALUES (@Code, @Title, @Description, @CreatedBy)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Code", code ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Title", title);
                cmd.Parameters.AddWithValue("@Description", description ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@CreatedBy", createdBy);

                conn.Open();
                return (long)cmd.ExecuteScalar();
            }
        }

        public bool PublishPaper(long paperId)
        {
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                string query = "UPDATE TestPapers SET IsPublished = 1 WHERE Id = @PaperId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@PaperId", paperId);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool DeletePaper(long paperId)
        {
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                string query = "DELETE FROM TestPapers WHERE Id = @PaperId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@PaperId", paperId);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

		public bool CreateTestPaper(string code, string title, string description, long createdBy, string pdfFileName, string pdfFilePath)
		{
			using (SqlConnection conn = DatabaseConnection.GetConnection())
			{
				conn.Open();

				string query = @"
            INSERT INTO TestPapers (
                Code, 
                Title, 
                Description, 
                PdfFileName, 
                PdfFilePath, 
                CreatedBy, 
                IsPublished,  -- ⬅️ QUAN TRỌNG
                CreatedAt
            )
            VALUES (
                @Code, 
                @Title, 
                @Description, 
                @PdfFileName, 
                @PdfFilePath, 
                @CreatedBy, 
                1,  -- ⬅️ PHẢI LÀ 1 ĐỂ HIỂN THỊ TRONG UI
                GETDATE()
            )";

				using (SqlCommand cmd = new SqlCommand(query, conn))
				{
					cmd.Parameters.AddWithValue("@Code", code);
					cmd.Parameters.AddWithValue("@Title", title);
					cmd.Parameters.AddWithValue("@Description", description ?? (object)DBNull.Value);
					cmd.Parameters.AddWithValue("@PdfFileName", pdfFileName);
					cmd.Parameters.AddWithValue("@PdfFilePath", pdfFilePath);
					cmd.Parameters.AddWithValue("@CreatedBy", createdBy);

					int affected = cmd.ExecuteNonQuery();

					System.Diagnostics.Debug.WriteLine($"TestPaperDAL.CreateTestPaper: {affected} row(s) affected");

					return affected > 0;
				}
			}
		}

		public List<TestPaperDTO> GetAllTestPapers()
        {
            List<TestPaperDTO> list = new();

            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                conn.Open();
                string query = @"
                    SELECT Id, Code, Title, Description,
                           IsPublished, CreatedBy, CreatedAt
                    FROM TestPapers
                    ORDER BY CreatedAt DESC
                ";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        list.Add(new TestPaperDTO
                        {
                            Id = r.GetInt64(0),
                            Code = r.GetString(1),
                            Title = r.GetString(2),
                            Description = r.IsDBNull(3) ? "" : r.GetString(3),
                            IsPublished = r.GetBoolean(4),
                            CreatedBy = r.GetInt64(5),
                            CreatedAt = r.GetDateTime(6)
                        });
                    }
                }
            }

            return list;
        }

        public int GetMaxTestPaperId()
        {
            string query = "SELECT ISNULL(MAX(Id), 0) FROM TestPapers";

            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    object result = cmd.ExecuteScalar();

                    return Convert.ToInt32(result);
                }
            }
        }

        public List<TestPaperDTO> NewGetAll()
        {
            var list = new List<TestPaperDTO>();

            string sql = @"SELECT * FROM TestPapers";

            using (SqlConnection conn = DatabaseConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(MapReader(reader));
                }
            }

            return list;
        }

        private TestPaperDTO MapReader(SqlDataReader reader)
        {
            return new TestPaperDTO
            {
                Id = Convert.ToInt64(reader["Id"]),
                Code = reader["Code"]?.ToString(),
                Title = reader["Title"].ToString(),
                Description = reader["Description"]?.ToString(),
                PdfFileName = reader["PdfFileName"]?.ToString(),
                PdfFilePath = reader["PdfFilePath"]?.ToString(),
                MockTestId = reader["MockTestId"] as long?,
                TestMonth = reader["TestMonth"] as int?,
                IsPublished = Convert.ToBoolean(reader["IsPublished"]),
                CreatedBy = Convert.ToInt64(reader["CreatedBy"]),
                CreatedAt = Convert.ToDateTime(reader["CreatedAt"])
            };
        }

        public long Insert(TestPaperDTO paper)
        {
            using SqlConnection conn = DatabaseConnection.GetConnection();
            conn.Open();

            string sql = @"
        INSERT INTO TestPapers
        (Code, Title, Description, PdfFileName, PdfFilePath,
         MockTestId, TestMonth, CreatedBy, CreatedAt)
        OUTPUT INSERTED.Id
        VALUES
        (@Code, @Title, @Description, @PdfFileName, @PdfFilePath,
         @MockTestId, @TestMonth, @CreatedBy, @CreatedAt)
    ";

            using SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Code", paper.Code);
            cmd.Parameters.AddWithValue("@Title", paper.Title);
            cmd.Parameters.AddWithValue("@Description", (object?)paper.Description ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@PdfFileName", paper.PdfFileName);
            cmd.Parameters.AddWithValue("@PdfFilePath", paper.PdfFilePath);
            cmd.Parameters.AddWithValue("@MockTestId", (object?)paper.MockTestId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@TestMonth", (object?)paper.TestMonth ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CreatedBy", paper.CreatedBy);
            cmd.Parameters.AddWithValue("@CreatedAt", paper.CreatedAt);

            return (long)cmd.ExecuteScalar();
        }


        // ===== TEST SECTION =====
        public List<TestSectionDTO> GetSectionsByPaper(long paperId)
        {
            List<TestSectionDTO> sections = new List<TestSectionDTO>();
            string query = @"
                SELECT Id, PaperId, Skill, TimeLimitMinutes, AudioFilePath, PdfFileName, PdfFilePath
                FROM TestSections
                WHERE PaperId = @PaperId
                ORDER BY Skill";

            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@PaperId", paperId);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    sections.Add(MapTestSection(reader));
                }
            }

            return sections;
        }

        public TestSectionDTO GetSectionById(long id)
        {
            string query = @"
                SELECT Id, PaperId, Skill, TimeLimitMinutes, AudioFilePath, PdfFileName, PdfFilePath
                FROM TestSections
                WHERE Id = @Id";

            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    return MapTestSection(reader);
                }
            }

            return null;
        }

        public long CreateSection(TestSectionDTO section)
        {
            string query = @"
                INSERT INTO TestSections (PaperId, Skill, TimeLimitMinutes, AudioFilePath, PdfFileName, PdfFilePath)
                VALUES (@PaperId, @Skill, @TimeLimitMinutes, @AudioFilePath, @PdfFileName, @PdfFilePath);
                SELECT CAST(SCOPE_IDENTITY() AS BIGINT);";

            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                AddSectionParameters(cmd, section);
                conn.Open();
                return (long)cmd.ExecuteScalar();
            }
        }

        public bool UpdateSection(TestSectionDTO section)
        {
            string query = @"
                UPDATE TestSections
                SET Skill = @Skill, TimeLimitMinutes = @TimeLimitMinutes,
                    AudioFilePath = @AudioFilePath, PdfFileName = @PdfFileName, PdfFilePath = @PdfFilePath
                WHERE Id = @Id";

            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", section.Id);
                AddSectionParameters(cmd, section);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool DeleteSection(long id)
        {
            string query = "DELETE FROM TestSections WHERE Id = @Id";

            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        private TestSectionDTO MapTestSection(SqlDataReader reader)
        {
            return new TestSectionDTO
            {
                Id = reader.GetInt64(0),
                PaperId = reader.GetInt64(1),
                Skill = reader.GetString(2),
                TimeLimitMinutes = reader.IsDBNull(3) ? (int?)null : reader.GetInt32(3),
                AudioFilePath = reader.IsDBNull(4) ? null : reader.GetString(4),
                PdfFileName = reader.IsDBNull(5) ? null : reader.GetString(5),
                PdfFilePath = reader.IsDBNull(6) ? null : reader.GetString(6)
            };
        }

        private void AddSectionParameters(SqlCommand cmd, TestSectionDTO section)
        {
            cmd.Parameters.AddWithValue("@PaperId", section.PaperId);
            cmd.Parameters.AddWithValue("@Skill", section.Skill);
            cmd.Parameters.AddWithValue("@TimeLimitMinutes", (object)section.TimeLimitMinutes ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@AudioFilePath", (object)section.AudioFilePath ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@PdfFileName", (object)section.PdfFileName ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@PdfFilePath", (object)section.PdfFilePath ?? DBNull.Value);
        }

        // ===== PASSAGE =====
        public List<PassageDTO> GetPassagesBySection(long sectionId)
        {
            List<PassageDTO> passages = new List<PassageDTO>();
            string query = @"
                SELECT Id, SectionId, Title, ContentText, Position
                FROM Passages
                WHERE SectionId = @SectionId
                ORDER BY Position";

            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@SectionId", sectionId);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    passages.Add(new PassageDTO
                    {
                        Id = reader.GetInt64(0),
                        SectionId = reader.GetInt64(1),
                        Title = reader.IsDBNull(2) ? null : reader.GetString(2),
                        ContentText = reader.IsDBNull(3) ? null : reader.GetString(3),
                        Position = reader.GetInt32(4)
                    });
                }
            }

            return passages;
        }

        public long CreatePassage(PassageDTO passage)
        {
            string query = @"
                INSERT INTO Passages (SectionId, Title, ContentText, Position)
                VALUES (@SectionId, @Title, @ContentText, @Position);
                SELECT CAST(SCOPE_IDENTITY() AS BIGINT);";

            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@SectionId", passage.SectionId);
                cmd.Parameters.AddWithValue("@Title", (object)passage.Title ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ContentText", (object)passage.ContentText ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Position", passage.Position);
                conn.Open();
                return (long)cmd.ExecuteScalar();
            }
        }

        public bool UpdatePassage(PassageDTO passage)
        {
            string query = @"
                UPDATE Passages
                SET Title = @Title, ContentText = @ContentText, Position = @Position
                WHERE Id = @Id";

            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", passage.Id);
                cmd.Parameters.AddWithValue("@Title", (object)passage.Title ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ContentText", (object)passage.ContentText ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Position", passage.Position);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool DeletePassage(long id)
        {
            string query = "DELETE FROM Passages WHERE Id = @Id";

            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // ===== QUESTION =====
        public List<QuestionDTO> GetQuestionsBySection(long sectionId)
        {
            Dictionary<long, QuestionDTO> questionsDict = new Dictionary<long, QuestionDTO>();

            string query = @"
                SELECT 
                    q.Id, q.SectionId, q.PassageId, q.QuestionType, q.QuestionText, q.Points, q.Position,
                    qc.Id AS ChoiceId, qc.ChoiceText, qc.IsCorrect, qc.Position AS ChoicePosition,
                    qak.AnswerData
                FROM Questions q
                LEFT JOIN QuestionChoices qc ON q.Id = qc.QuestionId
                LEFT JOIN QuestionAnswerKeys qak ON q.Id = qak.QuestionId
                WHERE q.SectionId = @SectionId
                ORDER BY q.Position, qc.Position";

            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@SectionId", sectionId);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    long questionId = reader.GetInt64(0);

                    if (!questionsDict.ContainsKey(questionId))
                    {
                        questionsDict[questionId] = new QuestionDTO
                        {
                            Id = questionId,
                            SectionId = reader.GetInt64(1),
                            PassageId = reader.IsDBNull(2) ? (long?)null : reader.GetInt64(2),
                            QuestionType = reader.GetString(3),
                            QuestionText = reader.GetString(4),
                            Points = reader.GetDecimal(5),
                            Position = reader.GetInt32(6),
                            AnswerData = reader.IsDBNull(11) ? null : reader.GetString(11),
                            Choices = new List<QuestionChoiceDTO>()
                        };
                    }

                    if (!reader.IsDBNull(7)) // Has choice
                    {
                        var choice = new QuestionChoiceDTO
                        {
                            Id = reader.GetInt64(7),
                            QuestionId = questionId,
                            ChoiceText = reader.GetString(8),
                            IsCorrect = reader.GetBoolean(9),
                            Position = reader.GetInt32(10)
                        };

                        if (!questionsDict[questionId].Choices.Any(c => c.Id == choice.Id))
                        {
                            questionsDict[questionId].Choices.Add(choice);
                        }
                    }
                }
            }

            return questionsDict.Values.OrderBy(q => q.Position).ToList();
        }

        public QuestionDTO GetQuestionById(long id)
        {
            QuestionDTO question = null;
            Dictionary<long, QuestionDTO> questionsDict = new Dictionary<long, QuestionDTO>();

            string query = @"
                SELECT 
                    q.Id, q.SectionId, q.PassageId, q.QuestionType, q.QuestionText, q.Points, q.Position,
                    qc.Id AS ChoiceId, qc.ChoiceText, qc.IsCorrect, qc.Position AS ChoicePosition,
                    qak.AnswerData
                FROM Questions q
                LEFT JOIN QuestionChoices qc ON q.Id = qc.QuestionId
                LEFT JOIN QuestionAnswerKeys qak ON q.Id = qak.QuestionId
                WHERE q.Id = @Id
                ORDER BY qc.Position";

            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    if (question == null)
                    {
                        question = new QuestionDTO
                        {
                            Id = reader.GetInt64(0),
                            SectionId = reader.GetInt64(1),
                            PassageId = reader.IsDBNull(2) ? (long?)null : reader.GetInt64(2),
                            QuestionType = reader.GetString(3),
                            QuestionText = reader.GetString(4),
                            Points = reader.GetDecimal(5),
                            Position = reader.GetInt32(6),
                            AnswerData = reader.IsDBNull(11) ? null : reader.GetString(11),
                            Choices = new List<QuestionChoiceDTO>()
                        };
                    }

                    if (!reader.IsDBNull(7)) // Has choice
                    {
                        var choice = new QuestionChoiceDTO
                        {
                            Id = reader.GetInt64(7),
                            QuestionId = id,
                            ChoiceText = reader.GetString(8),
                            IsCorrect = reader.GetBoolean(9),
                            Position = reader.GetInt32(10)
                        };

                        if (!question.Choices.Any(c => c.Id == choice.Id))
                        {
                            question.Choices.Add(choice);
                        }
                    }
                }
            }

            return question;
        }

        public long CreateQuestion(QuestionDTO question)
        {
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // Insert question
                    string queryQuestion = @"
                        INSERT INTO Questions (SectionId, PassageId, QuestionType, QuestionText, Points, Position)
                        VALUES (@SectionId, @PassageId, @QuestionType, @QuestionText, @Points, @Position);
                        SELECT CAST(SCOPE_IDENTITY() AS BIGINT);";

                    SqlCommand cmdQuestion = new SqlCommand(queryQuestion, conn, transaction);
                    cmdQuestion.Parameters.AddWithValue("@SectionId", question.SectionId);
                    cmdQuestion.Parameters.AddWithValue("@PassageId", (object)question.PassageId ?? DBNull.Value);
                    cmdQuestion.Parameters.AddWithValue("@QuestionType", question.QuestionType);
                    cmdQuestion.Parameters.AddWithValue("@QuestionText", question.QuestionText);
                    cmdQuestion.Parameters.AddWithValue("@Points", question.Points);
                    cmdQuestion.Parameters.AddWithValue("@Position", question.Position);

                    long questionId = (long)cmdQuestion.ExecuteScalar();

                    // Insert choices if any
                    if (question.Choices != null && question.Choices.Count > 0)
                    {
                        foreach (var choice in question.Choices)
                        {
                            string queryChoice = @"
                                INSERT INTO QuestionChoices (QuestionId, ChoiceText, IsCorrect, Position)
                                VALUES (@QuestionId, @ChoiceText, @IsCorrect, @Position)";

                            SqlCommand cmdChoice = new SqlCommand(queryChoice, conn, transaction);
                            cmdChoice.Parameters.AddWithValue("@QuestionId", questionId);
                            cmdChoice.Parameters.AddWithValue("@ChoiceText", choice.ChoiceText);
                            cmdChoice.Parameters.AddWithValue("@IsCorrect", choice.IsCorrect);
                            cmdChoice.Parameters.AddWithValue("@Position", choice.Position);
                            cmdChoice.ExecuteNonQuery();
                        }
                    }

                    // Insert answer key if provided
                    if (!string.IsNullOrEmpty(question.AnswerData))
                    {
                        string queryAnswer = @"
                            INSERT INTO QuestionAnswerKeys (QuestionId, AnswerData)
                            VALUES (@QuestionId, @AnswerData)";

                        SqlCommand cmdAnswer = new SqlCommand(queryAnswer, conn, transaction);
                        cmdAnswer.Parameters.AddWithValue("@QuestionId", questionId);
                        cmdAnswer.Parameters.AddWithValue("@AnswerData", question.AnswerData);
                        cmdAnswer.ExecuteNonQuery();
                    }

                    transaction.Commit();
                    return questionId;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public bool UpdateQuestion(QuestionDTO question)
        {
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // Update question
                    string queryQuestion = @"
                        UPDATE Questions
                        SET PassageId = @PassageId, QuestionType = @QuestionType,
                            QuestionText = @QuestionText, Points = @Points, Position = @Position
                        WHERE Id = @Id";

                    SqlCommand cmdQuestion = new SqlCommand(queryQuestion, conn, transaction);
                    cmdQuestion.Parameters.AddWithValue("@Id", question.Id);
                    cmdQuestion.Parameters.AddWithValue("@PassageId", (object)question.PassageId ?? DBNull.Value);
                    cmdQuestion.Parameters.AddWithValue("@QuestionType", question.QuestionType);
                    cmdQuestion.Parameters.AddWithValue("@QuestionText", question.QuestionText);
                    cmdQuestion.Parameters.AddWithValue("@Points", question.Points);
                    cmdQuestion.Parameters.AddWithValue("@Position", question.Position);
                    cmdQuestion.ExecuteNonQuery();

                    transaction.Commit();
                    return true;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public bool DeleteQuestion(long id)
        {
            string query = "DELETE FROM Questions WHERE Id = @Id";

            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // ===== QUESTION CHOICES =====
        public bool SaveQuestionChoices(long questionId, List<QuestionChoiceDTO> choices)
        {
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // Delete existing choices
                    string deleteQuery = "DELETE FROM QuestionChoices WHERE QuestionId = @QuestionId";
                    SqlCommand deleteCmd = new SqlCommand(deleteQuery, conn, transaction);
                    deleteCmd.Parameters.AddWithValue("@QuestionId", questionId);
                    deleteCmd.ExecuteNonQuery();

                    // Insert new choices
                    foreach (var choice in choices)
                    {
                        string insertQuery = @"
                            INSERT INTO QuestionChoices (QuestionId, ChoiceText, IsCorrect, Position)
                            VALUES (@QuestionId, @ChoiceText, @IsCorrect, @Position)";

                        SqlCommand insertCmd = new SqlCommand(insertQuery, conn, transaction);
                        insertCmd.Parameters.AddWithValue("@QuestionId", questionId);
                        insertCmd.Parameters.AddWithValue("@ChoiceText", choice.ChoiceText);
                        insertCmd.Parameters.AddWithValue("@IsCorrect", choice.IsCorrect);
                        insertCmd.Parameters.AddWithValue("@Position", choice.Position);
                        insertCmd.ExecuteNonQuery();
                    }

                    transaction.Commit();
                    return true;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        // ===== ANSWER KEY =====
        public bool SaveAnswerKey(long questionId, string answerData)
        {
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                conn.Open();

                // Check if answer key exists
                string checkQuery = "SELECT COUNT(*) FROM QuestionAnswerKeys WHERE QuestionId = @QuestionId";
                SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                checkCmd.Parameters.AddWithValue("@QuestionId", questionId);
                int count = (int)checkCmd.ExecuteScalar();

                string query;
                if (count > 0)
                {
                    query = "UPDATE QuestionAnswerKeys SET AnswerData = @AnswerData WHERE QuestionId = @QuestionId";
                }
                else
                {
                    query = "INSERT INTO QuestionAnswerKeys (QuestionId, AnswerData) VALUES (@QuestionId, @AnswerData)";
                }

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@QuestionId", questionId);
                cmd.Parameters.AddWithValue("@AnswerData", answerData);
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public string GetAnswerKey(long questionId)
        {
            string query = "SELECT AnswerData FROM QuestionAnswerKeys WHERE QuestionId = @QuestionId";

            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@QuestionId", questionId);
                conn.Open();
                var result = cmd.ExecuteScalar();
                return result != null ? result.ToString() : null;
            }
        }

    }
}
