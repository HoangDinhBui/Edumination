import React, { useState } from "react";
import { ChevronLeft, Mail, Clock, AlertTriangle, Send } from "lucide-react";

// 1. Đảm bảo đường dẫn này CHÍNH XÁC
// Dựa trên file HomePage, có vẻ bạn cần 2 dấu chấm lùi (../../)
import signInImage from "../../assets/img/Rectangle 123.png"; // <-- Đổi tên file nếu cần

/**
 * Component SVG cho logo Google.
 * Bạn không cần file ảnh riêng cho cái này, chỉ cần copy là đủ.
 */
const GoogleLogoIcon: React.FC = () => (
  <svg
    xmlns="http://www.w3.org/2000/svg"
    viewBox="0 0 24 24"
    width="18"
    height="18"
  >
    <path
      d="M22.56 12.25c0-.78-.07-1.53-.2-2.25H12v4.26h5.92c-.26 1.37-1.04 2.53-2.21 3.31v2.77h3.57c2.08-1.92 3.28-4.74 3.28-8.09z"
      fill="#4285F4"
    />
    <path
      d="M12 23c2.97 0 5.46-.98 7.28-2.66l-3.57-2.77c-.98.66-2.23 1.06-3.71 1.06-2.86 0-5.29-1.93-6.16-4.53H2.18v2.84C3.99 20.53 7.7 23 12 23z"
      fill="#34A853"
    />
    <path
      d="M5.84 14.09c-.22-.66-.35-1.36-.35-2.09s.13-1.43.35-2.09V7.07H2.18C1.43 8.55 1 10.22 1 12s.43 3.45 1.18 4.93l3.66-2.84z"
      fill="#FBBC05"
    />
    <path
      d="M12 5.38c1.62 0 3.06.56 4.21 1.64l3.15-3.15C17.45 2.09 14.97 1 12 1 7.7 1 3.99 3.47 2.18 7.07l3.66 2.84c.87-2.6 3.3-4.53 6.16-4.53z"
      fill="#EA4335"
    />
    <path d="M1 1h22v22H1z" fill="none" />
  </svg>
);

export default function ForgotPasswordPage() {
  // State để lưu trữ 6 chữ số của OTP
    const [otp, setOtp] = useState(['', '', '', '', '', '']);
  
    // Hàm xử lý việc nhập input cho từng ô
    // Thêm loại 'number' cho index và 'string' cho value
    const handleOtpChange = (index: number, value: string) => {
      // Chỉ cho phép nhập 1 chữ số
      if (value.length > 1) return;
  
      const newOtp = [...otp];
      newOtp[index] = value;
      setOtp(newOtp);
  
      // Tự động focus sang ô tiếp theo
      if (value && index < 5) {
        document.getElementById(`otp-input-${index + 1}`)?.focus();
      }
    };
  
    // Hàm xử lý khi nhấn phím lùi (Backspace)
    // Thêm loại 'number' cho index và 'React.KeyboardEvent<HTMLInputElement>' cho e
    const handleKeyDown = (index: number, e: React.KeyboardEvent<HTMLInputElement>) => {
      if (e.key === 'Backspace' && !otp[index] && index > 0) {
        // Nếu ô hiện tại rỗng, focus về ô trước đó
        document.getElementById(`otp-input-${index - 1}`)?.focus();
      }
    };

  return (
    // Container chính: Căn giữa, nền xám nhạt
    <div className="min-h-screen bg-slate-50 flex items-center justify-center p-4">
      {/* Thẻ (Card) đăng nhập: bo tròn, đổ bóng, chia 2 cột */}
      <div className="mx-auto w-full max-w-4xl bg-white shadow-2xl rounded-2xl overflow-hidden grid md:grid-cols-2">
        {/* === CỘT BÊN TRÁI (FORM) === */}
        <div className="p-8 md:p-12">
          {/* Link "Back to home" */}
          <a
            href="/" // Đổi thành "/" hoặc link trang chủ của bạn
            className="inline-flex items-center gap-1 text-sm text-slate-600 hover:text-slate-800"
          >
            <ChevronLeft className="h-4 w-4" />
            Back to home
          </a>

          <div className="text-center">
            {/* Tiêu đề */}
            <h1 className="mt-6 text-3xl font-bold text-slate-700">
                Enter OTP
            </h1>
            </div>

          {/* Thông báo hướng dẫn */}
          <div className="text-center">
            <p className="text-sm text-slate-500 pt-1">
                Check your email for a 6-digit code and enter it below to reset cho password
                email address.
              </p>
          </div>

          {/* Form */}
          <form className="space-y-6 pt-6">
              {/* === Ô INPUT OTP === */}
              <div className="flex justify-between gap-2">
                {/* Đã thêm loại 'number' cho index trong map */}
                {otp.map((digit, index: number) => (
                  <input
                    key={index}
                    id={`otp-input-${index}`}
                    type="text"
                    maxLength={1} 
                    value={digit}
                    // Sử dụng arrow function để truyền index và value
                    onChange={(e) => handleOtpChange(index, e.target.value)}
                    // Sử dụng arrow function để truyền index và event
                    onKeyDown={(e) => handleKeyDown(index, e)}
                    className="w-10 h-14 text-2xl text-center border border-slate-300 rounded-lg shadow-sm focus:outline-none focus:ring-sky-500 focus:border-sky-500"
                  />
                ))}
              </div>

              {/* Nút "Verify" */}
              <div className="pt-1">
                <button
                  type="submit"
                  className="w-full py-3 px-4 bg-slate-500 hover:bg-slate-600 text-white font-semibold rounded-lg shadow-md transition duration-150 ease-in-out"
                >
                  Verify
                </button>
              </div>

              {/* === CÁC THÔNG BÁO HƯỚNG DẪN === */}
              <div className="space-y-4 pt-4 text-slate-600 text-sm">
                <div className="flex items-center gap-3">
                  <Mail className="w-5 h-5 text-slate-500" />
                  <p>You should receive a 6-digit code via email</p>
                </div>
                
                <div className="flex items-center gap-3">
                  <Clock className="w-5 h-5 text-slate-500" />
                  <p>It expires in 15 minutes</p>
                </div>
                
                <div className="flex items-center gap-3">
                  <AlertTriangle className="w-5 h-5 text-slate-500" />
                  <p>
                    Can't find the code? Check spam or junk folder
                  </p>
                </div>
                
                <div className="flex items-center gap-3">
                  <Send className="w-5 h-5 text-slate-500" />
                  <p>
                    Still didn't receive the code? 
                    <a href="#" className="font-semibold text-sky-600 hover:text-sky-700 ml-1">
                       Resend it
                    </a>
                  </p>
                </div>
              </div>
            </form>
        </div>

        {/* === CỘT BÊN PHẢI (ẢNH) === */}
        <div className="hidden md:block">
          <img
            src={signInImage}
            alt="Students in a British Council classroom"
            className="w-full h-full object-cover"
          />
        </div>
      </div>
    </div>
  );
}