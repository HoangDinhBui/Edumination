import React, { useState, useEffect } from "react";
import { Clock, FileText, Menu, FileEdit, Volume2 } from "lucide-react";
import edmLogo from "../../assets/img/edm-logo.png";
{
  /* Import Google Fonts */
}
<style>{`
        @import url('https://fonts.googleapis.com/css2?family=Paytone+One&family=Montserrat:wght@300;400;500;600;700&family=Be+Vietnam+Pro:wght@300;400;500;600;700&display=swap');
      `}</style>;

// =================== MOCK DATA ===================
const questionSections = [
  {
    id: 1,
    partLabel: "Part 1",
    title: "Questions 1-5",
    instruction: "The housing officer takes some details from the girl.",
    subInstruction:
      "Complete the following form with NO MORE THAN THREE WORDS AND/OR A NUMBER for each answer.",
    qtype: "FORM_COMPLETION",
    formTitle: "PERSONAL DETAILS FOR HOMESTAY APPLICATION",
    questions: [
      { id: 1, label: "First name", value: "" },
      { id: 2, label: "Family name", value: "Yuanati" },
      { id: 3, label: "Gender", value: "Female" },
      { id: 4, label: "Age", value: "28" },
      { id: 5, label: "Passport number", value: "" },
      { id: 6, label: "Nationality", value: "Japanese" },
      { id: 7, label: "Course enrolled", value: "" },
      { id: 8, label: "Length of the course", value: "" },
      { id: 9, label: "Homestay time", value: "" },
    ],
  },
  {
    id: 2,
    partLabel: "Part 1",
    title: "Question 6",
    instruction: "Mark TWO letters that represent the correct answer.",
    subInstruction: "Which kind of family does the girls prefer?",
    qtype: "MCQ",
    questions: [
      {
        id: 6,
        stem: "Which kind of family does the girls prefer?",
        meta_json: {
          options: [
            { key: "A", text: "A big family with many young children" },
            { key: "B", text: "A family without smoker or drinkers" },
            { key: "C", text: "A family without any pets" },
            { key: "D", text: "A family with many animals or pets" },
          ],
        },
      },
    ],
  },
  {
    id: 3,
    partLabel: "Part 1",
    title: "Questions 7-10",
    instruction:
      "Fill in the blanks with NO MORE THAN THREE WORDS for each answer.",
    qtype: "FILL_BLANK",
    questions: [
      {
        id: 7,
        stem: "Although the girl is not a vegetarian, she doesn't eat a lot of meat. Her favourite food is",
        suffix: ".",
      },
      {
        id: 8,
        stem: "The girls has given up playing handball. Now, she just play",
        suffix: "with her friends at weekends.",
      },
      {
        id: 9,
        stem: "The girl does not like the bus because they are always late. She would rather",
        suffix: ".",
      },
      {
        id: 10,
        stem: "The girl can get the information about the homestay family that she wants",
        suffix: ".",
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
  const { mins, secs, isWarning, timeLeft } = useCountdown(32);
  const [volume, setVolume] = useState(70);

  useEffect(() => {
    if (timeLeft === 0) {
      alert("Time's up! Your test will be submitted automatically.");
    }
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
            className="w-24 h-1"
          />
          <FileEdit className="w-6 h-6 cursor-pointer hover:text-slate-800" />
          <Menu className="w-6 h-6 text-slate-600 cursor-pointer hover:text-slate-800" />

          <button className="bg-[#2986B7] text-white px-5 py-1.5 rounded-full flex items-center gap-2 hover:bg-blue-700 transition">
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

// =================== MAIN COMPONENT ===================
const ListeningTestPage = () => {
  const [activePart, setActivePart] = useState(1);

  const parts = [
    { id: 1, label: "Part 1" },
    { id: 2, label: "Part 2" },
    { id: 3, label: "Part 3" },
    { id: 4, label: "Part 4" },
  ];

  const renderQuestion = (section) => {
    if (section.qtype === "FORM_COMPLETION") {
      let questionCounter = 0; // Đếm số thứ tự câu hỏi

      return (
        <div className="space-y-4">
          <h4 className="font-semibold text-base text-[#294563] mb-7">
            {section.formTitle}
          </h4>
          <div className="space-y-3">
            {section.questions.map((q) => {
              const isQuestion = !q.value; // Nếu không có value thì là câu hỏi
              if (isQuestion) questionCounter++; // Tăng số thứ tự

              return (
                <div
                  key={q.id}
                  className="grid grid-cols-[200px_1fr] gap-4 items-center"
                >
                  <label className="text-slate-700 text-sm">{q.label}</label>
                  <div className="relative">
                    {q.value ? (
                      <span className="text-slate-700 text-sm">{q.value}</span>
                    ) : (
                      <>
                        <input
                          type="text"
                          className="w-full px-3 py-2 border-2 border rounded-full text-sm focus:outline-none focus:ring-2 focus:ring-blue-400"
                          placeholder="..."
                        />
                        <span className="absolute left-3 -top-3 bg-[#2986B7] text-white text-xs font-bold rounded-full w-5 h-5 flex items-center justify-center">
                          {questionCounter}
                        </span>
                      </>
                    )}
                  </div>
                </div>
              );
            })}
          </div>
        </div>
      );
    }

    if (section.qtype === "MCQ") {
      return (
        <div className="space-y-6">
          <p className="text-slate-700 text-base mb-6">
            {section.subInstruction}
          </p>
          <div className="space-y-4">
            {section.questions[0].meta_json.options.map((opt) => (
              <label
                key={opt.key}
                className="flex items-start gap-4 cursor-pointer group"
              >
                
                <span className="flex items-center justify-center w-8 h-8 rounded-full text-black font-bold text-base bg-[#C4D3E0]">
                  {opt.key}
                </span>

                {/* Checkbox */}
                <input
                  type="checkbox"
                  className="mt-1 w-5 h-5 accent-[#4282B6] text-blue-600 border-2 border-slate-300 rounded focus:ring-2 focus:ring-[#4282B6]"
                />

                {/* Text */}
                <div className="flex flex-col">
                  <span className="text-slate-800 text-base leading-relaxed">
                    {opt.text}
                  </span>
                </div>
              </label>
            ))}
          </div>
        </div>
      );
    }

    if (section.qtype === "FILL_BLANK") {
      return (
        <div className="space-y-6">
          {section.questions.map((q) => (
            <div
              key={q.id}
              className="flex items-center gap-2 text-l text-slate-700 flex-wrap"
            >
              <span className="font-semibold text-[#2986B7]">{q.id}</span>
              {q.stem && <span>{q.stem}</span>}
              <div className="relative inline-flex items-center">
                <input
                  type="text"
                  className="border-2 border rounded-full px-4 py-2 min-w-[180px] text-center outline-none focus:ring-2 focus:ring-blue-400"
                  placeholder="..."
                />
              </div>
              {q.suffix && <span>{q.suffix}</span>}
            </div>
          ))}
        </div>
      );
    }

    return null;
  };

  return (
    <div className="w-screen h-screen flex flex-col overflow-hidden bg-slate-50">
      <TopNavbar />

      {/* MAIN CONTENT */}
      <div className="flex-1 overflow-y-auto p-4 pb-10">
        {questionSections.length > 0 && (
          <h2
            className="text-[#2986B7] font-bold text-xl mb-3"
            style={{ fontFamily: "'Be Vietnam Pro', sans-serif" }}
          >
            {questionSections[0].partLabel}
          </h2>
        )}

        {questionSections.map((section) => (
          <div
            key={section.id}
            className="mb-10 bg-white rounded-lg shadow-sm border border-slate-200 p-6"
          >
            <div className="mb-6">
              <h3 className="text-[#2986B7] text-lg font-semibold text-base mb-2">
                {section.title}
              </h3>
              <p className="text-slate-600 text-sm italic">
                {section.instruction}
              </p>
              {section.subInstruction && section.qtype !== "MCQ" && (
                <p className="text-slate-700 text-sm mt-1">
                  {section.subInstruction}
                </p>
              )}
            </div>

            {renderQuestion(section)}
          </div>
        ))}
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
                    ? "border-[#2986B7] text-blue-700 bg-[blue-50] shadow-sm"
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
