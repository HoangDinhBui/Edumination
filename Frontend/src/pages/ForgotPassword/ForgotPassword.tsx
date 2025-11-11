import React, { useState } from "react";
import { ChevronLeft } from "lucide-react";
import { useNavigate } from "react-router-dom";
import axios from "axios";

// === IMPORT ẢNH SLIDE ===
import slide1 from "../../assets/img/adv.jpg";
import slide2 from "../../assets/img/adv1.jpg";
import slide3 from "../../assets/img/adv2.jpg";

const ForgotPasswordPage: React.FC = () => {
  const [email, setEmail] = useState("");
  const [loading, setLoading] = useState(false);
  const [message, setMessage] = useState<string | null>(null);
  const [error, setError] = useState<string | null>(null);
  const [currentSlide, setCurrentSlide] = useState(0);
  const navigate = useNavigate();

  const slides = [slide1, slide2, slide3];

  React.useEffect(() => {
    const timer = setInterval(
      () => setCurrentSlide((prev) => (prev + 1) % slides.length),
      4000
    );
    return () => clearInterval(timer);
  }, [slides.length]);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);
    setMessage(null);

    if (!email.trim()) {
      setError("Vui lòng nhập địa chỉ email.");
      return;
    }

    try {
      setLoading(true);

      const apiUrl = "http://localhost:8081/api/v1/auth/password/forgot";
      await axios.post(apiUrl, { email });

      setMessage(
        "Nếu email tồn tại, hướng dẫn đặt lại mật khẩu đã được gửi."
      );
      setEmail("");

      setTimeout(() => {
        navigate("/enter-otp", { state: { email, fromForgot: true } });
      }, 3000);
    } catch (err: any) {
      console.error("Lỗi Forgot Password:", err);
      if (axios.isAxiosError(err) && err.response) {
        setError("Máy chủ gặp lỗi. Vui lòng thử lại sau.");
      } else {
        setError("Lỗi mạng. Vui lòng kiểm tra kết nối Internet.");
      }
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
        <div className="flex flex-col justify-center items-center w-full md:w-[45%] bg-white p-8 md:p-16">
          <div className="w-full max-w-md">
            <button
              onClick={() => navigate("/signin")}
              className="inline-flex items-center gap-1 text-sm text-[#294563] hover:text-[#23B0EB] mb-6"
            >
              <ChevronLeft className="h-4 w-4" />s
              Back to login
            </button>

            <h1
              className="mt-4 text-3xl md:text-4xl font-extrabold text-[#294563] text-center"
              style={{ fontFamily: "'Paytone One', sans-serif" }}
            >
              Forgot Password?
            </h1>
            <p className="text-center text-[#666666] mb-10 mt-2">
              Enter your registered email to reset your password
            </p>

            <form className="space-y-6" onSubmit={handleSubmit}>
              <div>
                <label
                  htmlFor="email"
                  className="block text-l text-[#294563] font-semibold mb-2"
                  style={{ fontFamily: "'Be Vietnam Pro', sans-serif" }}
                >
                  Email
                </label>
                <input
                  id="email"
                  type="email"
                  value={email}
                  onChange={(e) => setEmail(e.target.value)}
                  required
                  placeholder="Please enter your email"
                  className="mt-1 block w-full px-4 py-3 border border-slate-300 rounded-md shadow-sm focus:ring-sky-500 focus:border-sky-500"
                  style={{ fontFamily: "'Montserrat', sans-serif" }}
                />
              </div>

              {error && (
                <p className="text-sm text-red-500 text-center">{error}</p>
              )}
              {message && (
                <p className="text-sm text-green-600 text-center">{message}</p>
              )}

              <button
                type="submit"
                disabled={loading}
                className="w-full py-3 bg-[#749BC2] hover:bg-sky-700 text-white text-l font-semibold rounded-full"
                style={{ fontFamily: "'Be Vietnam Pro', sans-serif" }}
              >
                {loading ? "Đang gửi..." : "Send reset link"}
              </button>

              <p className="text-center text-[#666666] pt-2 text-sm">
                Remember your password?{" "}
                <a
                  href="/login"
                  className="font-semibold text-[#23B0EB] hover:underline"
                >
                  Login now!
                </a>
              </p>
            </form>
          </div>
        </div>

        {/* === SLIDESHOW === */}
        <div className="relative w-full md:w-[55%] h-full overflow-hidden rounded-[5px] m-[5px] hidden md:block">
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

          {/* Dots */}
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

export default ForgotPasswordPage;
