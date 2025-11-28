import React, { useState, useEffect, useRef } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import { Clock, FileText, Loader2 } from "lucide-react";
import edmLogo from "../../assets/img/edm-logo.png";
import axios from "axios";

// =================== IMPORT FONT ===================
const fontLink = document.createElement("link");
fontLink.href =
  "https://fonts.googleapis.com/css2?family=Be+Vietnam+Pro:wght@300;400;500;600;700&display=swap";
fontLink.rel = "stylesheet";
document.head.appendChild(fontLink);

// =================== COUNTDOWN HOOK ===================
function useCountdown(initialSeconds: number) {
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

// =================== NAVBAR ===================
const TopNavbar: React.FC<{ timeProps: any; onSubmit: () => void }> = ({
  timeProps,
  onSubmit,
}) => {
  const { mins, secs, isWarning, timeLeft } = timeProps;

  useEffect(() => {
    if (timeLeft === 0) {
      alert("⏰ Time's up! Your writing test will be submitted automatically.");
      onSubmit();
    }
  }, [timeLeft, onSubmit]);

  return (
    <nav className="w-full bg-white shadow-sm sticky top-0 z-50 border-b border-slate-200">
      <div className="grid grid-cols-3 items-center px-6 py-3 h-16">
        <div className="flex items-center">
          <img src={edmLogo} alt="EDM" className="h-8 w-auto" />
        </div>
        <div className="flex justify-center items-center">
          <div
            className={`flex items-center gap-2 ${
              isWarning ? "text-red-600" : "text-[#F9AA5C]"
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
          <button
            onClick={onSubmit}
            className="bg-[#F9AA5C] text-white px-5 py-1.5 rounded-full flex items-center gap-2 hover:bg-orange-600 transition"
          >
            Submit
          </button>
        </div>
      </div>
    </nav>
  );
};

// =================== PDF VIEWER ===================
const MaterialViewer = ({ paperData }) => {
  const API_BASE_URL = "http://localhost:8081";
  const pdfAssetId = paperData?.PdfAssetId;
  const fullPdfUrl = pdfAssetId
    ? `${API_BASE_URL}/api/v1/assets/download/${pdfAssetId}#toolbar=0&navpanes=0&view=FitV`
    : null;

  return (
    <div className="h-full bg-[#F2F8FC] flex flex-col">
      {fullPdfUrl ? (
        <div className="flex-1 w-full h-full bg-white rounded-lg shadow-inner overflow-hidden">
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

// =================== WRITING EDITOR ===================
const WritingEditor = ({ taskData, essay, onEssayChange }) => {
  const wordCount = essay.trim().split(/\s+/).filter(Boolean).length;
  const minWords = taskData?.Title?.includes("150") ? 150 : 250;

  return (
    <div className="flex-1 p-10 bg-slate-50 flex flex-col">
      <div className="flex-1 flex flex-col">
        <div className="flex items-center justify-between mb-4">
          <h3 className="text-slate-700 font-semibold text-lg">
            {taskData?.Title || "Your Answer"}
          </h3>
          <div className="text-sm text-slate-500 bg-white px-4 py-2 rounded-full border border-slate-200 shadow-sm">
            <span className="font-semibold text-slate-700">{wordCount}</span>{" "}
            words
          </div>
        </div>

        <textarea
          value={essay}
          onChange={(e) => onEssayChange(e.target.value)}
          className="flex-1 border-2 border-slate-300 rounded-xl p-6 resize-none focus:ring-2 focus:ring-orange-400 focus:border-orange-400 outline-none text-slate-800 leading-relaxed bg-white shadow-sm"
          placeholder={taskData?.ContentText || "Type your essay here..."}
        />

        <div className="mt-4 text-sm text-slate-500 flex items-center gap-2">
          <span className="inline-block w-2 h-2 rounded-full bg-green-500"></span>
          <span>Minimum {minWords} words required</span>
        </div>
      </div>
    </div>
  );
};

// =================== FOOTER ===================
const WritingFooter = ({ activeTask, onSelect, tasks }) => {
  return (
    <footer className="fixed bottom-0 left-0 w-full bg-white border-t border-slate-300 shadow-md z-[999]">
      <div className="flex items-center h-16 px-4 gap-3">
        {tasks?.map((task) => (
          <button
            key={task.Id}
            onClick={() => onSelect(task.Position)}
            className={`flex-1 py-3 rounded-xl border transition-all duration-300 text-base font-semibold
              ${
                activeTask === task.Position
                  ? "border-orange-500 text-orange-700 bg-orange-50 shadow-sm"
                  : "border-slate-300 text-slate-700 hover:bg-slate-100 hover:border-orange-400"
              }`}
          >
            {task.Title}
          </button>
        ))}
      </div>
    </footer>
  );
};

// =================== MAIN PAGE ===================
const WritingTestPage = () => {
  const [activeTask, setActiveTask] = useState(1);
  // Default divider at 65% for larger PDF viewer
  const [dividerX, setDividerX] = useState(65);
  const [isDragging, setIsDragging] = useState(false);
  const containerRef = useRef(null);

  const location = useLocation();
  const navigate = useNavigate();
  const { paperId, paperName } = location.state || {};

  const [paperData, setPaperData] = useState(null);
  const [isLoading, setIsLoading] = useState(true);
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [error, setError] = useState(null);
  const [timeLimit, setTimeLimit] = useState(0);

  // NEW: Test attempt state
  const [testAttemptId, setTestAttemptId] = useState<number | null>(null);
  const [sectionAttempts, setSectionAttempts] = useState<any[]>([]);

  const [task1Essay, setTask1Essay] = useState("");
  const [task2Essay, setTask2Essay] = useState("");

  const timeProps = useCountdown(timeLimit);

  // =================== INITIALIZE TEST (UPDATED) ===================
  useEffect(() => {
    if (!paperId) {
      console.error(
        "paperId is undefined. Check if it is passed correctly in navigate()."
      );
      setError("Can't find the test. Please go back to the library page.");
      setIsLoading(false);
      return;
    }

    const TOKEN = localStorage.getItem("Token");
    if (!TOKEN) {
      navigate("/signin", { state: { from: location } });
      return;
    }

    const initializeTest = async () => {
      try {
        // 1. Fetch paper data
        const paperResponse = await fetch(
          `http://localhost:8081/api/v1/papers/${paperId}`,
          {
            headers: {
              Authorization: `Bearer ${TOKEN}`,
              "Content-Type": "application/json",
            },
          }
        );

        if (paperResponse.status === 401) {
          navigate("/signin");
          return;
        }

        if (!paperResponse.ok) {
          throw new Error(`Lỗi API: ${paperResponse.statusText}`);
        }

        const data = await paperResponse.json();
        setPaperData(data);

        const totalTimeInSeconds = data.Sections?.[0]?.TimeLimitSec || 3600;
        setTimeLimit(totalTimeInSeconds);

        // 2. Start test attempt
        console.log("🚀 Starting test attempt...");
        const attemptResponse = await axios.post(
          "http://localhost:8081/api/v1/test-attempts/start",
          { paperId },
          {
            headers: {
              Authorization: `Bearer ${TOKEN}`,
              "Content-Type": "application/json",
            },
          }
        );

        console.log("Test attempt created:", attemptResponse.data);
        setTestAttemptId(attemptResponse.data.testAttemptId);
        setSectionAttempts(attemptResponse.data.sectionAttempts);
      } catch (err: any) {
        console.error("Err when initialize the test:", err);
        setError(err.response?.data?.error || err.message);
      } finally {
        setIsLoading(false);
      }
    };

    initializeTest();
  }, [paperId, navigate, location]);

  const [isGrading, setIsGrading] = useState(false);

  // =================== SUBMIT HANDLER (UPDATED) ===================
  const handleSubmit = async () => {
    // Check if test is initialized
    if (!testAttemptId || sectionAttempts.length === 0) {
      return;
    }

    if (!task1Essay.trim() && !task2Essay.trim()) {
      alert("Please write at least one essay before submitting!");
      return;
    }

    const confirmSubmit = window.confirm(
      "Are you sure you want to submit your writing test? This action cannot be undone."
    );

    if (!confirmSubmit) return;

    setIsSubmitting(true);
    setIsGrading(true); // Start grading

    try {
      const allTasks = paperData?.Sections?.[0]?.Passages;
      const submissions = [];

      // Get the writing section attempt
      const writingSectionAttempt = sectionAttempts.find(
        (sa) => sa.skill?.toLowerCase() === "writing"
      );

      if (!writingSectionAttempt) {
        throw new Error("Writing section attempt not found");
      }

      console.log("Using section attempt ID:", writingSectionAttempt.id);

      // Submit Task 1
      if (task1Essay.trim()) {
        const task1Data = allTasks?.find((p) => p.Position === 1);
        submissions.push({
          sectionAttemptId: writingSectionAttempt.id,
          promptText: task1Data?.Title || "Task 1",
          contentText: task1Essay,
        });
      }

      // Submit Task 2
      if (task2Essay.trim()) {
        const task2Data = allTasks?.find((p) => p.Position === 2);
        submissions.push({
          sectionAttemptId: writingSectionAttempt.id,
          promptText: task2Data?.Title || "Task 2",
          contentText: task2Essay,
        });
      }

      const TOKEN = localStorage.getItem("Token");

      console.log("Submitting essays:", submissions);

      // Call API cho từng task
      const results = await Promise.all(
        submissions.map((data) =>
          axios.post("http://localhost:8081/api/v1/writing/submit", data, {
            headers: {
              Authorization: `Bearer ${TOKEN}`,
            },
          })
        )
      );

      console.log("Submissions successful:", results);

      // Navigate to results page
      navigate("/writing/results", {
        state: {
          results: results.map((r) => r.data),
          paperName: paperName,
          testAttemptId: testAttemptId,
        },
      });
    } catch (error: any) {
      console.error("Submit error:", error);
      alert(
        error.response?.data?.error ||
          "An error occurred while submitting. Please try again."
      );
    } finally {
      setIsSubmitting(false);
      setIsGrading(false); // End grading
    }
  };

  // Drag logic
  const startDrag = (e) => {
    setIsDragging(true);
    e.preventDefault();
  };

  useEffect(() => {
    const handleMouseMove = (e: MouseEvent) => {
      if (!isDragging || !containerRef.current) return;
      const container = containerRef.current as HTMLDivElement;
      const rect = container.getBoundingClientRect();
      const newPercent = ((e.clientX - rect.left) / rect.width) * 100;
      // Allow PDF viewer to be up to 85% width
      const clampedPercent = Math.max(50, Math.min(85, newPercent));
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

  if (error) {
    return (
      <div className="w-screen h-screen flex flex-col items-center justify-center bg-slate-50 text-red-600">
        <p className="font-medium text-lg">Lỗi: {error}</p>
        <button
          onClick={() => navigate("/library")}
          className="mt-4 px-4 py-2 bg-blue-600 text-white rounded"
        >
          Back to Library
        </button>
      </div>
    );
  }

  // Render
  const allTasks = paperData?.Sections?.[0]?.Passages;
  const currentTaskData = allTasks?.find((p) => p.Position === activeTask);
  const currentEssay = activeTask === 1 ? task1Essay : task2Essay;
  const setCurrentEssay = activeTask === 1 ? setTask1Essay : setTask2Essay;

  return (
    <div
      ref={containerRef}
      className="w-screen h-screen flex flex-col overflow-hidden bg-slate-50 font-['Be_Vietnam_Pro']"
    >
      {timeLimit > 0 && !isLoading && (
        <TopNavbar timeProps={timeProps} onSubmit={handleSubmit} />
      )}

      <div className="flex flex-1 overflow-hidden h-[calc(100vh-8rem)]">
        <div
          className="overflow-y-auto border-r border-slate-200 bg-white"
          style={{ width: `${dividerX}%` }}
        >
          <MaterialViewer paperData={paperData} />
        </div>

        <div
          className="relative flex items-center justify-center cursor-col-resize group"
          style={{ width: "12px" }}
          onMouseDown={startDrag}
        >
          <div
            className="absolute inset-0 bg-slate-300 group-hover:bg-orange-500 transition-colors"
            style={{ width: "2px", left: "5px" }}
          />
        </div>

        <div className="flex-1" style={{ width: `${100 - dividerX}%` }}>
          <WritingEditor
            taskData={currentTaskData}
            essay={currentEssay}
            onEssayChange={setCurrentEssay}
          />
        </div>
      </div>

      <WritingFooter
        activeTask={activeTask}
        onSelect={setActiveTask}
        tasks={allTasks}
      />

      {/* Grading overlay - non-blocking, appears above UI */}
      {isGrading && (
        <div className="fixed inset-0 z-[9999] flex flex-col items-center justify-center bg-black bg-opacity-40 grading-overlay">
          <div className="bg-white rounded-xl shadow-lg px-8 py-10 flex flex-col items-center gap-4">
            <Loader2 className="w-16 h-16 animate-spin text-orange-500" />
            <p className="text-lg font-semibold text-orange-600">
              Grading, please wait...
            </p>
          </div>
        </div>
      )}
    </div>
  );
};

export default WritingTestPage;
