import React, { useState, useEffect } from "react";
import Navbar from "../../components/Navbar";
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

// Giả sử bạn đang dùng react-router-dom

import { Link, useNavigate, useLocation } from "react-router-dom"; // <-- ĐÃ THÊM useLocation



// --- TÁI SỬ DỤNG TỪ HOMEPAGE ---

// LỖI ĐÃ SỬA: Thay thế import local bằng URL placeholder

import logoImage from "../../assets/img/Rectangle 78.png";

 import macbookImage from "../../assets/img/Laptop_img.png"; // <-- Ảnh laptop của bạn



// --- DỮ LIỆU CHO CÁC NÚT FILTER ---

const skillFilters = [

  { name: "All Skills", icon: LayoutGrid },

  { name: "Listening", icon: Headphones },

  { name: "Reading", icon: BookOpen },

  { name: "Writing", icon: Edit3 },

  { name: "Speaking", icon: Mic },

];



// === COMPONENT DROPDOWN (GIỮ NGUYÊN) ===

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



// === COMPONENT NAVBAR (GIỮ NGUYÊN) ===

// const Navbar: React.FC = () => {

//   return (

//     <header className="sticky top-0 z-40 bg-white/80 backdrop-blur border-b border-slate-200">

//       <div className="mx-auto max-w-7xl px-4 sm:px-6 lg:px-8 h-16 flex items-center justify-between">

//         <div className="flex items-center gap-8">

//           <Link to="/" className="flex items-center gap-2">

//             <img src={logoImage} className="h-7 rounded" alt="logo" />

//           </Link>

//           <nav className="hidden md:flex items-center gap-6">

//             <Link className="text-slate-700 hover:text-slate-900" to="/">

//               Home

//             </Link>

//             <Dropdown

//               title="IELTS Exam Library"

//               sections={[

//                 {

//                   items: [

//                     "IELTS Listening Test",

//                     "IELTS Reading Test",

//                     "IELTS Writing Test",

//                     "IELTS Speaking Test",

//                     "IELTS Test Collection",

//                   ],

//                 },

//               ]}

//             />

//             <Dropdown

//               title="IELTS Course"

//               sections={[

//                 {

//                   header: "IELTS Foundation (0.0–5.0)",

//                   items: [

//                     "IELTS 5.5–6.0 Booster",

//                     "IELTS 6.0–7.5 Intensive",

//                     "IELTS 7.5–9.0 Mastery",

//                   ],

//                 },

//               ]}

//             />

//             <Link className="text-slate-700 hover:text-slate-900" to="/ranking">

//               Ranking

//             </Link>

//           </nav>

//         </div>

//         <div className="flex items-center gap-3">

//           <Link

//             to="/signin"

//             className="text-slate-600 hover:text-slate-900 text-sm"

//           >

//             Sign in

//           </Link>

//           <Link

//             to="/signup"

//             className="text-sm font-semibold text-white bg-gradient-to-r from-emerald-400 to-sky-400 px-4 py-2 rounded-full shadow hover:opacity-95"

//           >

//             Sign up

//           </Link>

//         </div>

//       </div>

//     </header>

//   );

// };



// === COMPONENT FOOTER (GIỮ NGUYÊN) ===

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



// === NỘI DUNG CHÍNH (ĐÃ CẬP NHẬT) ===

const LibraryContent: React.FC = () => {

  // === 1: STATE ===

  const [activeSkill, setActiveSkill] = useState("All Skills");

  const [searchTerm, setSearchTerm] = useState("");

  const [currentTitle, setCurrentTitle] = useState("Loading...");

  const [currentItems, setCurrentItems] = useState<any[]>([]);

  const [isLoading, setIsLoading] = useState(true);

  const navigate = useNavigate();

  const location = useLocation(); // <-- THÊM MỚI



  // === 2: LẤY TOKEN ===

  const TOKEN = localStorage.getItem("Token");

  

  // === 3: HÀM XỬ LÝ CLICK (ĐÃ CẬP NHẬT) ===

  const handlePaperClick = (item: any) => {

    if (!TOKEN) {

      // 1. Logic kiểm tra đăng nhập (giữ nguyên)

      navigate("/signin", {

        state: { 

          from: location.pathname, // Gửi từ trang library (linh động)

          paperState: { paperId: item.id, paperName: item.Name } 

        }

      });

    } else {

      // 2. Logic điều hướng mới dựa trên activeSkill

      let targetPage = "/answer"; // Trang mặc định



      // Dùng activeSkill (đang được lưu trong state) để quyết định route

      switch (activeSkill.toUpperCase()) {

        case "LISTENING":

          // Giả sử trang ListeningTestPage.tsx của bạn được route tại "/listening-test"

          targetPage = "/listening-test"; 

          break;

        case "READING":

          // Giả sử bạn có trang ReadingTestPage.tsx được route tại "/reading-test"

          targetPage = "/reading-test"; 

          break;

        case "WRITING":

          targetPage = "/writing-test";

          break;

        case "SPEAKING":

          targetPage = "/speaking-test";

          break;

        default:

          // Nếu là kỹ năng lạ, dùng trang mặc định

          targetPage = "/answer"; 

      }



      // 3. Điều hướng đến trang đích

      navigate(targetPage, { 

        state: { paperId: item.Id, paperName: item.Name } 

      });

    }

  };



  // === 4: EFFECT GỌI API (GIỮ NGUYÊN) ===

  useEffect(() => {

    setIsLoading(true);

    const controller = new AbortController();

    const signal = controller.signal;



    const debounceTimer = setTimeout(() => {

      const params = new URLSearchParams();

      params.append("skill", activeSkill.toUpperCase());

      params.append("status", "PUBLISHED");

      params.append("sort", "latest");



      if (searchTerm) {

        params.append("search", searchTerm);

      }

      

      const API_URL = `http://localhost:8081/api/v1/papers?${params.toString()}`;



      const headers: HeadersInit = {

          "Content-Type": "application/json",

      };



      if (TOKEN) {

        headers["Authorization"] = `Bearer ${TOKEN}`;

      }



      fetch(API_URL, {

        signal,

        headers: headers,

      })

        .then((res) => {

          if (res.status === 401) {

            localStorage.removeItem("Token"); 

            navigate("/signin"); 

            throw new Error("Unauthorized");

          }

          if (!res.ok) {

            throw new Error(`HTTP error! status: ${res.status}`);

          }

          return res.json();

        })

        .then((data) => {

          setCurrentTitle(data.Title || "Test Library"); 

          setCurrentItems(data.Items || []);

        })

        .catch((err) => {

          if (err.name === "AbortError" || err.message === "Unauthorized") {

            console.log("Fetch aborted or user unauthorized");

          } else {

            console.error("Failed to fetch papers:", err);

            setCurrentTitle("Error loading tests");

            setCurrentItems([]);

          }

        })

        .finally(() => {

          setIsLoading(false);

        });

    }, 300); // 300ms debounce



    return () => {

      clearTimeout(debounceTimer);

      controller.abort();

    };

  }, [activeSkill, searchTerm, TOKEN, navigate]); // Bỏ location khỏi dependencies



  // === 5: JSX (GIỮ NGUYÊN) ===

  // Logic render item của bạn đã gọi handlePaperClick nên không cần sửa

  return (

    <main className="mx-auto max-w-7xl px-4 sm:px-6 lg:px-8 py-12 mt-10">

      <h1 className="text-4xl font-bold text-center text-slate-800 mb-10">

        IELTS Test Papers Library

      </h1>



      {/* Bộ lọc Kỹ năng */}

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



      {/* Thanh Tìm kiếm & Sắp xếp */}

      <div className="flex flex-col md:flex-row justify-between items-center mt-8 mb-6 gap-4">

        <div className="relative w-full md:max-w-3xl">

          <input

            type="search"

            placeholder="Search..."

            value={searchTerm}

            onChange={(e) => setSearchTerm(e.target.value)}

            className="w-full pl-10 pr-4 py-3 bg-white border border-slate-200 rounded-full shadow-sm focus:outline-none focus:ring-2 focus:ring-blue-500"

          />

          <Search className="absolute left-4 top-1/2 -translate-y-1/2 w-5 h-5 text-slate-400" />

        </div>

        <button className="flex items-center gap-2 bg-white px-5 py-3 rounded-full shadow-sm border border-slate-200 text-slate-600">

          <span>Latest</span>

          <ChevronDown className="w-4 h-4" />

        </button>

      </div>



      {/* === PHẦN NỘI DUNG ĐỘNG === */}

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

            No tests found.

          </div>

        )}



        {/* 3. Hiển thị layout */}

        {!isLoading && currentItems.length > 0 && (

          <div className="relative bg-white rounded-3xl shadow-xl overflow-hidden p-8 md:p-12 flex flex-col md:flex-row items-center gap-8">

            <span className="absolute top-5 right-10 text-purple-400 text-4xl">

              ...

            </span>

            <div className="w-full max-w-sm md:w-1/3 flex-shrink-0">

              <img

                src={macbookImage}

                alt="IELTS Mock Test"

                className="w-full h-auto -ml-4"

              />

            </div>

            <div className="flex-1 w-full">

              <h2 className="text-3xl font-semibold text-slate-800 mb-6">

                {currentTitle}

              </h2>

              <div className="grid grid-cols-1 sm:grid-cols-2 gap-5">

                {/* === CẬP NHẬT LOGIC RENDER ITEM (Giữ nguyên) === */}

                {currentItems.map((item: any) => {

                  // Nội dung của item (giữ nguyên)

                  const itemContent = (

                    <>

                      <h3 className="text-lg font-semibold text-slate-700">

                        {item.Name}

                      </h3>

                      <div className="flex items-center gap-1.5 mt-1 text-sm text-slate-500">

                        <Zap className="w-4 h-4 text-yellow-500 fill-yellow-500" />

                        <span>{item.Taken.toLocaleString()} tests taken</span>

                      </div>

                    </>

                  );



                  // Class chung cho item

                  const commonClasses =

                    "block bg-slate-50/70 border border-slate-200 rounded-lg p-4 hover:shadow-lg hover:border-slate-300 cursor-pointer transition-shadow";



                  // Xử lý điều hướng dựa trên activeSkill

                  if (activeSkill === "All Skills") {
                      const quarterName = item.Name.replace(/ /g, "-");
                      return (
                        <Link
                          key={item.Id || item.Name} // <-- SỬA TỪ "item.id"
                          to={`/quarter/${quarterName}`}
                          className={commonClasses}
                          state={{
                            quarterId: item.Id, // <-- SỬA TỪ "item.id"
                            quarterName: item.Name,
                          }}
                        >
                          {itemContent}
                        </Link>
                      );
                    } else {
                      return (
                        <div
                          key={item.Id || item.Name} // <-- SỬA TỪ "item.id"
                          onClick={() => handlePaperClick(item)}
                          className={commonClasses}
                        >
                          {itemContent}
                        </div>
                      );
                    }
                })}

              </div>

              <div className="text-center mt-8">

                <a

                  href="#"

                  className="text-blue-600 font-medium flex items-center justify-center gap-1.5 hover:underline"

                >

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







// --- COMPONENT TRANG CHÍNH (GIỮ NGUYÊN) ---

export default function TestLibraryPage() {

  return (

    <div className="min-h-screen text-slate-800">

      <Navbar />

      <LibraryContent />

      <Footer />

    </div>

  );

}