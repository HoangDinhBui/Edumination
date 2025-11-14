import React, { useState, useEffect, useRef } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import { useReactMediaRecorder } from "react-media-recorder";
import { 
    Clock, 
    FileText, 
    Menu, 
    Loader2, 
    Mic, 
    AlertTriangle, 
    Save, 
    LogOut,
    UploadCloud
} from "lucide-react";
import edmLogo from "../../assets/img/edm-logo.png";

// =================== IMPORT FONT ===================
const fontLink = document.createElement("link");
fontLink.href =
  "https://fonts.googleapis.com/css2?family=Be+Vietnam+Pro:wght@300;400;500;600;700&display=swap";
fontLink.rel = "stylesheet";
document.head.appendChild(fontLink);

// =================== COUNTDOWN HOOK ===================
function useCountdown(initialSeconds) {
  const [timeLeft, setTimeLeft] = useState(initialSeconds);
  useEffect(() => { setTimeLeft(initialSeconds); }, [initialSeconds]);
  useEffect(() => {
    if (timeLeft <= 0) return;
    const timer = setInterval(() => setTimeLeft((t) => Math.max(t - 1, 0)), 1000);
    return () => clearInterval(timer);
  }, [timeLeft]);
  const mins = Math.floor(timeLeft / 60);
  const secs = timeLeft % 60;
  const isWarning = mins < 5;
  return { mins, secs, isWarning, timeLeft };
}

// =================== NAVBAR ===================
const TopNavbar = ({ paperName, timeProps, onMiniMenuClick, onReviewClick }) => {
  const { mins, secs, isWarning, timeLeft } = timeProps;
  useEffect(() => {
    if (timeLeft === 0) alert("Time's up! Your test will be submitted automatically.");
  }, [timeLeft]);

  return (
    <nav className="w-full bg-white shadow-sm sticky top-0 z-50 border-b border-slate-200">
      <div className="grid grid-cols-3 items-center px-6 py-3 h-16">
        <div className="flex items-center">
          <img src={edmLogo} alt="Edumination Logo" className="h-6" />
           <span className="text-slate-700 font-medium text-sm hidden lg:block ml-4">
              {paperName || "Loading..."}
            </span>
        </div>
        <div className="flex justify-center items-center">
          <div className={`flex items-center gap-2 ${isWarning ? "text-red-600" : "text-[#C76378]"} font-medium transition-colors`}>
            <Clock className="w-5 h-5" />
            <span className="text-2xl font-semibold">
              {mins.toString().padStart(2, "0")}:{secs.toString().padStart(2, "0")}
            </span>
          </div>
        </div>
        <div className="flex justify-end items-center gap-4">
          <button onClick={onReviewClick} className="flex items-center gap-2 text-slate-600 px-3 py-1.5 rounded-full hover:bg-slate-100 transition">
            <FileText className="w-4 h-4" />
            <span>Review</span>
          </button>
          <button onClick={onMiniMenuClick} className="p-2 text-slate-600 cursor-pointer hover:bg-slate-100 rounded-full">
            <Menu className="w-5 h-5" />
          </button>
          <button className="bg-[#C76378] text-white px-5 py-1.5 rounded-full flex items-center gap-2 hover:bg-[#b35567] transition">
            Submit
          </button>
        </div>
      </div>
    </nav>
  );
};

// =================== MINI MENU POPUP ===================
const MiniMenuPopup = ({ isOpen, onClose, onSaveDraft, onExitTest }) => {
  if (!isOpen) return null;
  return (
    <div className="fixed inset-0 z-[998]" onClick={onClose}>
      <div 
        className="absolute top-16 right-6 bg-white rounded-2xl shadow-lg p-3 w-48 border border-slate-200"
        onClick={(e) => e.stopPropagation()}
      >
        <button onClick={onSaveDraft} className="flex items-center gap-3 w-full text-left px-3 py-2.5 rounded-lg font-medium text-white bg-green-500 hover:bg-green-600">
          <Save className="w-5 h-5" />
          <span>Save Draft</span>
        </button>
        <button onClick={onExitTest} className="flex items-center gap-3 w-full text-left px-3 py-2.5 rounded-lg font-medium text-slate-700 hover:bg-slate-100 mt-1">
          <LogOut className="w-5 h-5" />
          <span>Exit Test</span>
        </button>
      </div>
    </div>
  );
};

// =================== EXIT MODAL ===================
const ExitModal = ({ isOpen, onClose, onConfirm }) => {
  if (!isOpen) return null;
  return (
    <div className="fixed inset-0 bg-black/30 backdrop-blur-sm z-[999] flex items-center justify-center">
      <div className="bg-white rounded-2xl shadow-xl p-8 max-w-sm w-full text-center">
        <div className="w-16 h-16 bg-pink-100 text-[#C76378] rounded-full flex items-center justify-center mx-auto">
          <AlertTriangle className="w-8 h-8" strokeWidth={2.5} />
        </div>
        <h2 className="text-xl font-bold text-slate-800 mt-6">
          Are you sure to exit the test?
        </h2>
        <p className="text-slate-600 mt-2 text-sm">
          Your answers will not be saved when exist. You might need to save the draft first.
        </p>
        <div className="flex gap-4 mt-8">
          <button onClick={onClose} className="flex-1 px-4 py-2.5 bg-white border border-slate-300 text-slate-700 rounded-full font-semibold hover:bg-slate-50">
            Cancel
          </button>
          <button onClick={onConfirm} className="flex-1 px-4 py-2.5 bg-[#C76378] text-white rounded-full font-semibold hover:bg-[#b35567]">
            Yes
          </button>
        </div>
      </div>
    </div>
  );
};

// =================== VIDEO PLAYER ===================
const VideoPlayer = ({ assetId }) => {
  const videoUrl = assetId 
    ? `http://localhost:8081/api/v1/assets/download/${assetId}`
    : null;

  return (
    <div className="flex-shrink-0 w-80">
      <div className="bg-slate-100 rounded-lg overflow-hidden shadow-md">
        <div className="aspect-video bg-gradient-to-br from-slate-200 to-slate-300 flex items-center justify-center">
          {videoUrl ? (
            <video controls className="w-full h-full" src={videoUrl}>
              Your browser does not support the video tag.
            </video>
          ) : (
            <div className="text-center">
              <div className="w-20 h-20 mx-auto mb-3 bg-white rounded-full flex items-center justify-center shadow-lg">
                <div className="w-16 h-16 bg-slate-300 rounded-full"></div>
              </div>
              <p className="text-slate-500 text-xs">Video Preview</p>
            </div>
          )}
        </div>
      </div>
    </div>
  );
};

// =================== HIỂN THỊ CÂU HỎI (ĐÃ SỬA THEO PASCALCASE) ===================
const QuestionDisplay = ({ passage, currentQuestionIndex }) => {
  if (!passage) return null;

  // SỬA (PascalCase): Đọc 'Position' (chữ hoa)
  if (passage.Position === 2) { // Part 2
    // SỬA (PascalCase): Đọc 'ContentText' (chữ hoa)
    const instructions = passage.ContentText 
      ? passage.ContentText.split('\n').filter(line => line.trim() !== '') 
      : [];
    return (
      <div className="bg-slate-50 border border-slate-200 rounded-lg p-5 text-left max-w-md">
        {/* SỬA (PascalCase): Đọc 'Title' (chữ hoa) */}
        <h3 className="text-[#C76378] font-semibold text-sm mb-2">
          {passage.Title}
        </h3>
        <p className="text-slate-600 text-xs mb-2 italic">You should say:</p>
        <ul className="list-disc list-inside text-slate-700 text-xs space-y-1">
          {instructions.map((inst, idx) => (
            <li key={idx}>{inst}</li>
          ))}
        </ul>
      </div>
    );
  }

  // Part 1 & 3
  // SỬA (PascalCase): Đọc 'Questions' (chữ hoa)
  const question = passage.Questions?.[currentQuestionIndex];
  if (!question) return null; 

  return (
    <div className="bg-slate-50 border border-slate-200 rounded-lg p-4 text-left max-w-md">
      <div className="flex items-start gap-2">
        <div className="w-7 h-7 rounded-full bg-[#C76378] text-white flex items-center justify-center text-xs font-bold flex-shrink-0">
          {/* SỬA (PascalCase): Đọc 'Position' (chữ hoa) */}
          {question.Position}
        </div>
        {/* SỬA (PascalCase): Đọc 'Stem' (chữ hoa) */}
        <p className="text-slate-700 text-sm font-medium pt-1">
          {question.Stem}
        </p>
      </div>
    </div>
  );
};

// =================== MAIN SPEAKING CONTENT (ĐÃ SỬA THEO PASCALCASE) ===================
const SpeakingContent = ({ paperData, activePart, onNextPart, attemptId, sectionId }) => {
  const [recordingTime, setRecordingTime] = useState(0);
  const [currentQuestion, setCurrentQuestion] = useState(0);
  const [isSubmitting, setIsSubmitting] = useState(false);

  // SỬA (PascalCase): Đọc 'Sections', 'Passages', 'Position' (chữ hoa)
  const currentPassage = paperData?.Sections?.[0]?.Passages?.find(
    (p) => p.Position === activePart
  );

  // SỬA (PascalCase): Đọc 'AudioAssetId' (chữ hoa)
  const videoAssetId = paperData?.Sections?.[0]?.AudioAssetId;

  // Logic Upload
  const handleAudioUpload = async (blob: Blob) => {
    if (!attemptId || !sectionId) {
      console.error("Lỗi: attemptId hoặc sectionId bị thiếu!", { attemptId, sectionId });
      alert("Lỗi: Không thể nộp bài (Thiếu Attempt ID). Vui lòng tải lại trang."); 
      return;
    }
    
    console.log("Dừng ghi âm. Bắt đầu upload...");
    setIsSubmitting(true);
    const TOKEN = localStorage.getItem("Token"); // Key "Token" (T hoa) là đúng với chuẩn cũ

    const formData = new FormData();
    formData.append("AudioFile", blob, "speaking_recording.mp3");
    // SỬA (PascalCase): Đọc 'Title' (chữ hoa)
    formData.append("PromptText", currentPassage?.Title || "Speaking Prompt");
    formData.append("ConfirmSubmission", "true");
    
    try {
      const response = await fetch(
        `http://localhost:8081/api/v1/attempts/${attemptId}/sections/${sectionId}/speaking`,
        {
          method: "POST",
          headers: { "Authorization": `Bearer ${TOKEN}` },
          body: formData,
        }
      );
      
      if (!response.ok) {
        const errData = await response.json();
        // SỬA (PascalCase): Đọc lỗi 'Title' hoặc 'Detail' từ ProblemDetails của .NET
        throw new Error(errData.Title || errData.Detail || "Upload file thất bại");
      }
      
      const result = await response.json();
      console.log("Upload thành công:", result);
      alert("Đã nộp bài ghi âm thành công!");
      clearBlobUrl();
      
    } catch (err) {
      console.error("Lỗi khi upload audio:", err);
      alert(`Lỗi: Không thể nộp bài ghi âm. ${err.message}`);
    } finally {
      setIsSubmitting(false);
    }
  };

  // Logic Ghi Âm
  const { 
    status, 
    startRecording, 
    stopRecording, 
    mediaBlobUrl,
    clearBlobUrl
  } = useReactMediaRecorder({ 
    audio: true,
    onStop: (blobUrl, blob) => handleAudioUpload(blob)
  });
  
  const isRecording = status === "recording";

  useEffect(() => {
    let timer;
    if (isRecording) {
      timer = setInterval(() => setRecordingTime((t) => t + 1), 1000);
    } else {
      setRecordingTime(0);
    }
    return () => clearInterval(timer);
  }, [isRecording]);

  useEffect(() => {
    setCurrentQuestion(0);
    stopRecording(); 
  }, [activePart, stopRecording]);

  const formatTime = (seconds) => {
    const mins = Math.floor(seconds / 60);
    const secs = seconds % 60;
    return `${mins.toString().padStart(2, "0")}:${secs.toString().padStart(2, "0")}`;
  };

  const handleNextQuestion = () => {
    // SỬA (PascalCase): Đọc 'Questions' (chữ hoa)
    if (
      currentPassage?.Questions &&
      currentQuestion < currentPassage.Questions.length - 1
    ) {
      setCurrentQuestion((prev) => prev + 1);
    }
  };

  const handleNextPart = () => {
    if (activePart < 3) {
      onNextPart(activePart + 1);
    }
  };

  const handleMicClick = () => {
    if (isRecording) {
      stopRecording();
    } else {
      clearBlobUrl(); 
      startRecording();
    }
  };

  // SỬA (PascalCase): Đọc 'Questions' (chữ hoa)
  const isLastQuestion = currentPassage?.Questions
    ? currentQuestion === currentPassage.Questions.length - 1
    : false;
  const isLastPart = activePart === 3;

  if (!currentPassage) {
    return <div className="text-center p-10">Không tìm thấy dữ liệu.</div>;
  }

  return (
    <div className="flex-1 flex items-start justify-center bg-white px-6 pt-8 pb-24 overflow-y-auto">
      <div className="w-full max-w-3xl">
        <div className="w-full max-w-3xl text-center">
          {/* SỬA (PascalCase): Đọc 'Title' (chữ hoa) */}
          <h1 className="text-[#4A5568] text-xl font-bold mb-0.5">
            {currentPassage.Title} 
          </h1>
          <div className="flex items-start gap-6 mt-6">
            <VideoPlayer assetId={videoAssetId} />
            <div className="flex-1">
              <QuestionDisplay 
                passage={currentPassage} 
                currentQuestionIndex={currentQuestion} 
              />
            </div>
          </div>
          
          <div className="flex flex-col items-center gap-3 mt-8 mb-6">
            <div className="flex items-center gap-3 w-full max-w-md">
              <div className="flex-1 h-0.5 bg-slate-300 relative overflow-hidden">
                {(isRecording || isSubmitting) && (
                  <div className="absolute inset-0 bg-[#C76378] animate-pulse"></div>
                )}
              </div>
              <button
                onClick={handleMicClick}
                disabled={isSubmitting}
                className={`w-14 h-14 rounded-full flex items-center justify-center transition-all shadow-lg flex-shrink-0 ${
                  isRecording
                    ? "bg-[#C76378]"
                    : "bg-white border-2 border-[#C76378] hover:bg-[#C76378] hover:bg-opacity-10"
                } ${isSubmitting ? "bg-slate-300 border-slate-300 cursor-not-allowed" : ""}`}
              >
                {isSubmitting ? (
                  <UploadCloud className="w-7 h-7 text-slate-500 animate-pulse" />
                ) : (
                  <Mic
                    className={`w-7 h-7 ${
                      isRecording ? "text-white" : "text-[#C76378]"
                    }`}
                  />
                )}
              </button>
              <div className="flex-1 h-0.5 bg-slate-300 relative overflow-hidden">
                   {(isRecording || isSubmitting) && (
                    <div className="absolute inset-0 bg-[#C76378] animate-pulse"></div>
                  )}
              </div>
            </div>
            <div className="text-[#C76378] text-base font-semibold">
              {isSubmitting ? "Submitting..." : formatTime(recordingTime)}
            </div>
          </div>
          
          <div className="flex justify-center">
            {/* SỬA (PascalCase): Đọc 'Position' (chữ hoa) */}
            {currentPassage.Position === 2 ? ( 
              !isLastPart && (
                <button
                  onClick={handleNextPart}
                  className="bg-[#C76378] text-white px-6 py-2.5 rounded-full font-medium hover:bg-[#B35567] transition-all shadow-md text-sm"
                >
                  Next part →
                </button>
              )
            ) : (
              <>
                {!isLastQuestion ? (
                  <button
                    onClick={handleNextQuestion}
                    className="bg-[#C76378] text-white px-6 py-2.5 rounded-full font-medium hover:bg-[#B35567] transition-all shadow-md text-sm"
                  >
                    Next question →
                  </button>
                ) : (
                  !isLastPart && (
                    <button
                      onClick={handleNextPart}
                      className="bg-[#C76378] text-white px-6 py-2.5 rounded-full font-medium hover:bg-[#B35567] transition-all shadow-md text-sm"
                    >
                      Next part →
                    </button>
                  )
                )}
              </>
            )}
          </div>
        </div>
      </div>
    </div>
  );
};

// =================== MAIN PAGE (ĐÃ SỬA THEO PASCALCASE) ===================
const SpeakingTestPage = () => {
  const [activePart, setActivePart] = useState(1);
  const location = useLocation();
  const navigate = useNavigate();
  const { paperId, paperName } = location.state || {};
  const [paperData, setPaperData] = useState(null);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState(null);
  const [timeLimit, setTimeLimit] = useState(0);
  
  const [attemptId, setAttemptId] = useState<number | null>(null);
  const [sectionId, setSectionId] = useState<number | null>(null);
  
  const timeProps = useCountdown(timeLimit);
  
  const [isExitModalOpen, setIsExitModalOpen] = useState(false);
  const [isMiniMenuOpen, setIsMiniMenuOpen] = useState(false);
  
  // === SỬA LỖI: Đọc PascalCase TỪ API ===
  useEffect(() => {
    if (!paperId) {
      setError("Không tìm thấy bài test. Vui lòng quay lại trang thư viện.");
      setIsLoading(false);
      return;
    }
    // Key "Token" (T hoa) là đúng với chuẩn cũ
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
            body: JSON.stringify({ PaperId: paperId }) // SỬA (PascalCase): paperId -> PaperId
          }
        );
        if (!startResponse.ok) {
           const errData = await startResponse.json();
           // SỬA (PascalCase): Đọc lỗi 'Title' hoặc 'Detail' từ ProblemDetails của .NET
           throw new Error(errData.Title || errData.Detail || "Không thể bắt đầu bài test.");
        }
        
        const startData = await startResponse.json();
        
        // SỬA (PascalCase): Đọc 'AttemptId' và 'Sections' (chữ hoa)
        const currentAttemptId = startData.AttemptId; 
        const speakingSection = startData.Sections?.find(s => s.Skill === "SPEAKING"); // SỬA (PascalCase): s.Skill
        
        if (!speakingSection || !currentAttemptId) {
          throw new Error("Không tìm thấy Section Speaking hoặc AttemptId.");
        }

        // SỬA (PascalCase): Đọc 'Id' (chữ hoa)
        setAttemptId(currentAttemptId);
        setSectionId(speakingSection.Id);
        
        // BƯỚC 2: LẤY NỘI DUNG BÀI TEST
        const paperResponse = await fetch(
          `http://localhost:8081/api/v1/papers/${paperId}`,
          { headers: { "Authorization": `Bearer ${TOKEN}` } }
        );
        if (!paperResponse.ok) throw new Error(`Lỗi API: ${paperResponse.statusText}`);
        
        const paperData = await paperResponse.json();
        setPaperData(paperData);
        
        // SỬA (PascalCase): Đọc 'Sections' và 'TimeLimitSec' (chữ hoa)
        const totalTimeInSeconds = paperData.Sections?.[0]?.TimeLimitSec || (15 * 60);
        setTimeLimit(totalTimeInSeconds);

      } catch (err) {
        console.error("Lỗi khi fetch:", err);
        setError((err as Error).message);
      } finally {
        setIsLoading(false);
      }
    };

    startAndFetchTest();
  }, [paperId, navigate, location]);

  
  // HÀM CHO MODAL
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

  // State Loading/Error
  if (isLoading) {
    return (
      <div className="w-screen h-screen flex flex-col items-center justify-center bg-slate-50 text-[#C76378]">
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

  // Lấy các part (passages) từ API
  // SỬA (PascalCase): Đọc 'Sections', 'Passages' (chữ hoa)
  const parts = paperData?.Sections?.[0]?.Passages;

  return (
    <div
      className="w-screen h-screen flex flex-col bg-slate-50 overflow-hidden"
      style={{ fontFamily: "Be Vietnam Pro, sans-serif" }}
    >
      <TopNavbar 
        paperName={paperName} 
        timeProps={timeProps} 
        onMiniMenuClick={() => setIsMiniMenuOpen(true)}
        onReviewClick={() => alert("Review clicked!")}
      />

      <SpeakingContent 
        paperData={paperData}
        activePart={activePart} 
        onNextPart={setActivePart}
        attemptId={attemptId}
        sectionId={sectionId}
      />

      {/* FOOTER (ĐÃ SỬA THEO PASCALCASE) */}
      <footer className="fixed bottom-0 left-0 w-full bg-white border-t border-slate-300 shadow-md z-[999]">
        <div className="flex items-center h-16 px-4 gap-3">
          {parts?.map((p) => (
            // SỬA (PascalCase): Đọc 'Id', 'Position', 'Title' (chữ hoa)
            <button
              key={p.Id}
              onClick={() => setActivePart(p.Position)}
              className={`flex-1 py-3 rounded-xl border transition-all duration-300 text-base font-semibold
                ${
                  activePart === p.Position
                    ? "border-[#C76378] text-[#C76378] bg-pink-50"
                    : "border-slate-300 text-slate-700 hover:bg-slate-100 hover:border-[#C76378]"
                }`}
              style={{ fontFamily: "Be Vietnam Pro, sans-serif" }}
            >
              {p.Title}
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

export default SpeakingTestPage;