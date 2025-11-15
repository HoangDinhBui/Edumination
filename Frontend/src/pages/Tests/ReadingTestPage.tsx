import React, { useState, useEffect, useRef } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import {
  Clock,
  FileText,
  Menu,
  FileEdit,
  Loader2,
  LogOut,
  AlertTriangle, // SỬA: Dùng icon giống trang Speaking
  Save,           // THÊM: Icon Save
} from "lucide-react";
import edmLogo from "../../assets/img/edm-logo.png";

// Import Google Font
const fontLink = document.createElement("link");
fontLink.href =
  "https://fonts.googleapis.com/css2?family=Be+Vietnam+Pro:wght@300;400;500;600;700&display=swap";
fontLink.rel = "stylesheet";
document.head.appendChild(fontLink);

// =================== COUNTDOWN HOOK ===================
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

// =================== NAVBAR (ĐÃ CẬP NHẬT) ===================
const TopNavbar = ({ 
  paperName, 
  timeProps, 
  onMiniMenuClick, 
  onSubmitClick, // Thêm prop cho submit
  isSubmitting  // Thêm prop để hiện loading
}) => {
  const { mins, secs, isWarning, timeLeft } = timeProps;

  useEffect(() => {
    if (timeLeft === 0) {
      alert("Time's up! Your test will be submitted automatically.");
      onSubmitClick(); // Tự động nộp bài khi hết giờ
    }
  }, [timeLeft, onSubmitClick]);

  return (
    <nav className="w-full bg-white shadow-sm sticky top-0 z-50 border-b border-slate-200">
      <div className="grid grid-cols-3 items-center px-6 py-3 h-16">
        <div className="flex items-center">
          <button
            onClick={onMiniMenuClick}
            className="p-2 rounded-full hover:bg-slate-100 text-slate-600 mr-2"
            aria-label="Menu"
          >
            <Menu className="w-5 h-5" />
          </button>

          <div className="flex items-center">
            <img src={edmLogo} alt="EDM" className="h-8 w-auto" />
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
            <span className="text-2xl font-semibold">
              {mins.toString().padStart(2, "0")}:
              {secs.toString().padStart(2, "0")}
            </span>
          </div>
        </div>
        
        {/* === SỬA NÚT SUBMIT === */}
        <div className="flex justify-end items-center gap-4">
          <button 
            onClick={onSubmitClick}
            disabled={isSubmitting}
            className="bg-green-600 text-white px-5 py-1.5 rounded-full flex items-center justify-center gap-2 hover:bg-green-700 transition disabled:bg-slate-400 w-28" // Thêm w-28
          >
            {isSubmitting ? (
              <Loader2 className="w-4 h-4 animate-spin" />
            ) : (
              "Submit"
            )}
          </button>
        </div>
      </div>
    </nav>
  );
};


// =================== PDF VIEWER (GIỮ NGUYÊN) ===================
const MaterialViewer = ({ paperData }) => {
  const API_BASE_URL = "http://localhost:8081";
  const pdfAssetId = paperData?.PdfAssetId;
  const fullPdfUrl = pdfAssetId
    ? `${API_BASE_URL}/api/v1/assets/download/${pdfAssetId}`
    : null;

  return (
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
        <div className="flex flex-col items-center justify-center h-full text-slate-500">
          <FileText className="w-16 h-16 text-slate-400 mb-4" />
          <p>Không tìm thấy tài liệu PDF cho bài test này.</p>
        </div>
      )}
    </div>
  );
};

// =================== RENDER QUESTION (SỬA LẠI) ===================
const QuestionRenderer = ({ question, answer, onAnswerChange }) => {
  const { Id, Qtype, Choices, Position } = question;
  let inputComponent;

  switch (Qtype.toUpperCase()) {
    case "MCQ":
      inputComponent = (
        <select
          value={answer || ""}
          onChange={(e) => onAnswerChange(Id, e.target.value)}
          className="flex-1 border border-slate-300 rounded-lg px-4 py-2.5 text-slate-700 text-sm focus:ring-2 focus:ring-green-500 focus:border-green-500 outline-none cursor-pointer bg-white hover:border-slate-400 transition-colors"
        >
          <option value=""></option>
          {/* Sửa: Dùng choice.Id cho value, vì đây là ID của lựa chọn */}
          {Choices?.map((choice) => (
            <option key={choice.Id} value={choice.Id}>
              {choice.Content}
            </option>
          ))}
        </select>
      );
      return (
        <div className="flex items-center gap-3">
          <span className="text-slate-700 font-medium text-sm w-5">
            {Position}.
          </span>
          {inputComponent}
        </div>
      );

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
      return (
        <div className="flex items-center gap-3">
          <div className="w-10 h-10 rounded-full bg-green-600 text-white flex items-center justify-center font-semibold text-sm flex-shrink-0">
            {Position}
          </div>
          {inputComponent}
        </div>
      );
  }
};

// =================== CÁC COMPONENT POPUP (GIỐNG SPEAKING) ===================
const MiniMenuPopup = ({ isOpen, onClose, onSaveDraft, onExitTest }) => {
  if (!isOpen) return null;
  return (
    <div className="fixed inset-0 z-[998]" onClick={onClose}>
      <div
        className="absolute top-16 right-6 bg-white rounded-2xl shadow-lg p-3 w-48 border border-slate-200"
        onClick={(e) => e.stopPropagation()}
      >
        <button
          onClick={onSaveDraft}
          className="flex items-center gap-3 w-full text-left px-3 py-2.5 rounded-lg font-medium text-white bg-green-500 hover:bg-green-600"
        >
          <Save className="w-5 h-5" />
          <span>Save Draft</span>
        </button>
        <button
          onClick={onExitTest}
          className="flex items-center gap-3 w-full text-left px-3 py-2.5 rounded-lg font-medium text-slate-700 hover:bg-slate-100 mt-1"
        >
          <LogOut className="w-5 h-5" />
          <span>Exit Test</span>
        </button>
      </div>
    </div>
  );
};

const ExitModal = ({ isOpen, onClose, onConfirm }) => {
  if (!isOpen) return null;
  return (
    <div className="fixed inset-0 bg-black/30 backdrop-blur-sm z-[999] flex items-center justify-center p-4">
      <div className="bg-white rounded-2xl shadow-xl p-8 max-w-sm w-full text-center">
        <div className="w-16 h-16 bg-green-100 text-green-600 rounded-full flex items-center justify-center mx-auto">
          <AlertTriangle className="w-8 h-8" strokeWidth={2.5} />
        </div>
        <h2 className="text-xl font-bold text-slate-800 mt-6">
          Are you sure to exit the test?
        </h2>
        <p className="text-slate-600 mt-2 text-sm">
          Your answers will not be saved when exist. You might need to save the
          draft first.
        </p>
        <div className="flex gap-4 mt-8">
          <button
            onClick={onClose}
            className="flex-1 px-4 py-2.5 bg-white border border-slate-300 text-slate-700 rounded-full font-semibold hover:bg-slate-50"
          >
            Cancel
          </button>
          <button
            onClick={onConfirm}
            className="flex-1 px-4 py-2.5 bg-green-600 text-white rounded-full font-semibold hover:bg-green-700"
          >
            Yes
          </button>
        </div>
      </div>
    </div>
  );
};


// =================== MAIN COMPONENT (ĐÃ CẬP NHẬT) ===================
const ReadingTestPage = () => {
  const [dividerX, setDividerX] = useState(50);
  const [isDragging, setIsDragging] = useState(false);
  const containerRef = useRef(null);
  const location = useLocation();
  const navigate = useNavigate();
  const { paperId, paperName } = location.state || {};

  const [paperData, setPaperData] = useState(null);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState(null);
  const [timeLimit, setTimeLimit] = useState(0);
  const [answers, setAnswers] = useState({});

  // === THÊM MỚI: STATE CHO SUBMIT VÀ ATTEMPT ===
  const [attemptId, setAttemptId] = useState(null);
  const [sectionId, setSectionId] = useState(null);
  const [isSubmitting, setIsSubmitting] = useState(false);
  // ===========================================

  const [isMiniMenuOpen, setIsMiniMenuOpen] = useState(false);
  const [isExitModalOpen, setIsExitModalOpen] = useState(false);

  const timeProps = useCountdown(timeLimit);

  // === SỬA: handleAnswerChange ĐỂ GỌI API NGAY ===
  // (Cách này tốt hơn là submit tất cả 1 lúc)
  const handleAnswerChange = async (questionId, answer) => {
    // 1. Cập nhật state React ngay lập tức
    setAnswers((prevAnswers) => ({
      ...prevAnswers,
      [questionId]: answer,
    }));

    if (!attemptId || !sectionId) {
        console.warn("Attempt ID/Section ID chưa sẵn sàng, chưa thể lưu");
        return;
    }

    // 2. Chuẩn bị dữ liệu cho API
    const question = paperData?.Sections?.[0]?.Passages
                      .flatMap(p => p.Questions)
                      .find(q => q.Id == questionId);
                      
    let answerJson = {};
    if (question?.Qtype.toUpperCase() === "MCQ") {
      answerJson = { choice_id: parseInt(answer, 10) };
    } else {
      answerJson = { text_answer: answer };
    }

    const body = {
      QuestionId: parseInt(questionId, 10),
      AnswerJson: JSON.stringify(answerJson)
    };
    
    // 3. Gọi API để lưu (không cần chờ)
    const TOKEN = localStorage.getItem("Token");
    fetch(
      `http://localhost:8081/api/v1/attempts/${attemptId}/sections/${sectionId}/answer`,
      {
        method: "POST",
        headers: {
          "Authorization": `Bearer ${TOKEN}`,
          "Content-Type": "application/json",
        },
        body: JSON.stringify(body)
      }
    ).then(res => {
        if(!res.ok) console.error(`Lỗi lưu câu hỏi ${questionId}`);
        else console.log(`Đã lưu câu ${questionId}`);
    }).catch(err => {
        console.error(`Lỗi API khi lưu câu ${questionId}:`, err);
    });
  };

  // === THÊM MỚI: HÀM NỘP BÀI (CHỐT) ===
  const handleSubmit = async () => {
    if (isSubmitting) return;

    const confirmSubmit = window.confirm("Bạn có chắc chắn muốn nộp bài không? Bạn sẽ không thể sửa lại.");
    if (!confirmSubmit) {
      return;
    }

    setIsSubmitting(true);
    const TOKEN = localStorage.getItem("Token");
    
    try {
      // Chỉ cần gọi API SubmitSection (vì các câu trả lời đã được lưu)
      const submitSectionResponse = await fetch(
        `http://localhost:8081/api/v1/attempts/${attemptId}/sections/${sectionId}/submit`,
        {
          method: "POST",
          headers: {
            "Authorization": `Bearer ${TOKEN}`,
            "Content-Type": "application/json",
          },
          body: JSON.stringify({ ConfirmSubmission: true })
        }
      );

      if (!submitSectionResponse.ok) {
        const errData = await submitSectionResponse.json();
        throw new Error(errData.Title || errData.Detail || "Lỗi khi chốt bài nộp.");
      }

      const result = await submitSectionResponse.json();
      console.log("Nộp bài thành công:", result);
      alert(`Nộp bài thành công! Điểm của bạn: ${result.RawScore}`);
      
      // Chuyển hướng
      navigate('/library'); 

    } catch (err) {
      console.error("Lỗi khi nộp bài:", err);
      alert(`Lỗi: ${err.message}`);
    } finally {
      setIsSubmitting(false);
    }
  };


  // === SỬA: useEffect ĐỂ BẮT ĐẦU ATTEMPT ===
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

    const startAndFetchTest = async () => {
      try {
        // BƯỚC 1: BẮT ĐẦU BÀI TEST (Lấy Attempt ID)
        const startResponse = await fetch(
          `http://localhost:8081/api/v1/attempts`,
          {
            method: "POST",
            headers: {
              "Authorization": `Bearer ${TOKEN}`,
              "Content-Type": "application/json",
            },
            body: JSON.stringify({ PaperId: paperId }) 
          }
        );
        
        if (!startResponse.ok) {
           const errData = await startResponse.json();
           throw new Error(errData.Title || errData.Detail || "Không thể bắt đầu bài test.");
        }
        
        const startData = await startResponse.json();
        
        const currentAttemptId = startData.AttemptId; 
        const readingSection = startData.Sections?.find(s => s.Skill === "READING"); 
        
        if (!readingSection || !currentAttemptId) {
          throw new Error("Không tìm thấy Section Reading hoặc AttemptId.");
        }

        // Lưu ID để dùng cho các API call khác
        setAttemptId(currentAttemptId);
        setSectionId(readingSection.Id);
        
        // BƯỚC 2: LẤY NỘI DUNG BÀI TEST
        const paperResponse = await fetch(
          `http://localhost:8081/api/v1/papers/${paperId}`,
          { headers: { "Authorization": `Bearer ${TOKEN}` } }
        );
        if (!paperResponse.ok) throw new Error(`Lỗi API: ${paperResponse.statusText}`);
        
        const paperData = await paperResponse.json();
        setPaperData(paperData);
        
        const totalTimeInSeconds = paperData.Sections?.[0]?.TimeLimitSec || 3600;
        setTimeLimit(totalTimeInSeconds);

      } catch (err) {
        console.error("Lỗi khi fetch:", err);
        setError(err.message);
      } finally {
        setIsLoading(false);
      }
    };

    startAndFetchTest();
  }, [paperId, navigate, location]);

  // Kéo thanh chia (GiỮ NGUYÊN)
  const startDrag = (e) => {
    setIsDragging(true);
    e.preventDefault();
  };
  useEffect(() => {
    const handleMouseMove = (e) => {
      if (!isDragging || !containerRef.current) return;
      const container = containerRef.current;
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

  // Footer (GiỮ NGUYÊN)
  const [activePart, setActivePart] = useState(1);
  const parts = [
    { id: 1, label: "Part 1" },
    { id: 2, label: "Part 2" },
    { id: 3, label: "Part 3" },
  ];

  // Các hàm xử lý Menu (GIỮ NGUYÊN)
  const handleSaveDraft = () => {
    alert("Draft saved! (chức năng đang phát triển)");
    setIsMiniMenuOpen(false);
  };
  const handleExitClick = () => {
    setIsMiniMenuOpen(false);
    setIsExitModalOpen(true);
  };
  const handleExitConfirm = () => {
    setIsExitModalOpen(false);
    navigate('/library');
  };

  // State Loading/Error (GIỮ NGUYÊN)
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

  // === RENDER CHÍNH (ĐÃ CẬP NHẬT) ===
  return (
    <div className="w-screen h-screen flex flex-col overflow-hidden bg-slate-50 font-['Be_Vietnam_Pro']">
      <TopNavbar
        paperName={paperName}
        timeProps={timeProps}
        onMiniMenuClick={() => setIsMiniMenuOpen(true)}
        onSubmitClick={handleSubmit} // Thêm hàm submit
        isSubmitting={isSubmitting} // Thêm state loading
      />

      <div
        ref={containerRef}
        className="flex flex-1 overflow-hidden h-[calc(100vh-8rem)] relative"
      >
        {/* LEFT: Reading Passage */}
        <div
          className="border-r border-slate-200 overflow-y-auto"
          style={{ width: `${dividerX}%`, backgroundColor: "#F0FAF1" }}
        >
          <MaterialViewer paperData={paperData} />
        </div>

        {/* Divider */}
        <div
          className="relative flex items-center justify-center cursor-col-resize group"
          style={{ width: "12px" }}
          onMouseDown={startDrag}
        >
          <div
            className="absolute inset-0 bg-slate-300 group-hover:bg-blue-500 transition-colors"
            style={{ width: "2px", left: "5px" }}
          />
        </div>

        {/* RIGHT: Questions */}
        <div
          className="flex-1 overflow-y-auto p-6 pb-24"
          style={{ backgroundColor: "#FFFFFF" }}
        >
          <div className="max-w-xl">
            {paperData?.Sections?.[0]?.Passages?.map((passage) => (
              <div key={passage.Id} className="mb-8">
                <div className="mb-4">
                  <h2 className="text-green-700 font-semibold text-l mb-3">
                    {passage.Title}
                  </h2>
                </div>
                <div className="space-y-3">
                  {passage?.Questions?.map((q) => (
                    <div key={q.Id}>
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

      {/* FOOTER NAVIGATOR */}
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

      {/* MODALS */}
      <MiniMenuPopup
        isOpen={isMiniMenuOpen}
        onClose={() => setIsMiniMenuOpen(false)}
        onSaveDraft={handleSaveDraft}
        onExitTest={handleExitClick}
      />
      <ExitModal
        isOpen={isExitModalOpen}
        onClose={() => setIsExitModalOpen(false)}
        onConfirm={handleExitConfirm}
      />
    </div>
  );
};

export default ReadingTestPage;