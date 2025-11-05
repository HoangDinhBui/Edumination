import React, { useState, useEffect, useRef } from "react";
import { Clock, FileText, Menu, FileEdit } from "lucide-react";
import edmLogo from "../../assets/img/edm-logo.png";
import readingImg from "../../assets/img/readingImg.png";

// =================== MOCK DATA ===================
// MOCK DATA — dùng lại cho cả phần câu hỏi và footer
const questionSections = [
  {
    id: 1,
    partLabel: "Part 1",
    title: "Questions 1–4",
    instruction:
      "The text has 5 paragraphs (A–E). Which paragraph contains each of the following pieces of information?",
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
    title: "Questions 5–8",
    instruction:
      "Complete the following sentences using NO MORE THAN THREE WORDS from the text for each gap.",
    qtype: "FILL_BLANK",
    questions: [
      {
        id: 5,
        stem: "Safaricom is the ____ mobile phone company in Kenya.",
        meta_json: { blanks: 1 },
      },
      {
        id: 6,
        stem: "An M-Pesa account needs to be credited by ____.",
        meta_json: { blanks: 1 },
      },
      {
        id: 7,
        stem: "____ companies are particularly interested in using M-Pesa.",
        meta_json: { blanks: 1 },
      },
      {
        id: 8,
        stem: "Companies like Moneygram and Western Union have ____ the international money transfer market.",
        meta_json: { blanks: 1 },
      },
    ],
  },
  {
    id: 3,
    partLabel: "Part 1",
    title: "Questions 9–13",
    instruction:
      "Do the statements agree with the information given in Reading Passage 1? In boxes 9–13 on your answer sheet, write:",
    qtype: "SHORT_ANSWER",
    meta_json: { options: ["TRUE", "FALSE", "NOT GIVEN"] },
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
const TopNavbar: React.FC = () => {
  const { mins, secs, isWarning, timeLeft } = useCountdown(60);

  useEffect(() => {
    if (timeLeft === 0) {
      alert("⏰ Time’s up! Your test will be submitted automatically.");
    }
  }, [timeLeft]);

  return (
    <nav className="w-full bg-white shadow-sm sticky top-0 z-50 border-b border-slate-200">
      <div className="grid grid-cols-3 items-center px-6 py-3 h-16">
        {/* LEFT: Logo */}
        <div className="flex items-center">
          <img src={edmLogo} alt="EDM" className="h-8 w-auto" />
        </div>

        {/* CENTER: Timer */}
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

        {/* RIGHT: Buttons */}
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
const ReadingPassage: React.FC = () => {
  const passage = {
    part: "PART 1",
    title: "READING PASSAGE 1",
    instruction:
      "You should spend about 20 minutes on Questions 1–13, which are based on Reading Passage 1 below.",
    image: readingImg,
    subtitle: "Reading Passage 1",
    subdesc: "The impact of climate change on butterflies in Britain",
    articleTitle: "Money Transfers by Mobile",
    paragraphs: [
      {
        label: "A.",
        text: `The ping of a text message has never sounded so sweet. In what is being touted as a world first, Kenya’s biggest mobile operator is allowing subscribers to send cash to other phones by SMS. Known as M-Pesa, or mobile money, the service is expected to revolutionise banking in a country where more than 80% of people are excluded from the traditional financial sector. Apart from transferring cash – a service much in demand among urban Kenyans supporting relatives in rural areas – customers of Safaricom will be able to keep up to 50,000 shillings (£370) in a “virtual account” on their handsets.`,
      },
      {
        label: "B.",
        text: `Developed by Vodafone, which holds a 35% share in Safaricom, M-Pesa was formally launched in Kenya two weeks ago. More than 10,000 people have signed up for the service, with around 8 million shillings transferred so far, mostly in tiny denominations. Safaricom’s executives are confident that growth will be strong in Kenya, and later across Africa. “We are effectively giving people ATM cards without them ever having to open a bank account,” said Michael Joseph, chief executive of Safaricom, who said the money transfer concept the “next big thing” in mobile telephony.`,
      },
      {
        label: "C.",
        text: `M-Pesa’s simple. There is no need for a new handset or SIM card. To send money, you hand over the cash to a registered agent – typically a retailer – who credits your virtual account. You can then send between 100 shillings (74p) and 35,000 shillings (£259) via text message to the desired recipient – even someone on a different mobile network – who cashes it at an agent by entering a secret code and showing ID. A commission of up to 170 shillings (£1.25) is paid by the recipient but it compares favourably with fees levied by the major banks, whose services are too expensive for most of the population.`,
      },
      {
        label: "D.",
        text: `Mobile phone growth in Kenya, as in most of Africa, has been remarkable, even among the rural poor. In late 1999, Kenya had 15,000 mobile subscribers. Today, it has nearly 8 million out of a population of 35 million, and the two operators’ networks are as extensive as the access to banks is limited. Safaricom says it is not now competing with financial services companies as filling a void. In time, M-Pesa will allow people to borrow and repay money, and make purchases. Companies will be able to pay salaries directly into workers’ phones – something that has already proved attractive to aid agencies. In areas where banking is limited, those workers often have to be paid in cash as they do not have bank accounts. There are concerns about security, but Safaricom insists that even if someone’s phone is stolen, the PIN system prevents unauthorised withdrawals. Mr. Joseph said the only danger is sending cash to the wrong mobile number and the recipient redeeming it straight away.`,
      },
      {
        label: "E.",
        text: `The project is being watched closely by mobile operators around the world as a way of targeting the multibillion unbanked population cash transfer industry and competing with companies such as Western Union and Moneygram. Remittances from nearly 200 million migrant workers to developing countries totalled $102 billion last year, according to the World Bank. The GSM Association, the industry group for mobile operators worldwide, believes this could quadruple by 2012 if transfers by SMS become the norm. Vodafone has entered a partnership with Citigroup that will soon allow Kenyans in the UK to send money home via text message. The charge for sending £30 is expected to be about £3, less than a third of what some traditional services charge.`,
      },
    ],
  };

  return (
    <div className="p-8 overflow-y-auto h-full bg-[#F0FAF1] scroll-smooth text-slate-800 leading-relaxed">
      {/* Header */}
      <h2 className="text-[#294563] font-semibold text-sm uppercase tracking-wide">
        {passage.part}
      </h2>
      <h1 className="text-2xl font-bold text-[#294563] mt-1 mb-1">
        {passage.title}
      </h1>
      <p className="text-slate-600 italic text-sm mb-6">
        {passage.instruction}
      </p>

      {/* Image and subheader */}
      <div className="mb-8">
        {/* Hình lớn trên cùng */}
        <img
          src={passage.image}
          alt="Reading"
          className="w-full h-64 sm:h-80 object-cover rounded-xl shadow-md border border-green-100"
        />

        {/* Subtitle nằm dưới hình */}
        <div className="text-center mt-4">
          <h3 className="text-[#294563] font-semibold text-xl">
            {passage.subtitle}
          </h3>
          <p className="text-[#537D92] text-sm mt-1">{passage.subdesc}</p>
        </div>
      </div>

      {/* Content */}
      <h2 className="text-xl text-center font-bold text-[#294563] mb-4">
        {passage.articleTitle}
      </h2>

      <div className="space-y-5">
        {passage.paragraphs.map((p) => (
          <p key={p.label}>
            <span className="font-bold text-[#474e48] mr-2">{p.label}</span>
            {p.text}
          </p>
        ))}
      </div>
    </div>
  );
};

// =================== MAIN PAGE ===================
const ReadingPage: React.FC = () => {
  const [dividerX, setDividerX] = useState(50);
  const dragging = useRef(false);

  const startDrag = () => (dragging.current = true);
  const stopDrag = () => (dragging.current = false);

  const handleMove = (e: MouseEvent) => {
    if (!dragging.current) return;
    const newX = (e.clientX / window.innerWidth) * 100;
    if (newX > 20 && newX < 80) setDividerX(newX);
  };

  useEffect(() => {
    window.addEventListener("mousemove", handleMove);
    window.addEventListener("mouseup", stopDrag);
    return () => {
      window.removeEventListener("mousemove", handleMove);
      window.removeEventListener("mouseup", stopDrag);
    };
  }, []);

  return (
    <div className="w-screen h-screen flex flex-col overflow-hidden bg-slate-50">
      <TopNavbar />

      <div className="flex flex-1 overflow-hidden h-[calc(100vh-4rem)]">
        {/* LEFT */}
        <div
          className="border-r border-slate-200"
          style={{ width: `${dividerX}%`, backgroundColor: "#F0FAF1" }}
        >
          <ReadingPassage />
        </div>

        {/* Divider */}
        <div
          className="w-1 bg-slate-300 hover:bg-blue-500 cursor-col-resize transition-colors"
          onMouseDown={startDrag}
        />

        {/* RIGHT */}
        <div
          className="flex-1 overflow-y-auto p-10 border-l border-slate-200"
          style={{ backgroundColor: "#F8F9F9" }}
        >
          {questionSections.map((section) => (
            <div
              key={section.id}
              data-section-id={section.id}
              className="mb-10"
            >
              <h2 className="text-green-700 font-semibold text-xl mb-1">
                {section.title}
              </h2>
              <p className="text-slate-700 text-sm mb-6">
                {section.instruction}
              </p>

              <div className="space-y-4">
                {/* --- MCQ (Multiple Choice) --- */}
                {section.qtype === "MCQ" &&
                  section.questions.map((q) => {
                    const opts = q.meta_json?.options ?? []; // an toàn nếu options undefined
                    return (
                      <div
                        key={q.id}
                        className="flex items-start gap-3 p-4 rounded-lg border hover:shadow transition"
                      >
                        <span className="text-slate-700 font-medium w-5">
                          {q.id}.
                        </span>
                        <select className="border border-slate-300 rounded-full px-4 py-2 w-28 text-slate-700 text-base font-medium focus:ring-2 focus:ring-green-500 outline-none cursor-pointer">
                          <option value="">Select</option>
                          {opts.map((opt: string) => (
                            <option key={opt} value={opt}>
                              {opt}
                            </option>
                          ))}
                        </select>
                        <p className="text-slate-800 text-sm leading-snug">
                          {q.stem}
                        </p>
                      </div>
                    );
                  })}

                {/* --- FILL_BLANK --- */}
                {section.qtype === "FILL_BLANK" &&
                  section.questions.map((q) => {
                    const blanks = q.meta_json?.blanks ?? 1; // mặc định có ít nhất 1 ô trống
                    return (
                      <div key={q.id} className="p-4 rounded-lg border">
                        <p className="text-slate-800 text-sm leading-snug mb-2">
                          {q.id}.{" "}
                          {q.stem.split("____").map((part, idx) => (
                            <React.Fragment key={idx}>
                              {part}
                              {idx < blanks && (
                                <input
                                  type="text"
                                  className="border-b border-slate-400 focus:border-green-600 outline-none px-2 w-40 mx-1"
                                />
                              )}
                            </React.Fragment>
                          ))}
                        </p>
                      </div>
                    );
                  })}

                {/* --- SHORT_ANSWER (TRUE/FALSE/NOT GIVEN) --- */}
                {section.qtype === "SHORT_ANSWER" && (
                  <>
                    {/* Bảng hướng dẫn TRUE/FALSE/NOT GIVEN */}
                    <div className="mb-6 rounded-md overflow-hidden border border-slate-300">
                      <div className="grid grid-cols-2 text-sm">
                        <div className="bg-slate-200 font-semibold px-3 py-2 text-slate-800">
                          TRUE.
                        </div>
                        <div className="bg-slate-200 px-3 py-2 text-slate-700">
                          if the statement agrees with the information
                        </div>

                        <div className="bg-white font-semibold px-3 py-2 text-slate-800">
                          FALSE.
                        </div>
                        <div className="bg-white px-3 py-2 text-slate-700">
                          if the statement contradicts the information
                        </div>

                        <div className="bg-slate-200 font-semibold px-3 py-2 text-slate-800">
                          NOT GIVEN.
                        </div>
                        <div className="bg-slate-200 px-3 py-2 text-slate-700">
                          If there is no information on this
                        </div>
                      </div>
                    </div>

                    {/* Danh sách câu hỏi */}
                    {section.questions.map((q) => {
                      const opts = section.meta_json?.options ?? [
                        "TRUE",
                        "FALSE",
                        "NOT GIVEN",
                      ];
                      return (
                        <div
                          key={q.id}
                          className="flex flex-col gap-2 p-4 border rounded-lg"
                        >
                          <p className="text-slate-800 text-sm">
                            {q.id}. {q.stem}
                          </p>
                          <div className="flex gap-4 mt-1">
                            {opts.map((opt: string) => (
                              <label
                                key={opt}
                                className="flex items-center gap-2 text-sm text-slate-700"
                              >
                                <input
                                  type="radio"
                                  name={`q${q.id}`}
                                  value={opt}
                                  className="accent-green-600"
                                />
                                {opt}
                              </label>
                            ))}
                          </div>
                        </div>
                      );
                    })}
                  </>
                )}
              </div>
            </div>
          ))}
        </div>
      </div>
    </div>
  );
};

// =================== FOOTER NAVIGATOR (SIMPLE CLEAN VERSION) ===================
import { createRoot } from "react-dom/client";

const ReadingFooter: React.FC = () => {
  const [activePart, setActivePart] = useState<number>(1);

  const parts = [
    { id: 1, label: "Part 1", path: "/" },
    { id: 2, label: "Part 2", path: "/reading/part/2" },
    { id: 3, label: "Part 3", path: "/reading/part/3" },
  ];

  const handleSelectPart = (id: number, path: string) => {
    setActivePart(id);
    window.location.href = path; // Nếu dùng react-router → navigate(path)
  };

  return (
    <footer className="fixed bottom-0 left-0 w-full bg-white border-t border-slate-300 shadow-md z-[999]">
      <div className="flex justify-evenly items-center h-16 px-6 max-w-[1400px] mx-auto">
        {parts.map((p) => (
          <button
            key={p.id}
            onClick={() => handleSelectPart(p.id, p.path)}
            className={`flex-1 mx-2 py-3 rounded-xl border transition-all duration-300 text-base font-semibold
              ${
                activePart === p.id
                  ? "border-green-600 text-green-700 bg-green-50 shadow-sm scale-[1.02]"
                  : "border-slate-300 text-slate-700 hover:bg-slate-100 hover:border-green-400"
              }`}
          >
            {p.label}
          </button>
        ))}
      </div>
    </footer>
  );
};

// Mount Footer vào body (React 18)
(function mountFooter() {
  if (typeof document === "undefined") return;
  let rootEl = document.getElementById("reading-footer-root");
  if (rootEl) return;

  rootEl = document.createElement("div");
  rootEl.id = "reading-footer-root";
  document.body.appendChild(rootEl);

  const root = createRoot(rootEl);
  root.render(<ReadingFooter />);
})();

  



export default ReadingPage;
