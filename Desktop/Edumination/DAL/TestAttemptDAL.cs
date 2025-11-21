using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
namespace IELTS.DAL
{
    public class TestAttemptDAL
    {
        public long CreateAttempt(long userId, long paperId)
        {
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                // Lấy attempt number tiếp theo
                string getMaxQuery = @"SELECT ISNULL(MAX(AttemptNumber), 0) + 1 
                                      FROM TestAttempts 
                                      WHERE UserId = @UserId AND PaperId = @PaperId";

                SqlCommand getMaxCmd = new SqlCommand(getMaxQuery, conn);
                getMaxCmd.Parameters.AddWithValue("@UserId", userId);
                getMaxCmd.Parameters.AddWithValue("@PaperId", paperId);

                conn.Open();
                int attemptNumber = (int)getMaxCmd.ExecuteScalar();

                // Tạo attempt mới
                string insertQuery = @"INSERT INTO TestAttempts (UserId, PaperId, AttemptNumber) 
                                      OUTPUT INSERTED.Id
                                      VALUES (@UserId, @PaperId, @AttemptNumber)";

                SqlCommand insertCmd = new SqlCommand(insertQuery, conn);
                insertCmd.Parameters.AddWithValue("@UserId", userId);
                insertCmd.Parameters.AddWithValue("@PaperId", paperId);
                insertCmd.Parameters.AddWithValue("@AttemptNumber", attemptNumber);

                return (long)insertCmd.ExecuteScalar();
            }
        }

        public DataTable GetAttemptsByUserId(long userId)
        {
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                string query = @"SELECT ta.*, tp.Title as PaperTitle, tp.Code as PaperCode
                                FROM TestAttempts ta
                                JOIN TestPapers tp ON ta.PaperId = tp.Id
                                WHERE ta.UserId = @UserId
                                ORDER BY ta.StartedAt DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserId", userId);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }

        public bool FinishAttempt(long attemptId, decimal overallBand)
        {
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                string query = @"UPDATE TestAttempts 
                                SET FinishedAt = GETDATE(), Status = 'GRADED', OverallBand = @OverallBand
                                WHERE Id = @AttemptId";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@AttemptId", attemptId);
                cmd.Parameters.AddWithValue("@OverallBand", overallBand);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}
