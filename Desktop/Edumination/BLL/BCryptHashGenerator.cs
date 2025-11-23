using System;
using BCrypt.Net;

namespace IELTS.Tools
{
    /// <summary>
    /// Tool để generate BCrypt hash cho passwords
    /// Sử dụng để tạo hash cho passwords mới hoặc update database
    /// </summary>
    public class BCryptHashGenerator
    {
        /// <summary>
        /// Generate BCrypt hash cho password
        /// </summary>
        public static string GenerateHash(string password, int workFactor = 12)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, workFactor);
        }

        /// <summary>
        /// Verify password với hash
        /// </summary>
        public static bool VerifyHash(string password, string hash)
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

        /// <summary>
        /// Demo: Generate hash cho các passwords thông dụng
        /// </summary>
        public static void GenerateCommonPasswords()
        {
            Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║         BCRYPT PASSWORD HASH GENERATOR                     ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════╝");
            Console.WriteLine();

            string[] commonPasswords = { "123456", "password", "admin123", "student123", "teacher123" };

            foreach (string password in commonPasswords)
            {
                string hash = GenerateHash(password);
                Console.WriteLine($"Password: {password}");
                Console.WriteLine($"BCrypt Hash: {hash}");
                Console.WriteLine($"Hash Length: {hash.Length} characters");
                
                // Verify
                bool isValid = VerifyHash(password, hash);
                Console.WriteLine($"Verification: {(isValid ? "✅ VALID" : "❌ INVALID")}");
                Console.WriteLine();
            }

            Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                    COMPLETED                               ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════╝");
        }

        /// <summary>
        /// Interactive mode: Nhập password và generate hash
        /// </summary>
        public static void InteractiveMode()
        {
            Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║         BCRYPT HASH GENERATOR - INTERACTIVE MODE           ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════╝");
            Console.WriteLine();

            while (true)
            {
                Console.Write("Nhập password (hoặc 'exit' để thoát): ");
                string password = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(password) || password.ToLower() == "exit")
                {
                    Console.WriteLine("Đã thoát!");
                    break;
                }

                Console.Write("Work factor (6-31, mặc định 12): ");
                string workFactorInput = Console.ReadLine();
                int workFactor = 12;

                if (!string.IsNullOrWhiteSpace(workFactorInput))
                {
                    if (!int.TryParse(workFactorInput, out workFactor) || workFactor < 6 || workFactor > 31)
                    {
                        Console.WriteLine("Work factor không hợp lệ! Sử dụng mặc định: 12");
                        workFactor = 12;
                    }
                }

                Console.WriteLine();
                Console.WriteLine("Đang generate hash...");
                
                var startTime = DateTime.Now;
                string hash = GenerateHash(password, workFactor);
                var endTime = DateTime.Now;
                var duration = (endTime - startTime).TotalMilliseconds;

                Console.WriteLine();
                Console.WriteLine("═══════════════════════════════════════════════════════════");
                Console.WriteLine($"Password: {password}");
                Console.WriteLine($"Work Factor: {workFactor} (2^{workFactor} = {Math.Pow(2, workFactor):N0} iterations)");
                Console.WriteLine($"BCrypt Hash: {hash}");
                Console.WriteLine($"Hash Length: {hash.Length} characters");
                Console.WriteLine($"Generation Time: {duration:F2} ms");
                Console.WriteLine("═══════════════════════════════════════════════════════════");
                Console.WriteLine();

                // Verify
                Console.Write("Verify hash? (y/n): ");
                string verifyInput = Console.ReadLine();

                if (verifyInput?.ToLower() == "y")
                {
                    Console.Write("Nhập password để verify: ");
                    string verifyPassword = Console.ReadLine();

                    bool isValid = VerifyHash(verifyPassword, hash);
                    Console.WriteLine();
                    Console.WriteLine($"Verification Result: {(isValid ? "✅ VALID - Password khớp!" : "❌ INVALID - Password không khớp!")}");
                    Console.WriteLine();
                }

                Console.WriteLine();
            }
        }

        /// <summary>
        /// Generate SQL UPDATE statements cho database
        /// </summary>
        public static void GenerateSQLUpdates()
        {
            Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║         GENERATE SQL UPDATE STATEMENTS                     ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════╝");
            Console.WriteLine();

            // Định nghĩa users cần update
            var users = new[]
            {
                new { Email = "student@test.com", Password = "123456" },
                new { Email = "teacher@ielts.com", Password = "123456" },
                new { Email = "buidinhhoang1910@gmail.com", Password = "123456" }
            };

            Console.WriteLine("-- BCrypt Password Updates");
            Console.WriteLine("-- Generated at: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            Console.WriteLine();

            foreach (var user in users)
            {
                string hash = GenerateHash(user.Password);
                Console.WriteLine($"-- Email: {user.Email}, Password: {user.Password}");
                Console.WriteLine($"UPDATE Users SET PasswordHash = '{hash}' WHERE Email = '{user.Email}';");
                Console.WriteLine();
            }

            Console.WriteLine("-- Verify updates");
            Console.WriteLine("SELECT Email, LEFT(PasswordHash, 20) + '...' AS Hash_Preview, LEN(PasswordHash) AS Hash_Length FROM Users;");
            Console.WriteLine();
        }

        /// <summary>
        /// Main menu
        /// </summary>
        public static void ShowMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
                Console.WriteLine("║         BCRYPT PASSWORD HASH GENERATOR TOOL                ║");
                Console.WriteLine("╚════════════════════════════════════════════════════════════╝");
                Console.WriteLine();
                Console.WriteLine("1. Generate hash cho common passwords");
                Console.WriteLine("2. Interactive mode (nhập password tùy ý)");
                Console.WriteLine("3. Generate SQL UPDATE statements");
                Console.WriteLine("4. Exit");
                Console.WriteLine();
                Console.Write("Chọn option (1-4): ");

                string choice = Console.ReadLine();

                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        GenerateCommonPasswords();
                        break;
                    case "2":
                        InteractiveMode();
                        break;
                    case "3":
                        GenerateSQLUpdates();
                        break;
                    case "4":
                        Console.WriteLine("Goodbye!");
                        return;
                    default:
                        Console.WriteLine("Option không hợp lệ!");
                        break;
                }

                Console.WriteLine();
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }
    }
}
