import React from "react";
import { useState } from 'react';
import { Swiper, SwiperSlide } from 'swiper/react';
import type { SwiperSlideRenderProps } from 'swiper/react';
import type { Swiper as SwiperInstance } from 'swiper';
import 'swiper/css';
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
import logoImage from "../../assets/img/Rectangle 78.png";
import heroBackground from "../../assets/img/Ellipse 7.png";
import bcLogo from '../../assets/img/Rectangle 94.png'; 
import cambridgeLogo from '../../assets/img/Rectangle 95.png'; 
import idpLogo from '../../assets/img/Rectangle 96.png'; 
import iconWhatMakesUsDifferent from '../../assets/img/iconWhatMakesUsDifferent.png';
import imageGroupStudy1 from '../../assets/img/imageGroupStudy1.png';
import imageGroupStudy2 from '../../assets/img/imageGroupStudy2.png';
import imageGroupStudy3 from '../../assets/img/imageGroupStudy3.png';
import "../../index.css";
import vector5 from '../../assets/img/Vector 5.png';
import vector9 from '../../assets/img/Vector 9.png';
import Group25 from '../../assets/img/Group 25.png';
import Group26 from '../../assets/img/Group 26.png';
import Group27 from '../../assets/img/Group 27.png';
import classroomImg from '../../assets/img/Rectangle 116.png';
import halloweenImg from '../../assets/img/Rectangle 117.png';
import studentsImg from '../../assets/img/Rectangle 118.png';
import theForum from '../../assets/img/theForum.jpg';
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

const stats = [
  { value: "8+", label: "Years of operation" },
  { value: "10+", label: "Facilities nationwide" },
  { value: "200+", label: "Excellent students achieving IELTS 8.0+" }, 
  { value: "100%", label: "High-quality specialized teachers" }, 
  { value: "5.000+", label: "Students reached the finish line" },
  { value: "10+", label: "Top Platinum Partners of IDP and British Council Vietnam" }, 
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

const Dropdown: React.FC<{
  title: string;
  sections: { header?: string; items: string[] }[];
}> = ({ title, sections }) => {
  return (
    <div className="relative group">
      <button className="inline-flex items-center gap-1 text-slate-700 hover:text-slate-900 font-medium">
        {title}
        <ChevronDown className="h-4 w-4" />
      </button>
      <div className="absolute top-full left-1/2 -translate-x-1/2 hidden group-hover:flex gap-4 p-3">
        {sections.map((sec, i) => (
          <div
            key={i}
            className="bg-white/95 backdrop-blur shadow-xl ring-1 ring-slate-100 rounded-2xl p-3 w-56"
          >
            {sec.header && (
              <div className="text-[13px] font-semibold text-sky-700 mb-2">
                {sec.header}
              </div>
            )}
            <ul className="space-y-2">
              {sec.items.map((it) => (
                <li
                  key={it}
                  className="text-sm text-slate-600 hover:text-slate-900 cursor-pointer"
                >
                  {it}
                </li>
              ))}
            </ul>
          </div>
        ))}
      </div>
    </div>
  );
};

const Hero: React.FC = () => {
  const { idx, next, prev } = useCarousel([0, 1, 2]);
  return (
    <section className="relative overflow-hidden">
      {/* background decorative blob */}
      <div className="mx-auto max-w-7xl px-4 sm:px-6 lg:px-8 pt-2 pb-16">
        <div className="grid lg:grid-cols-2 items-center gap-12">
          {/* Left copy */}
          <div>
            <div className="inline-flex items-center gap-2 mb-4">
              <img
                src={logoImage}
                alt="EDM"
                className="h-7 rounded"
              />
            </div>
            <h1 className="text-4xl md:text-5xl font-serif tracking-tight text-slate-800">
              EDUMINATION <br />
              <span className="text-slate-700">English Center</span>
            </h1>
            <ul className="mt-6 space-y-3">
              {badges.map((b) => (
                <li key={b} className="flex items-start gap-3 text-slate-600">
                  <CheckCircle2 className="h-5 w-5 mt-0.5 text-emerald-500" />{" "}
                  {b}
                </li>
              ))}
            </ul>
            <div className="mt-8 flex flex-col items-start gap-4">
              <a
                href="#start"
                className="inline-flex items-center justify-center rounded-full bg-gradient-to-r from-rose-300 via-pink-300 to-emerald-300 px-6 py-3 text-slate-800 font-semibold shadow hover:opacity-95"
              >
                Start Your Free Practice
              </a>
              <div className="flex flex-col items-start gap-4 opacity-80">
                <div className="text-sm text-slate-600">
                  Collaborate with leading educational institutions:
                </div>
                <div className="flex items-center gap-6">
                  {partners.map((p) => (
                    <img
                      key={p.name}
                      src={p.logo}
                      alt={p.name}
                      className="h-10" // Điều chỉnh chiều cao (ví dụ: h-4, h-5, h-6) cho vừa ý
                    />
                  ))}
                </div>
              </div>
            </div>
          </div>

          {/* Right visual */}
          <div className="relative">
            <div className="relative mx-auto w-full max-w-[460px]">
              {/* circular bg */}
              <div className="absolute inset-0 -z-10">
                <img
                  src={heroBackground}
                  alt="Hero Background Pattern"
                  className="absolute left-1/2 top-1/2 -translate-x-1/2 -translate-y-1/2 w-[420px] h-[420px] object-contain opacity-90"
                />
              </div>

              <div className="aspect-[4/5] w-full overflow-hidden rounded-[28px]  shadow-xl ring-1 ring-slate-100 flex items-center justify-center">
                {/* Placeholder avatar */}
                <img
                  src={studentImage}
                  className="h-full object-cover"
                  alt="Student highlight"
                />
              </div>

              {/* Star badge */}
              <div className="absolute -left-4 top-[65%] -translate-y-1/2 z-10">
                <div className="relative">
                  <Star className="h-20 w-20 text-rose-500 fill-rose-500 rotate-6 drop-shadow" />
                  <div className="absolute inset-0 grid place-items-center text-white text-sm font-bold">
                    9.0
                    <span className="block text-[10px] font-semibold -mt-1">
                      IELTS
                    </span>
                  </div>
                </div>
              </div>

              {/* Carousel controls */}
              <button
                onClick={prev}
                aria-label="prev"
                className="absolute left-4 top-1/2 -translate-y-1/2 grid place-items-center h-10 w-10 rounded-full bg-sky-300/70 hover:bg-sky-300 text-white shadow z-10"
              >
                <ChevronLeft className="h-5 w-5" />
              </button>
              <button
                onClick={next}
                aria-label="next"
                className="absolute right-4 top-1/2 -translate-y-1/2 grid place-items-center h-10 w-10 rounded-full bg-sky-300/70 hover:bg-sky-300 text-white shadow z-10"
              >
                <ChevronRight className="h-5 w-5" />
              </button>

              {/* Caption */}
              <div className="mt-6 text-center">
                <div className="text-sky-700 font-semibold">Bui Dinh Hoang</div>
                <div className="text-xs text-slate-500 max-w-sm mx-auto">
                  a student of University of Transport and Communications – Ho
                  Chi Minh City Campus
                </div>
              </div>

              {/* Dots */}
              <div className="mt-2 flex items-center justify-center gap-2">
                {[0, 1, 2].map((i) => (
                  <span
                    key={i}
                    className={`h-2 w-2 rounded-full ${
                      idx === i ? "bg-sky-500" : "bg-slate-300"
                    }`}
                  />
                ))}
              </div>
            </div>
          </div>
        </div>
      </div>
    </section>
  );
};

const Stats: React.FC = () => (
  <section className="relative py-14">
    <div className="mx-auto max-w-7xl px-4 sm:px-6 lg:px-8">
      <div className="grid grid-cols-2 md:grid-cols-3 gap-6">
        {stats.map((s) => (
          <div
            key={s.label}
            className="rounded-3xl bg-white shadow-sm ring-1 ring-slate-100 p-6"
          >
            <div className="text-5xl font-bold text-rose-600 font-serif">{s.value}</div>
            <div className="text-slate-500 mt-1 text-sm">{s.label}</div>
          </div>
        ))}
      </div>
    </div>
  </section>
);

const WhyDifferent: React.FC = () => (
  <section className=" py-16 sm:py-24 relative" style={{ backgroundColor: '#749BC2' }}> {/* Thay đổi màu nền và padding */}
    <div className="mx-auto max-w-7xl px-4 sm:px-6 lg:px-8">
      {/* Tiêu đề và mô tả */}
      <div className="text-center mb-16 space-y-4 max-w-3xl mx-auto">
        <img
          src={iconWhatMakesUsDifferent} // Sử dụng icon SVG
          alt="What Makes Edumination Different"
          className="mx-auto h-12 w-12 text-sky-500 mb-4"
        />
        <div className="relative w-full">
          <img
            src={vector5}
            alt="bg"
            className="w-2/3 "
            style={{margin: '0 auto'}}
          />
          <div className="absolute inset-0 flex items-center justify-center">
            <p className="text-white text-xl font-semibold text-center" style={{fontSize: '24px', lineHeight: '1.5'}}>
              What makes <span className="font-bold">EDUMINATION</span><br></br> different?
            </p>
          </div>
        </div>
        <p className="text-lg text-white leading-relaxed">
          Edumination is an educational organization specializing in IELTS and SAT test preparation,
          as well as teaching English at all levels. Edumination combines the latest technology solutions
          with optimal expertise to deliver the highest quality of instruction.
        </p>
      </div>

      {/* Grid of alternating content */}
      <div className="space-y-20"> {/* Khoảng cách giữa các hàng nội dung */}

        {/* Row 1 */}
        <div className="grid lg:grid-cols-2 gap-12 items-center">
          <div className="relative overflow-hidden rounded-3xl shadow-xl ring-1 ring-slate-100 h-[380px]"> {/* Chiều cao cố định */}
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
                <CheckCircle2 className="h-5 w-5 mt-0.5 text-blue-500 flex-shrink-0" />
                <span>Effective band improvement learning models, applying knowledge in a practical way.</span>
              </li>
              <li className="flex items-start gap-3">
                <CheckCircle2 className="h-5 w-5 mt-0.5 text-blue-500 flex-shrink-0" />
                <span>60 different optional "Lecture" sessions alongside fixed classes.</span>
              </li>
              <li className="flex items-start gap-3">
                <CheckCircle2 className="h-5 w-5 mt-0.5 text-blue-500 flex-shrink-0" />
                <span>Diverse topics, supporting direction and developing one's language skills.</span>
              </li>
              <li className="flex items-start gap-3">
                <CheckCircle2 className="h-5 w-5 mt-0.5 text-blue-500 flex-shrink-0" />
                <span>100% free and unlimited for all students</span>
              </li>
            </ul>
          </div>
        </div>

        {/* Row 2 (Ảnh bên phải) */}
        <div className="grid lg:grid-cols-2 gap-12 items-center">
          <div className="order-2 lg:order-1"> {/* Đổi thứ tự cho màn hình lớn */}
            <h3 className="text-2xl font-bold text-white mb-4">
              High-quality supplementary online course
            </h3>
            <ul className="space-y-3 text-white">
              <li className="flex items-start gap-3">
                <CheckCircle2 className="h-5 w-5 mt-0.5 text-blue-500 flex-shrink-0" />
                <span>All courses come with 100% free supplementary Online Courses.</span>
              </li>
              <li className="flex items-start gap-3">
                <CheckCircle2 className="h-5 w-5 mt-0.5 text-blue-500 flex-shrink-0" />
                <span>Academic lecture video compiled by the Edumination expert.</span>
              </li>
              <li className="flex items-start gap-3">
                <CheckCircle2 className="h-5 w-5 mt-0.5 text-blue-500 flex-shrink-0" />
                <span>Supplement knowledge, helping students practice anytime, anywhere.</span>
              </li>
            </ul>
          </div>
          <div className="relative overflow-hidden rounded-3xl shadow-xl ring-1 ring-slate-100 h-[380px] order-1 lg:order-2"> {/* Chiều cao cố định */}
            <img
              src={imageGroupStudy2} // Sử dụng ảnh đã import
              className="w-full h-full object-cover"
              alt="High-quality supplementary online course"
            />
          </div>
        </div>

        {/* Row 3 (Ảnh bên trái) */}
        <div className="grid lg:grid-cols-2 gap-12 items-center">
          <div className="relative overflow-hidden rounded-3xl shadow-xl ring-1 ring-slate-100 h-[380px]"> {/* Chiều cao cố định */}
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
                <CheckCircle2 className="h-5 w-5 mt-0.5 text-blue-500 flex-shrink-0" />
                <span>All teaching materials are specifically designed according to the Cambridge criteria.</span>
              </li>
              <li className="flex items-start gap-3">
                <CheckCircle2 className="h-5 w-5 mt-0.5 text-blue-500 flex-shrink-0" />
                <span>Combine training programs researched in depth from leading language publishers.</span>
              </li>
              <li className="flex items-start gap-3">
                <CheckCircle2 className="h-5 w-5 mt-0.5 text-blue-500 flex-shrink-0" />
                <span>Diverse topics and progressively challenging types of exercises, suitable for the students' level.</span>
              </li>
            </ul>
          </div>
        </div>

      </div>
    </div>
  </section>
);

const ProgramCards: React.FC = () => (
  <section className="py-16">
    <div className="mx-auto max-w-7xl px-4 sm:px-6 lg:px-8">
      <div className="relative flex justify-center items-center mb-10">
      <img src="/src/assets/img/Vector%205-2.png" alt="bg" className="w-1/3" />
      <h3 className="absolute text-center text-2xl md:text-3xl font-serif" style={{color:'#749BC2'}}>
        High quality <br></br>training program
      </h3>
    </div>
      <div >
        <div>
          <img src={Group25} alt="IELTS Program" className="w-full h-100 object-cover rounded-2xl mb-4" />
        </div>
        <div style = {{display: "flex"}}>
          <img src={Group26} alt="Kid & Teenager Program" className="w-full  object-cover rounded-2xl mb-4" style={{height: "32rem"}}/>
          <img src={Group27} alt="SAT Preparation Program" className="w-full  object-cover rounded-2xl mb-4" style={{height: "32rem"}} />
        </div>
      </div>
    </div>
  </section>
);

const CampusCarousel: React.FC = () => {
  const [swiper, setSwiper] = useState<SwiperInstance | null>(null);
  return (
   <section className="py-14">
      <div className="mx-auto max-w-6xl px-4 sm:px-6 lg:px-8">
        
        {/* === PHẦN TEXT VÀ BUTTON GIỐNG TRONG ẢNH === */}
        <div className="mb-8 flex flex-col sm:flex-row justify-between items-start sm:items-center">
          {/* Đặt div này làm relative để ảnh nền và text bên trong được định vị tương đối */}
          <div className="mb-4 sm:mb-0 relative"> 
            {/* Ảnh nền */}
            <img 
              src={vector9} 
              alt="Background Swish" 
              className="absolute top-1/2 left-1/2 -translate-x-1/2 -translate-y-1/2 w-[200%] h-[200%] object-contain" 
              // w-[120%] h-[120%] để ảnh to hơn chữ một chút, object-contain để giữ tỉ lệ.
              // -translate-x-1/2 -translate-y-1/2 để căn giữa ảnh với text
            />

            {/* Text đã có màu xanh và đậm hơn, nằm trên ảnh nền */}
            <h2 className="text-3xl font-bold text-sky-800 relative z-[1]"> 
              Dynamic learning environment
            </h2>
            <p className="text-2xl text-gray-700 relative z-[1]">
              - international standard 
              {/* Thêm icon mục tiêu nếu bạn có nó dưới dạng component hoặc ảnh */}
              <span className="inline-block ml-2 text-red-500">🎯</span> {/* Ví dụ: thêm icon mục tiêu */}
            </p>
          </div>
          <a
            href="#"
            className="bg-red-600 text-white font-semibold px-6 py-3 rounded-lg shadow-md hover:bg-red-700 transition-colors duration-200"
          >
            Explore the courses
          </a>
        </div>
          
        {/* === PHẦN CAROUSEL ĐÃ CẬP NHẬT === */}
        {/* Bọc Swiper và các nút trong 1 div relative */}
        <div className="relative">
          {/* 4. Sử dụng Swiper component */}
          <Swiper
            onSwiper={setSwiper} // Lấy instance của Swiper khi nó khởi tạo
            loop={true}
            centeredSlides={true}
            grabCursor={true}
            // Đây là chìa khóa: Đặt số lượng slide hiển thị
            // số lẻ (ví dụ 1.3) và centeredSlides=true sẽ tạo hiệu ứng "peek"
            slidesPerView={1.2} 
            spaceBetween={24} // Khoảng cách giữa các slide
            
            // Responsive
            breakpoints={{
              // 640px trở lên
              640: {
                slidesPerView: 1.5,
                spaceBetween: 30,
              },
              // 1024px trở lên
              1024: {
                slidesPerView: 1.7, // Bạn có thể chỉnh số này để thấy ảnh 2 bên nhiều hay ít
                spaceBetween: 40,
              },
            }}
            className="!px-2 !py-4" // Thêm padding để bóng (shadow) không bị cắt
          >
            {campusImages.map((src, idx) => (
              <SwiperSlide key={idx}>
                {/* Thêm hiệu ứng: slide nào active (ở giữa) thì rõ 100%
                  slide không active thì mờ đi một chút, giống trong ảnh
                */}
                {({ isActive }: SwiperSlideRenderProps) => (
                  <img
                    src={src}
                    className={`w-full h-[420px] object-cover rounded-3xl shadow-xl transition-opacity duration-300
                      ${isActive ? 'opacity-100' : 'opacity-60'}`
                    }
                    alt={`campus ${idx + 1}`}
                  />
                )}
              </SwiperSlide>
            ))}
          </Swiper>

          {/* 5. Giữ nguyên các nút của bạn, chỉ thay đổi onClick */}
          <button
            onClick={() => swiper?.slidePrev()} // Dùng instance của Swiper để trượt
            className="absolute left-10 top-1/2 -translate-y-1/2 z-10 grid place-items-center h-10 w-10 rounded-full bg-blue-500/80 hover:bg-blue-600 text-white shadow-md transition-colors duration-200"
          >
            <ChevronLeft className="h-5 w-5" />
          </button>
          <button
            onClick={() => swiper?.slideNext()} // Dùng instance của Swiper để trượt
            className="absolute right-10 top-1/2 -translate-y-1/2 z-10 grid place-items-center h-10 w-10 rounded-full bg-blue-500/80 hover:bg-blue-600 text-white shadow-md transition-colors duration-200"
          >
            <ChevronRight className="h-5 w-5" />
          </button>
        </div>
      </div>
    </section>
  );
};

const Footer: React.FC = () => (
  <footer className="border-t border-slate-200 py-12">
    <div className="mx-auto max-w-7xl px-4 sm:px-6 lg:px-8 grid md:grid-cols-3 gap-10">
      <div>
        <div className="flex items-center gap-2">
          <img
            src={logoImage}
            alt="EDM"
            className="h-7 rounded"
          />
          <span className="font-semibold text-slate-800">Edumination</span>
        </div>
        <p className="mt-3 text-sm text-slate-600 max-w-sm">
          The leading English center specializing in IELTS and SAT exam
          preparation and teaching English for all levels.
        </p>
        <div className="mt-4 space-y-2 text-sm text-slate-600">
          <div className="flex gap-2 items-start">
            <Mail className="h-4 w-4 mt-0.5" /> eduminationielts@gmail.com
          </div>
          <div className="flex gap-2 items-start">
            <MapPin className="h-4 w-4 mt-0.5" /> 450 Le Van Viet, Tang Nhon
            Phu, Thu Duc, TPHCM
          </div>
          <div className="flex gap-2 items-start">
            <Phone className="h-4 w-4 mt-0.5" /> 0866704845
          </div>
        </div>
        <div className="text-xs text-slate-400 mt-4">© 2025 Edumination</div>
      </div>
      <div>
        <div className="font-semibold text-slate-800 mb-3">
          IELTS Exam Library
        </div>
        <ul className="space-y-2 text-sm text-slate-600">
          <li className="hover:text-slate-900">IELTS Listening Test</li>
          <li className="hover:text-slate-900">IELTS Reading Test</li>
          <li className="hover:text-slate-900">IELTS Writing Test</li>
          <li className="hover:text-slate-900">IELTS Speaking Test</li>
          <li className="hover:text-slate-900">IELTS Test Collection</li>
        </ul>
      </div>
      <div>
        <div className="font-semibold text-slate-800 mb-3">IELTS Courses</div>
        <ul className="space-y-2 text-sm text-slate-600">
          <li className="hover:text-slate-900">IELTS Foundation (0.0–5.0)</li>
          <li className="hover:text-slate-900">IELTS 5.5–6.0 Booster</li>
          <li className="hover:text-slate-900">IELTS 6.0–7.5 Intensive</li>
          <li className="hover:text-slate-900">IELTS 7.5–9.0 Mastery</li>
        </ul>
      </div>
    </div>
  </footer>
);

const Navbar: React.FC = () => {
  return (
    <header className="sticky top-0 z-40 bg-white/80 backdrop-blur border-b border-slate-200">
      <div className="mx-auto max-w-7xl px-4 sm:px-6 lg:px-8 h-16 flex items-center justify-between">
        <div className="flex items-center gap-8">
          <a href="#" className="flex items-center gap-2">
            <img
              src={logoImage}
              className="h-7 rounded"
              alt="logo"
            />
          </a>
          <nav className="hidden md:flex items-center gap-6">
            <a className="text-slate-700 hover:text-slate-900" href="#">
              Home
            </a>
            <Dropdown
              title="IELTS Exam Library"
              sections={[
                {
                  header: "",
                  items: [
                    "IELTS Listening Test",
                    "IELTS Reading Test",
                    "IELTS Writing Test",
                    "IELTS Speaking Test",
                    "IELTS Test Collection",
                  ],
                },
              ]}
            />
            <Dropdown
              title="IELTS Course"
              sections={[
                {
                  header: "IELTS Foundation (0.0–5.0)",
                  items: [
                    "IELTS 5.5–6.0 Booster",
                    "IELTS 6.0–7.5 Intensive",
                    "IELTS 7.5–9.0 Mastery",
                  ],
                },
              ]}
            />
            <a className="text-slate-700 hover:text-slate-900" href="#">
              Ranking
            </a>
          </nav>
        </div>
        <div className="flex items-center gap-3">
          <a
            href="#signin"
            className="text-slate-600 hover:text-slate-900 text-sm"
          >
            Sign in
          </a>
          <a
            href="#signup"
            className="text-sm font-semibold text-white bg-gradient-to-r from-emerald-400 to-sky-400 px-4 py-2 rounded-full shadow hover:opacity-95"
          >
            Sign up
          </a>
        </div>
      </div>
    </header>
  );
};

export default function HomePage() {
  return (
    <div className="min-h-screen  text-slate-800">
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
