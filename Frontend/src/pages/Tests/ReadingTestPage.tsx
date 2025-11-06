import React, { useState, useEffect, useRef } from "react";
import { Clock, FileText, Menu, FileEdit } from "lucide-react";
import readImg from "../../assets/img/readingImg.png";
import edmLogo from "../../assets/img/edm-logo.png";

// Import Google Font
const fontLink = document.createElement('link');
fontLink.href = 'https://fonts.googleapis.com/css2?family=Be+Vietnam+Pro:wght@300;400;500;600;700&display=swap';
fontLink.rel = 'stylesheet';
document.head.appendChild(fontLink);

// =================== MOCK DATA ===================
const questionSections = [
  {
    id: 1,
    partLabel: "Part 1",
    title: "Questions 1-4",
    instruction:
      "The text has 5 paragraphs (A–E).",
    subInstruction: "Which paragraph contains each of the following pieces of information?",
    qtype: "MCQ",
    questions: [
      {
        id: 1,
        stem: "A possible security problem",
        meta_json: { options: ["A", "B", "C", "D", "E"] },
      },
      {
        id: 2,
        stem: "The cost of M-Pesa",
        meta_json: { options: ["A", "B", "C", "D", "E"] },
      },
      {
        id: 3,
        stem: "An international service similar to M-Pesa",
        meta_json: { options: ["A", "B", "C", "D", "E"] },
      },
      {
        id: 4,
        stem: "The fact that most Kenyans do not have a bank account",
        meta_json: { options: ["A", "B", "C", "D", "E"] },
      },
    ],
  },
  {
    id: 2,
    partLabel: "Part 1",
    title: "Questions 5-8",
    instruction:
      "Complete the following sentences using NO MORE THAN THREE WORDS from the text for each gap.",
    qtype: "FILL_BLANK",
    questions: [
      {
        id: 5,
        stem: "Safaricom is the",
        suffix: "mobile phone company in Kenya.",
      },
      {
        id: 6,
        stem: "An M-Pesa account needs to be credited by",
        suffix: "",
      },
      {
        id: 7,
        stem: "",
        suffix: "companies are particularly interested in using M-Pesa.",
      },
      {
        id: 8,
        stem: "Companies like Moneygram and Western Union have",
        suffix: "the international money transfer market.",
      },
    ],
  },
  {
    id: 3,
    partLabel: "Part 1",
    title: "Questions 9-13",
    instruction:
      "Do the statements on the next page agree with the information given in Reading Passage 1?",
    subInstruction: "In boxes 9 - 13 on your answer sheet, write",
    qtype: "SHORT_ANSWER",
    meta_json: { options: ["TRUE", "FALSE", "NOT GIVEN"] },
    legendItems: [
      { value: "TRUE", description: "if the statement agrees with the information" },
      { value: "FALSE", description: "if the statement contradicts the information" },
      { value: "NOT GIVEN", description: "if there is no information on this" },
    ],
    questions: [
      {
        id: 9,
        stem: "Most Kenyans working in urban areas have relatives in rural areas.",
      },
      {
        id: 10,
        stem: "So far, most of the people using M-Pesa have used it to send small amounts of money.",
      },
      {
        id: 11,
        stem: "M-Pesa can only be used by people using one phone network.",
      },
      {
        id: 12,
        stem: "M-Pesa can be used to buy products and services.",
      },
      {
        id: 13,
        stem: "The GSM Association is a consumer organisation.",
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

// =================== READING PASSAGE ===================
const ReadingPassage = () => {
  const passage = {
    part: "PART 1",
    title: "READING PASSAGE 1",
    instruction:
      "You should spend about 20 minutes on Questions 1–13, which are based on Reading Passage 1 below.",
    subtitle: "Reading Passage 1",
    image: readImg,
    articleTitle: "Money Transfers by Mobile",
    paragraphs: [
      {
        label: "A.",
        text: `The ping of a text message has never sounded so sweet. In what is being touted as a world first, Kenya's biggest mobile operator is allowing subscribers to send cash to other phones by SMS. Known as M-Pesa, or mobile money, the service is expected to revolutionise banking in a country where more than 80% of people are excluded from the traditional financial sector. Apart from transferring cash – a service much in demand among urban Kenyans supporting relatives in rural areas – customers of Safaricom will be able to keep up to 50,000 shillings (£370) in a "virtual account" on their handsets.`,
      },
      {
        label: "B.",
        text: `Developed by Vodafone, which holds a 35% share in Safaricom, M-Pesa was formally launched in Kenya two weeks ago. More than 10,000 people have signed up for the service, with around 8 million shillings transferred so far, mostly in tiny denominations. Safaricom's executives are confident that growth will be strong in Kenya, and later across Africa. "We are effectively giving people ATM cards without them ever having to open a bank account," said Michael Joseph, chief executive of Safaricom, who said the money transfer concept the "next big thing" in mobile telephony.`,
      },
      {
        label: "C.",
        text: `M-Pesa's simple. There is no need for a new handset or SIM card. To send money, you hand over the cash to a registered agent – typically a retailer – who credits your virtual account. You can then send between 100 shillings (74p) and 35,000 shillings (£259) via text message to the desired recipient – even someone on a different mobile network – who cashes it at an agent by entering a secret code and showing ID. A commission of up to 170 shillings (£1.25) is paid by the recipient but it compares favourably with fees levied by the major banks, whose services are too expensive for most of the population.`,
      },
      {
        label: "D.",
        text: `Mobile phone growth in Kenya, as in most of Africa, has been remarkable, even among the rural poor. In late 1999, Kenya had 15,000 mobile subscribers. Today, it has nearly 8 million out of a population of 35 million, and the two operators' networks are as extensive as the access to banks is limited. Safaricom says it is not now competing with financial services companies as filling a void. In time, M-Pesa will allow people to borrow and repay money, and make purchases. Companies will be able to pay salaries directly into workers' phones – something that has already proved attractive to aid agencies. In areas where banking is limited, those workers often have to be paid in cash as they do not have bank accounts. There are concerns about security, but Safaricom insists that even if someone's phone is stolen, the PIN system prevents unauthorised withdrawals. Mr. Joseph said the only danger is sending cash to the wrong mobile number and the recipient redeeming it straight away.`,
      },
      {
        label: "E.",
        text: `The project is being watched closely by mobile operators around the world as a way of targeting the multibillion unbanked population cash transfer industry and competing with companies such as Western Union and Moneygram. Remittances from nearly 200 million migrant workers to developing countries totalled $102 billion last year, according to the World Bank. The GSM Association, the industry group for mobile operators worldwide, believes this could quadruple by 2012 if transfers by SMS become the norm. Vodafone has entered a partnership with Citigroup that will soon allow Kenyans in the UK to send money home via text message. The charge for sending £30 is expected to be about £3, less than a third of what some traditional services charge.`,
      },
    ],
  };

  return (
    <div className="p-8 overflow-y-auto h-full bg-green-50 scroll-smooth text-slate-800 leading-relaxed">
      <h2 className="text-[#294563] font-semibold text-xl uppercase tracking-wide">
        {passage.part}
      </h2>
      <h1 className="text-2xl font-bold text-[#294563] mt-1 mb-1">
        {passage.title}
      </h1>
      <p className="text-[#9BA19C] italic text-sm mb-6">
        {passage.instruction}
      </p>

      <div className="mb-8">
        {/* Hình lớn trên cùng */}
        <img
          src={passage.image}
          alt="Reading"
          className="w-full h-64 sm:h-80 object-cover rounded-xl shadow-md border border-green-100"
        />

        <div className="text-center mt-4">
          <h3 className="text-[#294563] font-semibold text-xl">
            {passage.subtitle}
          </h3>
        </div>
      </div>

      <h2 className="text-xl text-center font-bold text-[#294563] mb-4">
        {passage.articleTitle}
      </h2>

      <div className="space-y-5">
        {passage.paragraphs.map((p) => (
          <p key={p.label}>
            <span className="font-bold text-slate-700 mr-2">{p.label}</span>
            {p.text}
          </p>
        ))}
      </div>
      <br></br> <br></br> <br></br> <br></br>
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
    if (section.qtype === "MCQ" || section.qtype === "SHORT_ANSWER") {
      const options = q.meta_json?.options || section.meta_json?.options || [];
      return (
        <div className="flex items-center gap-3 min-w-fit">
          <span className="text-slate-700 font-medium text-sm">{q.id}.</span>
          <select className="border border-slate-300 rounded px-3 py-1.5 text-slate-700 text-sm focus:ring-1 focus:ring-green-500 focus:border-green-500 outline-none cursor-pointer bg-white hover:border-slate-400 transition-colors">
            <option value="">Select</option>
            {options.map((opt) => (
              <option key={opt} value={opt}>
                {opt}
              </option>
            ))}
          </select>
          <p className="text-slate-700 text-sm flex-1">{q.stem}</p>
        </div>
      );
    }

    if (section.qtype === "FILL_BLANK") {
      return (
        <div className="flex items-center gap-2 text-sm text-slate-700 flex-wrap">
          <span className="font-medium">{q.id}.</span>
          {q.stem && <span>{q.stem}</span>}
          <div className="relative inline-flex items-center">
            <input
              type="text"
              className="border-2 border-green-600 rounded-full px-4 py-2 min-w-[180px] text-center font-medium text-white bg-white outline-none focus:ring-2 focus:ring-green-400"
              placeholder="..."
            />
          </div>
          {q.suffix && <span>{q.suffix}</span>}
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
          className="border-r border-slate-200 overflow-hidden"
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
          className="flex-1 overflow-y-auto p-8"
          style={{ backgroundColor: "#FFFFFF" }}
        >
          {questionSections.map((section) => (
            <div key={section.id} className="mb-10">
              {/* Header */}
              <div className="mb-6">
                <h2 className="text-green-700 font-bold text-lg mb-2">
                  {section.title}
                </h2>
                <p className="text-slate-600 text-sm italic mb-1">
                  {section.instruction}
                </p>
                {section.subInstruction && (
                  <p className="text-slate-700 text-sm">
                    {section.subInstruction}
                  </p>
                )}
              </div>

              {/* Legend for TRUE/FALSE/NOT GIVEN */}
              {section.legendItems && (
                <div className="mb-6 bg-slate-50 border border-slate-200 rounded-lg p-4">
                  {section.legendItems.map((item, idx) => (
                    <div key={idx} className="flex gap-3 mb-2 last:mb-0">
                      <span className="font-bold text-slate-700 min-w-[90px]">{item.value}</span>
                      <span className="text-slate-600 text-sm">{item.description}</span>
                    </div>
                  ))}
                </div>
              )}

              {/* Questions */}
              <div className="space-y-3">
                {section.questions.map((q) => (
                  <div key={q.id} className="bg-white">
                    {renderQuestion(q, section)}
                  </div>
                ))}
              </div>
            </div>
          ))}
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