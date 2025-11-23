using IELTS.BLL;
using IELTS.DTO;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Text;

namespace IELTS.API
{
    /// <summary>
    /// API Controller cho Authentication (Login, Register, Validate Token)
    /// </summary>
    public class AuthController
    {
        private readonly UserBLL userBLL;

        public AuthController()
        {
            userBLL = new UserBLL();
        }

        /// <summary>
        /// API Login - POST /api/auth/login
        /// Request Body: { "email": "user@example.com", "password": "123456" }
        /// Response: LoginResponseDTO với token
        /// </summary>
        public string Login(LoginRequestDTO request)
        {
            try
            {
                if (request == null)
                {
                    return JsonConvert.SerializeObject(new LoginResponseDTO(
                        false, 
                        "Request không hợp lệ!"
                    ));
                }

                // Gọi BLL để xử lý login
                var response = userBLL.LoginWithToken(request.Email, request.Password);

                // Trả về JSON response
                return JsonConvert.SerializeObject(response);
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new LoginResponseDTO(
                    false, 
                    $"Lỗi server: {ex.Message}"
                ));
            }
        }

        /// <summary>
        /// API Validate Token - POST /api/auth/validate
        /// Request Body: { "token": "jwt_token_here" }
        /// Response: { "valid": true/false, "user": UserDTO }
        /// </summary>
        public string ValidateToken(string token)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(token))
                {
                    return JsonConvert.SerializeObject(new
                    {
                        valid = false,
                        message = "Token không được để trống!"
                    });
                }

                // Validate token
                bool isValid = JwtHelper.IsTokenValid(token);

                if (!isValid)
                {
                    return JsonConvert.SerializeObject(new
                    {
                        valid = false,
                        message = "Token không hợp lệ hoặc đã hết hạn!"
                    });
                }

                // Lấy thông tin user từ token
                var user = JwtHelper.GetUserFromToken(token);

                return JsonConvert.SerializeObject(new
                {
                    valid = true,
                    message = "Token hợp lệ!",
                    user = user
                });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new
                {
                    valid = false,
                    message = $"Lỗi validate token: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// API Register - POST /api/auth/register
        /// Request Body: RegisterRequestDTO
        /// Response: { "success": true/false, "message": "..." }
        /// </summary>
        public string Register(RegisterRequestDTO request)
        {
            try
            {
                if (request == null)
                {
                    return JsonConvert.SerializeObject(new
                    {
                        success = false,
                        message = "Request không hợp lệ!"
                    });
                }

                // Gọi BLL để đăng ký
                bool result = userBLL.Register(
                    request.Email,
                    request.Password,
                    request.ConfirmPassword,
                    request.FullName
                );

                if (result)
                {
                    return JsonConvert.SerializeObject(new
                    {
                        success = true,
                        message = "Đăng ký thành công! Vui lòng đăng nhập."
                    });
                }
                else
                {
                    return JsonConvert.SerializeObject(new
                    {
                        success = false,
                        message = "Đăng ký thất bại!"
                    });
                }
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new
                {
                    success = false,
                    message = $"Lỗi: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// API Logout - POST /api/auth/logout
        /// Trong JWT, logout thường được xử lý ở client side bằng cách xóa token
        /// </summary>
        public string Logout()
        {
            return JsonConvert.SerializeObject(new
            {
                success = true,
                message = "Đăng xuất thành công! Vui lòng xóa token ở client."
            });
        }

        /// <summary>
        /// API Forgot Password - POST /api/auth/forgot-password
        /// Request Body: { "email": "user@example.com" }
        /// Response: { "success": true/false, "message": "...", "otpToken": "..." }
        /// </summary>
        public string ForgotPassword(ForgotPasswordRequestDTO request)
        {
            try
            {
                if (request == null || string.IsNullOrWhiteSpace(request.Email))
                {
                    return JsonConvert.SerializeObject(new ForgotPasswordResponseDTO(
                        false,
                        "Email không được để trống!"
                    ));
                }

                // Gọi BLL để gửi OTP
                var response = userBLL.SendForgotPasswordOtp(request.Email);

                return JsonConvert.SerializeObject(response);
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new ForgotPasswordResponseDTO(
                    false,
                    $"Lỗi server: {ex.Message}"
                ));
            }
        }

        /// <summary>
        /// API Reset Password - POST /api/auth/reset-password
        /// Request Body: { "email": "...", "otpCode": "...", "newPassword": "...", "confirmPassword": "..." }
        /// Response: { "success": true/false, "message": "..." }
        /// </summary>
        public string ResetPassword(ResetPasswordRequestDTO request)
        {
            try
            {
                if (request == null)
                {
                    return JsonConvert.SerializeObject(new ForgotPasswordResponseDTO(
                        false,
                        "Request không hợp lệ!"
                    ));
                }

                // Gọi BLL để reset password
                var response = userBLL.ResetPassword(
                    request.Email,
                    request.OtpCode,
                    request.NewPassword,
                    request.ConfirmPassword
                );

                return JsonConvert.SerializeObject(response);
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new ForgotPasswordResponseDTO(
                    false,
                    $"Lỗi server: {ex.Message}"
                ));
            }
        }
    }

    /// <summary>
    /// Simple HTTP Server để host API (cho demo)
    /// Trong production nên dùng ASP.NET Core Web API
    /// </summary>
    public class SimpleApiServer
    {
        private readonly HttpListener listener;
        private readonly AuthController authController;
        private readonly PaymentController paymentController;
        private bool isRunning;

        public SimpleApiServer(string prefix = "http://localhost:5000/")
        {
            listener = new HttpListener();
            listener.Prefixes.Add(prefix);
            authController = new AuthController();
            paymentController = new PaymentController();
            isRunning = false;
        }

        public void Start()
        {
            if (isRunning) return;

            listener.Start();
            isRunning = true;

            Console.WriteLine("API Server started at http://localhost:5000/");
            Console.WriteLine("Endpoints:");
            Console.WriteLine("  POST /api/auth/login");
            Console.WriteLine("  POST /api/auth/register");
            Console.WriteLine("  POST /api/auth/validate");
            Console.WriteLine("  POST /api/auth/logout");
            Console.WriteLine("  POST /api/auth/forgot-password");
            Console.WriteLine("  POST /api/auth/reset-password");
            Console.WriteLine("  POST /api/payment/create-session");
            Console.WriteLine("  POST /api/payment/verify");

            // Listen for requests
            while (isRunning)
            {
                try
                {
                    var context = listener.GetContext();
                    ProcessRequest(context);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        public void Stop()
        {
            isRunning = false;
            listener.Stop();
            listener.Close();
        }

        private void ProcessRequest(HttpListenerContext context)
        {
            var request = context.Request;
            var response = context.Response;

            // Enable CORS
            response.AddHeader("Access-Control-Allow-Origin", "*");
            response.AddHeader("Access-Control-Allow-Methods", "POST, GET, OPTIONS");
            response.AddHeader("Access-Control-Allow-Headers", "Content-Type");

            // Handle OPTIONS request (CORS preflight)
            if (request.HttpMethod == "OPTIONS")
            {
                response.StatusCode = 200;
                response.Close();
                return;
            }

            string responseString = "";

            try
            {
                // Read request body
                string requestBody = "";
                if (request.HasEntityBody)
                {
                    using (var reader = new System.IO.StreamReader(request.InputStream, request.ContentEncoding))
                    {
                        requestBody = reader.ReadToEnd();
                    }
                }

                // Route requests
                string path = request.Url.AbsolutePath.ToLower();
                string method = request.HttpMethod.ToUpper();
                Console.WriteLine($"[API] Received request: {method} {path}");

                if (path == "/api/auth/login" && method == "POST")
                {
                    var loginRequest = JsonConvert.DeserializeObject<LoginRequestDTO>(requestBody);
                    responseString = authController.Login(loginRequest);
                }
                else if (path == "/api/auth/register" && method == "POST")
                {
                    var registerRequest = JsonConvert.DeserializeObject<RegisterRequestDTO>(requestBody);
                    responseString = authController.Register(registerRequest);
                }
                else if (path == "/api/auth/validate" && method == "POST")
                {
                    var tokenRequest = JsonConvert.DeserializeObject<dynamic>(requestBody);
                    string token = tokenRequest?.token;
                    responseString = authController.ValidateToken(token);
                }
                else if (path == "/api/auth/logout" && method == "POST")
                {
                    responseString = authController.Logout();
                }
                else if (path == "/api/auth/forgot-password" && method == "POST")
                {
                    var forgotRequest = JsonConvert.DeserializeObject<ForgotPasswordRequestDTO>(requestBody);
                    responseString = authController.ForgotPassword(forgotRequest);
                }
                else if (path == "/api/auth/reset-password" && method == "POST")
                {
                    var resetRequest = JsonConvert.DeserializeObject<ResetPasswordRequestDTO>(requestBody);
                    responseString = authController.ResetPassword(resetRequest);
                }
                else if (path == "/api/payment/create-session" && method == "POST")
                {
                    var paymentRequest = JsonConvert.DeserializeObject<CreatePaymentSessionRequestDTO>(requestBody);
                    responseString = paymentController.CreateSession(paymentRequest);
                }
                else if (path == "/api/payment/verify" && method == "POST")
                {
                    var verifyRequest = JsonConvert.DeserializeObject<VerifyPaymentRequestDTO>(requestBody);
                    responseString = paymentController.VerifyPayment(verifyRequest);
                }
                // Xử lý callback từ Stripe (GET request)
                else if (path.StartsWith("/payment/success"))
                {
                    string sessionId = request.QueryString["session_id"];
                    if (!string.IsNullOrEmpty(sessionId))
                    {
                        // Tự động verify thanh toán
                        var verifyRequest = new VerifyPaymentRequestDTO { SessionId = sessionId };
                        paymentController.VerifyPayment(verifyRequest);
                    }

                    response.ContentType = "text/html";
                    responseString = "<html><body><h1 style='color:green'>Payment Successful!</h1><p>You can return to the application now.</p><script>setTimeout(function(){window.close();}, 3000);</script></body></html>";
                }
                else if (path.StartsWith("/payment/cancel"))
                {
                    response.ContentType = "text/html";
                    responseString = "<html><body><h1 style='color:red'>Payment Cancelled!</h1><p>You can try again later.</p><script>setTimeout(function(){window.close();}, 3000);</script></body></html>";
                }
                else
                {
                    response.StatusCode = 404;
                    responseString = JsonConvert.SerializeObject(new
                    {
                        success = false,
                        message = "Endpoint not found"
                    });
                }

                // Send response
                if (response.ContentType == null)
                {
                    response.ContentType = "application/json";
                }
                
                response.ContentEncoding = Encoding.UTF8;
                byte[] buffer = Encoding.UTF8.GetBytes(responseString);
                response.ContentLength64 = buffer.Length;
                response.OutputStream.Write(buffer, 0, buffer.Length);
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.ContentType = "application/json"; // Đảm bảo lỗi trả về JSON
                responseString = JsonConvert.SerializeObject(new
                {
                    success = false,
                    message = $"Server error: {ex.Message}"
                });

                byte[] buffer = Encoding.UTF8.GetBytes(responseString);
                response.OutputStream.Write(buffer, 0, buffer.Length);
            }
            finally
            {
                response.Close();
            }
        }
    }
}
