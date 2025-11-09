import React, { useState } from "react";

import { ChevronLeft, Eye, EyeOff } from "lucide-react";

import { useNavigate } from "react-router-dom";

import axios from "axios";

// 1. Đảm bảo đường dẫn này CHÍNH XÁC

import signInImage from "../../assets/img/Rectangle 123.png";

/**

 * Component SVG cho logo Google.

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

export default function SignInPage() {
  const [showPassword, setShowPassword] = useState(false);

  // === State cho form ===

  const [email, setEmail] = useState("");

  const [password, setPassword] = useState("");

  const [loading, setLoading] = useState(false);

  const [error, setError] = useState<string | null>(null);

  const navigate = useNavigate();

  // === Hàm xử lý submit form ===

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

      // Dựa trên log console của bạn: {Token: "ey...", ...}

      const token = response.data.Token;

      if (!token || typeof token !== "string") {
        console.error(
          "LỖI: KHÔNG TÌM THẤY 'Token' (T hoa) HỢP LỆ (string) TRONG response.data"
        );

        setError("Lỗi đăng nhập: Không nhận được token từ server.");

        return; // Dừng lại, không điều hướng
      }

      // === FIX 2: SỬA KEY LƯU TRỮ ===

      // Thống nhất dùng "Token" (T hoa) để ExamsLibrary có thể đọc được

      localStorage.setItem("Token", token);

      // 5. Chuyển hướng đến trang chính

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
    // Container chính

    <div className="min-h-screen bg-slate-50 flex items-center justify-center p-4">
      {/* Thẻ (Card) đăng nhập */}

      <div className="mx-auto w-full max-w-4xl bg-white shadow-2xl rounded-2xl overflow-hidden grid md:grid-cols-2">
        {/* === CỘT BÊN TRÁI (FORM) === */}

        <div className="p-8 md:p-12">
          S {/* Link "Back to home" */}
          <a
            href="/"
            className="inline-flex items-center gap-1 text-sm text-slate-600 hover:text-slate-800"
          >
            <ChevronLeft className="h-4 w-4" />
            Back to home
          </a>
          <div className="text-center">
            {/* Tiêu đề */}

            <h1 className="mt-6 text-3xl font-bold text-slate-700">
              Welcome back!
            </h1>

            <p className="mt-2 text-slate-600">Login to your account</p>
          </div>
          {/* Form: Thêm onSubmit */}
          <form className="mt-8 space-y-6" onSubmit={handleSubmit}>
            {/* Trường Email */}

            <div>
              <label
                htmlFor="email"
                className="block text-sm font-medium text-slate-700"
              >
                Email
              </label>

              <input
                id="email"
                name="email"
                type="email"
                autoComplete="email"
                required
                placeholder="Please enter Username/Email"
                className="mt-1 block w-full px-4 py-3 border border-slate-300 rounded-md shadow-sm focus:outline-none focus:ring-sky-500 focus:border-sky-500"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
              />
            </div>

            {/* Trường Password */}

            <div>
              <label
                htmlFor="password"
                className="block text-sm font-medium text-slate-700"
              >
                Password
              </label>

              <div className="relative">
                <input
                  id="password"
                  name="password"
                  type={showPassword ? "text" : "password"}
                  autoComplete="current-password"
                  required
                  placeholder="Please enter password"
                  className="mt-1 block w-full px-4 py-3 border border-slate-300 rounded-md shadow-sm focus:outline-none focus:ring-sky-500 focus:border-sky-500"
                  value={password}
                  onChange={(e) => setPassword(e.target.value)}
                />

                {/* Nút ẩn/hiện password */}

                <button
                  type="button"
                  onClick={() => setShowPassword(!showPassword)}
                  className="absolute inset-y-0 right-0 pr-3 flex items-center text-slate-400 hover:text-slate-600"
                  aria-label="Toggle password visibility"
                >
                  {showPassword ? (
                    <EyeOff className="h-5 w-5" />
                  ) : (
                    <Eye className="h-5 w-5" />
                  )}
                </button>
              </div>
            </div>

            {/* Link "Forgot password?" */}

            <div className="text-right">
              <a
                href="#" // Đổi sang "/forgot-password"
                className="text-sm font-medium text-sky-600 hover:underline"
              >
                Forgot password?
              </a>
            </div>

            {/* === Hiển thị lỗi (nếu có) === */}

            {error && (
              <div className="text-sm text-red-600 text-center">{error}</div>
            )}

            {/* Nút "Sign in" */}

            <div>
              <button
                type="submit"
                className="w-full py-3 px-4 bg-slate-500 hover:bg-slate-600 text-white font-semibold rounded-lg shadow-md focus:outline-none focus:ring-2 focus:ring-slate-500 focus:ring-offset-2 disabled:opacity-50"
                disabled={loading}
              >
                {loading ? "Signing in..." : "Sign in"}
              </button>
            </div>

            {/* Đường kẻ "or" */}

            <div className="relative my-6">
              <div className="absolute inset-0 flex items-center">
                .
                <span className="w-full border-t border-slate-300" />
              </div>

              <div className="relative flex justify-center text-sm">
                <span className="bg-white px-2 text-slate-500">or</span>
              </div>
            </div>

            {/* Nút "Login with Google" */}

            <div>
              <button
                type="button"
                className="w-full py-3 px-4 flex justify-center items-center gap-3 bg-white border border-slate-300 rounded-lg shadow-sm hover:bg-slate-50 focus:outline-none focus:ring-2 focus:ring-slate-500 focus:ring-offset-2"
              >
                <GoogleLogoIcon />

                <span className="text-sm font-medium text-slate-700">
                  Login with Google
                </span>
              </button>
            </div>
          </form>
          {/* Link "Register now" */}
          <p className="mt-8 text-center text-sm text-slate-600">
            Don't have an account?{" "}
            <a
              href="#" // Đổi sang "/register"
              className="font-medium text-sky-600 hover:underline"
            >
              Register now!
            </a>
          </p>
        </div>

        {/* === CỘT BÊN PHẢI (ẢNH) === */}

        <div className="hidden md:block">
          <img
            src={signInImage}
            alt="Students in a classroom"
            className="w-full h-full object-cover"
          />
        </div>
      </div>
    </div>
  );
}
