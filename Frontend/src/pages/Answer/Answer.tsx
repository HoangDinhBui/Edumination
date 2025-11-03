import React from "react";
import {
  // Icons cho Sidebar
  Library,
  Lightbulb,
  BookMarked,
  LayoutDashboard,
  History,
  // Icons cho Header & Result
  Star,
  Calendar,
  CheckSquare,
  User,
  ListChecks,
  CheckCircle2,
  XCircle,
  Clock, // Icon mới cho donut
  // Icons cho Review & Explanation
  Play,
  RotateCcw,
  RotateCw,
  Volume2,
  Cog,
  MapPin,
  FileText,
} from "lucide-react";
import Avatar from "../../assets/img/Ellipse 24.png"

import EPicture from "../../assets/img/Rectangle 111141430.png"

// --- DỮ LIỆU MẪU (Mock Data) ---

const navItems = [
  { name: "IELTS Exam Library", icon: Library, active: true },
  { name: "IELTS Tips", icon: Lightbulb, active: false },
  { name: "IELTS Courses", icon: BookMarked, active: false },
  { name: "My Dashboard", icon: LayoutDashboard, active: false },
  { name: "Practice Test History", icon: History, active: false },
];

const sampleAnswers = [
  { id: 1, text: "Keiko:", isCorrect: false },
  { id: 11, text: "B, D: C, D (Correct 1/2)", isCorrect: false },
  { id: 2, text: "J06337:", isCorrect: false },
  { id: 12, text: "Seafood:", isCorrect: true },
  { id: 3, text: "Advanced English studies:", isCorrect: false },
  { id: 13, text: "Tennis:", isCorrect: false },
  { id: 4, text: "5 months:", isCorrect: true },
  { id: 14, text: "Take the train:", isCorrect: false },
  { id: 5, text: "About 4 months:", isCorrect: false },
  { id: 15, text: "This afternoon:", isCorrect: false },
];

const answerKeyData = [
  { title: "Part 1: Question 1 - 10", answers: sampleAnswers.slice(0, 10) },
  { title: "Part 2: Question 11 - 20", answers: sampleAnswers.slice(0, 10) },
  { title: "Part 3: Question 21 - 30", answers: sampleAnswers.slice(0, 10) },
  { title: "Part 4: Question 31 - 40", answers: sampleAnswers.slice(0, 10) },
];

// --- COMPONENT: Sidebar ---
const Sidebar: React.FC = () => (
  <aside className="w-64 h-screen bg-white shadow-lg p-6 flex flex-col flex-shrink-0">
    <a href="#" className="text-3xl font-bold text-blue-600">
      ATY
    </a>
    <nav className="mt-10 space-y-2">
      {navItems.map((item) => (
        <a
          key={item.name}
          href="#"
          className={`flex items-center gap-3 px-4 py-3 rounded-lg font-medium ${
            item.active
              ? "bg-blue-100 text-blue-600"
              : "text-slate-600 hover:bg-slate-50"
          }`}
        >
          <item.icon className="w-5 h-5" />
          <span>{item.name}</span>
        </a>
      ))}
    </nav>
  </aside>
);

// --- COMPONENT: TestHeader (Đã cập nhật) ---
const TestHeader: React.FC = () => (
  <section className="bg-white p-6 rounded-lg shadow-sm flex items-start gap-6">
    <img
      src={EPicture} // <-- THAY THẾ ẢNH NÀY
      alt="IELTS Mock Test 2025"
      className="w-24 rounded-md flex-shrink-0"
    />
    <div className="flex-1">
      <h1 className="text-2xl font-bold text-slate-800">
        IELTS Mock Test 2025 January
      </h1>
      <div className="flex items-center mt-2">
        {[...Array(4)].map((_, i) => (
          <Star key={i} className="w-5 h-5 text-yellow-500 fill-yellow-500" />
        ))}
        <Star className="w-5 h-5 text-yellow-500" />
        <span className="ml-2 text-sm text-slate-500">(755 vote)</span>
      </div>
      <div className="mt-3 text-sm text-slate-600 space-y-2">
        <div className="flex items-center gap-2">
          <Calendar className="w-4 h-4 text-slate-500" />
          <span>Posted on: 06 Jun 2025</span>
        </div>
        <div className="flex items-center gap-2">
          <CheckSquare className="w-4 h-4 text-slate-500" />
          <span>Test taken: 1.000</span>
        </div>
      </div>
    </div>
  </section>
);

// --- COMPONENT: ResultDonut (Đã cập nhật để linh hoạt hơn) ---
interface ResultDonutProps {
  subText: string;
  borderColorClass: string;
  children: React.ReactNode;
}

const ResultDonut: React.FC<ResultDonutProps> = ({
  subText,
  borderColorClass,
  children,
}) => (
  <div className="flex flex-col items-center justify-center bg-white p-6 rounded-lg shadow-sm">
    <div
      className={`w-32 h-32 rounded-full border-8 ${borderColorClass} flex items-center justify-center`}
    >
      {children}
    </div>
    <p className="mt-3 text-sm font-medium text-slate-600 text-center">
      {subText}
    </p>
  </div>
);

// --- COMPONENT: AnswerKeyPart & AnswerItem ---
interface AnswerItemProps {
  number: number;
  text: string;
  isCorrect: boolean;
}
const AnswerItem: React.FC<AnswerItemProps> = ({ number, text, isCorrect }) => (
  <div className="flex items-center gap-3">
    <span className="flex-shrink-0 w-6 h-6 rounded-full bg-blue-600 text-white text-xs flex items-center justify-center font-bold">
      {number}
    </span>
    <span className="flex-1 text-slate-700 text-sm">{text}</span>
    {isCorrect ? (
      <CheckCircle2 className="w-5 h-5 text-green-500" />
    ) : (
      <XCircle className="w-5 h-5 text-red-500" />
    )}
  </div>
);

interface AnswerKeyPartProps {
  title: string;
  answers: { id: number; text: string; isCorrect: boolean }[];
}
const AnswerKeyPart: React.FC<AnswerKeyPartProps> = ({ title, answers }) => (
  <div>
    <h3 className="text-base font-semibold text-slate-800 border-b border-slate-200 pb-2 mb-4">
      {title}
    </h3>
    <div className="grid grid-cols-1 md:grid-cols-2 gap-x-8 gap-y-3">
      {answers.map((answer) => (
        <AnswerItem
          key={answer.id}
          number={answer.id}
          text={answer.text}
          isCorrect={answer.isCorrect}
        />
      ))}
    </div>
  </div>
);

// =======================================================
// === COMPONENT MỚI: Review & Explanation (Bắt đầu) ===
// =======================================================

// --- Helper: Nút bấm nhỏ (Listen, Locate, Explain) ---
const ReviewAnswerButton: React.FC<{
  icon: React.ElementType;
  text: string;
}> = ({ icon: Icon, text }) => (
  <button className="flex items-center gap-1.5 px-3 py-1 bg-white border border-slate-300 rounded-full text-xs text-slate-600 hover:bg-slate-50">
    <Icon className="w-3 h-3" />
    {text}
  </button>
);

// --- Helper: Hàng trong form (Label + Answer) ---
const ReviewFormRow: React.FC<{
  label: string;
  answer: string;
  number?: number;
}> = ({ label, answer, number }) => (
  <div className="flex items-center py-2 border-b border-slate-200">
    <span className="w-1/3 text-sm text-slate-600">{label}</span>
    <div className="flex-1 flex items-center gap-2">
      <span className="text-sm font-medium bg-slate-100 px-3 py-1 rounded w-full">
        {answer}
      </span>
      {number && (
        <span className="w-5 h-5 rounded-full bg-blue-600 text-white text-xs flex items-center justify-center font-bold flex-shrink-0">
          {number}
        </span>
      )}
    </div>
  </div>
);

// --- Helper: Giả lập Audio Player ---
const AudioPlayerMockup: React.FC = () => (
  <div className="bg-white border border-slate-200 rounded-lg p-3 flex items-center gap-3 shadow-sm">
    <button className="w-8 h-8 rounded-full bg-blue-600 text-white flex items-center justify-center flex-shrink-0">
      <Play className="w-4 h-4 fill-white" />
    </button>
    <span className="text-sm text-slate-500">00:00</span>
    {/* Thanh trượt (slider) */}
    <input
      type="range"
      defaultValue="0"
      className="flex-1 h-1.5 bg-slate-200 rounded-full appearance-none cursor-pointer"
    />
    <span className="text-sm text-slate-500">03:30</span>
    <button className="text-slate-500 hover:text-slate-800">
      <RotateCcw className="w-4 h-4" />
    </button>
    <button className="text-slate-500 hover:text-slate-800">
      <RotateCw className="w-4 h-4" />
    </button>
    <button className="text-slate-500 hover:text-slate-800">
      <Volume2 className="w-4 h-4" />
    </button>
    <button className="text-sm text-blue-600 hover:underline hidden md:block">
      Change Audio Sources
    </button>
  </div>
);

// --- Component Review & Explanation chính ---
const ReviewExplanation: React.FC = () => {
  // Dữ liệu mẫu cho component này
  const reviewAnswers = [
    { number: 1, text: "Keiko" },
    { number: 2, text: "J06337" },
    { number: 3, text: "Advanced English studies" },
  ];

  return (
    <section className="mt-8 bg-white rounded-2xl shadow-lg border-2 border-blue-500 overflow-hidden">
      <div className="flex items-center gap-2 p-4 bg-blue-500 text-white">
        <BookMarked className="w-5 h-5" />
        <h2 className="text-xl font-bold">Review & Explanation</h2>
      </div>

      <div className="grid grid-cols-1 lg:grid-cols-2 gap-6 p-6">
        {/* === CỘT BÊN TRÁI === */}
        <div>
          <h3 className="font-semibold text-slate-800">Questions 1-5</h3>
          <p className="text-sm text-slate-600 mt-1">
            The housing officer takes some details from the girl.
          </p>
          <p className="text-sm text-red-600 font-medium my-3">
            Complete the following form with{" "}
            <strong>NO MORE THAN THREE WORDS AND/OR A NUMBER</strong> for each
            answer.
          </p>

          {/* Form */}
          <div className="bg-blue-50 text-blue-800 font-semibold p-3 rounded-t-lg text-sm">
            PERSONAL DETAILS FOR HOMESTAY APPLICATION
          </div>
          <div className="bg-white p-4 rounded-b-lg shadow-inner border border-slate-200">
            <ReviewFormRow label="First name" answer="Keiko" />
            <ReviewFormRow label="Family name" answer="Yuinichi" number={1} />
            <ReviewFormRow label="Gender" answer="Female" />
            <ReviewFormRow label="Age" answer="28" />
            <ReviewFormRow label="Passport number" answer="" number={2} />
            <ReviewFormRow label="Nationality" answer="Japanese" />
            <ReviewFormRow label="Course enrolled" answer="" number={3} />
            <ReviewFormRow label="Length of the course" answer="" number={4} />
            <ReviewFormRow label="Homestay time" answer="" number={5} />
          </div>

          {/* Danh sách câu trả lời */}
          <div className="mt-6 space-y-4">
            {reviewAnswers.map((ans) => (
              <div key={ans.number}>
                <p className="text-sm text-slate-800">
                  {ans.number}. Answer:{" "}
                  <strong className="font-semibold">{ans.text}</strong>
                </p>
                <div className="flex items-center gap-2 mt-2">
                  <ReviewAnswerButton icon={Play} text="Listen from here" />
                  <ReviewAnswerButton icon={MapPin} text="Locate" />
                  <ReviewAnswerButton icon={FileText} text="Explain" />
                </div>
              </div>
            ))}
          </div>
        </div>

        {/* === CỘT BÊN PHẢI === */}
        <div>
          <AudioPlayerMockup />
          <h3 className="text-xl font-semibold text-slate-800 mt-6">Part 1</h3>
          <div className="mt-4 space-y-3 text-slate-700 text-sm leading-relaxed max-h-96 overflow-y-auto">
            <p>
              Please turn to section 1 of listening practice test. Listen to the
              conversation between a Japanese student and a housing officer and
              complete the form. First you have some time to look at questions 1
              to 5. You will see that there is an example which has been done for
              you. The conversation relating to this will be played first.
            </p>
            <p>
              <strong>Man:</strong> Yes? What can I do for you?
            </p>
            <p>
              <strong>Girl:</strong> My friend is in a homestay and she really
              enjoys it, so I’d like to join a family as well.
            </p>
            <p>
              <strong>Man:</strong> Okay let me get some details. What’s your
              name?
            </p>
            <p>
              <strong>Girl:</strong> My name is Keiko Yuichini.
            </p>
            <p>
              <strong>Man:</strong> Could you spell your family name for me,
              please?
            </p>
            <p>
              <strong>Girl:</strong> Yes. It’s Yuichini. That’s Y U I C H I N I.
            </p>
            <p>
              The student’s family name is Yuichini. So that has been written on
              the form. Now we shall begin. You should answer the questions as
              you listen because you will not hear the recording a second time.
              Now listen carefully and answer questions 1 to 5.
            </p>
            <p>
              <strong>Man:</strong> Yes? What can I do for you?
            </p>
            <p>
              <strong>Girl:</strong> My friend is in a homestay and she really
              enjoys it so I’d like to join a family as well.
            </p>
          </div>
        </div>
      </div>
    </section>
  );
};

// =====================================================
// === COMPONENT MỚI: Review & Explanation (Kết thúc) ===
// =====================================================

// --- COMPONENT TRANG CHÍNH (Đã cập nhật) ---

export default function AnswerPage() {
  return (
    <div className="min-h-screen flex bg-slate-50">
      {/* 1. Sidebar */}
      <Sidebar />

      {/* 2. Nội dung chính */}
      <main className="flex-1 p-8 overflow-y-auto">
        {/* Header bài test */}
        <TestHeader />

        {/* Khối Avatar và Result (Căn giữa) */}
        <div className="flex flex-col items-center my-8">
          <div className="w-20 h-20 rounded-full overflow-hidden shadow-md">
            <img
              src={Avatar} // <-- THAY THẾ ẢNH AVATAR
              alt="Tran Dung"
              className="w-full h-full object-cover"
            />
          </div>
          <span className="mt-3 text-lg font-medium text-slate-800">
            Tran Dung
          </span>
          <h2 className="text-3xl font-bold text-slate-800 mt-4">Result</h2>
        </div>

        {/* Phần Result Donut (Cập nhật theo ảnh closeup mới) */}
        <section className="grid grid-cols-1 md:grid-cols-3 gap-6">
          <ResultDonut
            subText="The correct answer"
            borderColorClass="border-gray-300"
          >
            <div className="flex flex-col items-center">
              <CheckSquare className="w-8 h-8 text-blue-600" />
              <span className="text-xl font-bold text-slate-700 mt-1">
                2/40
              </span>
            </div>
          </ResultDonut>

          <ResultDonut
            subText="Band score"
            borderColorClass="border-blue-200"
          >
            <span className="text-5xl font-bold text-blue-600">2</span>
          </ResultDonut>

          <ResultDonut
            subText="Time to complete the exam"
            borderColorClass="border-blue-600"
          >
            <div className="flex flex-col items-center">
              <Clock className="w-8 h-8 text-blue-600" />
              <span className="text-xl font-bold text-slate-700 mt-1">
                32:00
              </span>
            </div>
          </ResultDonut>
        </section>

        {/* Phần Answer Keys */}
        <section className="mt-8 bg-white p-6 rounded-lg shadow-sm">
          <div className="flex items-center gap-2 mb-6">
            <ListChecks className="w-6 h-6 text-blue-600" />
            <h2 className="text-xl font-bold text-slate-800">Answer Keys</h2>
          </div>
          <div className="space-y-8">
            {answerKeyData.map((part) => (
              <AnswerKeyPart
                key={part.title}
                title={part.title}
                answers={part.answers}
              />
            ))}
          </div>
        </section>

        {/* === PHẦN MỚI THÊM === */}
        {/* Component Review & Explanation được thêm vào đây */}
        <ReviewExplanation />
      </main>
    </div>
  );
}