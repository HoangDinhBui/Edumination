import React, { useState, useEffect, useRef } from "react";
import { Clock, FileText, Menu, FileEdit } from "lucide-react";
import edmLogo from "../../assets/img/edm-logo.png";
import writingDiagram from "../../assets/img/writing-diagram.png";

// =================== MOCK DATA ===================
const writingTasks = [
  {
    id: 1,
    title: "Writing Task 1",
    part: 1,
    time: 20,
    instruction: [
      "You should spend about 20 minutes on this task.",
      "The diagram below shows how ethanol fuel is produced from corn.",
      "Summarise the information by selecting and reporting the main features and make comparisons where relevant.",
      "You should write at least 150 words.",
    ],
    image: writingDiagram,
    placeholder: "Write your Task 1 essay here...",
  },
   {
    id: 2,
    part: 2,
    title: "Writing Task 2",
    time: 40,
    instruction: [
      "You should spend about 40 minutes on this task.",
      "Write about the following topic:",
      "Some people believe that technology has made our lives more complex. Others think it has made life easier. Discuss both views and give your own opinion.",
      "Give reasons for your answer and include any relevant examples from your own knowledge or experience.",
      "Write at least 250 words.",
    ],
    image: writingDiagram,
    placeholder: "Write your Task 2 essay here...",
  },
];

// =================== COUNTDOWN HOOK ===================
function useCountdown(minutes: number) {
  const [timeLeft, setTimeLeft] = useState(minutes * 60);

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
const TopNavbar: React.FC<{ duration: number }> = ({ duration }) => {
  const { mins, secs, isWarning, timeLeft } = useCountdown(duration);

  useEffect(() => {
    if (timeLeft === 0) {
      alert("⏰ Time’s up! Your writing test will be submitted automatically.");
    }
  }, [timeLeft]);

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
            <span className="text-2xl font-semibold">{mins}</span>
            <span className="text-slate-600">minutes remaining</span>
            <span className="ml-1 text-sm text-slate-500">
              ({secs.toString().padStart(2, "0")}s)
            </span>
          </div>
        </div>

        <div className="flex justify-end items-center gap-4">
          <FileEdit className="w-6 h-6 cursor-pointer hover:text-slate-800" />
          <Menu className="w-6 h-6 text-slate-600 cursor-pointer hover:text-slate-800" />
          <button className="flex items-center gap-2 border border-slate-300 text-slate-600 px-4 py-1.5 rounded-full hover:bg-slate-100 transition">
            <FileText className="w-4 h-4" />
            <span>Review</span>
          </button>
          <button className="bg-[#F9AA5C] text-white px-5 py-1.5 rounded-full flex items-center gap-2 hover:bg-green-700 transition">
            Submit
            <svg
              xmlns="http://www.w3.org/2000/svg"
              fill="none"
              viewBox="0 0 24 24"
              strokeWidth={2}
              stroke="currentColor"
              className="w-4 h-4"
            >
              <path
                strokeLinecap="round"
                strokeLinejoin="round"
                d="M5 12h14M12 5l7 7-7 7"
              />
            </svg>
          </button>
        </div>
      </div>
    </nav>
  );
};

// =================== WRITING TASK COMPONENT ===================
const WritingTask = ({ data, dividerX, onDragStart }) => {
  const [essay, setEssay] = useState("");
  const wordCount = essay.trim().split(/\s+/).filter(Boolean).length;

  return (
    <div className="flex flex-1 overflow-hidden h-[calc(100vh-4rem)]">
      {/* LEFT SIDE - Instructions */}
      <div
        className="overflow-y-auto border-r border-slate-200 bg-white"
        style={{ width: `${dividerX}%` }}
      >
        <div className="h-full bg-white flex items-center justify-center p-4 mb-5">
              <div className="w-full h-full bg-slate-100 rounded-lg shadow-inner flex items-center justify-center">
                <div className="text-center">
                  <FileText className="w-16 h-16 text-slate-400 mx-auto mb-4" />
                  <p className="text-slate-500 text-sm">PDF Document Viewer</p>
                  <p className="text-slate-400 text-xs mt-2">Cambridge 19 - Reading Test</p>
                </div>
              </div>
            </div>
        <br></br> <br></br> <br></br>
      </div>

      {/* DIVIDER */}
      <div
        className="relative flex items-center justify-center cursor-col-resize group"
        style={{ width: "12px" }}
        onMouseDown={onDragStart}
      >
        <div
          className="absolute inset-0 bg-slate-300 group-hover:bg-orange-500 transition-colors"
          style={{ width: "2px", left: "5px" }}
        />
        <div className="absolute inset-0" style={{ width: "12px" }} />
      </div>

      {/* RIGHT SIDE - Text Editor */}
      <div
        className="flex-1 p-10 bg-slate-50 flex flex-col"
        style={{ width: `${100 - dividerX}%` }}
      >
        <div className="flex-1 flex flex-col">
          <div className="flex items-center justify-between mb-4">
            <h3 className="text-slate-700 font-semibold text-lg">
              Your Answer
            </h3>
            <div className="text-sm text-slate-500 bg-white px-4 py-2 rounded-full border border-slate-200 shadow-sm">
              <span className="font-semibold text-slate-700">{wordCount}</span>{" "}
              words
            </div>
          </div>

          <textarea
            value={essay}
            onChange={(e) => setEssay(e.target.value)}
            className="flex-1 border-2 border-slate-300 rounded-xl p-6 resize-none focus:ring-2 focus:ring-orange-400 focus:border-orange-400 outline-none text-slate-800 leading-relaxed bg-white shadow-sm"
            placeholder={data.placeholder}
          />

          <div className="mt-4 text-sm text-slate-500 flex items-center gap-2">
            <span className="inline-block w-2 h-2 rounded-full bg-green-500"></span>
            <span>Minimum 150 words required</span>
          </div>
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
        {tasks.map((t) => (
          <button
            key={t.id}
            onClick={() => onSelect(t.id)}
            className={`flex-1 py-3 rounded-xl border transition-all duration-300 text-base font-semibold
              ${
                activeTask === t.id
                  ? "border-orange-500 text-orange-700 bg-orange-50 shadow-sm"
                  : "border-slate-300 text-slate-700 hover:bg-slate-100 hover:border-orange-400"
              }`}
          >
            {t.title}
          </button>
        ))}
      </div>
    </footer>
  );
};

// =================== MAIN PAGE ===================
const WritingTestPage = () => {
  const [activeTask, setActiveTask] = useState(1);
  const currentTask = writingTasks.find((t) => t.id === activeTask);
  const [dividerX, setDividerX] = useState(50);
  const [isDragging, setIsDragging] = useState(false);
  const containerRef = useRef(null);

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

  return (
    <div
      ref={containerRef}
      className="w-screen h-screen flex flex-col overflow-hidden bg-slate-50 font-['Be_Vietnam_Pro']"
    >
      <TopNavbar duration={currentTask.time} />
      <WritingTask
        data={currentTask}
        dividerX={dividerX}
        onDragStart={startDrag}
      />
      <WritingFooter
        activeTask={activeTask}
        onSelect={setActiveTask}
        tasks={writingTasks}
      />
    </div>
  );
};

export default WritingTestPage;
