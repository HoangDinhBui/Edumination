using Stripe;
using Stripe.Checkout;
using System;
using System.Collections.Generic;

namespace IELTS.BLL
{
    /// <summary>
    /// Service xử lý thanh toán qua Stripe
    /// </summary>
    public class StripePaymentService
    {
        // TODO: Thay đổi Stripe API Key của bạn
        // Lấy từ: https://dashboard.stripe.com/apikeys
        // LƯU Ý: Không commit Secret Key lên GitHub! Hãy sử dụng Environment Variable hoặc User Secrets trong thực tế.
        private const string STRIPE_SECRET_KEY = "sk_test_your_secret_key_here"; // Test mode
        private const string STRIPE_PUBLISHABLE_KEY = "pk_test_your_publishable_key_here";
        
        // URL callback sau khi thanh toán
        private const string SUCCESS_URL = "http://localhost:5000/payment/success?session_id={CHECKOUT_SESSION_ID}";
        private const string CANCEL_URL = "http://localhost:5000/payment/cancel";

        public StripePaymentService()
        {
            // Load biến môi trường từ file .env
            string envPath = null;
            try 
            {
                // 1. Tìm file .env từ thư mục hiện tại lùi dần ra thư mục gốc
                // 1. Tìm file .env từ thư mục hiện tại lùi dần ra thư mục gốc ổ đĩa
                string currentDir = AppDomain.CurrentDomain.BaseDirectory;
                while (currentDir != null)
                {
                    string path = System.IO.Path.Combine(currentDir, ".env");
                    if (System.IO.File.Exists(path))
                    {
                        envPath = path;
                        break;
                    }
                    
                    var parent = System.IO.Directory.GetParent(currentDir);
                    if (parent == null) break; // Đã đến root ổ đĩa
                    currentDir = parent.FullName;
                }

                // Không còn sử dụng đường dẫn cứng fallback nữa
                // Code sẽ tự động tìm thấy nếu file .env nằm trong cấu trúc thư mục cha

                if (!string.IsNullOrEmpty(envPath))
                {
                    Console.WriteLine($"✅ [Stripe] Loaded .env from: {envPath}");
                    DotNetEnv.Env.Load(envPath);
                }
                else
                {
                    Console.WriteLine("⚠️ [Stripe] .env file not found!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ [Stripe] Error loading .env: {ex.Message}");
            }

            // Lấy Key từ biến môi trường
            string secretKey = Environment.GetEnvironmentVariable("STRIPE_SECRET_KEY");
            
            // Debug log (chỉ hiện 10 ký tự đầu để bảo mật)
            if (!string.IsNullOrEmpty(secretKey))
            {
                string maskedKey = secretKey.Length > 10 ? secretKey.Substring(0, 10) + "..." : "***";
                Console.WriteLine($"✅ [Stripe] Key loaded: {maskedKey}");
            }
            else
            {
                Console.WriteLine("❌ [Stripe] STRIPE_SECRET_KEY not found in environment variables!");
                // Fallback (sẽ gây lỗi nếu không sửa code, nhưng để báo hiệu rõ ràng)
                secretKey = "sk_test_your_secret_key_here"; 
            }

            StripeConfiguration.ApiKey = secretKey;
        }

        /// <summary>
        /// Tạo Checkout Session cho thanh toán khóa học
        /// </summary>
        public string CreateCheckoutSession(long courseId, string courseTitle, int priceVND, long userId, string userEmail)
        {
            try
            {
                Console.WriteLine($"\n💳 [STRIPE] Tạo Checkout Session");
                Console.WriteLine($"   Course: {courseTitle}");
                Console.WriteLine($"   Price: {priceVND:N0} VND");
                Console.WriteLine($"   User: {userEmail}");

                // Sử dụng trực tiếp VND (Stripe hỗ trợ VND)
                // VND là zero-decimal currency, nên 1 VND = 1 unit
                // Tuy nhiên, Stripe yêu cầu số tiền tối thiểu khoảng $0.50 USD (~12,000 VND)
                
                Console.WriteLine($"   Price: {priceVND:N0} VND");

                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes = new List<string>
                    {
                        "card", // Thẻ tín dụng/ghi nợ
                    },
                    LineItems = new List<SessionLineItemOptions>
                    {
                        new SessionLineItemOptions
                        {
                            PriceData = new SessionLineItemPriceDataOptions
                            {
                                Currency = "vnd", // Sử dụng VND
                                ProductData = new SessionLineItemPriceDataProductDataOptions
                                {
                                    Name = courseTitle,
                                    Description = $"IELTS Course - {courseTitle}",
                                    Images = new List<string>
                                    {
                                        "https://via.placeholder.com/300x200?text=IELTS+Course"
                                    }
                                },
                                UnitAmount = priceVND, // Giá trị VND trực tiếp
                            },
                            Quantity = 1,
                        },
                    },
                    Mode = "payment",
                    SuccessUrl = SUCCESS_URL,
                    CancelUrl = CANCEL_URL,
                    CustomerEmail = userEmail,
                    Metadata = new Dictionary<string, string>
                    {
                        { "course_id", courseId.ToString() },
                        { "user_id", userId.ToString() },
                        { "price_vnd", priceVND.ToString() }
                    }
                };

                var service = new SessionService();
                Session session = service.Create(options);

                Console.WriteLine($"✅ [STRIPE] Session created: {session.Id}");
                Console.WriteLine($"   Checkout URL: {session.Url}");

                return session.Url; // URL để redirect user đến trang thanh toán Stripe
            }
            catch (StripeException ex)
            {
                Console.WriteLine($"❌ [STRIPE] Error: {ex.Message}");
                Console.WriteLine($"   Code: {ex.StripeError?.Code}");
                throw new Exception($"Lỗi Stripe: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ [STRIPE] Unexpected error: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Verify payment session sau khi user quay lại từ Stripe
        /// </summary>
        public bool VerifyPaymentSession(string sessionId)
        {
            try
            {
                Console.WriteLine($"\n🔍 [STRIPE] Verifying session: {sessionId}");

                var service = new SessionService();
                Session session = service.Get(sessionId);

                Console.WriteLine($"   Status: {session.PaymentStatus}");
                Console.WriteLine($"   Amount: {session.AmountTotal:N0} VND");

                bool isPaid = session.PaymentStatus == "paid";

                if (isPaid)
                {
                    Console.WriteLine($"✅ [STRIPE] Payment verified successfully!");
                }
                else
                {
                    Console.WriteLine($"❌ [STRIPE] Payment not completed");
                }

                return isPaid;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ [STRIPE] Verification error: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Lấy thông tin session
        /// </summary>
        public Dictionary<string, string> GetSessionMetadata(string sessionId)
        {
            try
            {
                var service = new SessionService();
                Session session = service.Get(sessionId);

                return new Dictionary<string, string>
                {
                    { "course_id", session.Metadata["course_id"] },
                    { "user_id", session.Metadata["user_id"] },
                    { "price_vnd", session.Metadata["price_vnd"] },
                    { "payment_status", session.PaymentStatus },
                    { "amount_total", (session.AmountTotal ?? 0).ToString() }
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ [STRIPE] Error getting metadata: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Kiểm tra cấu hình Stripe có hợp lệ không
        /// </summary>
        public bool IsConfigured()
        {
            return !STRIPE_SECRET_KEY.Contains("your_secret_key") &&
                   !STRIPE_PUBLISHABLE_KEY.Contains("your_publishable_key");
        }

        /// <summary>
        /// Hướng dẫn cấu hình Stripe
        /// </summary>
        public static string GetConfigurationGuide()
        {
            return @"
📝 HƯỚNG DẪN CẤU HÌNH STRIPE:

1. Đăng ký tài khoản Stripe:
   - Truy cập: https://dashboard.stripe.com/register
   - Đăng ký tài khoản miễn phí

2. Lấy API Keys:
   - Đăng nhập vào Dashboard
   - Vào: Developers → API keys
   - Copy 2 keys:
     + Publishable key (pk_test_...)
     + Secret key (sk_test_...)

3. Cập nhật code:
   - Mở file: BLL/StripePaymentService.cs
   - Dòng 12-13, thay đổi:
     private const string STRIPE_SECRET_KEY = ""sk_test_..."";
     private const string STRIPE_PUBLISHABLE_KEY = ""pk_test_..."";

4. Test thanh toán:
   - Dùng thẻ test: 4242 4242 4242 4242
   - Expiry: Bất kỳ (tương lai)
   - CVC: Bất kỳ 3 số
   - ZIP: Bất kỳ

⚠️ LƯU Ý:
- Test mode: Không charge tiền thật
- Production mode: Cần verify business
- Không commit API keys lên Git
";
        }
    }
}
