import React, { useState, useEffect, useRef } from "react";
import { Clock, FileText, Menu, FileEdit, Volume2 } from "lucide-react";
import edmLogo from "../../assets/img/edm-logo.png";

// =================== IMPORT FONT ===================
const fontLink = document.createElement("link");
fontLink.href =
  "https://fonts.googleapis.com/css2?family=Be+Vietnam+Pro:wght@300;400;500;600;700&display=swap";
fontLink.rel = "stylesheet";
document.head.appendChild(fontLink);

// =================== COUNTDOWN HOOK ===================
function useCountdown(minutes) {
  const [timeLeft, setTimeLeft] = useState(minutes * 60);

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
const TopNavbar = () => {
  const { mins, secs, isWarning, timeLeft } = useCountdown(32);
  const [volume, setVolume] = useState(70);

  useEffect(() => {
    if (timeLeft === 0) alert("Time's up! Your test will be submitted automatically.");
  }, [timeLeft]);

  return (
    <nav className="w-full bg-white shadow-sm sticky top-0 z-50 border-b border-slate-200">
      <div className="grid grid-cols-3 items-center px-6 py-3 h-16">
        <div className="flex items-center">
          <img src={edmLogo} alt="Edumination Logo" className="h-6" />
        </div>

        <div className="flex justify-center items-center">
          <div
            className={`flex items-center gap-2 ${
              isWarning ? "text-red-600" : "text-[#2986B7]"
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
          <Volume2 className="w-5 h-5 text-[#2986B7]" />
          <input
            type="range"
            min="0"
            max="100"
            value={volume}
            onChange={(e) => setVolume(Number(e.target.value))}
            className="w-24 h-1 accent-[#2986B7]"
          />
          <FileEdit className="w-6 h-6 cursor-pointer hover:text-slate-800" />
          <Menu className="w-6 h-6 text-slate-600 cursor-pointer hover:text-slate-800" />
          <button className="bg-[#2986B7] text-white px-5 py-1.5 rounded-full flex items-center gap-2 hover:bg-blue-700 transition">
            Submit
          </button>
        </div>
      </div>
    </nav>
  );
};

// =================== LEFT COLUMN (PDF / AUDIO AREA) ===================
const ListeningMaterial = () => (
  <div className="h-full bg-[#F2F8FC] flex flex-col items-center justify-center p-6">
    <div className="w-full h-full bg-slate-100 rounded-lg shadow-inner flex flex-col items-center justify-center">
      <FileText className="w-16 h-16 text-[#7AAEDB] mb-4" />
      <p className="text-slate-500 text-sm">Listening Material / PDF Viewer</p>
      <p className="text-slate-400 text-xs mt-2">Your listening passage here</p>
    </div>
  </div>
);

// =================== MOCK STRUCTURE (chỉ để test) ===================
const questionSections = [
  {
    id: 1,
    title: "Questions 1–6",
    qtype: "FILL_BLANK",
    questions: [{ id: 1 }, { id: 2 }, { id: 3 }, { id: 4 }, { id: 5 }, { id: 6 }],
  },
  {
    id: 2,
    title: "Questions 7–10",
    qtype: "MCQ",
    questions: [
      { id: 7, meta_json: { options: ["TRUE", "FALSE", "NOT GIVEN"] } },
      { id: 8, meta_json: { options: ["TRUE", "FALSE", "NOT GIVEN"] } },
      { id: 9, meta_json: { options: ["TRUE", "FALSE", "NOT GIVEN"] } },
      { id: 10, meta_json: { options: ["TRUE", "FALSE", "NOT GIVEN"] } },
    ],
  },
  {
    id: 3,
    title: "Questions 11–12",
    qtype: "MULTI_SELECT",
    questions: [
      { id: 11, meta_json: { options: ["A", "B", "C", "D", "E"] } },
      { id: 12, meta_json: { options: ["A", "B", "C", "D", "E"] } },
    ],
  },
  {
    id: 4,
    title: "Questions 15–18",
    qtype: "MCQ",
    questions: [
      { id: 15, meta_json: { options: ["A", "B", "C"] } },
      { id: 16, meta_json: { options: ["A", "B", "C"] } },
      { id: 17, meta_json: { options: ["A", "B", "C"] } },
      { id: 18, meta_json: { options: ["A", "B", "C"] } },
    ],
  },
];

// =================== RENDER QUESTION ===================
const renderQuestion = (q, section) => {
  const color = "#2986B7";
  const options = q.meta_json?.options || [];

  if (section.qtype === "FILL_BLANK") {
    return (
      <div key={q.id} className="flex items-center gap-2 mb-3">
        <div className="w-7 h-7 rounded-full bg-[#2986B7] text-white flex items-center justify-center text-sm font-semibold flex-shrink-0">
          {q.id}
        </div>
        <input
          type="text"
          className="w-48 border border-slate-300 rounded-full px-3 py-1.5 text-slate-700 text-sm focus:ring-2 focus:ring-[#2986B7] outline-none"
        />
      </div>
    );
  }

  if (section.qtype === "MCQ") {
    const opts = options.length > 0 ? options : ["TRUE", "FALSE", "NOT GIVEN"];
    return (
      <div key={q.id} className="flex items-center gap-2 mb-3">
        <div className="w-7 h-7 rounded-full bg-[#2986B7] text-white flex items-center justify-center text-sm font-semibold flex-shrink-0">
          {q.id}
        </div>
        <select className="w-48 border border-slate-300 rounded-full px-3 py-1.5 text-slate-700 text-sm focus:ring-2 focus:ring-[#2986B7] outline-none">
          <option value="">Select</option>
          {opts.map((opt) => (
            <option key={opt} value={opt}>
              {opt}
            </option>
          ))}
        </select>
      </div>
    );
  }

   if (section.qtype === "MULTI_SELECT") {
    const opts = q.meta_json?.options || ["A", "B", "C", "D", "E"];
    return (
      <div key={q.id} className="mb-4">
        <h4 className="text-[#2986B7] font-medium text-sm mb-2">
          Question {q.id}
        </h4>
        <div className="flex flex-wrap gap-2">
          {opts.map((opt) => (
            <label
              key={opt}
              className="flex items-center gap-2 px-3 py-1.5 rounded-full border border-slate-300 cursor-pointer hover:bg-blue-50"
            >
              <input type="checkbox" className="accent-[#2986B7] w-4 h-4" />
              <span className="text-sm text-slate-700 font-medium">{opt}</span>
            </label>
          ))}
        </div>
      </div>
    );
  }

  return null;
};

// =================== MAIN PAGE ===================
const ListeningTestPage = () => {
  const [dividerX, setDividerX] = useState(50);
  const [isDragging, setIsDragging] = useState(false);
  const containerRef = useRef(null);
  const [activePart, setActivePart] = useState(1);

  const parts = [
    { id: 1, label: "Part 1" },
    { id: 2, label: "Part 2" },
    { id: 3, label: "Part 3" },
    { id: 4, label: "Part 4" },
  ];

  const startDrag = (e) => {
    setIsDragging(true);
    e.preventDefault();
  };

  useEffect(() => {
    const handleMove = (e) => {
      if (!isDragging || !containerRef.current) return;
      const rect = containerRef.current.getBoundingClientRect();
      const newPercent = ((e.clientX - rect.left) / rect.width) * 100;
      const clamped = Math.max(30, Math.min(70, newPercent));
      setDividerX(clamped);
    };
    const stopDrag = () => setIsDragging(false);
    if (isDragging) {
      document.addEventListener("mousemove", handleMove);
      document.addEventListener("mouseup", stopDrag);
      document.body.style.cursor = "col-resize";
    }
    return () => {
      document.removeEventListener("mousemove", handleMove);
      document.removeEventListener("mouseup", stopDrag);
      document.body.style.cursor = "default";
    };
  }, [isDragging]);

  return (
    <div className="w-screen h-screen flex flex-col bg-slate-50 font-['Be_Vietnam_Pro'] overflow-hidden">
      <TopNavbar />

      <div ref={containerRef} className="flex flex-1 overflow-hidden relative bg-[#F8FAFC]">
        {/* LEFT: PDF VIEWER */}
        <div className="border-r border-slate-200 overflow-y-auto" style={{ width: `${dividerX}%` }}>
          <ListeningMaterial />
        </div>

        {/* DIVIDER */}
        <div
          className="relative flex items-center justify-center cursor-col-resize group"
          style={{ width: "12px" }}
          onMouseDown={startDrag}
        >
          <div
            className="absolute inset-0 bg-slate-300 group-hover:bg-[#2986B7] transition-colors"
            style={{ width: "2px", left: "5px" }}
          />
        </div>

        {/* RIGHT: QUESTIONS */}
        <div className="flex-1 overflow-y-auto px-10 py-8 bg-white">
          <div className="max-w-md">
            {questionSections.map((section) => (
              <div key={section.id} className="mb-8">
                <h3 className="text-[#2986B7] font-semibold text-base mb-2">{section.title}</h3>
                {section.instruction && (
                  <p className="text-slate-700 text-sm mb-3">{section.instruction}</p>
                )}
                {section.questions.map((q) => renderQuestion(q, section))}
              </div>
            ))}
          </div>
        </div>
      </div>

      {/* FOOTER giữ nguyên */}
      <footer className="fixed bottom-0 left-0 w-full bg-white border-t border-slate-300 shadow-md z-[999]">
        <div className="flex items-center h-16 px-4 gap-3">
          {parts.map((p) => (
            <button
              key={p.id}
              onClick={() => setActivePart(p.id)}
              className={`flex-1 py-3 rounded-xl border transition-all duration-300 text-base font-semibold
                ${
                  activePart === p.id
                    ? "border-[#2986B7] text-[#2986B7] bg-blue-50"
                    : "border-slate-300 text-slate-700 hover:bg-slate-100 hover:border-blue-400"
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

export default ListeningTestPage;
