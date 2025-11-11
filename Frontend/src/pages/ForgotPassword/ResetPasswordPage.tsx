import React, { useState } from "react";
import { ChevronLeft, Eye, EyeOff } from "lucide-react";
import { useNavigate } from "react-router-dom";

// Ảnh slide show
import slide1 from "../../assets/img/adv.jpg";
import slide2 from "../../assets/img/adv1.jpg";
import slide3 from "../../assets/img/adv2.jpg";

const ResetPasswordPage: React.FC = () => {
  const [showPassword, setShowPassword] = useState(false);
  const [showConfirmPassword, setShowConfirmPassword] = useState(false);
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

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    alert("Password changed successfully!");
    navigate("/login");
  };

  return (
    <>
      <style>{`
        @import url('https://fonts.googleapis.com/css2?family=Paytone+One&family=Montserrat:wght@300;400;500;600;700&family=Be+Vietnam+Pro:wght@300;400;500;600;700&display=swap');
      `}</style>

      <div className="w-screen h-screen flex flex-col md:flex-row overflow-hidden">
        {/* === CỘT FORM === */}
        <div className="flex flex-col justify-center items-center w-full md:w-[45%] bg-white p-8 md:p-16">
          <div className="w-full max-w-md">
            <button
              onClick={() => navigate("/enter-otp")}
              className="inline-flex items-center gap-1 text-sm text-[#294563] hover:text-[#23B0EB] mb-6"
            >
              <ChevronLeft className="h-4 w-4" />
              Back to Verify OTP
            </button>

            <h1
              className="mt-4 text-3xl md:text-4xl font-extrabold text-[#294563] text-center"
              style={{ fontFamily: "'Paytone One', sans-serif" }}
            >
              Reset Password
            </h1>
            <p className="text-center text-[#666666] mb-10 mt-2">
              Enter your new password below
            </p>

            <form className="space-y-6" onSubmit={handleSubmit}>
              {/* Password */}
              <div>
                <label
                  htmlFor="password"
                  className="block text-sm font-medium text-slate-700 mb-1 relative after:content-['*'] after:ml-0.5 after:text-red-500"
                  style={{ fontFamily: "'Be Vietnam Pro', sans-serif" }}
                >
                  New Password
                </label>
                <div className="relative">
                  <input
                    id="password"
                    name="password"
                    type={showPassword ? "text" : "password"}
                    autoComplete="new-password"
                    required
                    placeholder="Enter new password"
                    className="mt-1 block w-full px-4 py-3 pr-10 border border-slate-300 rounded-md shadow-sm focus:outline-none focus:ring-[#23B0EB] focus:border-[#23B0EB]"
                    style={{ fontFamily: "'Montserrat', sans-serif" }}
                  />
                  <div
                    onClick={() => setShowPassword(!showPassword)}
                    className="absolute inset-y-0 right-3 flex items-center cursor-pointer text-slate-400 hover:text-[#23B0EB] transition-colors"
                  >
                    {showPassword ? (
                      <EyeOff className="h-5 w-5" />
                    ) : (
                      <Eye className="h-5 w-5" />
                    )}
                  </div>
                </div>
              </div>

              {/* Confirm Password */}
              <div>
                <label
                  htmlFor="confirm-password"
                  className="block text-sm font-medium text-slate-700 mb-1 relative after:content-['*'] after:ml-0.5 after:text-red-500"
                  style={{ fontFamily: "'Be Vietnam Pro', sans-serif" }}
                >
                  Confirm New Password
                </label>
                <div className="relative">
                  <input
                    id="confirm-password"
                    name="confirm-password"
                    type={showConfirmPassword ? "text" : "password"}
                    autoComplete="new-password"
                    required
                    placeholder="Re-enter new password"
                    className="mt-1 block w-full px-4 py-3 pr-10 border border-slate-300 rounded-md shadow-sm focus:outline-none focus:ring-[#23B0EB] focus:border-[#23B0EB]"
                    style={{ fontFamily: "'Montserrat', sans-serif" }}
                  />
                  <div
                    onClick={() =>
                      setShowConfirmPassword(!showConfirmPassword)
                    }
                    className="absolute inset-y-0 right-3 flex items-center cursor-pointer text-slate-400 hover:text-[#23B0EB] transition-colors"
                  >
                    {showConfirmPassword ? (
                      <EyeOff className="h-5 w-5" />
                    ) : (
                      <Eye className="h-5 w-5" />
                    )}
                  </div>
                </div>
              </div>

              <button
                type="submit"
                className="w-full py-3 bg-[#749BC2] hover:bg-sky-700 text-white text-l font-semibold rounded-full shadow-md transition-all duration-200"
                style={{ fontFamily: "'Be Vietnam Pro', sans-serif" }}
              >
                Change Password
              </button>

              <p className="text-center text-sm text-slate-500 pt-2">
                <a
                  href="/signin"
                  className="font-medium text-[#23B0EB] hover:underline"
                >
                  Back to login
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
