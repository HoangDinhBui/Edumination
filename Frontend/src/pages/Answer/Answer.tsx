import React, { useEffect, useState } from "react";
import { useLocation, useNavigate } from "react-router-dom";
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
  Trophy,
  Loader2, // Thêm Loader
} from "lucide-react";

// Giữ lại các import ảnh cũ của bạn
import AvatarDefault from "../../assets/img/Ellipse 24.png";
import EPicture from "../../assets/img/Rectangle 111141430.png";
import Navbar from "../../components/Navbar";

// --- COMPONENT: TestHeader (Đã sửa để nhận Props động) ---
interface TestHeaderProps {
  title: string;
  date: string;
  testTaken?: number;
}

const TestHeader: React.FC<TestHeaderProps> = ({
  title,
  date,
  testTaken = 1000,
}) => {
  // Mock static data cho phần hiển thị đẹp (Rating, Category...)
  const staticData = {
    rating: 4.5,
    votes: 755,
    category: "IELTS",
    image: EPicture,
  };

  React.useEffect(() => {
    document.body.style.overflow = "auto";
    document.documentElement.style.overflow = "auto";
    return () => {
      document.body.style.overflow = "";
      document.documentElement.style.overflow = "";
    };
  }, []);

  const filledStars = Math.floor(staticData.rating);
  const hasHalfStar = staticData.rating % 1 !== 0;

  return (
    <section className="w-full p-8 rounded-2xl shadow-md border border-blue-100 flex flex-col md:flex-row items-start gap-8 bg-white">
      {/* Hình ảnh minh họa */}
      <div className="w-full md:w-56 h-40 rounded-xl overflow-hidden shadow-lg flex-shrink-0 relative">
        <img
          src={staticData.image}
          alt={title}
          className="w-full h-full object-cover transition-transform duration-500 hover:scale-105"
        />
        <div className="absolute bottom-2 left-2 bg-blue-600 text-white text-xs font-semibold px-3 py-1 rounded-md shadow-md">
          {staticData.category}
        </div>
      </div>

      {/* Nội dung thông tin */}
      <div className="flex-1">
        <h1 className="text-3xl font-bold text-slate-800 mb-3">{title}</h1>

        {/* Rating */}
        <div className="flex items-center mb-4">
          {[...Array(filledStars)].map((_, i) => (
            <Star key={i} className="w-5 h-5 text-yellow-400 fill-yellow-400" />
          ))}
          {hasHalfStar && (
            <Star className="w-5 h-5 text-yellow-400 opacity-60" />
          )}
          <span className="ml-2 text-sm text-slate-600 font-medium">
            ({staticData.votes} votes)
          </span>
        </div>

        {/* Meta info */}
        <div className="flex flex-wrap gap-6 text-sm text-slate-700">
          <div className="flex items-center gap-2">
            <Calendar className="w-5 h-5 text-blue-600" />
            <span>
              <strong>Date:</strong> {new Date().toLocaleDateString()}{" "}
              {/* Hoặc dùng props date */}
            </span>
          </div>
          <div className="flex items-center gap-2">
            <CheckSquare className="w-5 h-5 text-green-600" />
            <span>
              <strong>Test taken:</strong> {testTaken.toLocaleString()}
            </span>
          </div>
        </div>
      </div>
    </section>
  );
};

// --- COMPONENT: ResultDonut (Giữ nguyên) ---
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

// --- COMPONENT: AnswerKeyPart & AnswerItem (Giữ nguyên) ---
interface AnswerItemProps {
  number: number;
  userAnswer: string;
  correctAnswer: string;
  isCorrect: boolean;
}

const AnswerItem: React.FC<AnswerItemProps> = ({
  number,
  userAnswer,
  correctAnswer,
  isCorrect,
}) => (
  <div className="flex items-start gap-3 p-2 rounded-lg hover:bg-slate-50 transition-colors">
    <span className="flex-shrink-0 w-6 h-6 rounded-full bg-blue-600 text-white text-xs flex items-center justify-center font-bold mt-0.5">
      {number}
    </span>
    <div className="flex-1 flex flex-col text-sm">
      <div className="flex gap-1">
        <span className="text-slate-500 font-medium w-14">Your:</span>
        <span
          className={`font-semibold ${
            isCorrect ? "text-green-600" : "text-red-500"
          }`}
        >
          {userAnswer || "(No Answer)"}
        </span>
      </div>
      {!isCorrect && (
        <div className="flex gap-1 mt-1">
          <span className="text-slate-500 w-14">Key:</span>
          <span className="font-semibold text-green-600">{correctAnswer}</span>
        </div>
      )}
    </div>
    <div className="mt-0.5">
      {isCorrect ? (
        <CheckCircle2 className="w-5 h-5 text-green-500" />
      ) : (
        <XCircle className="w-5 h-5 text-red-500" />
      )}
    </div>
  </div>
);

interface AnswerKeyPartProps {
  title: string;
  answers: any[];
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
          number={answer.position}
          userAnswer={answer.userAnswer}
          correctAnswer={answer.correctAnswer}
          isCorrect={answer.isCorrect}
        />
      ))}
    </div>
  </div>
);

// =======================================================
// === COMPONENT: Review & Explanation (MOCK DATA - Để Designer vui) ===
// =======================================================

const ReviewAnswerButton: React.FC<{
  icon: React.ElementType;
  text: string;
}> = ({ icon: Icon, text }) => (
  <button className="flex items-center gap-1.5 px-3 py-1 bg-white border border-slate-300 rounded-full text-xs text-slate-600 hover:bg-slate-50">
    <Icon className="w-3 h-3" />
    {text}
  </button>
);

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

const AudioPlayerMockup: React.FC = () => (
  <div className="bg-white border border-slate-200 rounded-lg p-3 flex items-center gap-3 shadow-sm">
    <button className="w-8 h-8 rounded-full bg-blue-600 text-white flex items-center justify-center flex-shrink-0">
      <Play className="w-4 h-4 fill-white" />
    </button>
    <span className="text-sm text-slate-500">00:00</span>
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
  ],
  reviewAnswers: [
    { number: 1, text: "Keiko" },
    { number: 2, text: "J06337" },
  ],
  audioScript: [
    "Man: Yes? What can I do for you?",
    "Girl: My friend is in a homestay and she really enjoys it, so I'd like to join a family as well.",
    "Man: Okay let me get some details. What's your name?",
    "Girl: My name is Keiko Yuichini.",
  ],
};

const ReviewExplanation: React.FC = () => {
  const data = mockReviewExplanationData;
  return (
    <section className="mt-8 bg-white rounded-2xl shadow-lg border-2 border-blue-500 overflow-hidden">
      <div className="flex items-center gap-2 p-4 bg-blue-500 text-white">
        <BookMarked className="w-5 h-5" />
        <h2 className="text-xl font-bold">{data.title}</h2>
      </div>
      <div className="grid grid-cols-1 lg:grid-cols-2 gap-6 p-6">
        <div>
          <h3 className="font-semibold text-slate-800">{data.questionRange}</h3>
          <p className="text-sm text-slate-600 mt-1">{data.instructionTitle}</p>
          <p className="text-sm text-red-600 font-medium my-3">
            {data.instructionNote}
          </p>
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
        </div>
        <div>
          <AudioPlayerMockup />
          <h3 className="text-xl font-semibold text-slate-800 mt-6">
            {data.partTitle}
          </h3>
          <div className="mt-4 space-y-3 text-slate-700 text-sm leading-relaxed max-h-96 overflow-y-auto">
            {data.audioScript.map((line, i) => (
              <p key={i}>{line}</p>
            ))}
          </div>
        </div>
      </div>
    </section>
  );
};

const ViewRankingButton: React.FC = () => {
  const navigate = useNavigate();
  return (
    <button
      onClick={() => navigate("/ranking")}
      className="mt-8 flex items-center gap-2 px-6 py-3 bg-gradient-to-r from-sky-500 to-indigo-500 text-white font-semibold rounded-full shadow-lg hover:shadow-xl hover:scale-105 transition-all duration-300"
    >
      <Trophy className="w-5 h-5 text-white" />
      <span>View Global Ranking</span>
    </button>
  );
};

// =================== TRANG CHÍNH ANSWER PAGE ===================
export default function AnswerPage() {
  const location = useLocation();
  const navigate = useNavigate();

  // Lấy ID từ state được truyền từ trang Reading/Listening
  const { attemptId, sectionId } = location.state || {};

  const [data, setData] = useState<any>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  // --- FETCH DATA TỪ API ---
  useEffect(() => {
    if (!attemptId || !sectionId) {
      // Nếu vào trực tiếp mà không qua nộp bài -> Quay về thư viện
      // Hoặc có thể hiển thị Mock Data nếu muốn test UI
      // navigate("/library");
      setError("Không tìm thấy thông tin bài thi. Vui lòng nộp bài trước.");
      setLoading(false);
      return;
    }

    const fetchResult = async () => {
      try {
        const TOKEN = localStorage.getItem("Token");
        const res = await fetch(
          `http://localhost:8081/api/v1/attempts/${attemptId}/sections/${sectionId}/result`,
          {
            headers: {
              Authorization: `Bearer ${TOKEN}`,
            },
          }
        );

        if (!res.ok) {
          throw new Error("Failed to load result");
        }

        const resultData = await res.json();
        setData(resultData);
      } catch (err: any) {
        console.error("Error fetching result:", err);
        setError("Có lỗi khi tải kết quả bài thi.");
      } finally {
        setLoading(false);
      }
    };

    fetchResult();
  }, [attemptId, sectionId, navigate]);

  // --- XỬ LÝ TRẠNG THÁI ---
  if (loading) {
    return (
      <div className="min-h-screen flex flex-col items-center justify-center bg-slate-50">
        <Loader2 className="w-12 h-12 text-blue-600 animate-spin mb-4" />
        <p className="text-slate-600">Đang tính điểm và tổng hợp kết quả...</p>
      </div>
    );
  }

  if (error) {
    return (
      <div className="min-h-screen flex flex-col items-center justify-center bg-slate-50 text-center">
        <XCircle className="w-16 h-16 text-red-500 mb-4" />
        <h2 className="text-2xl font-bold text-slate-800">Đã có lỗi xảy ra</h2>
        <p className="text-slate-600 mt-2">{error}</p>
        <button
          onClick={() => navigate("/library")}
          className="mt-6 px-6 py-2 bg-blue-600 text-white rounded-full"
        >
          Quay lại thư viện
        </button>
      </div>
    );
  }

  if (!data) return null;

  // --- MAP DATA TỪ API SANG FORMAT CỦA UI ---

  // 1. Nhóm câu hỏi theo Part (PassageTitle)
  // API trả về list câu hỏi phẳng, ta cần gom lại để hiển thị theo nhóm
  const groupedQuestions = data.Questions.reduce((acc: any, q: any) => {
    const key = q.PartTitle || "Questions"; // Dùng PartTitle làm key nhóm
    if (!acc[key]) {
      acc[key] = [];
    }
    acc[key].push({
      id: q.Id,
      position: q.Position,
      userAnswer: q.UserAnswerText,
      correctAnswer: q.CorrectAnswerText,
      isCorrect: q.IsCorrect,
    });
    return acc;
  }, {});

  // Chuyển object grouped thành mảng để map
  const answerKeys = Object.entries(groupedQuestions).map(
    ([title, answers]) => ({
      title,
      answers,
    })
  );

  return (
    <div className="min-h-screen bg-slate-50">
      <Navbar />

      {/* Nội dung chính - Full width với container */}
      <main className="w-full px-8 py-8 mt-10">
        {/* Header bài test */}
        <TestHeader
          title={data.PaperTitle}
          date={new Date().toLocaleDateString("en-GB")}
        />

        {/* Khối Avatar và Result */}
        <div className="flex flex-col items-center my-12">
          <div className="w-24 h-24 rounded-full overflow-hidden shadow-xl ring-4 ring-blue-100 bg-gradient-to-br from-blue-400 to-indigo-500 flex items-center justify-center">
            {data.AvatarUrl ? (
              <img
                src={data.AvatarUrl}
                alt={data.CandidateName}
                className="w-full h-full object-cover"
              />
            ) : (
              <span className="text-2xl font-bold text-white">
                {data.CandidateName?.charAt(0)?.toUpperCase()}
              </span>
            )}
          </div>
          <span className="mt-4 text-xl font-semibold text-slate-800">
            {data.CandidateName}
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
                {data.RawScore} {/* Ví dụ: "35/40" */}
              </span>
            </div>
          </ResultDonut>

          <ResultDonut subText="Band score" borderColorClass="border-blue-300">
            <span className="text-6xl font-bold text-blue-600">
              {data.BandScore || "0.0"}
            </span>
          </ResultDonut>

          <ResultDonut
            subText="Time to complete the exam"
            borderColorClass="border-blue-500"
          >
            <div className="flex flex-col items-center">
              <Clock className="w-10 h-10 text-blue-600" />
              <span className="text-2xl font-bold text-slate-700 mt-2">
                {data.TimeTaken}
              </span>
            </div>
          </ResultDonut>
        </section>

        {/* Phần Answer Keys */}
        <section className="bg-white p-8 rounded-2xl shadow-lg border border-slate-200 mb-8">
          <div className="flex items-center gap-3 mb-8 pb-4 border-b-2 border-blue-100">
            <ListChecks className="w-7 h-7 text-blue-600" />
            <h2 className="text-2xl font-bold text-slate-800">
              Detailed Answers
            </h2>
          </div>

          <div className="space-y-10">
            {answerKeys.map((part: any) => (
              <AnswerKeyPart
                key={part.title}
                title={part.title}
                answers={part.answers}
              />
            ))}
          </div>
        </section>
      </main>
    </div>
  );
}
