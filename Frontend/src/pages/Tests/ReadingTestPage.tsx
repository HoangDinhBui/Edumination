import React, { useState, useEffect, useRef } from "react";
import { Clock, FileText, Menu, FileEdit } from "lucide-react";
import readImg from "../../assets/img/readingImg.png";
import edmLogo from "../../assets/img/edm-logo.png";

// Import Google Font
const fontLink = document.createElement('link');
fontLink.href = 'https://fonts.googleapis.com/css2?family=Be+Vietnam+Pro:wght@300;400;500;600;700&display=swap';
fontLink.rel = 'stylesheet';
document.head.appendChild(fontLink);

const questionSections = [
  {
    id: 1,
    partLabel: "Part 1",
    title: "Questions 1-6",
    instruction: "",
    qtype: "MCQ",
    questions: [
      {
        id: 1,
        stem: "",
        meta_json: { options: ["A", "B", "C", "D", "E"] },
      },
      {
        id: 2,
        stem: "",
        meta_json: { options: ["A", "B", "C", "D", "E"] },
      },
      {
        id: 3,
        stem: "",
        meta_json: { options: ["A", "B", "C", "D", "E"] },
      },
      {
        id: 4,
        stem: "",
        meta_json: { options: ["A", "B", "C", "D", "E"] },
      },
      {
        id: 5,
        stem: "",
        meta_json: { options: ["A", "B", "C", "D", "E"] },
      },
      {
        id: 6,
        stem: "",
        meta_json: { options: ["A", "B", "C", "D", "E"] },
      },
    ],
  },
  {
    id: 2,
    partLabel: "Part 1",
    title: "Questions 7-13",
    instruction: "",
    qtype: "FILL_BLANK",
    questions: [
      {
        id: 7,
        stem: "",
      },
      {
        id: 8,
        stem: "",
      },
      {
        id: 9,
        stem: "",
      },
      {
        id: 10,
        stem: "",
      },
      {
        id: 11,
        stem: "",
      },
      {
        id: 12,
        stem: "",
      },
      {
        id: 13,
        stem: "",
      },
    ],
  },
];


// =================== COUNTDOWN HOOK ===================
function useCountdown(minutes) {
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
const TopNavbar = () => {
  const { mins, secs, isWarning, timeLeft } = useCountdown(60);

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
        </div>
        </div>

        <div className="flex justify-center items-center">
          <div
            className={`flex items-center gap-2 ${
              isWarning ? "text-red-600" : "text-green-700"
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

          <button className="bg-green-600 text-white px-5 py-1.5 rounded-full flex items-center gap-2 hover:bg-green-700 transition">
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

// =================== READING PASSAGE (PDF Viewer) ===================
const ReadingPassage = () => {
  return (
    <div className="h-full bg-white flex items-center justify-center p-4 mb-5">
      <div className="w-full h-full bg-slate-100 rounded-lg shadow-inner flex items-center justify-center">
        <div className="text-center">
          <FileText className="w-16 h-16 text-slate-400 mx-auto mb-4" />
          <p className="text-slate-500 text-sm">PDF Document Viewer</p>
          <p className="text-slate-400 text-xs mt-2">Cambridge 19 - Reading Test</p>
        </div>
      </div>
    </div>
  );
};

// =================== MAIN COMPONENT ===================
const ReadingTestPage = () => {
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

  const [activePart, setActivePart] = useState(1);

  const parts = [
    { id: 1, label: "Part 1" },
    { id: 2, label: "Part 2" },
    { id: 3, label: "Part 3" },
  ];

  const renderQuestion = (q, section) => {
    if (section.qtype === "MCQ") {
      const options = q.meta_json?.options || [];
      return (
        <div className="flex items-center gap-3">
          <span className="text-slate-700 font-medium text-sm w-5">{q.id}.</span>
          <select className="flex-1 border border-slate-300 rounded-lg px-4 py-2.5 text-slate-700 text-sm focus:ring-2 focus:ring-green-500 focus:border-green-500 outline-none cursor-pointer bg-white hover:border-slate-400 transition-colors">
            <option value=""></option>
            {options.map((opt) => (
              <option key={opt} value={opt}>
                {opt}
              </option>
            ))}
          </select>
        </div>
      );
    }

    if (section.qtype === "FILL_BLANK") {
      return (
        <div className="flex items-center gap-3">
          <div className="w-10 h-10 rounded-full bg-green-600 text-white flex items-center justify-center font-semibold text-sm flex-shrink-0">
            {q.id}
          </div>
          <input
            type="text"
            className="flex-1 border border-slate-300 rounded-full px-4 py-2.5 text-slate-700 text-sm outline-none focus:ring-2 focus:ring-green-500 focus:border-green-500 bg-white"
            placeholder=""
          />
        </div>
      );
    }

    return null;
  };

  return (
    <div className="w-screen h-screen flex flex-col overflow-hidden bg-slate-50 font-['Be_Vietnam_Pro']">
      <TopNavbar />

      <div
        ref={containerRef}
        className="flex flex-1 overflow-hidden h-[calc(100vh-4rem)] relative"
      >
        {/* LEFT: Reading Passage */}
        <div
          className="border-r border-slate-200 overflow-y-auto"
          style={{ width: `${dividerX}%`, backgroundColor: "#F0FAF1" }}
        >
          <ReadingPassage />
        </div>

        {/* Divider */}
        <div
          className="relative flex items-center justify-center cursor-col-resize group"
          style={{ width: '12px' }}
          onMouseDown={startDrag}
        >
          <div className="absolute inset-0 bg-slate-300 group-hover:bg-blue-500 transition-colors" style={{ width: '2px', left: '5px' }} />
          <div className="absolute inset-0" style={{ width: '12px' }} />
        </div>

         {/* RIGHT: Questions */}
        <div
          className="flex-1 overflow-y-auto p-6 pb-24"
          style={{ backgroundColor: "#FFFFFF" }}
        >
          <div className="max-w-xs">
            {questionSections.map((section) => (
              <div key={section.id} className="mb-8">
                {/* Header */}
                <div className="mb-4">
                  <h2 className="text-green-700 font-semibold text-l mb-3">
                    {section.title}
                  </h2>
                </div>

                {/* Questions */}
                <div className="space-y-3">
                  {section.questions.map((q) => (
                    <div key={q.id}>
                      {renderQuestion(q, section)}
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
    </div>
  );
};

export default ReadingTestPage;