using BCrypt.Net;
using IELTS.DAL;
using IELTS.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IELTS.BLL
{
    // 1. USER BLL
    // =========================================================
    public class UserBLL
    {
        private UserDAL userDAL = new UserDAL();

        /// <summary>
        /// Hash password bằng BCrypt (an toàn hơn SHA256)
        /// </summary>
        private string HashPassword(string password)
        {
            // BCrypt tự động generate salt và hash
            // WorkFactor 12 = 2^12 iterations (an toàn và nhanh)
            return BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);
        }

        /// <summary>
        /// Verify password với BCrypt hash
        /// </summary>
        private bool VerifyPassword(string password, string hash)
        {
            try
            {
                return BCrypt.Net.BCrypt.Verify(password, hash);
            }
            catch
            {
                return false;
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

        /// <summary>
        /// Login với JWT Token - Trả về LoginResponseDTO
        /// Sử dụng BCrypt để verify password
        /// </summary>
        public LoginResponseDTO LoginWithToken(string email, string password)
        {
            try
            {
                // Validation
                if (string.IsNullOrWhiteSpace(email))
                    return new LoginResponseDTO(false, "Email không được để trống!");

                if (string.IsNullOrWhiteSpace(password))
                    return new LoginResponseDTO(false, "Mật khẩu không được để trống!");

                if (!email.Contains("@"))
                    return new LoginResponseDTO(false, "Email không hợp lệ!");

                // Lấy user từ database (bao gồm PasswordHash)
                DataTable result = userDAL.GetUserByEmail(email);

                if (result.Rows.Count == 0)
                    return new LoginResponseDTO(false, "Email hoặc mật khẩu không đúng!");

                // Lấy thông tin user
                DataRow row = result.Rows[0];
                string storedHash = row["PasswordHash"].ToString();

                // Verify password với BCrypt
                if (!VerifyPassword(password, storedHash))
                    return new LoginResponseDTO(false, "Email hoặc mật khẩu không đúng!");

                // Tạo UserDTO từ DataTable
                var user = new UserDTO
                {
                    Id = Convert.ToInt64(row["Id"]),
                    Email = row["Email"].ToString(),
                    FullName = row["FullName"].ToString(),
                    Role = row["Role"].ToString(),
                    IsActive = Convert.ToBoolean(row["IsActive"])
                };
				// Chèn vào ngay sau dòng: FullName = row["FullName"].ToString()
				Console.WriteLine($"[DEBUG BLL] Tên lấy từ DB: '{row["FullName"]}'");
				if (string.IsNullOrEmpty(user.FullName))
				{
					MessageBox.Show("Cảnh báo: Tên trong BLL đang bị rỗng!");
				}
				// Tạo JWT token
				string token = JwtHelper.GenerateToken(user);

                // Trả về response thành công
                return new LoginResponseDTO(
                    success: true,
                    message: $"Chào mừng {user.FullName}!",
                    token: token,
                    user: user
                );
            }
            catch (Exception ex)
            {
                return new LoginResponseDTO(false, $"Lỗi đăng nhập: {ex.Message}");
            }
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

        public List<UserDTO> GetStudentsByCourse(long courseId, string keyword = "")
        {
            if (courseId <= 0)
                throw new Exception("Course ID không hợp lệ!");

            return userDAL.GetStudentsByCourse(courseId, keyword);
        }

        /// <summary>
        /// Gửi OTP qua email để reset password
        /// </summary>
        public ForgotPasswordResponseDTO SendForgotPasswordOtp(string email)
        {
            try
            {
                Console.WriteLine($"\n{'='*60}");
                Console.WriteLine($" [FORGOT PASSWORD] Bắt đầu xử lý");
                Console.WriteLine($" Email nhận được: '{email}'");
                Console.WriteLine($"   Length: {email?.Length ?? 0} ký tự");
                
                // Validate email
                if (string.IsNullOrWhiteSpace(email))
                {
                    Console.WriteLine($" Email rỗng!");
                    return new ForgotPasswordResponseDTO(false, "Email không được để trống!");
                }

                if (!email.Contains("@"))
                {
                    Console.WriteLine($" Email không hợp lệ (thiếu @)!");
                    return new ForgotPasswordResponseDTO(false, "Email không hợp lệ!");
                }

                // Kiểm tra email có tồn tại không
                Console.WriteLine($" Đang tìm kiếm email trong database...");
                DataTable result = userDAL.GetUserByEmail(email);
                
                Console.WriteLine($" Kết quả query: {result.Rows.Count} row(s)");
                
                if (result.Rows.Count == 0)
                {
                    Console.WriteLine($" Email không tồn tại trong hệ thống!");
                    Console.WriteLine($"{'='*60}\n");
                    return new ForgotPasswordResponseDTO(false, "Email không tồn tại trong hệ thống!");
                }

                // Lấy thông tin user
                string fullName = result.Rows[0]["FullName"].ToString();


                // Tạo mã OTP
                string otpCode = OtpManager.GenerateOtp();

                // Lưu OTP vào memory
                OtpManager.SaveOtp(email, otpCode);

                // Gửi email qua SMTP
                EmailService emailService = new EmailService();
                Console.WriteLine($"\nĐang gửi OTP qua email đến: {email}");
                
                bool emailSent = emailService.SendOtpEmail(email, otpCode, fullName);

                if (emailSent)
                {
                    return new ForgotPasswordResponseDTO(
                        true,
                        "Mã OTP đã được gửi đến email của bạn. Vui lòng kiểm tra hộp thư!",
                        otpCode // Token để verify sau này
                    );
                }
                else
                {
                    // Nếu gửi email thất bại, vẫn trả về OTP để có thể test
                    Console.WriteLine($" Không gửi được email, nhưng OTP vẫn được tạo: {otpCode}");
                    return new ForgotPasswordResponseDTO(
                        false, 
                        "Không thể gửi email. Vui lòng kiểm tra cấu hình email hoặc xem Console để lấy OTP."
                    );
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Lỗi trong SendForgotPasswordOtp: {ex.Message}");
                return new ForgotPasswordResponseDTO(false, $"Lỗi: {ex.Message}");
            }
        }

        /// <summary>
        /// Reset password sau khi verify OTP
        /// </summary>
        public ForgotPasswordResponseDTO ResetPassword(string email, string otpCode, string newPassword, string confirmPassword)
        {
            try
            {
                // Validate input
                if (string.IsNullOrWhiteSpace(email))
                    return new ForgotPasswordResponseDTO(false, "Email không được để trống!");

                if (string.IsNullOrWhiteSpace(otpCode))
                    return new ForgotPasswordResponseDTO(false, "Mã OTP không được để trống!");

                if (string.IsNullOrWhiteSpace(newPassword))
                    return new ForgotPasswordResponseDTO(false, "Mật khẩu mới không được để trống!");

                if (newPassword.Length < 6)
                    return new ForgotPasswordResponseDTO(false, "Mật khẩu phải có ít nhất 6 ký tự!");

                if (newPassword != confirmPassword)
                    return new ForgotPasswordResponseDTO(false, "Mật khẩu xác nhận không khớp!");

                // Verify OTP
                bool otpValid = OtpManager.VerifyOtp(email, otpCode);
                if (!otpValid)
                    return new ForgotPasswordResponseDTO(false, "Mã OTP không hợp lệ hoặc đã hết hạn!");

                // Hash password mới
                string newPasswordHash = HashPassword(newPassword);

                // Cập nhật password trong database
                bool updated = userDAL.UpdatePassword(email, newPasswordHash);

                if (updated)
                {
                    return new ForgotPasswordResponseDTO(true, "Đặt lại mật khẩu thành công! Vui lòng đăng nhập lại.");
                }
                else
                {
                    return new ForgotPasswordResponseDTO(false, "Không thể cập nhật mật khẩu. Vui lòng thử lại!");
                }
            }
            catch (Exception ex)
            {
                return new ForgotPasswordResponseDTO(false, $"Lỗi: {ex.Message}");
            }
        }

        // --- PHẦN BỔ SUNG CHO ADMIN (Thêm vào class UserBLL) ---

        // 1. Lấy danh sách UserDTO cho GridView
        public List<UserDTO> GetAll(string keyword = "")
        {
            return userDAL.GetListUsersDTO(keyword);
        }

        // 2. Admin thêm user mới
        public string AddUser(UserDTO user, string rawPassword)
        {
            // Validate cơ bản
            if (string.IsNullOrWhiteSpace(user.Email) || string.IsNullOrWhiteSpace(user.FullName))
                return "Vui lòng nhập Email và Họ tên.";

            // Kiểm tra email trùng (Optional: Bạn có thể thêm hàm CheckEmailExist trong DAL nếu cần)
            var existingUser = userDAL.GetUserByEmail(user.Email);
            if (existingUser != null && existingUser.Rows.Count > 0)
                return "Email này đã tồn tại trong hệ thống!";

            // Hash mật khẩu
            user.PasswordHash = HashPassword(rawPassword);

            // Gọi DAL
            if (userDAL.Admin_AddUser(user))
                return ""; // Rỗng = Thành công

            return "Lỗi thêm dữ liệu vào SQL.";
        }

        // 3. Admin cập nhật user
        public string UpdateUser(UserDTO user, string newPassword)
        {
            bool isChangePass = !string.IsNullOrEmpty(newPassword);

            if (isChangePass)
            {
                user.PasswordHash = HashPassword(newPassword);
            }

            if (userDAL.Admin_UpdateUser(user, isChangePass))
                return ""; // Thành công

            return "Lỗi cập nhật dữ liệu!";
        }

        // 4. Admin xóa user
        public bool DeleteUser(long id)
        {
            return userDAL.DeleteUser(id);
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

        private readonly CourseDAL _dal = new CourseDAL();

        public List<CourseDTO> GetAll(string keyword)
        {
            return courseDAL.GetListCourses(keyword);
        }

        public string AddCourse(CourseDTO course)
        {
            if (string.IsNullOrWhiteSpace(course.Title)) return "Tiêu đề khóa học không được để trống!";
            if (course.PriceVND < 0) return "Giá tiền không hợp lệ!";

            // Mặc định người tạo là 1 (Admin) nếu chưa có session
            if (course.CreatedBy == 0) course.CreatedBy = 1;

            if (courseDAL.AddCourse(course))
                return ""; // Thành công
            return "Lỗi thêm khóa học!";
        }

        public string UpdateCourse(CourseDTO course)
        {
            if (string.IsNullOrWhiteSpace(course.Title)) return "Tiêu đề khóa học không được để trống!";
            if (course.PriceVND < 0) return "Giá tiền không hợp lệ!";

            if (courseDAL.UpdateCourse(course))
                return ""; // Thành công
            return "Lỗi cập nhật khóa học!";
        }

        public bool DeleteCourse(long id)
        {
            return courseDAL.DeleteCourse(id);
        }

        public DataTable GetStudentsForViewer(long courseId)
        {
            return courseDAL.GetEnrolledUsersByCourse(courseId);
        }

        public DataTable GetStudentCourseDetail(long courseId, long studentId)
        {
            return courseDAL.GetCourseStudent(courseId, studentId);
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
