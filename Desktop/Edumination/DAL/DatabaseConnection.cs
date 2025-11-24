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
    using IELTS.DTO;
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

        public List<CourseDTO> GetListCourses(string keyword = "")
        {
            var list = new List<CourseDTO>();
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                conn.Open();
                string sql = @"SELECT c.*, u.FullName as CreatedByName 
                               FROM Courses c
                               LEFT JOIN Users u ON c.CreatedBy = u.Id
                               WHERE c.Title LIKE @Key OR c.Description LIKE @Key 
                               ORDER BY c.CreatedAt DESC";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Key", "%" + keyword + "%");
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new CourseDTO
                            {
                                Id = Convert.ToInt64(reader["Id"]),
                                Title = reader["Title"].ToString(),
                                Description = reader["Description"] != DBNull.Value ? reader["Description"].ToString() : "",
                                Level = reader["Level"].ToString(),
                                PriceVND = Convert.ToInt32(reader["PriceVND"]),
                                IsPublished = Convert.ToBoolean(reader["IsPublished"]),
                                CreatedBy = Convert.ToInt64(reader["CreatedBy"]),
                                CreatedByName = reader["CreatedByName"] != DBNull.Value ? reader["CreatedByName"].ToString() : "Unknown",
                                CreatedAt = Convert.ToDateTime(reader["CreatedAt"])
                            });
                        }
                    }
                }
            }
            return list;
        }

        public bool AddCourse(CourseDTO course)
        {
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand(
                @"INSERT INTO Courses (Title, Description, Level, PriceVND, IsPublished, CreatedBy, CreatedAt) 
          VALUES (@Title, @Desc, @Level, @Price, @Published, @CreatedBy, GETDATE())", conn))
            {
                cmd.Parameters.AddWithValue("@Title", course.Title);
                cmd.Parameters.AddWithValue("@Desc", (object)course.Description ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Level", course.Level); // DB: NVARCHAR(30)
                cmd.Parameters.AddWithValue("@Price", course.PriceVND); // DB: INT
                cmd.Parameters.AddWithValue("@Published", course.IsPublished);
                cmd.Parameters.AddWithValue("@CreatedBy", course.CreatedBy);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool UpdateCourse(CourseDTO course)
        {
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand(
                @"UPDATE Courses 
                  SET Title=@Title, Description=@Desc, Level=@Level, PriceVND=@Price, IsPublished=@Published
                  WHERE Id=@Id", conn))
            {
                cmd.Parameters.AddWithValue("@Id", course.Id);
                cmd.Parameters.AddWithValue("@Title", course.Title);
                cmd.Parameters.AddWithValue("@Desc", (object)course.Description ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Level", course.Level);
                cmd.Parameters.AddWithValue("@Price", course.PriceVND);
                cmd.Parameters.AddWithValue("@Published", course.IsPublished);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool DeleteCourse(long id)
        {
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand("DELETE FROM Courses WHERE Id = @Id", conn))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();
                try
                {
                    return cmd.ExecuteNonQuery() > 0;
                }
                catch (SqlException ex)
                {
                    // Mã lỗi 547 là lỗi vi phạm khóa ngoại (Foreign Key)
                    if (ex.Number == 547)
                    {
                        // Ném lỗi ra để BLL/UI bắt được và hiện thông báo
                        throw new Exception("Khóa học này đã có học viên đăng ký hoặc đơn hàng, không thể xóa!");
                    }
                    throw; // Ném các lỗi khác nếu có
                }
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
