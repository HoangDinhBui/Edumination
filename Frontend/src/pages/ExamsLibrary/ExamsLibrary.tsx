import React, { useState, useEffect } from "react";
import {
  ChevronDown,
  LayoutGrid, // Icon cho All Skills
  Headphones, // Icon cho Listening
  BookOpen, // Icon cho Reading
  Edit3, // Icon cho Writing
  Mic, // Icon cho Speaking
  Search,
  Zap, // Icon cho "tests taken"
  Mail,
  MapPin,
  Phone,
} from "lucide-react";

// --- TÁI SỬ DỤNG TỪ HOMEPAGE ---
import logoImage from "../../assets/img/Rectangle 78.png";
import macbookImage from "../../assets/img/Laptop_img.png"; // <-- Ảnh laptop của bạn

// --- DỮ LIỆU MẪU (Mock Data) ---

// 1. Dữ liệu cho các nút filter
const skillFilters = [
  { name: "All Skills", icon: LayoutGrid },
  { name: "Listening", icon: Headphones },
  { name: "Reading", icon: BookOpen },
  { name: "Writing", icon: Edit3 },
  { name: "Speaking", icon: Mic },
];

// 2. "Database" giả lập API (ĐÃ CẬP NHẬT)
const allTestData = {
  "All Skills": {
    title: "IELTS Mock Test 2025",
    items: [
      { name: "Quarter 1", taken: "951,605" },
      { name: "Quarter 2", taken: "951,605" },
      { name: "Quarter 3", taken: "951,605" },
      { name: "Quarter 4", taken: "951,605" },
    ],
  },
  "Listening": {
    title: "IELTS Listening Tests",
    items: [
      { name: "Quarter 1 Listening Practice Test 1", taken: "120,432" },
      { name: "Quarter 1 Listening Practice Test 2", taken: "110,222" },
      { name: "Quarter 2 Listening Practice Test 1", taken: "95,123" },
      { name: "Quarter 2 Listening Practice Test 2", taken: "88,456" },
    ],
  },
  "Reading": {
    title: "IELTS Reading Tests",
    items: [
      { name: "Academic Reading Test 1", taken: "205,112" },
      { name: "General Reading Test 1", taken: "190,332" },
      { name: "Academic Reading Test 2", taken: "150,987" },
      { name: "General Reading Test 2", taken: "140,123" },
    ],
  },
  "Writing": {
    title: "IELTS Writing Tasks",
    items: [], // (Để trống để test trường hợp "No tests found")
  },
  "Speaking": {
    title: "IELTS Speaking Practice",
    items: [
      { name: "Speaking Practice Part 1", taken: "50,123" },
      { name: "Speaking Practice Part 2/3", taken: "45,789" },
    ],
  },
};

// === COMPONENT DROPDOWN (TÁI SỬ DỤNG) ===
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
      <div className="absolute top-full left-1/2 -translate-x-1/2 hidden group-hover:flex gap-4 p-3 z-50">
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

// === COMPONENT NAVBAR (TÁI SỬ DỤNG) ===
const Navbar: React.FC = () => {
  return (
    <header className="sticky top-0 z-40 bg-white/80 backdrop-blur border-b border-slate-200">
      <div className="mx-auto max-w-7xl px-4 sm:px-6 lg:px-8 h-16 flex items-center justify-between">
        <div className="flex items-center gap-8">
          <a href="/" className="flex items-center gap-2">
            <img src={logoImage} className="h-7 rounded" alt="logo" />
            <span className="font-bold text-lg text-blue-600">EDM</span>
          </a>
          <nav className="hidden md:flex items-center gap-6">
            <a className="text-slate-700 hover:text-slate-900" href="/">
              Home
            </a>
            <Dropdown
              title="IELTS Exam Library"
              sections={[
                {
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
            href="/signin"
            className="text-slate-600 hover:text-slate-900 text-sm"
          >
            Sign in
          </a>
          <a
            href="/signup"
            className="text-sm font-semibold text-white bg-gradient-to-r from-emerald-400 to-sky-400 px-4 py-2 rounded-full shadow hover:opacity-95"
          >
            Sign up
          </a>
        </div>
      </div>
    </header>
  );
};

// === COMPONENT FOOTER (TÁI SỬ DỤNG) ===
const Footer: React.FC = () => (
  <footer className="border-t border-slate-200 py-12">
    <div className="mx-auto max-w-7xl px-4 sm:px-6 lg:px-8 grid md:grid-cols-3 gap-10">
      <div>
        <div className="flex items-center gap-2">
          <img src={logoImage} alt="EDM" className="h-7 rounded" />
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
            <MapPin className="h-4 w-4 mt-0.5" /> 450 Le Van Viet, Tang Nhon Phu,
            Thu Duc, TPHCM
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

// === NỘI DUNG CHÍNH CỦA TRANG NÀY (ĐÃ NÂNG CẤP) ===
const LibraryContent: React.FC = () => {
  // === 1: Cập nhật State ===
  const [activeSkill, setActiveSkill] = useState("All Skills");
  
  // State mới để giữ title và items một cách riêng biệt
  const [currentTitle, setCurrentTitle] = useState("IELTS Mock Test 2025");
  const [currentItems, setCurrentItems] = useState(allTestData["All Skills"].items);
  
  const [isLoading, setIsLoading] = useState(false);

  // === 2: Cập nhật Effect ===
  useEffect(() => {
    setIsLoading(true);

    const timer = setTimeout(() => {
      // Lấy dữ liệu mới từ "database"
      const data = (allTestData as any)[activeSkill] || { title: "Error", items: [] };
      
      setCurrentTitle(data.title);   // Cập nhật state tiêu đề
      setCurrentItems(data.items); // Cập nhật state danh sách test
      setIsLoading(false); // Tắt loading
    }, 300);

    return () => clearTimeout(timer);
  }, [activeSkill]); // <-- Chỉ chạy lại khi activeSkill thay đổi

  // === 3: Cập nhật JSX (Chỉ còn 1 layout) ===
  return (
    <main className="mx-auto max-w-7xl px-4 sm:px-6 lg:px-8 py-12">
      <h1 className="text-4xl font-bold text-center text-slate-800 mb-10">
        IELTS Test Papers Library
      </h1>

      {/* Bộ lọc Kỹ năng (Giữ nguyên) */}
      <div className="flex flex-wrap items-center justify-center gap-4">
        {skillFilters.map((filter) => (
          <button
            key={filter.name}
            onClick={() => setActiveSkill(filter.name)}
            className={`flex items-center gap-2.5 px-6 py-3 rounded-full font-medium text-sm transition-all ${
              activeSkill === filter.name
                ? "bg-blue-600 text-white shadow-lg"
                : "bg-white text-slate-600 shadow-sm hover:bg-slate-50"
            }`}
          >
            <filter.icon className="w-5 h-5" />
            <span>{filter.name}</span>
          </button>
        ))}
      </div>

      {/* Thanh Tìm kiếm & Sắp xếp (Giữ nguyên) */}
      <div className="flex flex-col md:flex-row justify-between items-center mt-8 mb-6 gap-4">
        <div className="relative w-full md:max-w-3xl">
          <input
            type="search"
            placeholder="Search..."
            className="w-full pl-10 pr-4 py-3 bg-white border border-slate-200 rounded-full shadow-sm focus:outline-none focus:ring-2 focus:ring-blue-500"
          />
          <Search className="absolute left-4 top-1/2 -translate-y-1/2 w-5 h-5 text-slate-400" />
        </div>
        <button className="flex items-center gap-2 bg-white px-5 py-3 rounded-full shadow-sm border border-slate-200 text-slate-600">
          <span>Latest</span>
          <ChevronDown className="w-4 h-4" />
        </button>
      </div>

      {/* === PHẦN NỘI DUNG ĐỘNG (Đã đơn giản hóa) === */}
      <div className="min-h-[300px]">
        {/* 1. Trạng thái Loading */}
        {isLoading && (
          <div className="text-center py-20 text-blue-600 font-medium">
            Loading...
          </div>
        )}

        {/* 2. Trạng thái không có test */}
        {!isLoading && currentItems.length === 0 && (
          <div className="text-center py-20 text-slate-500 font-medium">
            No tests found for this skill.
          </div>
        )}

        {/* 3. Hiển thị layout (BÂY GIỜ LÀ LAYOUT CHUNG) */}
        {!isLoading && currentItems.length > 0 && (
          <div className="relative bg-white rounded-3xl shadow-xl overflow-hidden p-8 md:p-12 flex flex-col md:flex-row items-center gap-8">
            <span className="absolute top-5 right-10 text-purple-400 text-4xl">...</span>
            {/* Cột trái: Ảnh Laptop (Luôn hiển thị) */}
            <div className="w-full max-w-sm md:w-1/3 flex-shrink-0">
              <img src={macbookImage} alt="IELTS Mock Test" className="w-full h-auto -ml-4" />
            </div>
            {/* Cột phải: Nội dung động */}
            <div className="flex-1 w-full">
              {/* Tiêu đề động từ state */}
              <h2 className="text-3xl font-semibold text-slate-800 mb-6">
                {currentTitle}
              </h2>
              {/* Lưới 2x2 động từ state */}
              <div className="grid grid-cols-1 sm:grid-cols-2 gap-5">
                {currentItems.map((item: any) => (
                  <div key={item.name} className="bg-slate-50/70 border border-slate-200 rounded-lg p-4 hover:shadow-lg hover:border-slate-300 cursor-pointer transition-shadow">
                    <h3 className="text-lg font-semibold text-slate-700">{item.name}</h3>
                    <div className="flex items-center gap-1.5 mt-1 text-sm text-slate-500">
                      <Zap className="w-4 h-4 text-yellow-500 fill-yellow-500" />
                      <span>{item.taken} tests taken</span>
                    </div>
                  </div>
                ))}
              </div>
              {/* Link xem thêm (tùy chọn) */}
              <div className="text-center mt-8">
                <a href="#" className="text-blue-600 font-medium flex items-center justify-center gap-1.5 hover:underline">
                  <span>View more 2 tests</span>
                  <ChevronDown className="w-4 h-4" />
                </a>
              </div>
            </div>
          </div>
        )}
      </div>
    </main>
  );
};


// --- COMPONENT TRANG CHÍNH ---

export default function TestLibraryPage() {
  return (
    // Sử dụng màu nền xám nhạt cho toàn trang
    <div className="min-h-screen bg-slate-50 text-slate-800">
      <Navbar />
      <LibraryContent />
      <Footer />
    </div>
  );
}