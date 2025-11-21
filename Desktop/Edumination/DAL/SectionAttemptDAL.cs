using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
namespace IELTS.DAL
{
    public class SectionAttemptDAL
    {
        public long CreateSectionAttempt(long testAttemptId, long sectionId)
        {
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                string query = @"INSERT INTO SectionAttempts (TestAttemptId, SectionId) 
                                OUTPUT INSERTED.Id
                                VALUES (@TestAttemptId, @SectionId)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@TestAttemptId", testAttemptId);
                cmd.Parameters.AddWithValue("@SectionId", sectionId);

                conn.Open();
                return (long)cmd.ExecuteScalar();
            }
        }

        public bool UpdateSectionScore(long sectionAttemptId, decimal rawScore, decimal bandScore)
        {
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                string query = @"UPDATE SectionAttempts 
                                SET RawScore = @RawScore, BandScore = @BandScore, 
                                    FinishedAt = GETDATE(), Status = 'GRADED'
                                WHERE Id = @SectionAttemptId";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@SectionAttemptId", sectionAttemptId);
                cmd.Parameters.AddWithValue("@RawScore", rawScore);
                cmd.Parameters.AddWithValue("@BandScore", bandScore);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public DataTable GetSectionAttemptsByTestAttemptId(long testAttemptId)
        {
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                string query = @"SELECT sa.*, ts.Skill
                                FROM SectionAttempts sa
                                JOIN TestSections ts ON sa.SectionId = ts.Id
                                WHERE sa.TestAttemptId = @TestAttemptId";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@TestAttemptId", testAttemptId);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }
    }
}
