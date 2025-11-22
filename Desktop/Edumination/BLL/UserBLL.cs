using IELTS.DAL;
using IELTS.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text;
using System.Threading.Tasks;

namespace IELTS.BLL
{
    // 1. USER BLL
    // =========================================================
    public class UserBLL
    {
        private UserDAL userDAL = new UserDAL();

        // Hash password bằng SHA256
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public DataTable Login(string email, string password)
        {
            // Validation
            if (string.IsNullOrWhiteSpace(email))
                throw new Exception("Email không được để trống!");

            if (string.IsNullOrWhiteSpace(password))
                throw new Exception("Mật khẩu không được để trống!");

            if (!email.Contains("@"))
                throw new Exception("Email không hợp lệ!");

            string passwordHash = HashPassword(password);
            DataTable result = userDAL.Login(email, passwordHash);

            if (result.Rows.Count == 0)
                throw new Exception("Email hoặc mật khẩu không đúng!");

            return result;
        }

        public bool Register(string email, string password, string confirmPassword, string fullName)
        {
            // Validation
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(fullName))
                throw new Exception("Vui lòng điền đầy đủ thông tin!");

            if (!email.Contains("@"))
                throw new Exception("Email không hợp lệ!");

            if (password.Length < 6)
                throw new Exception("Mật khẩu phải có ít nhất 6 ký tự!");

            if (password != confirmPassword)
                throw new Exception("Mật khẩu xác nhận không khớp!");

            string passwordHash = HashPassword(password);
            return userDAL.Register(email, passwordHash, fullName);
        }

        //public DataTable GetUserById(long userId)
        //{
        //    if (userId <= 0)
        //        throw new Exception("User ID không hợp lệ!");

        //    return userDAL.GetUserById(userId);
        //}

        public UserDTO GetUserById(long id)
        {
            return userDAL.GetUserById(id);
        }

        public bool UpdateProfile(long userId, string fullName, string phone, DateTime? dob)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                throw new Exception("Họ tên không được để trống!");

            if (dob.HasValue && dob.Value > DateTime.Now)
                throw new Exception("Ngày sinh không hợp lệ!");

            return userDAL.UpdateProfile(userId, fullName, phone, dob);
        }

        public DataTable GetAllUsers()
        {
            return userDAL.GetAllUsers();
        }
    }

    // =========================================================
    // 2. TEST PAPER BLL
    // =========================================================
    

    // =========================================================
    // 3. TEST SECTION BLL
    // =========================================================
    

    // =========================================================
    // 4. QUESTION BLL
    // =========================================================
    

    // =========================================================
    // 5. TEST ATTEMPT BLL
    // =========================================================
    

    // =========================================================
    // 6. SECTION ATTEMPT BLL
    // =========================================================
    

    // =========================================================
    // 7. ANSWER BLL
    // =========================================================
    

    // =========================================================
    // 8. COURSE BLL
    // =========================================================
    public class CourseBLL
    {
        private CourseDAL courseDAL = new CourseDAL();

        public DataTable GetAllPublishedCourses()
        {
            return courseDAL.GetAllPublishedCourses();
        }

        public DataTable GetCourseById(long courseId)
        {
            if (courseId <= 0)
                throw new Exception("Course ID không hợp lệ!");

            return courseDAL.GetCourseById(courseId);
        }

        public bool EnrollCourse(long userId, long courseId)
        {
            if (userId <= 0 || courseId <= 0)
                throw new Exception("User ID hoặc Course ID không hợp lệ!");

            return courseDAL.EnrollCourse(userId, courseId);
        }

        public DataTable GetEnrolledCoursesByUserId(long userId)
        {
            if (userId <= 0)
                throw new Exception("User ID không hợp lệ!");

            return courseDAL.GetEnrolledCoursesByUserId(userId);
        }
    }

    // =========================================================
    // 9. STATISTICS BLL
    // =========================================================
    public class StatisticsBLL
    {
        private StatisticsDAL statisticsDAL = new StatisticsDAL();

        public void UpdateUserStatistics(long userId)
        {
            if (userId <= 0)
                throw new Exception("User ID không hợp lệ!");

            statisticsDAL.UpdateUserStatistics(userId);
        }

        public DataTable GetUserStatistics(long userId)
        {
            if (userId <= 0)
                throw new Exception("User ID không hợp lệ!");

            return statisticsDAL.GetUserStatistics(userId);
        }

        public string GetUserProgressSummary(long userId)
        {
            DataTable stats = GetUserStatistics(userId);

            if (stats.Rows.Count == 0)
                return "Chưa có dữ liệu thống kê";

            DataRow row = stats.Rows[0];

            StringBuilder summary = new StringBuilder();
            summary.AppendLine($"📊 Tổng số bài thi: {row["TotalTests"]}");

            if (row["BestBand"] != DBNull.Value)
                summary.AppendLine($"⭐ Điểm cao nhất: {row["BestBand"]}");

            summary.AppendLine("\n📈 Điểm trung bình theo kỹ năng:");

            if (row["AverageListening"] != DBNull.Value)
                summary.AppendLine($"   🎧 Listening: {row["AverageListening"]}");

            if (row["AverageReading"] != DBNull.Value)
                summary.AppendLine($"   📖 Reading: {row["AverageReading"]}");

            if (row["AverageWriting"] != DBNull.Value)
                summary.AppendLine($"   ✍️ Writing: {row["AverageWriting"]}");

            if (row["AverageSpeaking"] != DBNull.Value)
                summary.AppendLine($"   🗣️ Speaking: {row["AverageSpeaking"]}");

            return summary.ToString();
        }
    }

    // =========================================================
    // 10. SESSION MANAGER (Quản lý phiên đăng nhập)
    // =========================================================
    

}
