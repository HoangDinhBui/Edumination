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
    public class TestSectionDAL
    {
        public DataTable GetSectionsByPaperId(long paperId)
        {
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                string query = "SELECT * FROM TestSections WHERE PaperId = @PaperId ORDER BY Skill";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@PaperId", paperId);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }

        public long InsertTestSection(TestSectionDTO section)
        {
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                conn.Open();

                string query = @"
                    INSERT INTO TestSections (PaperId, Skill, TimeLimitMinutes, AudioFilePath, PdfFileName, PdfFilePath)
                    VALUES (@PaperId, @Skill, @TimeLimitMinutes, @AudioFilePath, @PdfFileName, @PdfFilePath);
                    SELECT CAST(SCOPE_IDENTITY() AS BIGINT);";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@PaperId", section.PaperId);
                    cmd.Parameters.AddWithValue("@Skill", section.Skill);
                    cmd.Parameters.AddWithValue("@TimeLimitMinutes", (object)section.TimeLimitMinutes ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@AudioFilePath", (object)section.AudioFilePath ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@PdfFileName", (object)section.PdfFileName ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@PdfFilePath", (object)section.PdfFilePath ?? DBNull.Value);

                    return (long)cmd.ExecuteScalar();
                }
            }
        }

        public bool UpdateTestSection(TestSectionDTO section)
        {
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                conn.Open();

                string query = @"
                    UPDATE TestSections 
                    SET Skill = @Skill, 
                        TimeLimitMinutes = @TimeLimitMinutes, 
                        AudioFilePath = @AudioFilePath,
                        PdfFileName = @PdfFileName,
                        PdfFilePath = @PdfFilePath
                    WHERE Id = @Id";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", section.Id);
                    cmd.Parameters.AddWithValue("@Skill", section.Skill);
                    cmd.Parameters.AddWithValue("@TimeLimitMinutes", (object)section.TimeLimitMinutes ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@AudioFilePath", (object)section.AudioFilePath ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@PdfFileName", (object)section.PdfFileName ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@PdfFilePath", (object)section.PdfFilePath ?? DBNull.Value);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        // <summary>
        /// Lấy TestSection theo ID
        /// </summary>
        public TestSectionDTO GetTestSectionById(long id)
        {
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                conn.Open();

                string query = @"
                    SELECT Id, PaperId, Skill, TimeLimitMinutes, AudioFilePath, PdfFileName, PdfFilePath
                    FROM TestSections
                    WHERE Id = @Id";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return MapToDTO(reader);
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Lấy tất cả TestSections của một Paper
        /// </summary>
        public List<TestSectionDTO> GetTestSectionsByPaperId(long paperId)
        {
            List<TestSectionDTO> sections = new List<TestSectionDTO>();

            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                conn.Open();

                string query = @"
                    SELECT Id, PaperId, Skill, TimeLimitMinutes, AudioFilePath, PdfFileName, PdfFilePath
                    FROM TestSections
                    WHERE PaperId = @PaperId";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@PaperId", paperId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            sections.Add(MapToDTO(reader));
                        }
                    }
                }
            }
            return sections;
        }

        /// <summary>
        /// Xóa TestSection
        /// </summary>
        public bool DeleteTestSection(long id)
        {
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                conn.Open();

                string query = "DELETE FROM TestSections WHERE Id = @Id";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        private TestSectionDTO MapToDTO(SqlDataReader reader)
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

        public List<TestSectionDTO> GetByPaperId(long paperId)
        {
            List<TestSectionDTO> list = new();

            using SqlConnection conn = DatabaseConnection.GetConnection();
            string sql = @"
                SELECT Id, PaperId, Skill, TimeLimitMinutes,
                       AudioFilePath, PdfFileName, PdfFilePath
                FROM TestSections
                WHERE PaperId = @PaperId
                ORDER BY Skill
            ";

            using SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@PaperId", paperId);

            conn.Open();
            using SqlDataReader r = cmd.ExecuteReader();
            while (r.Read())
            {
                list.Add(new TestSectionDTO
                {
                    Id = r.GetInt64(0),
                    PaperId = r.GetInt64(1),
                    Skill = r.GetString(2),
                    TimeLimitMinutes = r.IsDBNull(3) ? null : r.GetInt32(3),
                    AudioFilePath = r.IsDBNull(4) ? null : r.GetString(4),
                    PdfFileName = r.IsDBNull(5) ? null : r.GetString(5),
                    PdfFilePath = r.IsDBNull(6) ? null : r.GetString(6)
                });
            }

            return list;
        }
    }
}

