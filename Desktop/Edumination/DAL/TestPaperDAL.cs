using IELTS.DAL;
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
            using SqlConnection conn = DatabaseConnection.GetConnection();
            string sql = @"INSERT INTO TestPapers
                   (Code, Title, Description, CreatedBy, PdfFileName, PdfFilePath)
                   VALUES (@Code, @Title, @Desc, @CreatedBy, @PdfFileName, @PdfFilePath)";

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Code", code);
                cmd.Parameters.AddWithValue("@Title", title);
                cmd.Parameters.AddWithValue("@Desc", description);
                cmd.Parameters.AddWithValue("@CreatedBy", createdBy);
                cmd.Parameters.AddWithValue("@PdfFileName", pdfFileName);
                cmd.Parameters.AddWithValue("@PdfFilePath", pdfFilePath);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}
