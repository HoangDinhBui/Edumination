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
    }
}
