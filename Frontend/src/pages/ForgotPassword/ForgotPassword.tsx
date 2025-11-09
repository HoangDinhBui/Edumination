import React, { useState } from "react";
import { ChevronLeft } from "lucide-react";
import { useNavigate } from "react-router-dom";
import axios from "axios";
import signInImage from "../../assets/img/Rectangle 123.png";

const ForgotPasswordPage: React.FC = () => {
  const [email, setEmail] = useState("");
  const [loading, setLoading] = useState(false);
  const [message, setMessage] = useState<string | null>(null);
  const [error, setError] = useState<string | null>(null);
  const navigate = useNavigate();

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

      // ✅ Gọi API Forgot Password
      const apiUrl = "http://localhost:8081/api/v1/auth/password/forgot";
      await axios.post(apiUrl, { email });

      setMessage("Nếu email tồn tại, hướng dẫn đặt lại mật khẩu đã được gửi.");
      setEmail("");

      // ⏳ Chuyển hướng sau 3 giây
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
    <div className="min-h-screen bg-slate-50 flex items-center justify-center p-4">
      <div className="mx-auto w-full max-w-4xl bg-white shadow-2xl rounded-2xl overflow-hidden grid md:grid-cols-2">
        {/* Cột trái: Form */}
        <div className="p-8 md:p-12">
          <a
            href="/"
            className="inline-flex items-center gap-1 text-sm text-slate-600 hover:text-slate-800"
          >
            <ChevronLeft className="h-4 w-4" />
            Back to home
          </a>

          <div className="text-center">
            <h1 className="mt-6 text-3xl font-bold text-slate-700">
              Forgot password?
            </h1>
          </div>

          {/* Form */}
          <form className="mt-8 space-y-6" onSubmit={handleSubmit}>
            <div>
              <label
                htmlFor="email"
                className="block text-sm font-medium text-slate-700"
              >
                Email address
              </label>
              <input
                id="email"
                type="email"
                value={email}
                onChange={(e) => setEmail(e.target.value)} // 👈 Gắn state
                required
                placeholder="Please enter your email"
                className="mt-1 block w-full px-4 py-3 border border-slate-300 rounded-md shadow-sm focus:outline-none focus:ring-sky-500 focus:border-sky-500"
              />
            </div>

            {/* Hiển thị thông báo */}
            {error && (
              <p className="text-sm text-red-500 text-center">{error}</p>
            )}
            {message && (
              <p className="text-sm text-green-600 text-center">{message}</p>
            )}

            <p className="text-sm text-slate-500 pt-1 text-center">
              Password reset instructions will be sent to your registered email.
            </p>

            <div>
              <button
                type="submit"
                disabled={loading}
                className="w-full py-3 px-4 bg-slate-500 hover:bg-slate-600 text-white font-semibold rounded-lg shadow-md focus:outline-none focus:ring-2 focus:ring-slate-500 focus:ring-offset-2"
              >
                {loading ? "Đang gửi..." : "Submit"}
              </button>
            </div>

            <p className="text-center text-slate-600 hover:text-slate-800 pt-2">
              <a href="/login" className="font-medium text-sky-600 hover:underline">
                Login to your account
              </a>
            </p>
          </form>
        </div>

        {/* Cột phải: Ảnh */}
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
};

export default ForgotPasswordPage;
