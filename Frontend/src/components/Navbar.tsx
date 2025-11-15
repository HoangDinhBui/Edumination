import React, { useState, useEffect } from "react";
import { ChevronDown } from "lucide-react";
import edmLogo from "../assets/img/edm-logo.png";

// ====================== DROPDOWN ======================
const Dropdown: React.FC<{
  title: string;
  sections: { header?: string; items: string[] }[];
}> = ({ title, sections }) => {
  const [isOpen, setIsOpen] = useState(false);

  return (
    <div
      className="relative"
      onMouseEnter={() => setIsOpen(true)}
      onMouseLeave={() => setIsOpen(false)}
    >
      <button
        className={`inline-flex items-center gap-1.5 transition-all duration-300 group px-4 py-2 rounded-full ${
          isOpen
            ? "bg-white text-[#749BC2] shadow-md"
            : "bg-white text-[#666666] hover:text-gray-900 hover:shadow-md"
        }`}
      >
        {title}
        <ChevronDown
          className={`h-4 w-4 transition-all duration-300 ${
            isOpen ? "rotate-180 text-[#2986B7]" : ""
          }`}
        />
      </button>

      {/* DROPDOWN CONTENT */}
      <div
        className={`absolute top-full left-1/2 -translate-x-1/2 pt-4 transition-all duration-300 ${
          isOpen
            ? "opacity-100 visible translate-y-0"
            : "opacity-0 invisible -translate-y-2"
        }`}
        style={{ zIndex: 100 }}
      >
        <div className="flex gap-4">
          {sections.map((sec, i) => (
            <div
              key={i}
              className={`bg-white shadow-2xl rounded-2xl p-5 w-60 border border-gray-100 transition-all duration-300 ${
                isOpen ? "scale-100" : "scale-95"
              }`}
              style={{
                transitionDelay: `${i * 50}ms`,
              }}
            >
              {sec.header && (
                <div className="text-xs font-bold text-[#2986B7] mb-3 uppercase tracking-wider flex items-center gap-2">
                  <div className="w-1 h-4 bg-gradient-to-b from-[#4AB8A1] to-[#2986B7] rounded-full"></div>
                  {sec.header}
                </div>
              )}
              <ul className="space-y-1">
                {sec.items.map((it, idx) => (
                  <li
                    key={it}
                    className="text-gray-600 hover:text-gray-900 cursor-pointer transition-all duration-200 py-2.5 px-3 rounded-lg hover:bg-gradient-to-r hover:from-blue-50 hover:to-teal-50 hover:translate-x-1 group/item"
                    style={{
                      transitionDelay: `${idx * 30}ms`,
                    }}
                  >
                    <span className="flex items-center justify-between text-sm font-medium">
                      {it}
                      <span className="opacity-0 group-hover/item:opacity-100 transition-opacity text-[#2986B7]">
                        →
                      </span>
                    </span>
                  </li>
                ))}
              </ul>
            </div>
          ))}
        </div>
      </div>
    </div>
  );
};

// ====================== NAVBAR ======================
const Navbar: React.FC = () => {
  const [scrolled, setScrolled] = useState(false);

  useEffect(() => {
    const handleScroll = () => {
      setScrolled(window.scrollY > 20);
    };
    window.addEventListener("scroll", handleScroll);
    return () => window.removeEventListener("scroll", handleScroll);
  }, []);

  return (
    <>
      <header
        className={`fixed top-0 left-0 right-0 z-[9999] transition-all duration-500 ${
          scrolled
            ? "bg-white/95 backdrop-blur-xl shadow-xl"
            : "bg-white/80 backdrop-blur-md shadow-md"
        }`}
      >
        <div className="max-w-[1400px] mx-auto px-8">
          <div
            className={`flex items-center justify-between transition-all duration-500 ${
              scrolled ? "h-14" : "h-16"
            }`}
          >
            {/* LEFT: Logo */}
            <a href="#" className="flex items-center gap-3 group relative">
              <img
                src={edmLogo}
                alt="EDM Logo"
                className="h-8 transition-all duration-300 group-hover:scale-105 group-hover:brightness-110"
              />
            </a>

            {/* CENTER: Navigation */}
            <nav>
              <ul className="flex items-center justify-center gap-8 text-[15px] font-semibold">
                {[
                  { label: "Home", href: "#" },
                  {
                    label: "IELTS Exam Library",
                    dropdown: [
                      {
                        items: [
                          "IELTS Listening Test",
                          "IELTS Reading Test",
                          "IELTS Writing Test",
                          "IELTS Speaking Test",
                          "IELTS Test Collection",
                        ],
                      },
                    ],
                  },
                  {
                    label: "IELTS Course",
                    dropdown: [
                      {
                        header: "Our Courses",
                        items: [
                          "Foundation (0.0–5.0)",
                          "Booster (5.5–6.0)",
                          "Intensive (6.0–7.5)",
                          "Mastery (7.5–9.0)",
                        ],
                      },
                    ],
                  },
                  { label: "Ranking", href: "#" },
                ].map((item) => (
                  <li key={item.label} className="relative group">
                    {item.dropdown ? (
                      <Dropdown title={item.label} sections={item.dropdown} />
                    ) : (
                      <a
                        href={item.href}
                        className="text-gray-700 hover:text-gray-900 relative group/link transition-all duration-300 px-1"
                      >
                        {item.label}
                        <span className="absolute left-0 -bottom-1 w-0 h-0.5 bg-gradient-to-r from-[#4AB8A1] to-[#2986B7] group-hover/link:w-full transition-all duration-500 rounded-full"></span>
                      </a>
                    )}
                  </li>
                ))}
              </ul>
            </nav>

            {/* RIGHT: Auth Buttons */}
            <div className="flex items-center gap-3">
              <a
                href="/signin"
                className="text-gray-700 hover:text-gray-900 text-sm font-semibold transition-all duration-300 px-5 py-2 rounded-full hover:bg-gray-100"
              >
                Sign in
              </a>
              <a
                href="/signup"
                className="relative inline-flex items-center justify-center px-7 py-2.5 rounded-full overflow-hidden group transition-all duration-300 hover:scale-105 hover:shadow-2xl font-montserrat font-bold text-white"
              >
                {/* Hai lớp gradient nền */}
                <div className="absolute inset-0 bg-gradient-to-r from-[#4AB8A1] to-[#2986B7] transition-all duration-300"></div>
                <div className="absolute inset-0 bg-gradient-to-r from-[#2986B7] to-[#4AB8A1] opacity-0 group-hover:opacity-100 transition-opacity duration-500"></div>

                {/* Nội dung chữ */}
                <span className="relative z-10 flex items-center gap-2 text-white">
                  Sign up
                  <span className="inline-block group-hover:translate-x-1 transition-transform duration-300">
                    →
                  </span>
                </span>
              </a>
            </div>
          </div>
        </div>
      </header>

      <style>{`
        @keyframes gradient-x {
          0%, 100% { background-position: 0% 50%; }
          50% { background-position: 100% 50%; }
        }
        .animate-gradient-x {
          background-size: 200% 200%;
          animation: gradient-x 3s ease infinite;
        }
      `}</style>
    </>
  );
};

export default Navbar;
