using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
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

        public long CreateSection(long paperId, string skill, int? timeLimitMinutes, string audioFilePath)
        {
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                string query = @"INSERT INTO TestSections (PaperId, Skill, TimeLimitMinutes, AudioFilePath) 
                                OUTPUT INSERTED.Id
                                VALUES (@PaperId, @Skill, @TimeLimitMinutes, @AudioFilePath)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@PaperId", paperId);
                cmd.Parameters.AddWithValue("@Skill", skill);
                cmd.Parameters.AddWithValue("@TimeLimitMinutes", timeLimitMinutes ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@AudioFilePath", audioFilePath ?? (object)DBNull.Value);

                conn.Open();
                return (long)cmd.ExecuteScalar();
            }
        }
    }
}
