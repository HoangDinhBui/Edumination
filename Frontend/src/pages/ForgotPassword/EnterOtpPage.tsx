import React, { useState, useEffect } from "react";
import { Mail, Clock, AlertTriangle, Send } from "lucide-react";
import { useNavigate } from "react-router-dom";
import axios from "axios";

import slide1 from "../../assets/img/adv.jpg";
import slide2 from "../../assets/img/adv1.jpg";
import slide3 from "../../assets/img/adv2.jpg";

const EnterOtpPage: React.FC = () => {
  const [otp, setOtp] = useState(["", "", "", "", "", ""]);
  const [currentSlide, setCurrentSlide] = useState(0);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const navigate = useNavigate();

  const slides = [slide1, slide2, slide3];

  useEffect(() => {
    const timer = setInterval(
      () => setCurrentSlide((prev) => (prev + 1) % slides.length),
      4000
    );
    return () => clearInterval(timer);
  }, [slides.length]);

  // === LOGIC OTP ===
  const handleOtpChange = (index: number, value: string) => {
    if (value.length > 1) return;
    const newOtp = [...otp];
    newOtp[index] = value;
    setOtp(newOtp);
    if (value && index < 5) {
      document.getElementById(`otp-input-${index + 1}`)?.focus();
    }
  };

  const handleKeyDown = (
    index: number,
    e: React.KeyboardEvent<HTMLInputElement>
  ) => {
    if (e.key === "Backspace" && !otp[index] && index > 0) {
      document.getElementById(`otp-input-${index - 1}`)?.focus();
    }
  };

  // === LƯU OTP VÀ CHUYỂN TRANG (KHÔNG GỌI API VERIFY) ===
  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);

    const code = otp.join("");
    if (code.length < 6) {
      setError("Vui lòng nhập đầy đủ 6 chữ số OTP!");
      return;
    }

    // Kiểm tra có email không
    const email = sessionStorage.getItem("resetEmail");
    if (!email) {
      setError(
        "Không tìm thấy email. Vui lòng quay lại trang Forgot Password."
      );
      setTimeout(() => navigate("/forgot-password"), 2000);
      return;
    }

    // ✅ Lưu OTP vào localStorage (API sẽ verify khi reset password)
    localStorage.setItem("otp", code);

    console.log("✅ OTP saved:", code);

    // Chuyển sang trang Reset Password
    navigate("/reset-password");
  };

  // === RESEND OTP ===
  const handleResendOtp = async () => {
    const email = sessionStorage.getItem("resetEmail");
    if (!email) {
      setError(
        "Không tìm thấy email. Vui lòng quay lại trang Forgot Password."
      );
      return;
    }

    try {
      setLoading(true);
      setError(null);

      const apiUrl = "http://localhost:8081/api/v1/auth/password/forgot";
      const response = await axios.post(apiUrl, { email });

      if (response.data.Success) {
        alert("Mã OTP mới đã được gửi đến email của bạn!");
        setOtp(["", "", "", "", "", ""]);
        document.getElementById("otp-input-0")?.focus();
      }
    } catch (err: any) {
      console.error("Lỗi Resend OTP:", err);
      setError("Không thể gửi lại OTP. Vui lòng thử lại sau.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <>
      {/* Google Fonts */}
      <style>{`
        @import url('https://fonts.googleapis.com/css2?family=Paytone+One&family=Montserrat:wght@300;400;500;600;700&family=Be+Vietnam+Pro:wght@300;400;500;600;700&display=swap');
      `}</style>

      <div className="w-screen h-screen flex flex-col md:flex-row overflow-hidden">
        {/* === FORM === */}
        <div className="flex flex-col justify-center items-center w-full md:w-[45%] bg-white px-6 md:px-12 py-8">
          <div className="w-full max-w-sm">
            <h1
              className="text-3xl md:text-4xl font-extrabold text-[#294563] text-center"
              style={{ fontFamily: "'Paytone One', sans-serif" }}
            >
              Enter OTP
            </h1>
            <p className="text-center text-[#666666] mb-8 mt-2">
              Please enter the 6-digit code sent to your email
            </p>

            <form className="space-y-6" onSubmit={handleSubmit}>
              {/* OTP INPUTS */}
              <div className="flex justify-between gap-2">
                {otp.map((digit, index) => (
                  <input
                    key={index}
                    id={`otp-input-${index}`}
                    type="text"
                    maxLength={1}
                    value={digit}
                    onChange={(e) => handleOtpChange(index, e.target.value)}
                    onKeyDown={(e) => handleKeyDown(index, e)}
                    disabled={loading}
                    className="w-10 h-12 text-xl text-center border border-slate-300 rounded-md shadow-sm focus:ring-sky-500 focus:border-sky-500 disabled:opacity-50"
                    style={{ fontFamily: "'Montserrat', sans-serif" }}
                  />
                ))}
              </div>

              {error && (
                <div className="p-3 bg-red-100 border border-red-400 text-red-700 rounded-lg text-sm">
                  {error}
                </div>
              )}

              <button
                type="submit"
                disabled={loading}
                className="w-full py-2.5 bg-[#749BC2] hover:bg-sky-700 text-white text-base font-semibold rounded-full disabled:opacity-50 disabled:cursor-not-allowed"
                style={{ fontFamily: "'Be Vietnam Pro', sans-serif" }}
              >
                {loading ? "Đang xác thực..." : "Verify OTP"}
              </button>

              {/* THÔNG TIN HƯỚNG DẪN */}
              <div className="space-y-3 text-[#294563] pt-3 text-sm">
                <div className="flex items-center gap-3">
                  <Mail className="w-5 h-5 text-[#294563]" />
                  <p>Check your email inbox for a 6-digit code</p>
                </div>
                <div className="flex items-center gap-3">
                  <Clock className="w-5 h-5 text-[#294563]" />
                  <p>This code expires in 15 minutes</p>
                </div>
                <div className="flex items-center gap-3">
                  <AlertTriangle className="w-5 h-5 text-[#294563]" />
                  <p>If not found, check your spam or junk folder</p>
                </div>
                <div className="flex items-center gap-3">
                  <Send className="w-5 h-5 text-[#294563]" />
                  <p>
                    Didn't receive the code?
                    <button
                      type="button"
                      onClick={handleResendOtp}
                      disabled={loading}
                      className="font-semibold text-[#23B0EB] hover:underline ml-1 disabled:opacity-50"
                    >
                      Resend now
                    </button>
                  </p>
                </div>
              </div>
            </form>
          </div>
        </div>

        {/* === SLIDESHOW === */}
        <div className="relative w-full md:w-[55%] h-full overflow-hidden hidden md:block">
          {slides.map((slide, index) => (
            <img
              key={index}
              src={slide}
              alt={`Slide ${index}`}
              className={`absolute inset-0 w-full h-full object-cover transition-opacity duration-1000 ${
                index === currentSlide ? "opacity-100" : "opacity-0"
              }`}
            />
          ))}
          <div className="absolute bottom-6 left-1/2 -translate-x-1/2 flex gap-2">
            {slides.map((_, index) => (
              <button
                key={index}
                onClick={() => setCurrentSlide(index)}
                className={`rounded-full transition-all duration-300 ${
                  index === currentSlide
                    ? "w-8 h-2 bg-white"
                    : "w-2 h-2 bg-white/40 hover:bg-white/60"
                }`}
              />
            ))}
          </div>
        </div>
      </div>
    </>
  );
};

export default EnterOtpPage;
