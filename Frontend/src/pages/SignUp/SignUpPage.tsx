import React, { useState } from "react";
import { ChevronLeft, Eye, EyeOff } from "lucide-react";

// 1. Dùng lại ảnh từ trang Sign In
import signUpImage from "../../assets/img/Rectangle 123.png"; // <-- Dùng cùng file ảnh

/**
 * Component SVG cho logo Google (Giữ nguyên)
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

export default function SignUpPage() {
  // 2. Cần 2 state riêng biệt cho 2 ô password
  const [showPassword, setShowPassword] = useState(false);
  const [showConfirmPassword, setShowConfirmPassword] = useState(false);

  return (
    <div className="min-h-screen bg-slate-50 flex items-center justify-center p-4">
      <div className="mx-auto w-full max-w-4xl bg-white shadow-2xl rounded-2xl overflow-hidden grid md:grid-cols-2">
        {/* === CỘT BÊN TRÁI (FORM) === */}
        <div className="p-8 md:p-12">
          {/* Link "Back to home" (Giữ nguyên) */}
          <a
            href="/"
            className="inline-flex items-center gap-1 text-sm text-slate-600 hover:text-slate-800"
          >
            <ChevronLeft className="h-4 w-4" />
            Back to home
          </a>

          {/* 3. Thay đổi Tiêu đề */}
          <div className ="text-center">
            <h1 className="mt-6 text-3xl font-bold text-slate-700">
            Get Started Now!
            </h1>
            <p className="mt-2 text-slate-600">Create an account</p>
          </div>

          <form className="mt-8 space-y-6">
            {/* Trường Email (Giữ nguyên) */}
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
              />
            </div>

            {/* Trường Password (Giữ nguyên) */}
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
                  autoComplete="new-password" // Đổi thành new-password
                  required
                  placeholder="Please enter password"
                  className="mt-1 block w-full px-4 py-3 border border-slate-300 rounded-md shadow-sm focus:outline-none focus:ring-sky-500 focus:border-sky-500"
                />
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

            {/* 4. Thêm trường "Confirm password" */}
            <div>
              <label
                htmlFor="confirm-password"
                className="block text-sm font-medium text-slate-700"
              >
                Confirm password
              </label>
              <div className="relative">
                <input
                  id="confirm-password"
                  name="confirm-password"
                  type={showConfirmPassword ? "text" : "password"}
                  autoComplete="new-password"
                  required
                  placeholder="Please enter password once again"
                  className="mt-1 block w-full px-4 py-3 border border-slate-300 rounded-md shadow-sm focus:outline-none focus:ring-sky-500 focus:border-sky-500"
                />
                <button
                  type="button"
                  onClick={() => setShowConfirmPassword(!showConfirmPassword)}
                  className="absolute inset-y-0 right-0 pr-3 flex items-center text-slate-400 hover:text-slate-600"
                  aria-label="Toggle confirm password visibility"
                >
                  {showConfirmPassword ? (
                    <EyeOff className="h-5 w-5" />
                  ) : (
                    <Eye className="h-5 w-5" />
                  )}
                </button>
              </div>
            </div>

            {/* 5. Nút "Sign up" */}
            <div className="pt-2">
              <button
                type="submit"
                className="w-full py-3 px-4 bg-slate-500 hover:bg-slate-600 text-white font-semibold rounded-lg shadow-md focus:outline-none focus:ring-2 focus:ring-slate-500 focus:ring-offset-2"
              >
                Sign up
              </button>
            </div>

            {/* Đường kẻ "or" (Giữ nguyên) */}
            <div className="relative my-4">
              <div className="absolute inset-0 flex items-center">
                <span className="w-full border-t border-slate-300" />
              </div>
              <div className="relative flex justify-center text-sm">
                <span className="bg-white px-2 text-slate-500">or</span>
              </div>
            </div>

            {/* Nút "Login with Google" (Giữ nguyên) */}
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

          {/* 6. Thay đổi Link chân trang */}
          <p className="mt-8 text-center text-sm text-slate-600">
            Have an account already?{" "}
            <a
              href="/signin" // Đổi link này về trang đăng nhập
              className="font-medium text-sky-600 hover:underline"
            >
              Please login here!
            </a>
          </p>
        </div>

        {/* === CỘT BÊN PHẢI (ẢNH) === (Giữ nguyên) */}
        <div className="hidden md:block">
          <img
            src={signUpImage}
            alt="Students in a British Council classroom"
            className="w-full h-full object-cover"
          />
        </div>
      </div>
    </div>
  );
}