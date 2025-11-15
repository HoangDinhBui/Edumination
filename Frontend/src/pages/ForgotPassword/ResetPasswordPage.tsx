import React, { useState, useEffect } from "react";
import { Eye, EyeOff } from "lucide-react";
import { useNavigate } from "react-router-dom";
import axios from "axios";
import slide1 from "../../assets/img/adv.jpg";
import slide2 from "../../assets/img/adv1.jpg";
import slide3 from "../../assets/img/adv2.jpg";

const ResetPasswordPage: React.FC = () => {
  const [showPassword, setShowPassword] = useState(false);
  const [showConfirmPassword, setShowConfirmPassword] = useState(false);
  const [password, setPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [currentSlide, setCurrentSlide] = useState(0);
  const navigate = useNavigate();

  const slides = [slide1, slide2, slide3];

  useEffect(() => {
    const timer = setInterval(
      () => setCurrentSlide((prev) => (prev + 1) % slides.length),
      4000
    );
    return () => clearInterval(timer);
  }, [slides.length]);

  // Kiểm tra có OTP đã verify chưa
  useEffect(() => {
    const otp = localStorage.getItem("otp");
    if (!otp) {
      alert("Vui lòng xác thực OTP trước!");
      navigate("/enter-otp");
    }
  }, [navigate]);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);

    // Validate password match
    if (password !== confirmPassword) {
      setError("Mật khẩu xác nhận không khớp!");
      return;
    }

    // Validate password strength
    if (password.length < 8) {
      setError("Mật khẩu phải có ít nhất 8 ký tự!");
      return;
    }
    if (!/[A-Z]/.test(password)) {
      setError("Mật khẩu phải chứa ít nhất 1 chữ hoa (A-Z)!");
      return;
    }
    if (!/[a-z]/.test(password)) {
      setError("Mật khẩu phải chứa ít nhất 1 chữ thường (a-z)!");
      return;
    }
    if (!/[0-9]/.test(password)) {
      setError("Mật khẩu phải chứa ít nhất 1 chữ số (0-9)!");
      return;
    }

    // ✅ Lấy OTP đã được lưu (6 chữ số)
    const otp = localStorage.getItem("otp");

    if (!otp) {
      setError("Không tìm thấy mã OTP. Vui lòng quay lại trang Enter OTP.");
      setTimeout(() => navigate("/enter-otp"), 2000);
      return;
    }

    try {
      setLoading(true);
      const apiUrl = "http://localhost:8081/api/v1/auth/password/reset";

      // ✅ Payload theo format API của bạn: { token, newPassword }
      const payload = {
        token: otp, // token chính là OTP 6 số
        newPassword: password,
      };

      console.log("Reset Password Payload:", payload);

      const response = await axios.post(apiUrl, payload, {
        headers: { "Content-Type": "application/json" },
      });

      console.log("Reset Password Response:", response.data);

      if (response.data?.Success) {
        // ✅ Thành công - Clean up
        localStorage.removeItem("otp");
        sessionStorage.removeItem("resetEmail");

        alert("Mật khẩu đã được thay đổi thành công!");
        navigate("/signin");
      } else {
        setError(
          response.data?.Message ??
            response.data?.Error ??
            "Không thể đặt lại mật khẩu. Vui lòng thử lại."
        );
      }
    } catch (err: any) {
      console.error("Lỗi Reset Password:", err);
      if (axios.isAxiosError(err) && err.response) {
        console.error("Response data:", err.response.data);
        setError(
          err.response.data?.Message ||
            err.response.data?.Error ||
            "Không thể đặt lại mật khẩu. OTP có thể đã hết hạn."
        );
      } else {
        setError("Lỗi mạng. Vui lòng kiểm tra kết nối Internet.");
      }
    } finally {
      setLoading(false);
    }
  };

  return (
    <>
      <style>{`
        @import url('https://fonts.googleapis.com/css2?family=Paytone+One&family=Montserrat:wght@300;400;500;600;700&family=Be+Vietnam+Pro:wght@300;400;500;600;700&display=swap');
      `}</style>

      <div className="w-screen h-screen flex flex-col md:flex-row overflow-hidden">
        <div className="flex flex-col justify-center items-center w-full md:w-[45%] bg-white px-6 md:px-12 py-8">
          <div className="w-full max-w-sm">
            <h1
              className="text-3xl md:text-4xl font-extrabold text-[#294563] text-center"
              style={{ fontFamily: "'Paytone One', sans-serif" }}
            >
              Reset Password
            </h1>
            <p className="text-center text-[#666666] mb-8 mt-2">
              Enter your new password below
            </p>

            <form className="space-y-6" onSubmit={handleSubmit}>
              <div>
                <label
                  htmlFor="password"
                  className="block text-sm font-semibold text-[#294563] mb-2"
                  style={{ fontFamily: "'Be Vietnam Pro', sans-serif" }}
                >
                  New Password
                </label>
                <div className="relative">
                  <input
                    id="password"
                    type={showPassword ? "text" : "password"}
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                    placeholder="Enter new password"
                    required
                    disabled={loading}
                    className="w-full px-4 py-2.5 border border-[#D0D0D0] rounded-lg focus:outline-none focus:ring-2 focus:ring-[#749BC2] disabled:opacity-50"
                    style={{ fontFamily: "'Be Vietnam Pro', sans-serif" }}
                  />
                  <button
                    type="button"
                    onClick={() => setShowPassword(!showPassword)}
                    className="absolute right-3 top-1/2 -translate-y-1/2 text-[#666666]"
                  >
                    {showPassword ? <EyeOff size={20} /> : <Eye size={20} />}
                  </button>
                </div>
              </div>

              <div>
                <label
                  htmlFor="confirmPassword"
                  className="block text-sm font-semibold text-[#294563] mb-2"
                  style={{ fontFamily: "'Be Vietnam Pro', sans-serif" }}
                >
                  Confirm Password
                </label>
                <div className="relative">
                  <input
                    id="confirmPassword"
                    type={showConfirmPassword ? "text" : "password"}
                    value={confirmPassword}
                    onChange={(e) => setConfirmPassword(e.target.value)}
                    placeholder="Confirm password"
                    required
                    disabled={loading}
                    className="w-full px-4 py-2.5 border border-[#D0D0D0] rounded-lg focus:outline-none focus:ring-2 focus:ring-[#749BC2] disabled:opacity-50"
                    style={{ fontFamily: "'Be Vietnam Pro', sans-serif" }}
                  />
                  <button
                    type="button"
                    onClick={() => setShowConfirmPassword(!showConfirmPassword)}
                    className="absolute right-3 top-1/2 -translate-y-1/2 text-[#666666]"
                  >
                    {showConfirmPassword ? (
                      <EyeOff size={20} />
                    ) : (
                      <Eye size={20} />
                    )}
                  </button>
                </div>
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
                {loading ? "Đang xử lý..." : "Reset Password"}
              </button>

              <p className="text-center text-sm text-[#666666]">
                Quay lại{" "}
                <button
                  type="button"
                  onClick={() => navigate("/signin")}
                  className="text-[#749BC2] font-semibold hover:underline"
                >
                  Sign In
                </button>
              </p>
            </form>
          </div>
        </div>

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

export default ResetPasswordPage;
