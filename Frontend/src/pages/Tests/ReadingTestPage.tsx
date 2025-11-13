import React, { useState, useEffect, useRef } from "react";
import { useLocation, useNavigate } from "react-router-dom"; // THÊM MỚI
import { Clock, FileText, Menu, FileEdit, Loader2 } from "lucide-react"; // THÊM Loader2
import edmLogo from "../../assets/img/edm-logo.png";
// import readImg from "../../assets/img/readingImg.png"; // XÓA MOCK

// Import Google Font
const fontLink = document.createElement('link');
fontLink.href = 'https://fonts.googleapis.com/css2?family=Be+Vietnam+Pro:wght@300;400;500;600;700&display=swap';
fontLink.rel = 'stylesheet';
document.head.appendChild(fontLink);

// =================== XÓA MOCK DATA ===================
// (Toàn bộ 'questionSections' đã bị xóa)

// =================== COUNTDOWN HOOK ===================
// (Giữ nguyên)
function useCountdown(initialSeconds) {
  const [timeLeft, setTimeLeft] = useState(initialSeconds);

  useEffect(() => {
    setTimeLeft(initialSeconds);
  }, [initialSeconds]);

  useEffect(() => {
    if (timeLeft <= 0) return;
    const timer = setInterval(() => {
      setTimeLeft((prev) => Math.max(prev - 1, 0));
    }, 1000);
    return () => clearInterval(timer);
  }, [timeLeft]);

  const mins = Math.floor(timeLeft / 60);
  const secs = timeLeft % 60;
  const isWarning = mins < 5;

  return { mins, secs, isWarning, timeLeft };
}

// =================== NAVBAR (ĐÃ SỬA) ===================
const TopNavbar = ({ paperName, timeProps }) => {
  const { mins, secs, isWarning, timeLeft } = timeProps; // Sửa: Dùng props

  useEffect(() => {
    if (timeLeft === 0) {
      alert("Time's up! Your test will be submitted automatically.");
    }
  }, [timeLeft]);

  return (
    <nav className="w-full bg-white shadow-sm sticky top-0 z-50 border-b border-slate-200">
      <div className="grid grid-cols-3 items-center px-6 py-3 h-16">
        <div className="flex items-center">
          <div className="flex items-center">
            <img src={edmLogo} alt="EDM" className="h-8 w-auto" />
            {/* THÊM MỚI: Tên bài test */}
            <span className="text-slate-700 font-medium text-sm hidden lg:block ml-4">
              {paperName || "Loading..."}
            </span>
          </div>
        </div>
        <div className="flex justify-center items-center">
          <div
            className={`flex items-center gap-2 ${
              isWarning ? "text-red-600" : "text-green-700"
            } font-medium transition-colors`}
          >
            <Clock className="w-5 h-5" />
            {/* Sửa: Hiển thị phút:giây */}
            <span className="text-2xl font-semibold">
              {mins.toString().padStart(2, "0")}:
              {secs.toString().padStart(2, "0")}
            </span>
          </div>
        </div>
        <div className="flex justify-end items-center gap-4">
          <button className="bg-green-600 text-white px-5 py-1.5 rounded-full flex items-center gap-2 hover:bg-green-700 transition">
            Submit
          </button>
        </div>
      </div>
    </nav>
  );
};

// =================== PDF VIEWER (KHUNG BÊN TRÁI) ===================
// (Lấy từ WritingTestPage, chỉ hiển thị PDF)
const MaterialViewer = ({ paperData }) => {
  const API_BASE_URL = "http://localhost:8081";
  
  // Đọc 'PdfAssetId' (chữ hoa) từ 'paperData'
  const pdfAssetId = paperData?.PdfAssetId; 
  
  // Tự xây dựng URL download
  const fullPdfUrl = pdfAssetId 
    ? `${API_BASE_URL}/api/v1/assets/download/${pdfAssetId}` 
    : null;

  return (
    // Xóa padding 'p-4' và 'mb-5' để PDF chiếm 100%
    <div className="h-full bg-white flex flex-col">
      {fullPdfUrl ? (
        <div className="flex-1 w-full h-full bg-white shadow-inner overflow-hidden">
          <iframe
            src={fullPdfUrl}
            className="w-full h-full border-0"
            title="Test Questions"
          />
        </div>
      ) : (
        // Fallback nếu không có PDF
        <div className="flex flex-col items-center justify-center h-full text-slate-500">
          <FileText className="w-16 h-16 text-slate-400 mb-4" />
          <p>Không tìm thấy tài liệu PDF cho bài test này.</p>
        </div>
      )}
    </div>
  );
};


// =================== RENDER QUESTION (KHUNG BÊN PHẢI) ===================
// (Tách ra thành component riêng, logic giống hệt ListeningTest)
const QuestionRenderer = ({ question, answer, onAnswerChange }) => {
  const { Id, Qtype, Stem, Choices, Position } = question;
  let inputComponent;

  switch (Qtype.toUpperCase()) {
    
    // Case 1: Chọn 1 đáp án (Dropdown)
    case "MCQ":
      inputComponent = (
        <select 
          value={answer || ""}
          onChange={(e) => onAnswerChange(Id, e.target.value)}
          className="flex-1 border border-slate-300 rounded-lg px-4 py-2.5 text-slate-700 text-sm focus:ring-2 focus:ring-green-500 focus:border-green-500 outline-none cursor-pointer bg-white hover:border-slate-400 transition-colors"
        >
          <option value=""></option>
          {Choices?.map((choice, index) => (
            <option key={index} value={choice.Content}>
              {choice.Content}
            </option>
          ))}
        </select>
      );
      return ( // Render với style cũ của bạn (số bên trái)
         <div className="flex items-center gap-3">
           <span className="text-slate-700 font-medium text-sm w-5">{Position}.</span>
           {inputComponent}
         </div>
      );

    // Case 2: Điền vào chỗ trống
    case "FILL_BLANK":
    default:
      inputComponent = (
        <input
          type="text"
          value={answer || ""}
          onChange={(e) => onAnswerChange(Id, e.target.value)}
          className="flex-1 border border-slate-300 rounded-full px-4 py-2.5 text-slate-700 text-sm outline-none focus:ring-2 focus:ring-green-500 focus:border-green-500 bg-white"
          placeholder=""
        />
      );
      return ( // Render với style cũ của bạn (số trong vòng tròn)
        <div className="flex items-center gap-3">
          <div className="w-10 h-10 rounded-full bg-green-600 text-white flex items-center justify-center font-semibold text-sm flex-shrink-0">
            {Position}
          </div>
          {inputComponent}
        </div>
      );
  }
};


// =================== MAIN COMPONENT ===================
const ReadingTestPage = () => {
  const [dividerX, setDividerX] = useState(50);
  const [isDragging, setIsDragging] = useState(false);
  const containerRef = useRef(null);

  // === THÊM STATE MỚI TỪ API ===
  const location = useLocation();
  const navigate = useNavigate();
  const { paperId, paperName } = location.state || {};

  const [paperData, setPaperData] = useState(null);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState(null);
  const [timeLimit, setTimeLimit] = useState(0);
  const [answers, setAnswers] = useState({});

  // SỬA: Gọi useCountdown ở top level
  const timeProps = useCountdown(timeLimit);
  
  const handleAnswerChange = (questionId, answer) => {
    setAnswers((prevAnswers) => ({
      ...prevAnswers,
      [questionId]: answer,
    }));
  };

  // === THÊM MỚI: useEffect ĐỂ GỌI API ===
  useEffect(() => {
    if (!paperId) {
      setError("Không tìm thấy bài test. Vui lòng quay lại trang thư viện.");
      setIsLoading(false);
      return;
    }
    const TOKEN = localStorage.getItem("Token");
    if (!TOKEN) {
      navigate("/signin", { state: { from: location } });
      return;
    }

    const fetchPaper = async () => {
      try {
        const response = await fetch(
          `http://localhost:8081/api/v1/papers/${paperId}`,
          {
            headers: {
              Authorization: `Bearer ${TOKEN}`,
              "Content-Type": "application/json",
            },
          }
        );
        if (response.status === 401) navigate("/signin");
        if (!response.ok) throw new Error(`Lỗi API: ${response.statusText}`);
        
        const data = await response.json();
        setPaperData(data);
        
        const totalTimeInSeconds = data.Sections?.[0]?.TimeLimitSec || 3600;
        setTimeLimit(totalTimeInSeconds);

      } catch (err) {
        console.error("Lỗi khi fetch:", err);
        setError((err as Error).message);
      } finally {
        setIsLoading(false);
      }
    };

    fetchPaper();
  }, [paperId, navigate, location]);


  // Kéo thanh chia (Giữ nguyên)
  const startDrag = (e) => {
    setIsDragging(true);
    e.preventDefault();
  };
  useEffect(() => {
    const handleMouseMove = (e) => {
      if (!isDragging || !containerRef.current) return;
      const container = containerRef.current as HTMLDivElement;
      const rect = container.getBoundingClientRect();
      const newPercent = ((e.clientX - rect.left) / rect.width) * 100;
      const clampedPercent = Math.max(30, Math.min(70, newPercent));
      setDividerX(clampedPercent);
    };
    const handleMouseUp = () => {
      setIsDragging(false);
    };
    if (isDragging) {
      document.addEventListener("mousemove", handleMouseMove);
      document.addEventListener("mouseup", handleMouseUp);
      document.body.style.cursor = "col-resize";
      document.body.style.userSelect = "none";
    }
    return () => {
      document.removeEventListener("mousemove", handleMouseMove);
      document.removeEventListener("mouseup", handleMouseUp);
      document.body.style.cursor = "default";
      document.body.style.userSelect = "auto";
    };
  }, [isDragging]);


  // === Footer (Giữ nguyên) ===
  const [activePart, setActivePart] = useState(1);
  const parts = [
    { id: 1, label: "Part 1" },
    { id: 2, label: "Part 2" },
    { id: 3, label: "Part 3" },
  ];


  // === THÊM MỚI: State Loading/Error ===
  if (isLoading) {
    return (
      <div className="w-screen h-screen flex flex-col items-center justify-center bg-slate-50 text-green-700">
        <Loader2 className="w-12 h-12 animate-spin mb-4" />
        <p className="font-medium text-lg">Đang tải bài test...</p>
      </div>
    );
  }
  if (error) {
    return (
      <div className="w-screen h-screen flex flex-col items-center justify-center bg-slate-50 text-red-600">
        <p className="font-medium text-lg">Lỗi: {error}</p>
        <button
          onClick={() => navigate("/library")}
          className="mt-4 px-4 py-2 bg-blue-600 text-white rounded"
        >
          Quay lại thư viện
        </button>
      </div>
    );
  }

  // === RENDER CHÍNH (ĐÃ SỬA) ===
  return (
    <div className="w-screen h-screen flex flex-col overflow-hidden bg-slate-50 font-['Be_Vietnam_Pro']">
      {/* Sửa: Truyền props cho Navbar */}
      <TopNavbar paperName={paperName} timeProps={timeProps} />

      <div
        ref={containerRef}
        className="flex flex-1 overflow-hidden h-[calc(100vh-8rem)] relative" // Sửa: Thêm h-[calc(100vh-8rem)]
      >
        {/* LEFT: Reading Passage (ĐÃ SỬA) */}
        <div
          className="border-r border-slate-200 overflow-y-auto"
          style={{ width: `${dividerX}%`, backgroundColor: "#F0FAF1" }}
        >
          {/* Sửa: Dùng MaterialViewer động */}
          <MaterialViewer paperData={paperData} />
        </div>

        {/* Divider (Giữ nguyên) */}
        <div
          className="relative flex items-center justify-center cursor-col-resize group"
          style={{ width: '12px' }}
          onMouseDown={startDrag}
        >
          <div className="absolute inset-0 bg-slate-300 group-hover:bg-blue-500 transition-colors" style={{ width: '2px', left: '5px' }} />
        </div>

        {/* RIGHT: Questions (ĐÃ SỬA) */}
        <div
          className="flex-1 overflow-y-auto p-6 pb-24"
          style={{ backgroundColor: "#FFFFFF" }}
        >
          {/* Sửa: Giới hạn chiều rộng (giống các trang khác) */}
          <div className="max-w-xl"> 
            
            {/* Sửa: Lặp qua 'paperData' thay vì 'questionSections' */}
            {paperData?.Sections?.[0]?.Passages?.map((passage) => (
              <div key={passage.Id} className="mb-8">
                {/* Header */}
                <div className="mb-4">
                  <h2 className="text-green-700 font-semibold text-l mb-3">
                    {passage.Title}
                  </h2>
                </div>

                {/* Questions */}
                <div className="space-y-3">
                  {passage?.Questions?.map((q) => (
                    <div key={q.Id}>
                      {/* Sửa: Dùng QuestionRenderer */}
                      <QuestionRenderer 
                        question={q} 
                        answer={answers[q.Id]} 
                        onAnswerChange={handleAnswerChange} 
                      />
                    </div>
                  ))}
                </div>
              </div>
            ))}
          </div>
        </div>
      </div>

      {/* FOOTER NAVIGATOR (Giữ nguyên) */}
      <footer className="fixed bottom-0 left-0 w-full bg-white border-t border-slate-300 shadow-md z-[999]">
        <div className="flex items-center h-16 px-4 gap-3">
          {parts.map((p) => (
            <button
              key={p.id}
              onClick={() => setActivePart(p.id)}
              className={`flex-1 py-3 rounded-xl border transition-all duration-300 text-base font-semibold
                ${
                  activePart === p.id
                    ? "border-green-600 text-green-700 bg-green-50 shadow-sm"
                    : "border-slate-300 text-slate-700 hover:bg-slate-100 hover:border-green-400"
                }`}
            >
              {p.label}
            </button>
          ))}
        </div>
      </footer>
    </div>
  );
};

export default ReadingTestPage;