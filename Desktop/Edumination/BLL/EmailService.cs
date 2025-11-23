using System;
using System.Net;
using System.Net.Mail;
using System.Collections.Generic;

namespace IELTS.BLL
{
    /// <summary>
    /// Service gửi email thật qua SMTP (Gmail)
    /// Cần cấu hình App Password trong Gmail để sử dụng
    /// </summary>
    public class EmailService
    {
        // Cấu hình SMTP Gmail
        private const string SMTP_HOST = "smtp.gmail.com";
        private const int SMTP_PORT = 587;
        
        // TODO: Thay đổi thông tin email của bạn ở đây
        private const string SENDER_EMAIL = "eduminationielts@gmail.com";
        private const string SENDER_PASSWORD = "psdsvbubflzlvfra"; // App Password, không phải mật khẩu Gmail
        private const string SENDER_NAME = "Edumination IELTS";

        /// <summary>
        /// Gửi email OTP
        /// </summary>
        public bool SendOtpEmail(string toEmail, string otpCode, string userName = "")
        {
            try
            {
                // Tạo nội dung email
                string subject = "Mã OTP đặt lại mật khẩu - Edumination";
                string body = GetOtpEmailTemplate(otpCode, userName);

                return SendEmail(toEmail, subject, body);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi gửi OTP email: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Gửi email chung
        /// </summary>
        public bool SendEmail(string toEmail, string subject, string body, bool isHtml = true)
        {
            try
            {
                // Tạo MailMessage
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(SENDER_EMAIL, SENDER_NAME);
                mail.To.Add(toEmail);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = isHtml;
                mail.Priority = MailPriority.High;

                // Cấu hình SMTP Client
                SmtpClient smtp = new SmtpClient(SMTP_HOST, SMTP_PORT);
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(SENDER_EMAIL, SENDER_PASSWORD);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Timeout = 20000; // 20 seconds

                // Gửi email
                smtp.Send(mail);

                Console.WriteLine($"Email đã gửi thành công đến: {toEmail}");
                return true;
            }
            catch (SmtpException smtpEx)
            {
                Console.WriteLine($"SMTP Error: {smtpEx.Message}");
                Console.WriteLine($"Status Code: {smtpEx.StatusCode}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Template HTML cho email OTP
        /// </summary>
        private string GetOtpEmailTemplate(string otpCode, string userName)
        {
            string greeting = string.IsNullOrEmpty(userName) ? "Xin chào" : $"Xin chào {userName}";

            return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <style>
        body {{
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            background-color: #f4f4f4;
            margin: 0;
            padding: 20px;
        }}
        .container {{
            max-width: 600px;
            margin: 0 auto;
            background-color: white;
            border-radius: 10px;
            overflow: hidden;
            box-shadow: 0 2px 10px rgba(0,0,0,0.1);
        }}
        .header {{
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            color: white;
            padding: 30px;
            text-align: center;
        }}
        .header h1 {{
            margin: 0;
            font-size: 28px;
        }}
        .content {{
            padding: 40px 30px;
        }}
        .otp-box {{
            background-color: #f8f9fa;
            border: 2px dashed #667eea;
            border-radius: 8px;
            padding: 20px;
            text-align: center;
            margin: 30px 0;
        }}
        .otp-code {{
            font-size: 36px;
            font-weight: bold;
            color: #667eea;
            letter-spacing: 8px;
            margin: 10px 0;
        }}
        .warning {{
            background-color: #fff3cd;
            border-left: 4px solid #ffc107;
            padding: 15px;
            margin: 20px 0;
            border-radius: 4px;
        }}
        .footer {{
            background-color: #f8f9fa;
            padding: 20px;
            text-align: center;
            font-size: 12px;
            color: #6c757d;
        }}
        .btn {{
            display: inline-block;
            padding: 12px 30px;
            background-color: #667eea;
            color: white;
            text-decoration: none;
            border-radius: 5px;
            margin: 20px 0;
        }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>Đặt lại mật khẩu</h1>
            <p>Edumination IELTS Learning Platform</p>
        </div>
        
        <div class='content'>
            <p style='font-size: 16px;'>{greeting},</p>
            
            <p>Chúng tôi đã nhận được yêu cầu đặt lại mật khẩu cho tài khoản của bạn. Vui lòng sử dụng mã OTP bên dưới để tiếp tục:</p>
            
            <div class='otp-box'>
                <p style='margin: 0; color: #6c757d; font-size: 14px;'>Mã OTP của bạn là:</p>
                <div class='otp-code'>{otpCode}</div>
                <p style='margin: 0; color: #6c757d; font-size: 12px;'>Mã này có hiệu lực trong 5 phút</p>
            </div>
            
            <div class='warning'>
                <strong>Lưu ý bảo mật:</strong>
                <ul style='margin: 10px 0; padding-left: 20px;'>
                    <li>Không chia sẻ mã OTP này với bất kỳ ai</li>
                    <li>Edumination sẽ không bao giờ yêu cầu mã OTP qua điện thoại</li>
                    <li>Nếu bạn không yêu cầu đặt lại mật khẩu, vui lòng bỏ qua email này</li>
                </ul>
            </div>
            
            <p style='color: #6c757d; font-size: 14px; margin-top: 30px;'>
                Nếu bạn gặp vấn đề, vui lòng liên hệ với chúng tôi qua email hỗ trợ.
            </p>
        </div>
        
        <div class='footer'>
            <p>© 2024 Edumination IELTS. All rights reserved.</p>
            <p>Email này được gửi tự động, vui lòng không trả lời.</p>
        </div>
    </div>
</body>
</html>";
        }

        /// <summary>
        /// Kiểm tra cấu hình email có hợp lệ không
        /// </summary>
        public bool IsConfigured()
        {
            // Check xem có phải placeholder mặc định không
            return !SENDER_EMAIL.Contains("eduminationielts@gmail.com") && 
                   !SENDER_PASSWORD.Contains("psdsvbubflzlvfra") &&
                   !SENDER_PASSWORD.Contains("edumination123!") &&
                   SENDER_PASSWORD.Length >= 16; // App Password phải có ít nhất 16 ký tự
        }

        /// <summary>
        /// Hướng dẫn cấu hình Gmail App Password
        /// </summary>
        public static string GetConfigurationGuide()
        {
            return @"
HƯỚNG DẪN CÁU HÌNH GMAIL APP PASSWORD:

1. Đăng nhập vào Gmail của bạn
2. Vào Google Account Settings (https://myaccount.google.com/)
3. Chọn 'Security' → 'How you sign in to Google'
4. Bật '2-Step Verification' (nếu chưa bật)
5. Quay lại 'Security' → Chọn 'App passwords'
6. Chọn app: 'Mail', device: 'Windows Computer'
7. Click 'Generate' → Copy mã 16 ký tự
8. Dán mã này vào SENDER_PASSWORD trong EmailService.cs

LƯU Ý:
- App Password khác với mật khẩu Gmail thường
- Mỗi App Password chỉ hiển thị 1 lần, hãy lưu lại
- Không chia sẻ App Password với ai
";
        }
    }

    /// <summary>
    /// Quản lý OTP codes (lưu tạm trong memory)
    /// Trong production nên lưu vào database hoặc Redis
    /// </summary>
    public class OtpManager
    {
        private static Dictionary<string, OtpData> otpStorage = new Dictionary<string, OtpData>();

        public class OtpData
        {
            public string Code { get; set; }
            public DateTime ExpiryTime { get; set; }
            public string Email { get; set; }
        }

        /// <summary>
        /// Tạo mã OTP 6 số
        /// </summary>
        public static string GenerateOtp()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString();
        }

        /// <summary>
        /// Lưu OTP với thời gian hết hạn 5 phút
        /// </summary>
        public static void SaveOtp(string email, string otpCode)
        {
            var otpData = new OtpData
            {
                Code = otpCode,
                Email = email.ToLower(),
                ExpiryTime = DateTime.Now.AddMinutes(5)
            };

            // Xóa OTP cũ nếu có
            if (otpStorage.ContainsKey(email.ToLower()))
            {
                otpStorage.Remove(email.ToLower());
            }

            otpStorage.Add(email.ToLower(), otpData);
            
            Console.WriteLine($"OTP saved for {email}: {otpCode} (expires at {otpData.ExpiryTime:HH:mm:ss})");
        }

        /// <summary>
        /// Verify OTP
        /// </summary>
        public static bool VerifyOtp(string email, string otpCode)
        {
            string emailKey = email.ToLower();

            if (!otpStorage.ContainsKey(emailKey))
            {
                Console.WriteLine($"No OTP found for {email}");
                return false;
            }

            var otpData = otpStorage[emailKey];

            // Kiểm tra hết hạn
            if (DateTime.Now > otpData.ExpiryTime)
            {
                otpStorage.Remove(emailKey);
                Console.WriteLine($"OTP expired for {email}");
                return false;
            }

            // Kiểm tra mã OTP
            if (otpData.Code != otpCode)
            {
                Console.WriteLine($"Invalid OTP for {email}. Expected: {otpData.Code}, Got: {otpCode}");
                return false;
            }

            // OTP hợp lệ - xóa sau khi verify thành công
            otpStorage.Remove(emailKey);
            Console.WriteLine($"OTP verified successfully for {email}");
            return true;
        }

        /// <summary>
        /// Xóa OTP
        /// </summary>
        public static void RemoveOtp(string email)
        {
            if (otpStorage.ContainsKey(email.ToLower()))
            {
                otpStorage.Remove(email.ToLower());
            }
        }

        /// <summary>
        /// Dọn dẹp OTP hết hạn (gọi định kỳ)
        /// </summary>
        public static void CleanupExpiredOtps()
        {
            var expiredKeys = new List<string>();

            foreach (var kvp in otpStorage)
            {
                if (DateTime.Now > kvp.Value.ExpiryTime)
                {
                    expiredKeys.Add(kvp.Key);
                }
            }

            foreach (var key in expiredKeys)
            {
                otpStorage.Remove(key);
            }

            if (expiredKeys.Count > 0)
            {
                Console.WriteLine($"Cleaned up {expiredKeys.Count} expired OTPs");
            }
        }
    }
}
