import React from "react";
import { useState } from "react";
import { Swiper, SwiperSlide } from "swiper/react";
import type { SwiperSlideRenderProps } from "swiper/react";
import type { Swiper as SwiperInstance } from "swiper";
import "swiper/css";
import {
  ChevronDown,
  CheckCircle2,
  Star,
  ChevronLeft,
  ChevronRight,
  Mail,
  MapPin,
  Phone,
} from "lucide-react";
import studentImage from "../../assets/img/Ellipse 9.png";
import logoImage from "../../assets/img/edm-logo.png";
import heroBackground from "../../assets/img/Ellipse 7.png";
import bcLogo from "../../assets/img/Rectangle 94.png";
import cambridgeLogo from "../../assets/img/Rectangle 95.png";
import idpLogo from "../../assets/img/Rectangle 96.png";
import iconWhatMakesUsDifferent from "../../assets/img/iconWhatMakesUsDifferent.png";
import imageGroupStudy1 from "../../assets/img/imageGroupStudy1.png";
import imageGroupStudy2 from "../../assets/img/imageGroupStudy2.png";
import imageGroupStudy3 from "../../assets/img/imageGroupStudy3.png";
import "../../index.css";
import vector5 from "../../assets/img/Vector 5.png";
import vector9 from "../../assets/img/Vector 9.png";
import Group25 from "../../assets/img/Group 25.png";
import Group26 from "../../assets/img/Group 26.png";
import Group27 from "../../assets/img/Group 27.png";
import classroomImg from "../../assets/img/Rectangle 116.png";
import halloweenImg from "../../assets/img/Rectangle 117.png";
import studentsImg from "../../assets/img/Rectangle 118.png";
import theForum from "../../assets/img/theForum.jpg";
import Navbar from "../../components/Navbar";
import BDH from "../../assets/img/BDH.png";
import TTMD from "../../assets/img/TTMD.png";
import TVMT from "../../assets/img/TVMT.png";
import DHL from "../../assets/img/DHL.png";
import WhyBG from "../../assets/img/WhyDifferentBG.png";
import cardimg from "../../assets/img/cardimg.png";
import { CheckCircle } from "lucide-react";
/**
 * EDM — HomePage (single-file preview)
 * - TailwindCSS layout, light & airy aesthetic
 * - Minimal interactivity (dropdowns on hover, simple carousels)
 * - No external state managers required
 */

const badges = [
  "Pioneering Learning Model University Lecture",
  "Rapidly upgrading your band level in just 90 hours of study",
  "Specialized training in IELTS/Children's English",
  "Commitment to output quality",
];

const partners = [
  { name: "British Council", logo: bcLogo },
  { name: "University of Cambridge", logo: cambridgeLogo },
  { name: "idp", logo: idpLogo },
];

const campusImages = [
  classroomImg, // Ảnh logo
  halloweenImg,
  studentsImg,
  theForum,
  studentsImg,
];

function useCarousel<T>(items: T[]) {
  const [idx, setIdx] = React.useState(0);
  const next = () => setIdx((p) => (p + 1) % items.length);
  const prev = () => setIdx((p) => (p - 1 + items.length) % items.length);
  return { idx, next, prev };
}

const students = [
  {
    image: { BDH },
    name: "Bui Dinh Hoang",
    ieltsScore: "9.0",
    description:
      "A student of University of Transport and Communications – Ho Chi Minh City Campus",
  },
  {
    image: { TTMD },
    name: "Tran Thi My Dung",
    ieltsScore: "8.5",
    description:
      "A student of University of Transport and Communications – Ho Chi Minh City Campus",
  },
  {
    image: { TVMT },
    name: "Tran Van Minh Tu",
    ieltsScore: "8.5",
    description:
      "A student of University of Transport and Communications – Ho Chi Minh City Campus",
  },
  {
    image: { DHL },
    name: "Dam Hoang Lam",
    ieltsScore: "8.5",
    description:
      "A student of University of Transport and Communications – Ho Chi Minh City Campus",
  },
];

const Hero: React.FC = () => {
  const [idx, setIdx] = useState(0);
  const next = () => setIdx((prev) => (prev + 1) % students.length);
  const prev = () =>
    setIdx((prev) => (prev - 1 + students.length) % students.length);

  return (
    <section className="relative overflow-hidden w-full bg-white">
      <div className="w-full px-4 sm:px-6 md:px-10 lg:px-20 py-8 md:py-12 lg:py-16">
        <div className="grid lg:grid-cols-2 items-center gap-16 max-w-7xl mx-auto">
          {/* Left Content */}
          <div className="space-y-6">
            <h1 className="text-3xl sm:text-4xl md:text-5xl lg:text-6xl font-playfair text-[#7BA5D1] leading-normal">
              EDUMINATION
              <br />
              English Center
              <span className="inline-block ml-2 md:ml-3 text-3xl md:text-4xl">🎯</span>
            </h1>

            <ul className="space-y-4 pt-4">
              {badges.map((b) => (
                <li key={b} className="flex items-start gap-3 text-gray-700">
                  <CheckCircle className="h-5 w-5 mt-0.5 text-[#7BA5D1] flex-shrink-0" />
                  <span className="text-xs sm:text-sm md:text-base font-montserrat">
                    {b}
                  </span>
                </li>
              ))}
            </ul>

            <div className="pt-4">
              <a
                href="#start"
                className="relative inline-flex items-center justify-center px-6 md:px-8 py-2.5 md:py-3.5 rounded-full overflow-hidden group transition-all duration-300 hover:scale-105 font-montserrat font-semibold text-white shadow-lg hover:shadow-2xl"
              >
                {/* Hai lớp gradient đổi màu khi hover */}
                <div className="absolute inset-0 bg-gradient-to-r from-[#4AB8A1] to-[#2986B7] transition-all duration-300"></div>
                <div className="absolute inset-0 bg-gradient-to-r from-[#2986B7] to-[#4AB8A1] opacity-0 group-hover:opacity-100 transition-opacity duration-500"></div>

                {/* Nội dung chữ */}
                <span className="relative z-10 flex items-center gap-2 text-white">
                  Start Your Free Practice
                  <span className="inline-block group-hover:translate-x-1 transition-transform duration-300">
                    →
                  </span>
                </span>
              </a>
            </div>

            <div className="pt-3 space-y-4">
              <div className="text-xs sm:text-sm text-gray-600 font-montserrat">
                Collaborate with leading educational institutions:
              </div>
              <div className="flex items-center gap-4 md:gap-6 flex-wrap">
                {partners.map((p) => (
                  <img
                    key={p.name}
                    src={p.logo}
                    alt={p.name}
                    className="h-8 md:h-10 opacity-80 hover:opacity-100 transition-opacity"
                  />
                ))}
              </div>
            </div>
          </div>

          {/* Right Visual */}
          <div className="relative">
            <div className="relative mx-auto w-full max-w-[500px]">
              {/* Main image container */}
              <div className="aspect-[4/5] w-full overflow-hidden bg-transparent flex items-center justify-center relative">
                {students.map((student, i) => (
                  <div
                    key={i}
                    className={`absolute inset-0 flex items-center justify-center transition-opacity duration-700 ease-in-out ${
                      idx === i ? "opacity-100 z-10" : "opacity-0 z-0"
                    }`}
                  >
                    <img
                      src={Object.values(student.image)[0]}
                      alt={student.name}
                      className="max-h-[90%] max-w-[90%] object-contain bg-transparent mx-auto my-auto"
                    />
                  </div>
                ))}
              </div>

              {/* Star badge - 9.0 IELTS */}
              <div className="absolute -left-6 top-[60%] -translate-y-1/2 z-10">
                <div className="relative">
                  <Star className="h-24 w-24 text-red-500 fill-red-500 rotate-12 drop-shadow-2xl" />
                  <div className="absolute inset-0 flex flex-col items-center justify-center text-white">
                    <span className="text-2xl font-bold font-sans">
                      {students[idx].ieltsScore}
                    </span>
                    <span className="text-xs font-semibold font-sans -mt-1">
                      IELTS
                    </span>
                  </div>
                </div>
              </div>

              {/* Carousel controls */}
              <button
                onClick={prev}
                aria-label="prev"
                className="absolute left-1 top-1/2 -translate-y-1/2 grid place-items-center h-12 w-12 rounded-full bg-sky-300/80 hover:bg-sky-400 text-white shadow-lg z-10 transition-all duration-300 hover:scale-110"
              >
                <ChevronLeft className="h-6 w-6" />
              </button>
              <button
                onClick={next}
                aria-label="next"
                className="absolute right-1 top-1/2 -translate-y-1/2 grid place-items-center h-12 w-12 rounded-full bg-sky-300/80 hover:bg-sky-400 text-white shadow-lg z-10 transition-all duration-300 hover:scale-110"
              >
                <ChevronRight className="justify-center h-6 w-6" />
              </button>

              {/* Caption */}
              <div className=" text-center space-y-2">
                <div className="text-[#5B9BD5] font-semibold text-lg font-sans">
                  {students[idx].name}
                </div>
                <div className="text-xs text-gray-600 max-w-sm mx-auto leading-relaxed font-sans">
                  {students[idx].description}
                </div>
              </div>

              {/* Dots indicator */}
              <div className="mt-4 flex items-center justify-center gap-1.5">
                {[0, 1, 2, 3].map((i) => (
                  <button
                    key={i}
                    onClick={() => setIdx(i)}
                    className={`h-1.5 rounded-full transition-all duration-300 ${
                      idx === i
                        ? "bg-sky-500 w-4"
                        : "bg-gray-300 w-1.5 hover:bg-gray-400"
                    }`}
                    aria-label={`Go to slide ${i + 1}`}
                  />
                ))}
              </div>
            </div>
          </div>
        </div>
      </div>

      <style>{`
        @import url('https://fonts.googleapis.com/css2?family=Playfair+Display:wght@700&family=Montserrat:wght@400;500;600;700&display=swap');
        
        .font-playfair {
          font-family: 'Playfair Display', serif;
        }
        
        .font-montserrat {
          font-family: 'Montserrat', sans-serif;
        }
      `}</style>
    </section>
  );
};

const Stats: React.FC = () => {
  const [isVisible, setIsVisible] = React.useState(false);
  const [hoveredIndex, setHoveredIndex] = React.useState<number | null>(null);
  const [mousePosition, setMousePosition] = React.useState({ x: 0, y: 0 });
  const sectionRef = React.useRef<HTMLElement>(null);

  const stats = [
    { value: "8+", label: "Years of operation" },
    { value: "10+", label: "Facilities nationwide" },
    { value: "200+", label: "Excellent students achieving IELTS 8.0+" },
    { value: "100%", label: "High-quality specialized teachers" },
    { value: "5,000+", label: "Students reached the finish line" },
    {
      value: "10+",
      label: "Top Platinum Partners of IDP and British Council Vietnam",
    },
  ];

  React.useEffect(() => {
    const observer = new IntersectionObserver(
      ([entry]) => {
        if (entry.isIntersecting) {
          setIsVisible(true);
        }
      },
      { threshold: 0.2 }
    );

    if (sectionRef.current) {
      observer.observe(sectionRef.current);
    }

    return () => {
      if (sectionRef.current) {
        observer.unobserve(sectionRef.current);
      }
    };
  }, []);

  const handleMouseMove = (
    e: React.MouseEvent<HTMLDivElement>,
    index: number
  ) => {
    const rect = e.currentTarget.getBoundingClientRect();
    setMousePosition({
      x: e.clientX - rect.left,
      y: e.clientY - rect.top,
    });
    setHoveredIndex(index);
  };

  return (
    <>
      <style>{`
        @import url('https://fonts.googleapis.com/css2?family=Playfair+Display:wght@700&family=Jomolhari&display=swap');
      `}</style>

      <section
        ref={sectionRef}
        className="relative py-20 w-full px-6 sm:px-10 lg:px-20 bg-white"
      >
        {/* Header */}
        <div className="text-center mb-16 relative">
          <div className="inline-block relative">
            {/* Decorative stars */}
            <svg
              className="absolute -top-2 -right-8 w-6 h-6 text-rose-500"
              viewBox="0 0 24 24"
              fill="none"
              stroke="currentColor"
              strokeWidth="2"
            >
              <path d="M12 2L15.09 8.26L22 9.27L17 14.14L18.18 21.02L12 17.77L5.82 21.02L7 14.14L2 9.27L8.91 8.26L12 2Z" />
            </svg>
            <svg
              className="absolute -bottom-2 -left-6 w-5 h-5 text-rose-500"
              viewBox="0 0 24 24"
              fill="none"
              stroke="currentColor"
              strokeWidth="2"
            >
              <path d="M12 2L15.09 8.26L22 9.27L17 14.14L18.18 21.02L12 17.77L5.82 21.02L7 14.14L2 9.27L8.91 8.26L12 2Z" />
            </svg>

            {/* Title with curved background */}
            <div className="relative">
              <svg
                className="absolute inset-0 w-full h-full -z-10"
                viewBox="0 0 500 100"
                preserveAspectRatio="none"
              >
                <path
                  d="M 0,50 Q 125,20 250,50 T 500,50 L 500,100 L 0,100 Z"
                  fill="#DBEAFE"
                  opacity="0.6"
                />
                <path
                  d="M 50,60 Q 150,35 280,58 T 450,60 L 450,100 L 50,100 Z"
                  fill="#BFDBFE"
                  opacity="0.4"
                />
              </svg>

              <h2 className="text-4xl md:text-5xl font-serif text-[#7BA5C8] px-8 py-4 relative">
                High quality
                <br />
                training program
              </h2>
            </div>
          </div>
        </div>

        {/* Stats Grid */}
        <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6 max-w-7xl mx-auto">
          {stats.map((stat, index) => (
            <div
              key={index}
              className="relative rounded-3xl bg-white shadow-sm ring-1 ring-slate-200 p-8 overflow-hidden group cursor-pointer transition-all duration-300 hover:shadow-lg"
              onMouseMove={(e) => handleMouseMove(e, index)}
              onMouseLeave={() => setHoveredIndex(null)}
            >
              {/* Animated blue underline */}
              <div
                className={`absolute bottom-0 left-0 h-1 bg-gradient-to-r from-sky-400 to-blue-500 transition-all duration-700 ease-out ${
                  isVisible ? "w-full" : "w-0"
                }`}
                style={{
                  transitionDelay: `${index * 100}ms`,
                }}
              />

              {/* Hover gradient effect following mouse */}
              {hoveredIndex === index && (
                <div
                  className="absolute inset-0 pointer-events-none transition-opacity duration-300 ease-out"
                  style={{
                    opacity: 0.45, // 👈 tăng từ 0.2 lên 0.45
                    background: `radial-gradient(
                      circle 250px at ${mousePosition.x}px ${mousePosition.y}px,
                      rgba(56, 189, 248, 0.55),  /* xanh sáng trung tâm */
                      rgba(56, 189, 248, 0.15) 40%, /* mờ dần */
                      transparent 70%
                    )`,
                    filter: "blur(6px)", // 👈 giúp viền sáng mềm hơn, không gắt
                    transition: "background 0.15s ease-out",
                  }}
                />
              )}

              {/* Content */}
              <div className="relative z-10">
                <div
                  className="text-5xl md:text-6xl font-bold text-rose-600 mb-3 transition-transform duration-300 group-hover:scale-110"
                  style={{ fontFamily: "'Playfair Display', serif" }}
                >
                  {stat.value}
                </div>
                <div
                  className="text-slate-600 text-sm md:text-base leading-relaxed"
                  style={{ fontFamily: "'Jomolhari', serif" }}
                >
                  {stat.label}
                </div>
              </div>
            </div>
          ))}
        </div>
      </section>
    </>
  );
};

const WhyDifferent: React.FC = () => (
  <section
    className="py-16 sm:py-24 relative w-full px-6 sm:px-10 lg:px-20"
    style={{
      backgroundImage: `url(${WhyBG})`,
      backgroundSize: "cover",
    }}
  >
    {" "}
    {/* Thay đổi màu nền và padding */}
    <div className="w-full px-8 sm:px-12 lg:px-20">
      {/* Tiêu đề và mô tả */}
      <div className="text-center mb-16 space-y-4 max-w-3xl mx-auto">
        <img
          src={iconWhatMakesUsDifferent} // Sử dụng icon SVG
          alt="What Makes Edumination Different"
          className="mx-auto h-16 w-13 text-sky-500 mb-4"
        />
        <div className="relative w-full">
          <img
            src={vector5}
            alt="bg"
            className="w-2/3 "
            style={{ margin: "0 auto" }}
          />
          <div className="absolute inset-0 flex items-center justify-center">
            <p
              className="text-white text-2xl font-semibold text-center"
              style={{
                fontSize: "24px",
                lineHeight: "1.5",
                fontFamily: "'Playfair Display', serif",
              }}
            >
              What makes{" "}
              <span
                className="font-bold"
                style={{ fontFamily: "'Playfair Display', serif" }}
              >
                EDUMINATION
              </span>
              <br></br> different?
            </p>
          </div>
        </div>
        <p
          className="text-lg text-white leading-relaxed"
          style={{ fontFamily: "'Jomolhari', sans-serif" }}
        >
          Edumination is an educational organization specializing in IELTS and
          SAT test preparation, as well as teaching English at all levels.
          Edumination combines the latest technology solutions with optimal
          expertise to deliver the highest quality of instruction.
        </p>
      </div>

      {/* Grid of alternating content */}
      <div className="space-y-20">
        {" "}
        {/* Khoảng cách giữa các hàng nội dung */}
        {/* Row 1 */}
        <div className="grid lg:grid-cols-2 gap-12 items-center">
          <div className="relative overflow-hidden rounded-3xl shadow-xl ring-1 ring-slate-100 h-[380px]">
            {" "}
            {/* Chiều cao cố định */}
            <img
              src={imageGroupStudy1} // Sử dụng ảnh đã import
              className="w-full h-full object-cover"
              alt="Exclusive Learning Model University Lecture"
            />
          </div>
          <div>
            <h3 className="text-2xl font-bold text-white mb-4">
              Exclusive Learning Model <br /> University Lecture
            </h3>
            <ul className="space-y-3 text-white">
              <li className="flex items-start gap-3">
                <CheckCircle2 className="h-5 w-5 mt-0.5 text-[#C8ECFF] flex-shrink-0" />
                <span>
                  Effective band improvement learning models, applying knowledge
                  in a practical way.
                </span>
              </li>
              <li className="flex items-start gap-3">
                <CheckCircle2 className="h-5 w-5 mt-0.5 text-[#C8ECFF] flex-shrink-0" />
                <span>
                  60 different optional "Lecture" sessions alongside fixed
                  classes.
                </span>
              </li>
              <li className="flex items-start gap-3">
                <CheckCircle2 className="h-5 w-5 mt-0.5 text-[#C8ECFF] flex-shrink-0" />
                <span>
                  Diverse topics, supporting direction and developing one's
                  language skills.
                </span>
              </li>
              <li className="flex items-start gap-3">
                <CheckCircle2 className="h-5 w-5 mt-0.5 text-[#C8ECFF] flex-shrink-0" />
                <span>100% free and unlimited for all students</span>
              </li>
            </ul>
          </div>
        </div>
        {/* Row 2 (Ảnh bên phải) */}
        <div className="grid lg:grid-cols-2 gap-12 items-center">
          <div className="order-2 lg:order-1">
            {" "}
            {/* Đổi thứ tự cho màn hình lớn */}
            <h3 className="text-2xl font-bold text-white mb-4">
              High-quality supplementary online course
            </h3>
            <ul className="space-y-3 text-white">
              <li className="flex items-start gap-3">
                <CheckCircle2 className="h-5 w-5 mt-0.5 text-[#C8ECFF] flex-shrink-0" />
                <span>
                  All courses come with 100% free supplementary Online Courses.
                </span>
              </li>
              <li className="flex items-start gap-3">
                <CheckCircle2 className="h-5 w-5 mt-0.5 text-[#C8ECFF] flex-shrink-0" />
                <span>
                  Academic lecture video compiled by the Edumination expert.
                </span>
              </li>
              <li className="flex items-start gap-3">
                <CheckCircle2 className="h-5 w-5 mt-0.5 text-[#C8ECFF] flex-shrink-0" />
                <span>
                  Supplement knowledge, helping students practice anytime,
                  anywhere.
                </span>
              </li>
            </ul>
          </div>
          <div className="relative overflow-hidden rounded-3xl shadow-xl ring-1 ring-slate-100 h-[380px] order-1 lg:order-2">
            {" "}
            {/* Chiều cao cố định */}
            <img
              src={imageGroupStudy2} // Sử dụng ảnh đã import
              className="w-full h-full object-cover"
              alt="High-quality supplementary online course"
            />
          </div>
        </div>
        {/* Row 3 (Ảnh bên trái) */}
        <div className="grid lg:grid-cols-2 gap-12 items-center">
          <div className="relative overflow-hidden rounded-3xl shadow-xl ring-1 ring-slate-100 h-[380px]">
            {" "}
            {/* Chiều cao cố định */}
            <img
              src={imageGroupStudy3} // Sử dụng ảnh đã import
              className="w-full h-full object-cover"
              alt="Specialized academic textbook designed specifically"
            />
          </div>
          <div>
            <h3 className="text-2xl font-bold text-white mb-4">
              Specialized academic textbook <br /> designed specifically
            </h3>
            <ul className="space-y-3 text-white">
              <li className="flex items-start gap-3">
                <CheckCircle2 className="h-5 w-5 mt-0.5 text-[#C8ECFF] flex-shrink-0" />
                <span>
                  All teaching materials are specifically designed according to
                  the Cambridge criteria.
                </span>
              </li>
              <li className="flex items-start gap-3">
                <CheckCircle2 className="h-5 w-5 mt-0.5 text-[#C8ECFF] flex-shrink-0" />
                <span>
                  Combine training programs researched in depth from leading
                  language publishers.
                </span>
              </li>
              <li className="flex items-start gap-3">
                <CheckCircle2 className="h-5 w-5 mt-0.5 text-[#C8ECFF] flex-shrink-0" />
                <span>
                  Diverse topics and progressively challenging types of
                  exercises, suitable for the students' level.
                </span>
              </li>
            </ul>
          </div>
        </div>
      </div>
    </div>
  </section>
);

const ProgramCards: React.FC = () => (
  <section className="py-16 w-full px-6 sm:px-10 lg:px-20">
    <div className="w-full px-8 sm:px-12 lg:px-20">
      <div>
        <img
          src={cardimg}
          alt="cardimg"
          className="text-center space-y-4 w-10 h-10 mx-auto"
        />
      </div>
      <div className="relative flex justify-center items-center mb-10">
        <img
          src="/src/assets/img/Vector%205-2.png"
          alt="bg"
          className="w-1/3"
        />

        <h3
          className="absolute text-center text-2xl md:text-3xl font-serif"
          style={{ color: "#749BC2" }}
        >
          High quality <br></br>training program
        </h3>
      </div>
      <div>
        <div>
          <img
            src={Group25}
            alt="IELTS Program"
            className="w-full h-100 object-cover rounded-2xl mb-4"
          />
        </div>
        <div style={{ display: "flex", gap: "2rem", marginTop: "2rem" }}>
          <img
            src={Group26}
            alt="Kid & Teenager Program"
            className="w-full  object-cover rounded-2xl mb-4"
            style={{ height: "32rem" }}
          />
          <img
            src={Group27}
            alt="SAT Preparation Program"
            className="w-full  object-cover rounded-2xl mb-4"
            style={{ height: "32rem" }}
          />
        </div>
      </div>
    </div>
  </section>
);

const CampusCarousel: React.FC = () => {
  const [swiper, setSwiper] = useState<SwiperInstance | null>(null);

  return (
    <section className="relative w-full py-16 sm:py-20 px-6 sm:px-10 lg:px-20 bg-white">
      <div className="mx-auto max-w-7xl">
        {/* ==== Header text + button ==== */}
        <div className="mb-10 flex flex-col sm:flex-row justify-between items-start sm:items-center gap-6">
          <div className="relative">
            {/* Decorative background wave - positioned behind text */}
            <div className="absolute inset-0 -z-10">
              <svg
                className="absolute top-1/2 left-0 -translate-y-1/2 w-[500px] h-[120px]"
                viewBox="0 0 500 120"
                preserveAspectRatio="none"
              >
                <path
                  d="M 0,60 Q 125,30 250,55 T 500,60 L 500,120 L 0,120 Z"
                  fill="#DBEAFE"
                  opacity="0.6"
                />
                <path
                  d="M 20,70 Q 140,40 270,65 T 500,70 L 500,120 L 20,120 Z"
                  fill="#BFDBFE"
                  opacity="0.5"
                />
              </svg>
            </div>

            {/* Text on top of wave */}
            <div className="relative z-10 pr-16">
              <h2 className="text-3xl md:text-4xl font-serif text-[#7BA5C8] leading-tight mb-1">
                Dynamic learning environment
              </h2>
              <p className="text-2xl md:text-3xl text-[#7BA5C8] font-serif flex items-center gap-2">
                - international standard
              </p>
            </div>

            {/* Target icon positioned to the right */}
            <div className="absolute -right-4 top-1/2 -translate-y-1/2 hidden sm:block">
              <svg className="w-16 h-16" viewBox="0 0 100 100">
                <circle
                  cx="50"
                  cy="50"
                  r="45"
                  fill="none"
                  stroke="#EF4444"
                  strokeWidth="2.5"
                />
                <circle
                  cx="50"
                  cy="50"
                  r="32"
                  fill="none"
                  stroke="#EF4444"
                  strokeWidth="2.5"
                />
                <circle
                  cx="50"
                  cy="50"
                  r="19"
                  fill="none"
                  stroke="#EF4444"
                  strokeWidth="2.5"
                />
                <circle cx="50" cy="50" r="8" fill="#EF4444" />
              </svg>
            </div>
          </div>

          <a
            href="#"
            className="bg-[#E63946] text-white font-semibold px-8 py-3.5 rounded-xl shadow-lg hover:bg-[#D62828] transition-all duration-300 whitespace-nowrap"
          >
            Explore the courses
          </a>
        </div>

        {/* ==== Swiper Section ==== */}
        <div className="relative">
          {/* Decorative stars at bottom left */}
          <div className="absolute -bottom-6 left-4 z-20 hidden md:block">
            <svg
              className="w-10 h-10 text-red-500"
              viewBox="0 0 24 24"
              fill="currentColor"
            >
              <path d="M12 2L15.09 8.26L22 9.27L17 14.14L18.18 21.02L12 17.77L5.82 21.02L7 14.14L2 9.27L8.91 8.26L12 2Z" />
            </svg>
            <svg
              className="w-7 h-7 text-red-500 ml-4 -mt-2"
              viewBox="0 0 24 24"
              fill="currentColor"
            >
              <path d="M12 2L15.09 8.26L22 9.27L17 14.14L18.18 21.02L12 17.77L5.82 21.02L7 14.14L2 9.27L8.91 8.26L12 2Z" />
            </svg>
          </div>

          <Swiper
            onSwiper={setSwiper}
            loop={true}
            centeredSlides={true}
            grabCursor={true}
            slidesPerView={1.1}
            spaceBetween={20}
            breakpoints={{
              640: { slidesPerView: 1.3, spaceBetween: 24 },
              1024: { slidesPerView: 1.6, spaceBetween: 30 },
            }}
            className="!px-4 !py-6"
          >
            {campusImages.map((src, idx) => (
              <SwiperSlide key={idx}>
                {({ isActive }: SwiperSlideRenderProps) => (
                  <div className="relative">
                    <img
                      src={src}
                      alt={`campus ${idx + 1}`}
                      className={`w-full h-[420px] md:h-[480px] object-cover rounded-[28px] transition-all duration-500 ${
                        isActive
                          ? "scale-100 opacity-100"
                          : "scale-[0.90] opacity-60"
                      }`}
                    />
                  </div>
                )}
              </SwiperSlide>
            ))}
          </Swiper>

          {/* ==== Navigation buttons ==== */}
          <button
            onClick={() => swiper?.slidePrev()}
            className="absolute left-6 top-1/2 -translate-y-1/2 
             flex items-center justify-center 
             h-14 w-14 rounded-full 
             bg-[#7BA5C8]/90 hover:bg-[#6B95B8] 
             text-white shadow-xl 
             transition-transform duration-300 hover:scale-110
             z-50 pointer-events-auto"
          >
            <ChevronLeft className="h-10 w-10 scale-110 stroke-[3] leading-none drop-shadow-sm" />
          </button>

          <button
            onClick={() => swiper?.slideNext()}
            className="absolute right-6 top-1/2 -translate-y-1/2 
             flex items-center justify-center 
             h-14 w-14 rounded-full 
             bg-[#7BA5C8]/90 hover:bg-[#6B95B8] 
             text-white shadow-xl 
             transition-transform duration-300 hover:scale-110
             z-50 pointer-events-auto"
          >
            <ChevronRight className="h-10 w-10 scale-110 stroke-[3] leading-none drop-shadow-sm" />
          </button>
        </div>
      </div>
    </section>
  );
};

const Footer: React.FC = () => {
  return (
    <footer className="relative bg-gradient-to-br from-slate-50 via-blue-50 to-slate-100 overflow-hidden">
      {/* Decorative elements */}
      <div className="absolute top-0 right-0 w-96 h-96 bg-blue-200/30 rounded-full blur-3xl"></div>
      <div className="absolute bottom-0 left-0 w-80 h-80 bg-sky-200/30 rounded-full blur-3xl"></div>
      
      {/* Subtle pattern */}
      <div
        className="absolute inset-0 opacity-[0.02]"
        style={{
          backgroundImage: `radial-gradient(circle, #3B82F6 1px, transparent 1px)`,
          backgroundSize: '30px 30px',
        }}
      ></div>

      <div className="relative z-10 max-w-7xl mx-auto px-6 sm:px-10 lg:px-20">
        {/* Main Content */}
        <div className="py-16 grid lg:grid-cols-12 gap-12">
          {/* Left Column - Brand */}
          <div className="lg:col-span-4">
            <div className="flex items-center gap-3 mb-6">
              <img src={logoImage} alt="EDM" className="h-12 rounded-lg shadow-md" />
              
            </div>
            
            <p className="text-slate-600 leading-relaxed mb-8 max-w-sm">
              The leading English center specializing in IELTS and SAT exam preparation and teaching English for all levels.
            </p>

            {/* ✅ Fixed Social Links */}
            <div className="flex gap-3">
              {[
                { name: 'Facebook', icon: 'F', color: 'from-blue-500 to-blue-600' },
                { name: 'Instagram', icon: 'I', color: 'from-pink-500 to-purple-600' },
                { name: 'YouTube', icon: 'Y', color: 'from-red-500 to-red-600' },
                { name: 'TikTok', icon: 'T', color: 'from-slate-700 to-slate-900' }
              ].map((platform) => (
                <a
                  key={platform.name}
                  href="#"
                  className={`w-11 h-11 rounded-xl bg-gradient-to-br ${platform.color} flex items-center justify-center transition-all hover:scale-110 hover:shadow-lg group`}
                >
                  <span className="text-sm font-bold text-white">
                    {platform.icon}
                  </span>
                </a>
              ))}
            </div>
          </div>

          {/* Middle Columns - Links */}
          <div className="lg:col-span-5 grid sm:grid-cols-2 gap-8">
            {/* IELTS Library */}
            <div>
              <h3 className="text-sm font-bold text-slate-800 mb-4 uppercase tracking-wider flex items-center gap-2">
                <span className="w-1 h-4 bg-blue-500 rounded-full"></span>
                IELTS Library
              </h3>
              <ul className="space-y-3">
                {['Listening Test', 'Reading Test', 'Writing Test', 'Speaking Test', 'Full Mock Tests'].map((item) => (
                  <li key={item}>
                    <a href="#" className="text-slate-600 hover:text-blue-600 transition-colors text-sm inline-flex items-center gap-2 group">
                      <svg className="w-1.5 h-1.5 text-blue-400 opacity-0 group-hover:opacity-100 transition-opacity" fill="currentColor" viewBox="0 0 8 8">
                        <circle cx="4" cy="4" r="3" />
                      </svg>
                      {item}
                    </a>
                  </li>
                ))}
              </ul>
            </div>

            {/* Courses */}
            <div>
              <h3 className="text-sm font-bold text-slate-800 mb-4 uppercase tracking-wider flex items-center gap-2">
                <span className="w-1 h-4 bg-sky-500 rounded-full"></span>
                Courses
              </h3>
              <ul className="space-y-3">
                {['Foundation', 'Intermediate', 'Advanced', 'Mastery', 'Private Tutoring'].map((item) => (
                  <li key={item}>
                    <a href="#" className="text-slate-600 hover:text-blue-600 transition-colors text-sm inline-flex items-center gap-2 group">
                      <svg className="w-1.5 h-1.5 text-blue-400 opacity-0 group-hover:opacity-100 transition-opacity" fill="currentColor" viewBox="0 0 8 8">
                        <circle cx="4" cy="4" r="3" />
                      </svg>
                      {item}
                    </a>
                  </li>
                ))}
              </ul>
            </div>
          </div>

          {/* Right Column - Contact */}
          <div className="lg:col-span-3">
            <h3 className="text-sm font-bold text-slate-800 mb-4 uppercase tracking-wider flex items-center gap-2">
              <span className="w-1 h-4 bg-purple-500 rounded-full"></span>
              Contact Us
            </h3>
            <div className="space-y-4">
              <a href="mailto:eduminationielts@gmail.com" className="flex items-start gap-3 text-slate-600 hover:text-blue-600 transition-colors group">
                <div className="mt-0.5 p-2 rounded-lg bg-white shadow-sm group-hover:shadow-md group-hover:bg-blue-50 transition-all">
                  <Mail className="h-4 w-4 text-blue-500" />
                </div>
                <span className="text-sm">eduminationielts@gmail.com</span>
              </a>

              <a href="#" className="flex items-start gap-3 text-slate-600 hover:text-blue-600 transition-colors group">
                <div className="mt-0.5 p-2 rounded-lg bg-white shadow-sm group-hover:shadow-md group-hover:bg-blue-50 transition-all">
                  <MapPin className="h-4 w-4 text-blue-500" />
                </div>
                <span className="text-sm">450 Le Van Viet, Tang Nhon Phu, Thu Duc, TPHCM</span>
              </a>

              <a href="tel:0866704845" className="flex items-start gap-3 text-slate-600 hover:text-blue-600 transition-colors group">
                <div className="mt-0.5 p-2 rounded-lg bg-white shadow-sm group-hover:shadow-md group-hover:bg-blue-50 transition-all">
                  <Phone className="h-4 w-4 text-blue-500" />
                </div>
                <span className="text-sm">0866 704 845</span>
              </a>
            </div>
          </div>
        </div>

        {/* Bottom Bar */}
        <div className="border-t border-slate-200 py-8">
          <div className="flex flex-col md:flex-row justify-between items-center gap-4">
            <p className="text-slate-500 text-sm">
              © 2025 Edumination. All rights reserved.
            </p>
            
            <div className="flex gap-6 text-sm text-slate-500">
              <a href="#" className="hover:text-blue-600 transition-colors">Privacy</a>
              <a href="#" className="hover:text-blue-600 transition-colors">Terms</a>
              <a href="#" className="hover:text-blue-600 transition-colors">Cookies</a>
            </div>
          </div>
        </div>
      </div>
    </footer>
  );
};



export default function HomePage() {
  return (
    <div className="min-h-screen w-full text-slate-800">
      <Navbar />
      <Hero />
      <Stats />
      <WhyDifferent />
      <ProgramCards />
      <CampusCarousel />
      <Footer />
    </div>
  );
}
