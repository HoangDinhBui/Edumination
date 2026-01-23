using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace IELTS.DAL
{
    internal class CourseStudentDAL
    {
        // 1. Danh sách học viên đã đăng ký (Enrollments)
        public DataTable GetEnrolledUsersByCourse(long courseId, string keyword = "")
        {
            using var conn = DatabaseConnection.GetConnection();

            string sql = @"
                SELECT 
                    u.Id AS UserId,
                    u.FullName,
                    u.Email,
                    e.EnrolledAt
                FROM Enrollments e
                JOIN Users u ON u.Id = e.UserId
                WHERE e.CourseId = @CourseId
                  AND (u.FullName LIKE @Key OR u.Email LIKE @Key)
                ORDER BY e.EnrolledAt DESC";

            var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@CourseId", courseId);
            cmd.Parameters.AddWithValue("@Key", "%" + keyword + "%");

            var dt = new DataTable();
            new SqlDataAdapter(cmd).Fill(dt);
            return dt;
        }

        // 2. Kiểm tra học viên đã bắt đầu học chưa (CourseStudents)
        public DataTable GetCourseStudentDetail(long courseId, long studentId)
        {
            using var conn = DatabaseConnection.GetConnection();

            string sql = @"
                SELECT 
                    cs.Id,
                    cs.CompletionPercentage,
                    cs.Status,
                    cs.LastAccessedAt,
                    i.FullName AS InstructorName
                FROM CourseStudents cs
                LEFT JOIN Users i ON i.Id = cs.InstructorId
                WHERE cs.CourseId = @CourseId
                  AND cs.StudentId = @StudentId";

            var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@CourseId", courseId);
            cmd.Parameters.AddWithValue("@StudentId", studentId);

            var dt = new DataTable();
            new SqlDataAdapter(cmd).Fill(dt);
            return dt;
        }
    }
}
