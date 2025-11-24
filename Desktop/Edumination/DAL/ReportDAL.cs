using IELTS.DTO;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IELTS.DAL
{
    public class ReportDAL
    {
        public DashboardSummaryDTO GetSummary()
        {
            var summary = new DashboardSummaryDTO();
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                conn.Open();

                // Đếm học viên (Role = STUDENT)
                using (var cmd = new SqlCommand("SELECT COUNT(*) FROM Users WHERE Role = 'STUDENT'", conn))
                    summary.TotalStudents = (int)cmd.ExecuteScalar();

                // Đếm khóa học (IsPublished = 1) - Bảng Courses
                using (var cmd = new SqlCommand("SELECT COUNT(*) FROM Courses WHERE IsPublished = 1", conn))
                    summary.TotalCourses = (int)cmd.ExecuteScalar();

                // Tổng doanh thu (Bảng Orders: Status = 'PAID', cột TotalVND)
                string sqlRevenue = "SELECT ISNULL(SUM(TotalVND), 0) FROM Orders WHERE Status = 'PAID'";
                using (var cmd = new SqlCommand(sqlRevenue, conn))
                    summary.TotalRevenue = Convert.ToDecimal(cmd.ExecuteScalar());

                // Tổng lượt thi (Bảng TestAttempts)
                using (var cmd = new SqlCommand("SELECT COUNT(*) FROM TestAttempts", conn))
                    summary.TotalTestsTaken = (int)cmd.ExecuteScalar();
            }
            return summary;
        }

        public List<RevenueChartDTO> GetRevenueLast6Months()
        {
            var list = new List<RevenueChartDTO>();
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                conn.Open();
                // Query này chạy tốt trên SQL Server 2012 trở lên
                string sql = @"
                    SELECT 
                        FORMAT(CreatedAt, 'MM/yyyy') as MonthTime, 
                        SUM(TotalVND) as Total
                    FROM Orders 
                    WHERE Status = 'PAID' 
                    AND CreatedAt >= DATEADD(MONTH, -6, GETDATE())
                    GROUP BY FORMAT(CreatedAt, 'MM/yyyy'), YEAR(CreatedAt), MONTH(CreatedAt)
                    ORDER BY YEAR(CreatedAt), MONTH(CreatedAt)";

                using (var cmd = new SqlCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new RevenueChartDTO
                        {
                            Month = reader["MonthTime"].ToString(),
                            Revenue = Convert.ToDecimal(reader["Total"])
                        });
                    }
                }
            }
            return list;
        }
    }
}
