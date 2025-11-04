import React from "react";
import { useParams, Link } from "react-router-dom"; // Sửa <a> thành <Link>
import {
  // Icons chung
  ChevronDown,
  Mail,
  MapPin,
  Phone,
  // Icons cho trang này
  Star,
  Calendar,
  Zap, // Giữ icon "tests taken"
  Headphones, // Listening
  BookOpen, // Reading
  Edit3, // Writing
  Mic, // Speaking
  Key, // Icon chì khóa
  LayoutGrid, // Icon Full Test
  Play, // Icon Start
  ChevronLeft, // Icon Back
} from "lucide-react";

// --- TÁI SỬ DỤNG ẢNH TỪ CÁC TRANG KHÁC ---
import logoImage from "../../assets/img/Rectangle 78.png";
// --- ẢNH MỚI CHO TRANG NÀY ---
import ipadImage from "../../assets/img/IpadImg.png"; // <-- (Nhớ thay tên ảnh)

// === COMPONENT DROPDOWN (TÁI SỬ DỤNG) ===
const Dropdown: React.FC<{
  title: string;
  sections: { header?: string; items: string[] }[];
}> = ({ title, sections }) => {
  /* ... (code component Dropdown y hệt) ... */
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
  /* ... (code component Navbar y hệt) ... */
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
const Footer: React.FC = () => {
  /* ... (code component Footer y hệt) ... */
  return (
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
        </div>
        <div>
          <div className="font-semibold text-slate-800 mb-3">
            IELTS Exam Library
          </div>
          <ul className="space-y-2 text-sm text-slate-600">
            <li className="hover:text-slate-900">IELTS Listening Test</li>
            <li className="hover:text-slate-900">IELTS Reading Test</li>
          </ul>
        </div>
        <div>
          <div className="font-semibold text-slate-800 mb-3">IELTS Courses</div>
          <ul className="space-y-2 text-sm text-slate-600">
            <li className="hover:text-slate-900">IELTS Foundation (0.0–5.0)</li>
            <li className="hover:text-slate-900">IELTS 5.5–6.0 Booster</li>
          </ul>
        </div>
      </div>
    </footer>
  );
};

// === COMPONENT MỚI: SkillCard (ĐÃ SỬA LỖI) ===
interface SkillCardProps {
  title: string;
  icon: React.ElementType;
  // Truyền đầy đủ các class Tailwind
  colorClasses: {
    text: string;
    border: string;
    bg: string;
    shadow: string;
  };
}

const SkillCard: React.FC<SkillCardProps> = ({
  title,
  icon: Icon,
  colorClasses,
}) => {
  // Giờ đây chúng ta sử dụng trực tiếp các class đầy đủ
  return (
    <div
      className={`flex flex-col items-center justify-between p-6 bg-white rounded-3xl border-2 ${colorClasses.border} shadow-lg ${colorClasses.shadow} min-h-[220px]`}
    >
      {/* 1. Tiêu đề (Icon + Text) */}
      <div className={`flex flex-col items-center gap-2 ${colorClasses.text}`}>
        <Icon className="w-7 h-7" />
        <span className="text-lg font-semibold">{title}</span>
      </div>

      {/* 2. Nút "Take Test" */}
      <button
        className={`w-full py-3 rounded-lg text-white font-semibold shadow-md ${colorClasses.bg} hover:opacity-90 transition-opacity`}
      >
        Take Test
      </button>

      {/* 3. Icon Chìa khóa */}
      <Key className="w-6 h-6 text-slate-400" />
    </div>
  );
};

// === NỘI DUNG CHÍNH CỦA TRANG NÀY ===
const QuarterDetailContent: React.FC<{ quarterName: string }> = ({
  quarterName,
}) => {
  // Cập nhật mảng này để chứa các class đầy đủ
  const skills = [
    {
      title: "Listening",
      icon: Headphones,
      colors: {
        text: "text-blue-500",
        border: "border-blue-500",
        bg: "bg-blue-500",
        shadow: "shadow-blue-500/30",
      },
    },
    {
      title: "Reading",
      icon: BookOpen,
      colors: {
        text: "text-green-500",
        border: "border-green-500",
        bg: "bg-green-500",
        shadow: "shadow-green-500/30",
      },
    },
    {
      title: "Writing",
      icon: Edit3,
      colors: {
        text: "text-orange-500",
        border: "border-orange-500",
        bg: "bg-orange-500",
        shadow: "shadow-orange-500/30",
      },
    },
    {
      title: "Speaking",
      icon: Mic,
      colors: {
        text: "text-red-500",
        border: "border-red-500",
        bg: "bg-red-500",
        shadow: "shadow-red-500/30",
      },
    },
  ];

  return (
    <main className="mx-auto max-w-7xl px-4 sm:px-6 lg:px-8 py-12">
      {/* Nút quay lại */}
      <Link
        to="/library" // (Đã sửa thành <Link> và "to")
        className="inline-flex items-center gap-2 text-slate-600 hover:text-blue-600 mb-6"
      >
        <ChevronLeft className="w-5 h-5" />
        <span className="font-medium">Back to IELTS Library</span>
      </Link>

      {/* Header: Ảnh + Thông tin */}
      <div className="flex flex-col md:flex-row items-center gap-8 md:gap-12">
        {/* Cột trái: Ảnh iPad */}
        <div className="w-full max-w-xs md:w-1/3 flex-shrink-0">
          <img
            src={ipadImage}
            alt="IELTS Mock Test"
            className="w-full h-auto"
          />
        </div>

        {/* Cột phải: Thông tin Test */}
        <div className="flex-1">
          <h1 className="text-3xl md:text-4xl font-bold text-slate-800">
            IELTS Mock Test 2025 {quarterName}
          </h1>
          {/* Rating */}
          <div className="flex items-center mt-3">
            <span className="text-xl font-bold text-slate-700 mr-2">5.0</span>
            {[...Array(5)].map((_, i) => (
              <Star
                key={i}
                className="w-6 h-6 text-yellow-500 fill-yellow-500"
              />
            ))}
          </div>
          {/* Stats */}
          <div className="mt-4 space-y-3 text-lg text-slate-700">
            <div className="flex items-center gap-3">
              <Calendar className="w-6 h-6 text-slate-500" />
              <span className="font-medium">
                Published on: <strong>03 Sep 2025</strong>
              </span>
            </div>
            <div className="flex items-center gap-3">
              <Zap className="w-6 h-6 text-slate-500" />
              <span className="font-medium">
                Tests Taken: <strong>951,605</strong>
              </span>
            </div>
          </div>
        </div>
      </div>

      {/* Thẻ (Card) chính chứa các kỹ năng */}
      <div className="mt-12 bg-white rounded-3xl shadow-xl p-8">
        <h2 className="text-2xl font-semibold text-slate-800 mb-6">
          Practice test 1
        </h2>

        {/* Lưới 4 cột cho các kỹ năng (ĐÃ CẬP NHẬT) */}
        <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-6">
          {skills.map((skill) => (
            <SkillCard
              key={skill.title}
              title={skill.title}
              icon={skill.icon}
              colorClasses={skill.colors} // <-- Truyền object chứa các class
            />
          ))}
        </div>

        {/* Thanh "Full Test" */}
        <div className="mt-8 p-4 bg-gradient-to-r from-blue-100 via-green-100 to-orange-100 rounded-full flex items-center justify-between shadow-inner">
          <div className="flex items-center gap-3 ml-4">
            <LayoutGrid className="w-6 h-6 text-slate-600" />
            <span className="text-lg font-semibold text-slate-700">
              Full Test
            </span>
          </div>
          <button className="flex items-center gap-2 px-6 py-3 bg-blue-600 text-white font-semibold rounded-full shadow-lg hover:bg-blue-700 transition-colors">
            <Play className="w-5 h-5 fill-white" />
            <span>Start</span>
          </button>
        </div>
      </div>
    </main>
  );
};

// --- COMPONENT TRANG CHÍNH ---
export default function QuarterDetailPage() {
  // Lấy 'quarterName' từ URL
  const { quarterName } = useParams(); // vd: "Quarter-1"

  // Chuyển đổi nó về định dạng đẹp (vd: "Quarter 1")
  const formattedName = quarterName
    ? quarterName.replace(/-/g, " ")
    : "Quarter 1"; // Tên mặc định

  return (
    <div className="min-h-screen bg-slate-50 text-slate-800">
      <Navbar />
      <QuarterDetailContent quarterName={formattedName} />
      <Footer />
    </div>
  );
}

