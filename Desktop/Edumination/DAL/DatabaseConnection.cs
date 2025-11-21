using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace IELTS.DAL
{
    // 1. DATABASE CONNECTION CLASS
    // =========================================================
    using Microsoft.Data.SqlClient;
    using System.Configuration;

    public class DatabaseConnection
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["IELTSConnection"].ConnectionString;

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }

        public static void TestConnection()
        {
            try
            {
                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();
                    Console.WriteLine("Connection successful!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Database connection failed: " + ex.Message);
            }
        }
    }


    // =========================================================
    // 2. USER DAL
    // =========================================================
    


    // =========================================================
    // 3. TEST PAPER DAL
    // =========================================================
    

    // =========================================================
    // 4. TEST SECTION DAL
    // =========================================================
    

    // =========================================================
    // 5. QUESTION DAL
    // =========================================================
    

    // =========================================================
    // 6. TEST ATTEMPT DAL
    // =========================================================
    

    // =========================================================
    // 7. SECTION ATTEMPT DAL
    // =========================================================
    

    // =========================================================
    // 8. ANSWER DAL
    // =========================================================
    

    // =========================================================
    // 9. COURSE DAL
    // =========================================================
    public class CourseDAL
    {
        public DataTable GetAllPublishedCourses()
        {
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                string query = @"SELECT c.*, u.FullName as CreatedByName
                                FROM Courses c
                                JOIN Users u ON c.CreatedBy = u.Id
                                WHERE c.IsPublished = 1
                                ORDER BY c.CreatedAt DESC";

                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }

        public DataTable GetCourseById(long courseId)
        {
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                string query = "SELECT * FROM Courses WHERE Id = @CourseId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CourseId", courseId);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }

        public bool EnrollCourse(long userId, long courseId)
        {
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                string query = @"IF NOT EXISTS (SELECT 1 FROM Enrollments WHERE UserId = @UserId AND CourseId = @CourseId)
                                    INSERT INTO Enrollments (UserId, CourseId) VALUES (@UserId, @CourseId)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@CourseId", courseId);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public DataTable GetEnrolledCoursesByUserId(long userId)
        {
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                string query = @"SELECT c.*, e.EnrolledAt
                                FROM Courses c
                                JOIN Enrollments e ON c.Id = e.CourseId
                                WHERE e.UserId = @UserId
                                ORDER BY e.EnrolledAt DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserId", userId);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }
    }

    // =========================================================
    // 10. STATISTICS DAL
    // =========================================================
    public class StatisticsDAL
    {
        public void UpdateUserStatistics(long userId)
        {
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                string query = @"
                    MERGE INTO UserStatistics AS target
                    USING (
                        SELECT 
                            @UserId AS UserId,
                            COUNT(DISTINCT ta.Id) AS TotalTests,
                            MAX(ta.OverallBand) AS BestBand,
                            AVG(CASE WHEN ts.Skill = 'LISTENING' THEN sa.BandScore END) AS AvgListening,
                            AVG(CASE WHEN ts.Skill = 'READING' THEN sa.BandScore END) AS AvgReading,
                            AVG(CASE WHEN ts.Skill = 'WRITING' THEN sa.BandScore END) AS AvgWriting,
                            AVG(CASE WHEN ts.Skill = 'SPEAKING' THEN sa.BandScore END) AS AvgSpeaking
                        FROM TestAttempts ta
                        LEFT JOIN SectionAttempts sa ON ta.Id = sa.TestAttemptId
                        LEFT JOIN TestSections ts ON sa.SectionId = ts.Id
                        WHERE ta.UserId = @UserId AND ta.Status = 'GRADED'
                    ) AS source
                    ON target.UserId = source.UserId
                    WHEN MATCHED THEN
                        UPDATE SET 
                            TotalTests = source.TotalTests,
                            BestBand = source.BestBand,
                            AverageListening = source.AvgListening,
                            AverageReading = source.AvgReading,
                            AverageWriting = source.AvgWriting,
                            AverageSpeaking = source.AvgSpeaking,
                            UpdatedAt = GETDATE()
                    WHEN NOT MATCHED THEN
                        INSERT (UserId, TotalTests, BestBand, AverageListening, AverageReading, AverageWriting, AverageSpeaking)
                        VALUES (source.UserId, source.TotalTests, source.BestBand, source.AvgListening, source.AvgReading, source.AvgWriting, source.AvgSpeaking);
                ";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserId", userId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public DataTable GetUserStatistics(long userId)
        {
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                string query = "SELECT * FROM UserStatistics WHERE UserId = @UserId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserId", userId);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }
    }

}
