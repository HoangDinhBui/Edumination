using IELTS.BLL;
using Newtonsoft.Json;
using System;

namespace IELTS.API
{
    /// <summary>
    /// API Controller cho Payment
    /// </summary>
    public class PaymentController
    {
        private readonly StripePaymentService paymentService;

        public PaymentController()
        {
            paymentService = new StripePaymentService();
        }

        /// <summary>
        /// Tạo Checkout Session
        /// POST /api/payment/create-session
        /// </summary>
        public string CreateSession(CreatePaymentSessionRequestDTO request)
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

                string sessionUrl = paymentService.CreateCheckoutSession(
                    request.CourseId,
                    request.CourseTitle,
                    request.PriceVND,
                    request.UserId,
                    request.UserEmail
                );

                return JsonConvert.SerializeObject(new
                {
                    success = true,
                    message = "Tạo session thành công",
                    sessionUrl = sessionUrl
                });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new
                {
                    success = false,
                    message = $"Lỗi server: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Verify Payment
        /// POST /api/payment/verify
        /// </summary>
        public string VerifyPayment(VerifyPaymentRequestDTO request)
        {
            try
            {
                if (request == null || string.IsNullOrEmpty(request.SessionId))
                {
                    return JsonConvert.SerializeObject(new
                    {
                        success = false,
                        message = "Session ID không hợp lệ!"
                    });
                }

                bool isPaid = paymentService.VerifyPaymentSession(request.SessionId);

                if (isPaid)
                {
                    // TODO: Cập nhật trạng thái đơn hàng trong database
                    // Ví dụ: EnrollCourse(userId, courseId)
                    
                    return JsonConvert.SerializeObject(new
                    {
                        success = true,
                        message = "Thanh toán thành công!"
                    });
                }
                else
                {
                    return JsonConvert.SerializeObject(new
                    {
                        success = false,
                        message = "Thanh toán chưa hoàn tất hoặc thất bại."
                    });
                }
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new
                {
                    success = false,
                    message = $"Lỗi server: {ex.Message}"
                });
            }
        }
    }

    public class CreatePaymentSessionRequestDTO
    {
        public long CourseId { get; set; }
        public string CourseTitle { get; set; }
        public int PriceVND { get; set; }
        public long UserId { get; set; }
        public string UserEmail { get; set; }
    }

    public class VerifyPaymentRequestDTO
    {
        public string SessionId { get; set; }
    }
}
