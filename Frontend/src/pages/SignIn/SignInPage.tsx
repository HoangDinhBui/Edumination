import React, { useState, useEffect } from "react";
import { ChevronLeft, Eye, EyeOff } from "lucide-react";
import { useNavigate } from "react-router-dom";
import axios from "axios";

// === IMPORT ẢNH SLIDE ===
import slide1 from "../../assets/img/adv.jpg";
import slide2 from "../../assets/img/adv1.jpg";
import slide3 from "../../assets/img/adv2.jpg";

// === SVG ICON GOOGLE ===
const GoogleLogoIcon: React.FC = () => (
  <svg
    xmlns="http://www.w3.org/2000/svg"
    viewBox="0 0 24 24"
    width="18"
    height="18"
  >
    <path
      d="M22.56 12.25c0-.78-.07-1.53-.2-2.25H12v4.26h5.92c-.26 1.37-1.04 2.53-2.21 3.31v2.77h3.57
      c2.08-1.92 3.28-4.74 3.28-8.09z"
      fill="#4285F4"
    />
    <path
      d="M12 23c2.97 0 5.46-.98 7.28-2.66l-3.57-2.77c-.98.66-2.23 1.06-3.71
      1.06-2.86 0-5.29-1.93-6.16-4.53H2.18v2.84C3.99
      20.53 7.7 23 12 23z"
      fill="#34A853"
    />
    <path
      d="M5.84 14.09c-.22-.66-.35-1.36-.35-2.09s.13-1.43.35-2.09V7.07H2.18C1.43
      8.55 1 10.22 1 12s.43 3.45 1.18 4.93l3.66-2.84z"
      fill="#FBBC05"
    />
    <path
      d="M12 5.38c1.62 0 3.06.56 4.21
      1.64l3.15-3.15C17.45 2.09 14.97 1 12
      1 7.7 1 3.99 3.47 2.18 7.07l3.66
      2.84c.87-2.6 3.3-4.53 6.16-4.53z"
      fill="#EA4335"
    />
  </svg>
);

export default function SignInPage() {
  const [showPassword, setShowPassword] = useState(false);
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const navigate = useNavigate();

  const slides = [slide1, slide2, slide3];
  const [currentSlide, setCurrentSlide] = useState(0);

  useEffect(() => {
    const timer = setInterval(
      () => setCurrentSlide((prev) => (prev + 1) % slides.length),
      4000
    );
    return () => clearInterval(timer);
  }, [slides.length]);

  // === API LOGIC TỪ CODE CŨ (GIỮ NGUYÊN 100%) ===
  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setError(null);

    const apiUrl = "http://localhost:8081/api/v1/auth/login";

    try {
      const response = await axios.post(apiUrl, {
        email: email,
        password: password,
      });

      setLoading(false);
      console.log("Response Data:", response.data);

      // === FIX 1: SỬA TÊN TOKEN (T VIẾT HOA) ===
      const token = response.data.Token;

      if (!token || typeof token !== "string") {
        console.error(
          "LỖI: KHÔNG TÌM THẤY 'Token' (T hoa) HỢP LỆ (string) TRONG response.data"
        );
        setError("Lỗi đăng nhập: Không nhận được token từ server.");
        return;
      }

      // === FIX 2: SỬA KEY LƯU TRỮ ===
      localStorage.setItem("Token", token);

      // Chuyển hướng đến trang ranking
      navigate("/ranking");
    } catch (err: any) {
      setLoading(false);

      if (err.response) {
        console.error("LỖI RESPONSE TỪ SERVER:", err.response);
        if (err.response.status === 401) {
          setError("Invalid email or password");
        } else {
          setError(`Lỗi máy chủ: ${err.response.status}`);
        }
      } else {
        console.error("LỖI MẠNG HOẶC CORS:", err.message);
        setError("Network Error. Check CORS or if server is running.");
      }
    }
  };

  return (
    <>
      {/* Import Google Fonts */}
      <style>{`
        @import url('https://fonts.googleapis.com/css2?family=Paytone+One&family=Montserrat:wght@300;400;500;600;700&family=Be+Vietnam+Pro:wght@300;400;500;600;700&display=swap');
      `}</style>

      <div className="w-screen h-screen flex flex-col md:flex-row overflow-hidden">
        {/* === FORM LOGIN === */}
        <div className="flex flex-col justify-center items-center w-full md:w-[45%] bg-white p-8 md:p-16">
          <div className="w-full max-w-md">
            <h1
              className="mt-6 text-3xl md:text-4xl font-extrabold text-[#294563] text-center"
              style={{ fontFamily: "'Paytone One', sans-serif" }}
            >
              Welcome back!
            </h1>
            <p className="text-center text-[#666666] mb-10 mt-2">
              Login to your account
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
                  required
                  placeholder="Please enter Username/Email"
                  className="mt-1 block w-full px-4 py-3 border border-slate-300 rounded-md shadow-sm focus:ring-sky-500 focus:border-sky-500"
                  style={{ fontFamily: "'Montserrat', sans-serif" }}
                  value={email}
                  onChange={(e) => setEmail(e.target.value)}
                />
              </div>

              <div>
                <label
                  htmlFor="password"
                  className="block text-l text-[#294563] font-semibold"
                  style={{ fontFamily: "'Be Vietnam Pro', sans-serif" }}
                >
                  Password
                </label>
                <div className="relative">
                  <input
                    id="password"
                    type={showPassword ? "text" : "password"}
                    required
                    placeholder="Please enter password"
                    className="mt-2 block w-full px-4 py-3 border border-slate-300 rounded-md shadow-sm focus:ring-sky-500 focus:border-sky-500"
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                  />
                  <button
                    type="button"
                    onClick={() => setShowPassword(!showPassword)}
                    className="absolute inset-y-0 right-1 pr-3 flex items-center text-slate-400 hover:text-slate-600 focus:outline-none bg-transparent border-none shadow-none"
                  >
                    {showPassword ? (
                      <EyeOff className="h-5 w-5" />
                    ) : (
                      <Eye className="h-5 w-5" />
                    )}
                  </button>
                </div>
              </div>

              <div className="text-right">
                <a
                  href="#"
                  className="text-sm font-medium text-[#23B0EB] hover:underline"
                >
                  Forgot password?
                </a>
              </div>

              {error && (
                <div className="text-sm text-red-600 text-center">{error}</div>
              )}

              <button
                type="submit"
                disabled={loading}
                className="w-full py-3 bg-[#749BC2] hover:bg-sky-700 text-white text-l font-semibold rounded-full"
                style={{ fontFamily: "'Be Vietnam Pro', sans-serif" }}
              >
                {loading ? "Signing in..." : "Sign in"}
              </button>

              <div className="relative my-6 flex items-center">
                <span className="w-full border-t border-slate-300"></span>
                <span className="px-2 text-slate-500 text-sm bg-white">or</span>
                <span className="w-full border-t border-slate-300"></span>
              </div>

              <button
                type="button"
                className="w-full py-3 flex justify-center items-center gap-3 bg-white border border-slate-300 rounded-full shadow-sm hover:bg-slate-50"
              >
                <GoogleLogoIcon />
                <span className="text-sm font-medium text-slate-700">
                  Login with Google
                </span>
              </button>

              <p className="mt-6 text-center text-sm text-[#666666]">
                Don't have an account?{" "}
                <a
                  href="#"
                  className="font-semibold text-[#749BC2] hover:underline"
                >
                  Register now!
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

          {/* === DOT INDICATORS === */}
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
}