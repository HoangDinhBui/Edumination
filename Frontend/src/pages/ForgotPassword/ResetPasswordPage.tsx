import React, { useState, useEffect } from "react";
import { Eye, EyeOff } from "lucide-react";
import { useNavigate } from "react-router-dom";

// === SLIDE HÌNH ẢNH ===
import slide1 from "../../assets/img/adv.jpg";
import slide2 from "../../assets/img/adv1.jpg";
import slide3 from "../../assets/img/adv2.jpg";

const ResetPasswordPage: React.FC = () => {
  const [showPassword, setShowPassword] = useState(false);
  const [showConfirmPassword, setShowConfirmPassword] = useState(false);
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

  // === GIỮ NGUYÊN LOGIC CỦA BẠN ===
  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    alert("Password changed successfully!");
    navigate("/login");
  };

  return (
    <>
      {/* GOOGLE FONT */}
      <style>{`
        @import url('https://fonts.googleapis.com/css2?family=Paytone+One&family=Montserrat:wght@300;400;500;600;700&family=Be+Vietnam+Pro:wght@300;400;500;600;700&display=swap');
      `}</style>

      <div className="w-screen h-screen flex flex-col md:flex-row overflow-hidden">
        {/* === CỘT FORM === */}
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
              {/* New Password */}
              <div>
                <label
                  htmlFor="password"
                  className="block text-sm text-[#294563] font-semibold mb-1"
                  style={{ fontFamily: "'Be Vietnam Pro', sans-serif" }}
                >
                  New Password
                </label>
                <div className="relative">
                  <input
                    id="password"
                    type={showPassword ? "text" : "password"}
                    required
                    placeholder="Enter new password"
                    className="w-full px-3 py-2 border border-slate-300 rounded-md shadow-sm focus:ring-sky-500 focus:border-sky-500"
                    style={{ fontFamily: "'Montserrat', sans-serif" }}
                  />
                  <button
                    type="button"
                    onClick={() => setShowPassword(!showPassword)}
                    className="absolute inset-y-0 right-0 pr-3 flex items-center text-slate-400 hover:text-slate-600 bg-transparent"
                  >
                    {showPassword ? (
                      <EyeOff className="h-5 w-5" />
                    ) : (
                      <Eye className="h-5 w-5" />
                    )}
                  </button>
                </div>
              </div>

              {/* Confirm Password */}
              <div>
                <label
                  htmlFor="confirm-password"
                  className="block text-sm text-[#294563] font-semibold mb-1"
                  style={{ fontFamily: "'Be Vietnam Pro', sans-serif" }}
                >
                  Confirm Password
                </label>
                <div className="relative">
                  <input
                    id="confirm-password"
                    type={showConfirmPassword ? "text" : "password"}
                    required
                    placeholder="Re-enter new password"
                    className="w-full px-3 py-2 border border-slate-300 rounded-md shadow-sm focus:ring-sky-500 focus:border-sky-500"
                    style={{ fontFamily: "'Montserrat', sans-serif" }}
                  />
                  <button
                    type="button"
                    onClick={() =>
                      setShowConfirmPassword(!showConfirmPassword)
                    }
                    className="absolute inset-y-0 right-0 pr-3 flex items-center text-slate-400 hover:text-slate-600 bg-transparent"
                  >
                    {showConfirmPassword ? (
                      <EyeOff className="h-5 w-5" />
                    ) : (
                      <Eye className="h-5 w-5" />
                    )}
                  </button>
                </div>
              </div>

              {/* Submit */}
              <button
                type="submit"
                className="w-full py-2.5 bg-[#749BC2] hover:bg-sky-700 text-white text-base font-semibold rounded-full"
                style={{ fontFamily: "'Be Vietnam Pro', sans-serif" }}
              >
                Change Password
              </button>

              <p className="text-center text-sm text-[#666666] pt-2">
                <a
                  href="/signin"
                  className="font-semibold text-[#749BC2] hover:underline"
                >
                  Back to login
                </a>
              </p>
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

export default ResetPasswordPage;
