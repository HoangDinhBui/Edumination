import React from "react";
import {
  // Icons cho Sidebar
  Star,
  Calendar,
  CheckSquare,
  ListChecks,
  CheckCircle2,
  XCircle,
  Clock,
  // Icons cho Review & Explanation
  Play,
  RotateCcw,
  RotateCw,
  Volume2,
  MapPin,
  FileText,
  BookMarked,
} from "lucide-react";
import Avatar from "../../assets/img/Ellipse 24.png";
import Navbar from "../../components/Navbar";
import EPicture from "../../assets/img/Rectangle 111141430.png";
import { useNavigate } from "react-router-dom"; // 👈 thêm dòng này
import { Trophy } from "lucide-react"; // 👈 biểu tượng cho nút

// --- DỮ LIỆU MẪU (Mock Data) ---

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

// === MOCK DATA ===
const mockTestHeaderData = {
  title: "IELTS Mock Test 2025 January",
  rating: 4.5,
  votes: 755,
  postedDate: "06 Jun 2025",
  testTaken: 1000,
  category: "IELTS",
  image: { EPicture },
};

// --- COMPONENT: TestHeader ---
const TestHeader: React.FC = () => {
  const data = mockTestHeaderData;
  const filledStars = Math.floor(data.rating);
  const hasHalfStar = data.rating % 1 !== 0;

  return (
    <section className="w-full p-8 rounded-2xl shadow-md border border-blue-100 flex flex-col md:flex-row items-start gap-8">
      {/* Hình ảnh minh họa */}
      <div className="w-full md:w-56 h-40 rounded-xl overflow-hidden shadow-lg flex-shrink-0 relative">
        <img
          src={Object.values(data.image)[0]}
          alt={data.title}
          className="w-full h-full object-cover transition-transform duration-500 hover:scale-105"
        />
        <div className="absolute bottom-2 left-2 bg-blue-600 text-white text-xs font-semibold px-3 py-1 rounded-md shadow-md">
          {data.category}
        </div>
      </div>

      {/* Nội dung thông tin */}
      <div className="flex-1">
        <h1 className="text-3xl font-bold text-slate-800 mb-3">{data.title}</h1>

        {/* Rating */}
        <div className="flex items-center mb-4">
          {[...Array(filledStars)].map((_, i) => (
            <Star key={i} className="w-5 h-5 text-yellow-400 fill-yellow-400" />
          ))}
          {hasHalfStar && (
            <Star className="w-5 h-5 text-yellow-400 opacity-60" />
          )}
          <span className="ml-2 text-sm text-slate-600 font-medium">
            ({data.votes} votes)
          </span>
        </div>

        {/* Meta info */}
        <div className="flex flex-wrap gap-6 text-sm text-slate-700">
          <div className="flex items-center gap-2">
            <Calendar className="w-5 h-5 text-blue-600" />
            <span>
              <strong>Posted on:</strong> {data.postedDate}
            </span>
          </div>
          <div className="flex items-center gap-2">
            <CheckSquare className="w-5 h-5 text-green-600" />
            <span>
              <strong>Test taken:</strong> {data.testTaken.toLocaleString()}
            </span>
          </div>
        </div>
      </div>
    </section>
  );
};

// --- COMPONENT: ResultDonut ---
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
  <div className="flex flex-col items-center justify-center bg-gradient-to-br from-white to-slate-50 p-8 rounded-2xl shadow-lg hover:shadow-xl transition-shadow">
    <div
      className={`w-36 h-36 rounded-full border-[10px] ${borderColorClass} flex items-center justify-center shadow-inner bg-white`}
    >
      {children}
    </div>
    <p className="mt-4 text-sm font-semibold text-slate-700 text-center">
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
// === COMPONENT: Review & Explanation ===
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

// === MOCK DATA CHO REVIEW & EXPLANATION ===
const mockReviewExplanationData = {
  title: "Review & Explanation",
  partTitle: "Part 1",
  questionRange: "Questions 1–5",
  instructionTitle: "The housing officer takes some details from the girl.",
  instructionNote:
    "Complete the following form with NO MORE THAN THREE WORDS AND/OR A NUMBER for each answer.",
  formTitle: "PERSONAL DETAILS FOR HOMESTAY APPLICATION",
  formData: [
    { label: "First name", answer: "Keiko" },
    { label: "Family name", answer: "Yuinichi", number: 1 },
    { label: "Gender", answer: "Female" },
    { label: "Age", answer: "28" },
    { label: "Passport number", answer: "", number: 2 },
    { label: "Nationality", answer: "Japanese" },
    { label: "Course enrolled", answer: "", number: 3 },
    { label: "Length of the course", answer: "", number: 4 },
    { label: "Homestay time", answer: "", number: 5 },
  ],
  reviewAnswers: [
    { number: 1, text: "Keiko" },
    { number: 2, text: "J06337" },
    { number: 3, text: "Advanced English studies" },
  ],
  audioScript: [
    "Please turn to section 1 of listening practice test. Listen to the conversation between a Japanese student and a housing officer and complete the form. First you have some time to look at questions 1 to 5. You will see that there is an example which has been done for you. The conversation relating to this will be played first.",
    "Man: Yes? What can I do for you?",
    "Girl: My friend is in a homestay and she really enjoys it, so I'd like to join a family as well.",
    "Man: Okay let me get some details. What's your name?",
    "Girl: My name is Keiko Yuichini.",
    "Man: Could you spell your family name for me, please?",
    "Girl: Yes. It's Yuichini. That's Y U I C H I N I.",
    "The student's family name is Yuichini. So that has been written on the form. Now we shall begin. You should answer the questions as you listen because you will not hear the recording a second time. Now listen carefully and answer questions 1 to 5.",
    "Man: Yes? What can I do for you?",
    "Girl: My friend is in a homestay and she really enjoys it so I'd like to join a family as well.",
  ],
};

// --- Component Review & Explanation chính ---
const ReviewExplanation: React.FC = () => {
  const data = mockReviewExplanationData;

  return (
    <section className="mt-8 bg-white rounded-2xl shadow-lg border-2 border-blue-500 overflow-hidden">
      {/* HEADER */}
      <div className="flex items-center gap-2 p-4 bg-blue-500 text-white">
        <BookMarked className="w-5 h-5" />
        <h2 className="text-xl font-bold">{data.title}</h2>
      </div>

      <div className="grid grid-cols-1 lg:grid-cols-2 gap-6 p-6">
        {/* === CỘT TRÁI === */}
        <div>
          <h3 className="font-semibold text-slate-800">{data.questionRange}</h3>
          <p className="text-sm text-slate-600 mt-1">{data.instructionTitle}</p>
          <p className="text-sm text-red-600 font-medium my-3">
            {data.instructionNote}
          </p>

          {/* FORM */}
          <div className="bg-blue-50 text-blue-800 font-semibold p-3 rounded-t-lg text-sm">
            {data.formTitle}
          </div>
          <div className="bg-white p-4 rounded-b-lg shadow-inner border border-slate-200">
            {data.formData.map((row, i) => (
              <ReviewFormRow
                key={i}
                label={row.label}
                answer={row.answer}
                number={row.number}
              />
            ))}
          </div>

          {/* CÂU TRẢ LỜI */}
          <div className="mt-6 space-y-4">
            {data.reviewAnswers.map((ans) => (
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

        {/* === CỘT PHẢI === */}
        <div>
          <AudioPlayerMockup />
          <h3 className="text-xl font-semibold text-slate-800 mt-6">
            {data.partTitle}
          </h3>
          <div className="mt-4 space-y-3 text-slate-700 text-sm leading-relaxed max-h-96 overflow-y-auto">
            {data.audioScript.map((line, i) => (
              <p key={i}>
                {line.includes("Man:") || line.includes("Girl:") ? (
                  <>
                    <strong>{line.split(":")[0]}:</strong> {line.split(":")[1]}
                  </>
                ) : (
                  line
                )}
              </p>
            ))}
          </div>
        </div>
      </div>
    </section>
  );
};

// --- COMPONENT: View Ranking Button ---
const ViewRankingButton: React.FC = () => {
  const navigate = useNavigate();

  return (
    <button
      onClick={() => navigate("/ranking")}
      className="mt-8 flex items-center gap-2 px-6 py-3 
                 bg-gradient-to-r from-sky-500 to-indigo-500 
                 text-white font-semibold rounded-full shadow-lg 
                 hover:shadow-xl hover:scale-105 transition-all duration-300"
    >
      <Trophy className="w-5 h-5 text-white" />
      <span>View Global Ranking</span>
    </button>
  );
};

// === MOCK DATA CHO ANSWER PAGE ===
const mockAnswerPageData = {
  user: {
    name: "Tran Dung",
    avatar: { Avatar },
  },
  result: {
    correctAnswers: "2/40",
    bandScore: "2",
    timeCompleted: "32:00",
  },
  answerKeys: [
    {
      title: "Part 1: Question 1 - 10",
      answers: [
        { id: 1, text: "Keiko", isCorrect: true },
        { id: 2, text: "J06337", isCorrect: false },
        { id: 3, text: "Advanced English studies", isCorrect: false },
        { id: 4, text: "5 months", isCorrect: true },
        { id: 5, text: "About 4 months", isCorrect: false },
      ],
    },
    {
      title: "Part 2: Question 11 - 20",
      answers: [
        { id: 11, text: "B, D: C, D (Correct 1/2)", isCorrect: false },
        { id: 12, text: "Seafood", isCorrect: true },
        { id: 13, text: "Tennis", isCorrect: false },
        { id: 14, text: "Take the train", isCorrect: false },
        { id: 15, text: "This afternoon", isCorrect: false },
      ],
    },
  ],
};

// --- COMPONENT TRANG CHÍNH ---
export default function AnswerPage() {
  const data = mockAnswerPageData;

  return (
    <div className="min-h-screen">
      <Navbar />

      {/* Nội dung chính - Full width với container */}
      <main className="w-full px-8 py-8 mt-10">
        {/* Header bài test */}
        <TestHeader />

        {/* Khối Avatar và Result */}
        <div className="flex flex-col items-center my-12">
          <div className="w-24 h-24 rounded-full overflow-hidden shadow-xl ring-4 ring-blue-100 bg-gradient-to-br from-blue-400 to-indigo-500 flex items-center justify-center">
            <img
              src={Object.values(data.user.avatar)[0]}
              alt={data.user.name}
              className="w-full h-full object-cover"
            />
          </div>
          <span className="mt-4 text-xl font-semibold text-slate-800">
            {data.user.name}
          </span>
          <h2 className="text-4xl font-bold text-slate-800 mt-6 mb-2">
            Result
          </h2>
          <div className="h-1 w-20 bg-gradient-to-r from-blue-500 to-indigo-500 rounded-full"></div>

          <ViewRankingButton />

        </div>

        {/* Phần Result Donut */}
        <section className="grid grid-cols-1 md:grid-cols-3 gap-8 mb-12">
          <ResultDonut
            subText="The correct answer"
            borderColorClass="border-slate-200"
          >
            <div className="flex flex-col items-center">
              <CheckSquare className="w-10 h-10 text-blue-600" />
              <span className="text-2xl font-bold text-slate-700 mt-2">
                {data.result.correctAnswers}
              </span>
            </div>
          </ResultDonut>

          <ResultDonut subText="Band score" borderColorClass="border-blue-300">
            <span className="text-6xl font-bold text-blue-600">
              {data.result.bandScore}
            </span>
          </ResultDonut>

          <ResultDonut
            subText="Time to complete the exam"
            borderColorClass="border-blue-500"
          >
            <div className="flex flex-col items-center">
              <Clock className="w-10 h-10 text-blue-600" />
              <span className="text-2xl font-bold text-slate-700 mt-2">
                {data.result.timeCompleted}
              </span>
            </div>
          </ResultDonut>
        </section>

        {/* Phần Answer Keys */}
        <section className="bg-white p-8 rounded-2xl shadow-lg border border-slate-200 mb-8">
          <div className="flex items-center gap-3 mb-8 pb-4 border-b-2 border-blue-100">
            <ListChecks className="w-7 h-7 text-blue-600" />
            <h2 className="text-2xl font-bold text-slate-800">Answer Keys</h2>
          </div>

          <div className="space-y-10">
            {data.answerKeys.map((part) => (
              <AnswerKeyPart
                key={part.title}
                title={part.title}
                answers={part.answers}
              />
            ))}
          </div>
        </section>

        {/* Component Review & Explanation */}
        <ReviewExplanation />
      </main>
    </div>
  );
}
