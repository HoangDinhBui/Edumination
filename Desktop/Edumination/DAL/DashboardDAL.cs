using IELTS.DTO;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IELTS.DAL
{
    public class DashboardDAL
    {
        private readonly string _connectionString;

        public DashboardDAL()
        {         
        }

        public DashboardStatisticsDTO GetDashboardStatistics()
        {
            var stats = new DashboardStatisticsDTO
            {
                RecentActivities = new List<RecentActivityDTO>(),
                TopTests = new List<TestStatDTO>(),
                TopCourses = new List<CourseStatDTO>(),
                MonthlyStudents = new List<MonthlyDataDTO>(),
                MonthlyTests = new List<MonthlyDataDTO>()
            };

            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                conn.Open();

                // Get total counts
                stats.TotalStudents = GetTotalStudents(conn);
                stats.TotalTests = GetTotalTests(conn);
                stats.TotalCourses = GetTotalCourses(conn);
                stats.ActiveEnrollments = GetActiveEnrollments(conn);

                // Get growth percentages
                stats.StudentGrowth = GetStudentGrowth(conn);
                stats.TestGrowth = GetTestGrowth(conn);
                stats.CourseGrowth = GetCourseGrowth(conn);
                stats.EnrollmentGrowth = GetEnrollmentGrowth(conn);

                // Get recent activities
                stats.RecentActivities = GetRecentActivities(conn);

                // Get top tests
                stats.TopTests = GetTopTests(conn);

                // Get top courses
                stats.TopCourses = GetTopCourses(conn);

                // Get monthly data
                stats.MonthlyStudents = GetMonthlyStudents(conn);
                stats.MonthlyTests = GetMonthlyTests(conn);
            }

            return stats;
        }

        private int GetTotalStudents(SqlConnection conn)
        {
            string query = "SELECT COUNT(*) FROM Users WHERE Role = 'STUDENT' AND IsActive = 1";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                return (int)cmd.ExecuteScalar();
            }
        }

        private int GetTotalTests(SqlConnection conn)
        {
            string query = "SELECT COUNT(*) FROM TestPapers WHERE IsPublished = 1";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                return (int)cmd.ExecuteScalar();
            }
        }

        private int GetTotalCourses(SqlConnection conn)
        {
            string query = "SELECT COUNT(*) FROM Courses WHERE IsPublished = 1";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                return (int)cmd.ExecuteScalar();
            }
        }

        private int GetActiveEnrollments(SqlConnection conn)
        {
            string query = "SELECT COUNT(*) FROM Enrollments";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                return (int)cmd.ExecuteScalar();
            }
        }

        private decimal GetStudentGrowth(SqlConnection conn)
        {
            string query = @"
                DECLARE @CurrentMonth INT = (SELECT COUNT(*) FROM Users 
                    WHERE Role = 'STUDENT' 
                    AND MONTH(CreatedAt) = MONTH(GETDATE()) 
                    AND YEAR(CreatedAt) = YEAR(GETDATE()));
                DECLARE @LastMonth INT = (SELECT COUNT(*) FROM Users 
                    WHERE Role = 'STUDENT' 
                    AND MONTH(CreatedAt) = MONTH(DATEADD(MONTH, -1, GETDATE())) 
                    AND YEAR(CreatedAt) = YEAR(DATEADD(MONTH, -1, GETDATE())));
                
                SELECT CASE 
                    WHEN @LastMonth = 0 THEN 100
                    ELSE (CAST(@CurrentMonth - @LastMonth AS DECIMAL) / @LastMonth) * 100
                END";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                var result = cmd.ExecuteScalar();
                return result != DBNull.Value ? Convert.ToDecimal(result) : 0;
            }
        }

        private decimal GetTestGrowth(SqlConnection conn)
        {
            string query = @"
                DECLARE @CurrentMonth INT = (SELECT COUNT(*) FROM TestAttempts 
                    WHERE MONTH(StartedAt) = MONTH(GETDATE()) 
                    AND YEAR(StartedAt) = YEAR(GETDATE()));
                DECLARE @LastMonth INT = (SELECT COUNT(*) FROM TestAttempts 
                    WHERE MONTH(StartedAt) = MONTH(DATEADD(MONTH, -1, GETDATE())) 
                    AND YEAR(StartedAt) = YEAR(DATEADD(MONTH, -1, GETDATE())));
                
                SELECT CASE 
                    WHEN @LastMonth = 0 THEN 100
                    ELSE (CAST(@CurrentMonth - @LastMonth AS DECIMAL) / @LastMonth) * 100
                END";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                var result = cmd.ExecuteScalar();
                return result != DBNull.Value ? Convert.ToDecimal(result) : 0;
            }
        }

        private decimal GetCourseGrowth(SqlConnection conn)
        {
            string query = @"
                DECLARE @CurrentMonth INT = (SELECT COUNT(*) FROM Courses 
                    WHERE MONTH(CreatedAt) = MONTH(GETDATE()) 
                    AND YEAR(CreatedAt) = YEAR(GETDATE()));
                DECLARE @LastMonth INT = (SELECT COUNT(*) FROM Courses 
                    WHERE MONTH(CreatedAt) = MONTH(DATEADD(MONTH, -1, GETDATE())) 
                    AND YEAR(CreatedAt) = YEAR(DATEADD(MONTH, -1, GETDATE())));
                
                SELECT CASE 
                    WHEN @LastMonth = 0 THEN 100
                    ELSE (CAST(@CurrentMonth - @LastMonth AS DECIMAL) / @LastMonth) * 100
                END";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                var result = cmd.ExecuteScalar();
                return result != DBNull.Value ? Convert.ToDecimal(result) : 0;
            }
        }

        private decimal GetEnrollmentGrowth(SqlConnection conn)
        {
            string query = @"
                DECLARE @CurrentMonth INT = (SELECT COUNT(*) FROM Enrollments 
                    WHERE MONTH(EnrolledAt) = MONTH(GETDATE()) 
                    AND YEAR(EnrolledAt) = YEAR(GETDATE()));
                DECLARE @LastMonth INT = (SELECT COUNT(*) FROM Enrollments 
                    WHERE MONTH(EnrolledAt) = MONTH(DATEADD(MONTH, -1, GETDATE())) 
                    AND YEAR(EnrolledAt) = YEAR(DATEADD(MONTH, -1, GETDATE())));
                
                SELECT CASE 
                    WHEN @LastMonth = 0 THEN 100
                    ELSE (CAST(@CurrentMonth - @LastMonth AS DECIMAL) / @LastMonth) * 100
                END";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                var result = cmd.ExecuteScalar();
                return result != DBNull.Value ? Convert.ToDecimal(result) : 0;
            }
        }

        private List<RecentActivityDTO> GetRecentActivities(SqlConnection conn)
        {
            var activities = new List<RecentActivityDTO>();

            string query = @"
                SELECT TOP 10 
                    u.FullName,
                    CASE 
                        WHEN ta.Id IS NOT NULL THEN 'Completed test: ' + tp.Title
                        WHEN e.UserId IS NOT NULL THEN 'Enrolled in: ' + c.Title
                        ELSE 'Joined the platform'
                    END AS Activity,
                    COALESCE(ta.FinishedAt, e.EnrolledAt, u.CreatedAt) AS Time
                FROM Users u
                LEFT JOIN TestAttempts ta ON u.Id = ta.UserId AND ta.Status = 'GRADED'
                LEFT JOIN TestPapers tp ON ta.PaperId = tp.Id
                LEFT JOIN Enrollments e ON u.Id = e.UserId
                LEFT JOIN Courses c ON e.CourseId = c.Id
                WHERE u.Role = 'STUDENT'
                ORDER BY Time DESC";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    activities.Add(new RecentActivityDTO
                    {
                        UserName = reader.GetString(0),
                        Activity = reader.GetString(1),
                        Time = reader.GetDateTime(2),
                        Icon = "👤"
                    });
                }
            }

            return activities;
        }

        private List<TestStatDTO> GetTopTests(SqlConnection conn)
        {
            var tests = new List<TestStatDTO>();

            string query = @"
                SELECT TOP 5
                    tp.Title,
                    COUNT(ta.Id) AS AttemptCount,
                    AVG(CAST(ta.OverallBand AS DECIMAL(5,2))) AS AverageBand
                FROM TestPapers tp
                LEFT JOIN TestAttempts ta ON tp.Id = ta.PaperId
                WHERE tp.IsPublished = 1
                GROUP BY tp.Title
                ORDER BY AttemptCount DESC";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    tests.Add(new TestStatDTO
                    {
                        TestTitle = reader.GetString(0),
                        AttemptCount = reader.GetInt32(1),
                        AverageBand = reader.IsDBNull(2) ? 0 : reader.GetDecimal(2)
                    });
                }
            }

            return tests;
        }

        private List<CourseStatDTO> GetTopCourses(SqlConnection conn)
        {
            var courses = new List<CourseStatDTO>();

            string query = @"
                SELECT TOP 5
                    c.Title,
                    COUNT(DISTINCT e.UserId) AS EnrollmentCount,
                    CASE 
                        WHEN COUNT(DISTINCT e.UserId) > 0 
                        THEN (COUNT(DISTINCT lc.UserId) * 100) / COUNT(DISTINCT e.UserId)
                        ELSE 0
                    END AS CompletionRate
                FROM Courses c
                LEFT JOIN Enrollments e ON c.Id = e.CourseId
                LEFT JOIN Lessons l ON c.Id = l.CourseId
                LEFT JOIN LessonCompletions lc ON l.Id = lc.LessonId
                WHERE c.IsPublished = 1
                GROUP BY c.Title
                ORDER BY EnrollmentCount DESC";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    courses.Add(new CourseStatDTO
                    {
                        CourseTitle = reader.GetString(0),
                        EnrollmentCount = reader.GetInt32(1),
                        CompletionRate = reader.GetInt32(2)
                    });
                }
            }

            return courses;
        }

        private List<MonthlyDataDTO> GetMonthlyStudents(SqlConnection conn)
        {
            var data = new List<MonthlyDataDTO>();

            string query = @"
                SELECT 
                    FORMAT(CreatedAt, 'MMM') AS Month,
                    COUNT(*) AS Count
                FROM Users
                WHERE Role = 'STUDENT' 
                    AND CreatedAt >= DATEADD(MONTH, -6, GETDATE())
                GROUP BY FORMAT(CreatedAt, 'MMM'), MONTH(CreatedAt)
                ORDER BY MONTH(CreatedAt)";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    data.Add(new MonthlyDataDTO
                    {
                        Month = reader.GetString(0),
                        Count = reader.GetInt32(1)
                    });
                }
            }

            return data;
        }

        private List<MonthlyDataDTO> GetMonthlyTests(SqlConnection conn)
        {
            var data = new List<MonthlyDataDTO>();

            string query = @"
                SELECT 
                    FORMAT(StartedAt, 'MMM') AS Month,
                    COUNT(*) AS Count
                FROM TestAttempts
                WHERE StartedAt >= DATEADD(MONTH, -6, GETDATE())
                GROUP BY FORMAT(StartedAt, 'MMM'), MONTH(StartedAt)
                ORDER BY MONTH(StartedAt)";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    data.Add(new MonthlyDataDTO
                    {
                        Month = reader.GetString(0),
                        Count = reader.GetInt32(1)
                    });
                }
            }

            return data;
        }
    }
}
