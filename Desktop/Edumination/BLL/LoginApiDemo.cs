using IELTS.BLL;
using IELTS.DTO;
using System;

namespace IELTS.Examples
{
    /// <summary>
    /// Demo class để test API Login
    /// </summary>
    public class LoginApiDemo
    {
        /// <summary>
        /// Test Login API với tài khoản Student
        /// </summary>
        public static void TestStudentLogin()
        {
            Console.WriteLine("=== TEST STUDENT LOGIN ===");
            Console.WriteLine();

            UserBLL userBLL = new UserBLL();

            // Test với tài khoản student
            string email = "student@test.com";
            string password = "123456";

            Console.WriteLine($"Đang đăng nhập với:");
            Console.WriteLine($"  Email: {email}");
            Console.WriteLine($"  Password: {password}");
            Console.WriteLine();

            // Gọi API Login
            LoginResponseDTO response = userBLL.LoginWithToken(email, password);

            // Hiển thị kết quả
            if (response.Success)
            {
                Console.WriteLine("✅ ĐĂNG NHẬP THÀNH CÔNG!");
                Console.WriteLine();
                Console.WriteLine($"Message: {response.Message}");
                Console.WriteLine();
                Console.WriteLine("User Information:");
                Console.WriteLine($"  ID: {response.User.Id}");
                Console.WriteLine($"  Email: {response.User.Email}");
                Console.WriteLine($"  Full Name: {response.User.FullName}");
                Console.WriteLine($"  Role: {response.User.Role}");
                Console.WriteLine($"  Is Active: {response.User.IsActive}");
                Console.WriteLine();
                Console.WriteLine("JWT Token:");
                Console.WriteLine($"  {response.Token}");
                Console.WriteLine();
                Console.WriteLine($"  Token Length: {response.Token.Length} characters");
                Console.WriteLine($"  Token Preview: {response.Token.Substring(0, Math.Min(50, response.Token.Length))}...");
                Console.WriteLine();

                // Test validate token
                TestValidateToken(response.Token);
            }
            else
            {
                Console.WriteLine("❌ ĐĂNG NHẬP THẤT BẠI!");
                Console.WriteLine($"Message: {response.Message}");
            }

            Console.WriteLine();
            Console.WriteLine("=========================");
            Console.WriteLine();
        }

        /// <summary>
        /// Test Login API với tài khoản Teacher
        /// </summary>
        public static void TestTeacherLogin()
        {
            Console.WriteLine("=== TEST TEACHER LOGIN ===");
            Console.WriteLine();

            UserBLL userBLL = new UserBLL();

            string email = "teacher@ielts.com";
            string password = "123456";

            Console.WriteLine($"Đang đăng nhập với:");
            Console.WriteLine($"  Email: {email}");
            Console.WriteLine($"  Password: {password}");
            Console.WriteLine();

            LoginResponseDTO response = userBLL.LoginWithToken(email, password);

            if (response.Success)
            {
                Console.WriteLine("✅ ĐĂNG NHẬP THÀNH CÔNG!");
                Console.WriteLine($"User: {response.User.FullName} ({response.User.Role})");
                Console.WriteLine($"Token: {response.Token.Substring(0, 30)}...");
            }
            else
            {
                Console.WriteLine("❌ ĐĂNG NHẬP THẤT BẠI!");
                Console.WriteLine($"Message: {response.Message}");
            }

            Console.WriteLine();
            Console.WriteLine("==========================");
            Console.WriteLine();
        }

        /// <summary>
        /// Test Login thất bại
        /// </summary>
        public static void TestFailedLogin()
        {
            Console.WriteLine("=== TEST FAILED LOGIN ===");
            Console.WriteLine();

            UserBLL userBLL = new UserBLL();

            // Test với password sai
            string email = "student@test.com";
            string password = "wrongpassword";

            Console.WriteLine($"Đang đăng nhập với:");
            Console.WriteLine($"  Email: {email}");
            Console.WriteLine($"  Password: {password} (SAI)");
            Console.WriteLine();

            LoginResponseDTO response = userBLL.LoginWithToken(email, password);

            if (response.Success)
            {
                Console.WriteLine("✅ ĐĂNG NHẬP THÀNH CÔNG! (Không nên xảy ra)");
            }
            else
            {
                Console.WriteLine("❌ ĐĂNG NHẬP THẤT BẠI! (Đúng như mong đợi)");
                Console.WriteLine($"Message: {response.Message}");
            }

            Console.WriteLine();
            Console.WriteLine("=========================");
            Console.WriteLine();
        }

        /// <summary>
        /// Test validate token
        /// </summary>
        public static void TestValidateToken(string token)
        {
            Console.WriteLine("--- Validate Token ---");
            Console.WriteLine();

            try
            {
                // Validate token
                bool isValid = JwtHelper.IsTokenValid(token);

                if (isValid)
                {
                    Console.WriteLine("✅ Token hợp lệ!");
                    Console.WriteLine();

                    // Lấy thông tin user từ token
                    UserDTO user = JwtHelper.GetUserFromToken(token);

                    Console.WriteLine("User từ token:");
                    Console.WriteLine($"  ID: {user.Id}");
                    Console.WriteLine($"  Email: {user.Email}");
                    Console.WriteLine($"  Full Name: {user.FullName}");
                    Console.WriteLine($"  Role: {user.Role}");
                    Console.WriteLine($"  Is Active: {user.IsActive}");
                }
                else
                {
                    Console.WriteLine("❌ Token không hợp lệ!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Lỗi validate token: {ex.Message}");
            }

            Console.WriteLine();
            Console.WriteLine("----------------------");
            Console.WriteLine();
        }

        /// <summary>
        /// Test SessionManager
        /// </summary>
        public static void TestSessionManager()
        {
            Console.WriteLine("=== TEST SESSION MANAGER ===");
            Console.WriteLine();

            UserBLL userBLL = new UserBLL();

            // Login
            LoginResponseDTO response = userBLL.LoginWithToken("student@test.com", "123456");

            if (response.Success)
            {
                // Lưu vào session
                SessionManager.SetToken(response.Token);
                
                // Giả lập lưu user info (trong thực tế sẽ dùng DataRow)
                SessionManager.CurrentUserId = response.User.Id;
                SessionManager.CurrentUserEmail = response.User.Email;
                SessionManager.CurrentUserName = response.User.FullName;
                SessionManager.CurrentUserRole = response.User.Role;

                Console.WriteLine("✅ Đã lưu vào SessionManager");
                Console.WriteLine();
                Console.WriteLine("Session Info:");
                Console.WriteLine($"  User ID: {SessionManager.CurrentUserId}");
                Console.WriteLine($"  Email: {SessionManager.CurrentUserEmail}");
                Console.WriteLine($"  Name: {SessionManager.CurrentUserName}");
                Console.WriteLine($"  Role: {SessionManager.CurrentUserRole}");
                Console.WriteLine($"  Token: {SessionManager.GetToken().Substring(0, 30)}...");
                Console.WriteLine();
                Console.WriteLine($"  Is Logged In: {SessionManager.IsLoggedIn}");
                Console.WriteLine($"  Is Student: {SessionManager.IsStudent}");
                Console.WriteLine($"  Is Teacher: {SessionManager.IsTeacher}");
                Console.WriteLine($"  Is Admin: {SessionManager.IsAdmin}");
                Console.WriteLine();

                // Test logout
                Console.WriteLine("--- Test Logout ---");
                SessionManager.Logout();
                Console.WriteLine($"  Is Logged In: {SessionManager.IsLoggedIn}");
                Console.WriteLine($"  Token: {SessionManager.GetToken() ?? "null"}");
            }

            Console.WriteLine();
            Console.WriteLine("============================");
            Console.WriteLine();
        }

        /// <summary>
        /// Chạy tất cả tests
        /// </summary>
        public static void RunAllTests()
        {
            Console.WriteLine("╔════════════════════════════════════════╗");
            Console.WriteLine("║   API LOGIN DEMO - EDUMINATION IELTS   ║");
            Console.WriteLine("╚════════════════════════════════════════╝");
            Console.WriteLine();

            try
            {
                // Test 1: Student Login
                TestStudentLogin();

                System.Threading.Thread.Sleep(1000);

                // Test 2: Teacher Login
                TestTeacherLogin();

                System.Threading.Thread.Sleep(1000);

                // Test 3: Failed Login
                TestFailedLogin();

                System.Threading.Thread.Sleep(1000);

                // Test 4: Session Manager
                TestSessionManager();

                Console.WriteLine("╔════════════════════════════════════════╗");
                Console.WriteLine("║         TẤT CẢ TESTS HOÀN THÀNH        ║");
                Console.WriteLine("╚════════════════════════════════════════╝");
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine("╔════════════════════════════════════════╗");
                Console.WriteLine("║              LỖI XẢY RA                ║");
                Console.WriteLine("╚════════════════════════════════════════╝");
                Console.WriteLine();
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
            }
        }
    }
}
