import React, { useState, useEffect, useRef } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import {
  Clock,
  FileText,
  Menu,
  Loader2,
  LogOut,
  AlertTriangle,
  Save,
} from "lucide-react";
import edmLogo from "../../assets/img/edm-logo.png";

// Import Google Font
const fontLink = document.createElement("link");
fontLink.href =
  "https://fonts.googleapis.com/css2?family=Be+Vietnam+Pro:wght@300;400;500;600;700&display=swap";
fontLink.rel = "stylesheet";
document.head.appendChild(fontLink);

// =================== COUNTDOWN HOOK (ĐÃ SỬA: Null Safety) ===================
function useCountdown(initialSeconds) {
  // Khởi tạo với null nếu chưa có thời gian
  const [timeLeft, setTimeLeft] = useState(initialSeconds);

  useEffect(() => {
    if (initialSeconds !== null) {
      setTimeLeft(initialSeconds);
    }
  }, [initialSeconds]);

  useEffect(() => {
    // Nếu null hoặc <= 0 thì không chạy
    if (initialSeconds === null || timeLeft === null || timeLeft <= 0) return;
    
    const timer = setInterval(() => {
      setTimeLeft((prev) => Math.max(prev - 1, 0));
    }, 1000);
    return () => clearInterval(timer);
  }, [timeLeft, initialSeconds]);

  const safeTime = timeLeft || 0;
  const mins = Math.floor(safeTime / 60);
  const secs = safeTime % 60;
  const isWarning = mins < 5;

  return { mins, secs, isWarning, timeLeft };
}

// =================== NAVBAR (ĐÃ SỬA: Fix Auto Submit) ===================
const TopNavbar = ({
  paperName,
  timeProps,
  onMiniMenuClick,
  onSubmitClick,
  isSubmitting,
  isLoading, // Nhận thêm isLoading
}) => {
  const { mins, secs, isWarning, timeLeft } = timeProps;
  const [volume, setVolume] = useState(70);
  const audioRef = useRef(null);

  useEffect(() => {
    // CHỈ NỘP BÀI KHI ĐÃ LOAD XONG VÀ THỰC SỰ HẾT GIỜ
    if (!isLoading && timeLeft !== null && timeLeft === 0) {
      alert("Time's up! Your test will be submitted automatically.");
      onSubmitClick();
    }
  }, [timeLeft, onSubmitClick, isLoading]);

  // Cập nhật âm lượng
  useEffect(() => {
    if (audioRef.current) {
      audioRef.current.volume = volume / 100;
    }
  }, [volume]);

  // Callback để nhận ref từ MaterialViewer
  const setAudioRef = (el) => {
    audioRef.current = el;
    if (el) el.volume = volume / 100;
  };

  return (
    <nav className="w-full bg-white shadow-sm sticky top-0 z-50 border-b border-slate-200">
      <div className="grid grid-cols-3 items-center px-6 py-3 h-16">
        <div className="flex items-center gap-4">
          <button
            onClick={onMiniMenuClick}
            className="p-2 rounded-full hover:bg-slate-100 text-slate-600 mr-2 lg:hidden"
          >
            <Menu className="w-5 h-5" />
          </button>
          <img src={edmLogo} alt="Edumination Logo" className="h-6" />
          <span className="text-slate-700 font-medium text-sm hidden lg:block">
            {paperName || "Loading..."}
          </span>
        </div>

        <div className="flex justify-center items-center">
          <div
            className={`flex items-center gap-2 ${
              isWarning ? "text-red-600" : "text-[#2986B7]"
            } font-medium transition-colors`}
          >
            <Clock className="w-5 h-5" />
            <span className="text-2xl font-semibold">
              {mins.toString().padStart(2, "0")}:
              {secs.toString().padStart(2, "0")}
            </span>
          </div>
        </div>

        <div className="flex justify-end items-center gap-4">
          <input
            type="range"
            min="0"
            max="100"
            value={volume}
            onChange={(e) => setVolume(Number(e.target.value))}
            className="w-24 h-1 accent-[#2986B7]"
          />
          <button
            onClick={onSubmitClick}
            disabled={isSubmitting}
            className="bg-[#2986B7] text-white px-5 py-1.5 rounded-full flex items-center gap-2 hover:bg-blue-700 transition disabled:bg-slate-400 w-28 justify-center"
          >
            {isSubmitting ? (
              <Loader2 className="w-4 h-4 animate-spin" />
            ) : (
              "Submit"
            )}
          </button>
        </div>
      </div>
      {/* Pass callback xuống component con (nếu component con nằm ở đây, 
          nhưng thực tế MaterialViewer nằm ở body, ta sẽ dùng context hoặc ref forwarding.
          Tạm thời để đơn giản: MaterialViewer tự quản lý audio, 
          hoặc ta pass ref từ cha xuống con trong Main Component) */}
    </nav>
  );
};

// =================== MATERIAL VIEWER (GIỮ NGUYÊN) ===================
const MaterialViewer = ({ paperData, audioRefCallback }) => {
  if (!paperData) return null;

  const firstSection = paperData?.Sections?.[0];
  const { AudioAssetId } = firstSection || {};
  const { PdfAssetId } = paperData;

  const API_BASE_URL = "http://localhost:8081";
  const audioUrl = AudioAssetId
    ? `${API_BASE_URL}/api/v1/assets/download/${AudioAssetId}`
    : null;
  const pdfUrl = PdfAssetId
    ? `${API_BASE_URL}/api/v1/assets/download/${PdfAssetId}`
    : null;

  return (
    <div className="h-full bg-[#F2F8FC] flex flex-col gap-4 p-4">
      {/* 1. Audio Player */}
      {audioUrl && (
        <div className="p-4 bg-white rounded-lg shadow-sm">
          <audio
            ref={audioRefCallback}
            controls
            className="w-full"
            src={audioUrl}
          >
            Your browser does not support the audio element.
          </audio>
        </div>
      )}

      {/* 2. PDF Viewer */}
      {pdfUrl && (
        <div className="flex-1 w-full h-full bg-white rounded-lg shadow-sm overflow-hidden">
          <iframe
            src={pdfUrl}
            className="w-full h-full border-0"
            title="Test Questions"
          />
        </div>
      )}

      {!audioUrl && !pdfUrl && (
        <div className="flex flex-col items-center justify-center h-full text-slate-500">
          <FileText className="w-16 h-16 text-[#7AAEDB] mb-4" />
          <p>Không tìm thấy tài liệu (Audio/PDF).</p>
        </div>
      )}
    </div>
  );
};

// =================== RENDER QUESTION (ĐÃ SỬA: Fix ID & Key) ===================
/* eslint-disable @typescript-eslint/no-explicit-any */
const QuestionRenderer = ({ question, answer, onAnswerChange }: any) => {
  // Chuẩn hóa input
  const Id = question.Id || question.id;
  const Qtype = (question.Qtype || question.qtype || "").toUpperCase();
  const Choices = question.Choices || question.choices || [];
  const Position = question.Position || question.position;

  let inputComponent;

  switch (Qtype) {
    case "FILL_BLANK":
      inputComponent = (
        <input
          type="text"
          value={answer || ""}
          onChange={(e) => onAnswerChange(Id, e.target.value)}
          className="w-full border-b-2 border-slate-300 focus:border-[#2986B7] outline-none px-1 py-1 text-slate-700 bg-transparent"
          autoComplete="off"
        />
      );
      break;

    case "MCQ":
      inputComponent = (
        <select
          value={answer || ""}
          onChange={(e) => onAnswerChange(Id, e.target.value)}
          className="w-full border border-slate-300 rounded-lg px-3 py-2 text-slate-700 text-sm focus:ring-2 focus:ring-[#2986B7] outline-none bg-white cursor-pointer"
        >
          <option value="">Select answer</option>
          {Choices?.map((choice: any, index: number) => {
            // Lấy ID an toàn
            const cId = choice.Id ?? choice.id;
            const cContent = choice.Content || choice.content;
            return (
              <option 
                key={cId ?? index} 
                value={cId} // Quan trọng: Value là ID (số)
              >
                {cContent}
              </option>
            );
          })}
        </select>
      );
      break;

    case "MULTI_SELECT":
      const selectedAnswers = Array.isArray(answer) ? answer : [];
      const handleMultiSelectChange = (choiceId: number) => {
        let newAnswers;
        if (selectedAnswers.includes(choiceId)) {
          newAnswers = selectedAnswers.filter((a: number) => a !== choiceId);
        } else {
          newAnswers = [...selectedAnswers, choiceId];
        }
        onAnswerChange(Id, newAnswers);
      };

      inputComponent = (
        <div className="flex flex-wrap gap-4">
          {Choices?.map((choice: any, index: number) => {
            const cId = choice.Id ?? choice.id;
            const cContent = choice.Content || choice.content;
            return (
              <label
                key={cId ?? index}
                className="flex items-center gap-2 cursor-pointer p-2 rounded-md hover:bg-slate-50 border border-transparent hover:border-slate-200"
              >
                <input
                  type="checkbox"
                  className="h-4 w-4 text-[#2986B7] focus:ring-[#2986B7]"
                  checked={selectedAnswers.includes(cId)}
                  onChange={() => handleMultiSelectChange(cId)}
                />
                <span className="font-medium text-slate-700">{cContent}</span>
              </label>
            );
          })}
        </div>
      );
      break;

    default:
      inputComponent = <div className="text-red-500">Unsupported Type</div>;
  }

  return (
    <div className="flex items-center gap-3 mb-4">
      <div className="w-8 h-8 rounded-full bg-[#2986B7] text-white flex items-center justify-center text-sm font-bold flex-shrink-0">
        {Position}
      </div>
      <div className="flex-1">{inputComponent}</div>
    </div>
  );
};

// =================== MODALS (GIỮ NGUYÊN) ===================
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
          Are you sure?
        </h2>
        <p className="text-slate-600 mt-2 text-sm">
          Your progress will not be saved if you exit now.
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
            className="flex-1 px-4 py-2.5 bg-[#2986B7] text-white rounded-full font-semibold hover:bg-blue-700"
          >
            Yes, Exit
          </button>
        </div>
      </div>
    </div>
  );
};

// =================== MAIN PAGE (ĐÃ SỬA LOGIC) ===================
const ListeningTestPage = () => {
  const [dividerX, setDividerX] = useState(40); // Mặc định audio bên trái nhỏ hơn xíu
  const [isDragging, setIsDragging] = useState(false);
  const containerRef = useRef(null);
  const [activePart, setActivePart] = useState(1);
  const audioControlRef = useRef(null);

  const location = useLocation();
  const navigate = useNavigate();
  const { paperId, paperName } = location.state || {};

  const [paperData, setPaperData] = useState(null);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState(null);
  // SỬA: Khởi tạo timeLimit là null
  const [timeLimit, setTimeLimit] = useState(null); 
  const [answers, setAnswers] = useState({});

  const [attemptId, setAttemptId] = useState(null);
  const [sectionId, setSectionId] = useState(null);
  const [isSubmitting, setIsSubmitting] = useState(false);

  const [isMiniMenuOpen, setIsMiniMenuOpen] = useState(false);
  const [isExitModalOpen, setIsExitModalOpen] = useState(false);

  const timeProps = useCountdown(timeLimit);

  // === SỬA: handleAnswerChange (Giống trang Reading) ===
  const handleAnswerChange = async (questionId, answer) => {
    // 1. Update UI
    setAnswers((prev) => ({ ...prev, [questionId]: answer }));

    // 2. Validate input
    if (!answer || answer === "" || (Array.isArray(answer) && answer.length === 0)) return;
    if (!attemptId || !sectionId) return;

    // 3. Find Question Info
    // Lưu ý: Listening có thể nằm trong nhiều Passages (Part 1, 2, 3)
    // Nên phải flatMap từ Sections -> Passages -> Questions
    const question = paperData?.Sections?.[0]?.Passages
                      ?.flatMap(p => p.Questions)
                      .find(q => (q.Id || q.id) == questionId);

    if (!question) return;

    // 4. Prepare Payload
    let answerJson = {};
    const qType = (question.Qtype || "").toUpperCase();

    if (qType === "MCQ") {
       const parsedId = parseInt(answer, 10);
       if (isNaN(parsedId)) return;
       answerJson = { choice_id: parsedId };
    } else if (qType === "MULTI_SELECT") {
       // Multi select gửi mảng ID
       answerJson = { choice_ids: answer };
    } else {
       // Fill blank
       answerJson = { text_answer: answer };
    }

    const body = {
      QuestionId: parseInt(questionId, 10),
      AnswerJson: JSON.stringify(answerJson)
    };

    // 5. Call API
    const TOKEN = localStorage.getItem("Token");
    try {
        await fetch(
           `http://localhost:8081/api/v1/attempts/${attemptId}/sections/${sectionId}/answer`,
           {
             method: "POST",
             headers: {
               "Authorization": `Bearer ${TOKEN}`,
               "Content-Type": "application/json",
             },
             body: JSON.stringify(body)
           }
        );
    } catch (err) {
        console.error("Save answer error:", err);
    }
  };

  // === START ATTEMPT ===
  useEffect(() => {
    if (!paperId) {
      setError("Missing paper ID.");
      setIsLoading(false);
      return;
    }
    const TOKEN = localStorage.getItem("Token");
    if (!TOKEN) {
      navigate("/signin");
      return;
    }

    const initTest = async () => {
      try {
        // 1. Start Attempt
        const startRes = await fetch(`http://localhost:8081/api/v1/attempts`, {
          method: "POST",
          headers: {
            "Authorization": `Bearer ${TOKEN}`,
            "Content-Type": "application/json",
          },
          body: JSON.stringify({ PaperId: paperId })
        });
        
        if (!startRes.ok) throw new Error("Failed to start attempt");
        const startData = await startRes.json();

        // Tìm section LISTENING (quan trọng)
        const listeningSection = startData.Sections?.find(s => s.Skill === "LISTENING");
        if (!listeningSection) throw new Error("No Listening section found.");

        setAttemptId(startData.AttemptId);
        setSectionId(listeningSection.Id);

        // 2. Get Paper Details
        const paperRes = await fetch(`http://localhost:8081/api/v1/papers/${paperId}`, {
           headers: { "Authorization": `Bearer ${TOKEN}` }
        });
        if (!paperRes.ok) throw new Error("Failed to load paper content");
        const pData = await paperRes.json();
        
        setPaperData(pData);
        // Set time limit
        const limit = pData.Sections?.[0]?.TimeLimitSec || 3600;
        setTimeLimit(limit);

      } catch (err) {
        setError(err.message);
      } finally {
        setIsLoading(false);
      }
    };

    initTest();
  }, [paperId]);

  // === SUBMIT ===
  const handleSubmit = async () => {
    if (isSubmitting) return;
    if (!window.confirm("Submit test?")) return;

    setIsSubmitting(true);
    const TOKEN = localStorage.getItem("Token");
    try {
        const res = await fetch(
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
        if (!res.ok) throw new Error("Submit failed");
        const data = await res.json();
        alert(`Submitted! Score: ${data.RawScore}`);
        navigate("/library");
    } catch (err) {
        alert(err.message);
    } finally {
        setIsSubmitting(false);
    }
  };

  // Drag Handle
  const startDrag = (e) => { setIsDragging(true); e.preventDefault(); };
  useEffect(() => {
    const move = (e) => {
       if (!isDragging || !containerRef.current) return;
       const rect = containerRef.current.getBoundingClientRect();
       const p = ((e.clientX - rect.left) / rect.width) * 100;
       setDividerX(Math.max(30, Math.min(70, p)));
    };
    const up = () => setIsDragging(false);
    if (isDragging) {
       document.addEventListener("mousemove", move);
       document.addEventListener("mouseup", up);
       document.body.style.cursor = "col-resize";
    }
    return () => {
       document.removeEventListener("mousemove", move);
       document.removeEventListener("mouseup", up);
       document.body.style.cursor = "default";
    };
  }, [isDragging]);

  // Callbacks
  const handleSaveDraft = () => { setIsMiniMenuOpen(false); alert("Saved!"); };
  const handleExitClick = () => { setIsMiniMenuOpen(false); setIsExitModalOpen(true); };
  const handleExitConfirm = () => { setIsExitModalOpen(false); navigate("/library"); };

  if (isLoading) return <div className="h-screen flex items-center justify-center text-[#2986B7]"><Loader2 className="animate-spin w-10 h-10"/></div>;
  if (error) return <div className="h-screen flex items-center justify-center text-red-500">{error}</div>;

  return (
    <div className="w-screen h-screen flex flex-col bg-slate-50 font-['Be_Vietnam_Pro'] overflow-hidden">
      {/* Truyền audioRefCallback xuống TopNavbar nếu muốn chỉnh volume từ đó, 
          hoặc đơn giản là TopNavbar chỉ gọi setVolume, và MaterialViewer listen state volume */}
      <TopNavbar
        paperName={paperName}
        timeProps={timeProps}
        onMiniMenuClick={() => setIsMiniMenuOpen(true)}
        onSubmitClick={handleSubmit}
        isSubmitting={isSubmitting}
        isLoading={isLoading}
      />

      <div ref={containerRef} className="flex flex-1 overflow-hidden relative bg-[#F8FAFC]">
        {/* Left: Media */}
        <div className="border-r border-slate-200 overflow-y-auto" style={{ width: `${dividerX}%` }}>
          <MaterialViewer 
             paperData={paperData} 
             audioRefCallback={(el) => (audioControlRef.current = el)}
          />
        </div>

        {/* Divider */}
        <div className="relative flex items-center justify-center cursor-col-resize group w-3" onMouseDown={startDrag}>
            <div className="absolute inset-0 bg-slate-300 group-hover:bg-[#2986B7] transition-colors" style={{width: "2px", left: "5px"}} />
        </div>

        {/* Right: Questions */}
        <div className="flex-1 overflow-y-auto px-10 py-8 bg-white">
          <div className="max-w-xl">
            {paperData?.Sections?.[0]?.Passages?.map((passage) => (
              <div key={passage.Id} className="mb-8">
                <h3 className="text-[#2986B7] font-semibold text-lg mb-4">{passage.Title}</h3>
                {passage?.Questions?.map((q) => (
                   <QuestionRenderer
                      key={q.Id}
                      question={q}
                      answer={answers[q.Id]}
                      onAnswerChange={handleAnswerChange}
                   />
                ))}
              </div>
            ))}
          </div>
        </div>
      </div>

      {/* Footer Parts - Logic chuyển part (Cuộn tới anchor hoặc filter data) */}
      <footer className="fixed bottom-0 left-0 w-full bg-white border-t border-slate-300 shadow-md z-[999]">
        <div className="flex items-center h-16 px-4 gap-3">
          {[1, 2, 3, 4].map((pId) => (
             <button
               key={pId}
               onClick={() => setActivePart(pId)}
               className={`flex-1 py-3 rounded-xl border font-semibold transition-colors
                 ${activePart === pId ? "border-[#2986B7] text-[#2986B7] bg-blue-50" : "border-slate-300 text-slate-700 hover:bg-slate-100"}
               `}
             >
               Part {pId}
             </button>
          ))}
        </div>
      </footer>

      <MiniMenuPopup isOpen={isMiniMenuOpen} onClose={() => setIsMiniMenuOpen(false)} onSaveDraft={handleSaveDraft} onExitTest={handleExitClick} />
      <ExitModal isOpen={isExitModalOpen} onClose={() => setIsExitModalOpen(false)} onConfirm={handleExitConfirm} />
    </div>
  );
};

export default ListeningTestPage;