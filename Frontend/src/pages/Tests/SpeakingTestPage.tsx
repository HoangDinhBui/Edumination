import React, { useState, useEffect, useRef } from "react";
import { Clock, FileText, Menu, FileEdit, Volume2, Mic } from "lucide-react";
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
    const timer = setInterval(
      () => setTimeLeft((t) => Math.max(t - 1, 0)),
      1000
    );
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
    if (timeLeft === 0)
      alert("Time's up! Your test will be submitted automatically.");
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
              isWarning ? "text-red-600" : "text-[#C76378]"
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
          <button className="bg-white text-slate-700 px-5 py-1.5 rounded-full border border-slate-300 flex items-center gap-2 hover:bg-slate-50 transition">
            Exit
          </button>
        </div>
      </div>
    </nav>
  );
};

// =================== MOCK DATA FOR SPEAKING PARTS ===================
const speakingParts = [
  {
    id: 1,
    title: "PART 1",
    subtitle: "Introduction and Interview",
    questions: [
      { id: 1, text: "What is your full name?" },
      { id: 2, text: "Where are you from?" },
      { id: 3, text: "Do you work or are you a student?" },
      { id: 4, text: "What do you like to do in your free time?" },
    ],
  },
  {
    id: 2,
    title: "PART 2",
    subtitle: "Topic",
    topic:
      "Describe an activity you would do when you are alone in your free time",
    instructions: [
      "What do you do",
      "How often do you do it",
      "Why do you like to do this activity?",
      "How do you feel when you do it?",
    ],
  },
  {
    id: 3,
    title: "PART 3",
    subtitle: "Discussion",
    questions: [
      {
        id: 1,
        text: "How do people in your country typically spend their free time?",
      },
      { id: 2, text: "Do you think people have enough leisure time today?" },
      { id: 3, text: "What are the benefits of spending time alone?" },
      {
        id: 4,
        text: "How has technology changed the way people spend their free time?",
      },
    ],
  },
];

// =================== RENDER QUESTION BY PART ===================
const renderQuestionByPart = (part, currentQuestion) => {
  // Part 2 - Topic Card
  if (part.id === 2) {
    return (
      <div className="bg-slate-50 border border-slate-200 rounded-lg p-5 text-left max-w-md">
        <h3 className="text-[#C76378] font-semibold text-sm mb-2">
          {part.topic}
        </h3>
        <p className="text-slate-600 text-xs mb-2 italic">You should say:</p>
        <ul className="list-disc list-inside text-slate-700 text-xs space-y-1">
          {part.instructions.map((inst, idx) => (
            <li key={idx}>{inst}</li>
          ))}
        </ul>
      </div>
    );
  }

  // Part 1 & 3 - Question Card
  if ((part.id === 1 || part.id === 3) && part.questions) {
    return (
      <div className="bg-slate-50 border border-slate-200 rounded-lg p-4 text-left max-w-md">
        <div className="flex items-start gap-2">
          <div className="w-7 h-7 rounded-full bg-[#C76378] text-white flex items-center justify-center text-xs font-bold flex-shrink-0">
            {currentQuestion + 1}
          </div>
          <p className="text-slate-700 text-sm font-medium pt-1">
            {part.questions[currentQuestion].text}
          </p>
        </div>
      </div>
    );
  }

  return null;
};

// =================== MAIN SPEAKING CONTENT ===================
const SpeakingContent = ({ activePart, onNextPart }) => {
  const [isRecording, setIsRecording] = useState(false);
  const [recordingTime, setRecordingTime] = useState(0);
  const [currentQuestion, setCurrentQuestion] = useState(0);

  const currentPart = speakingParts.find((p) => p.id === activePart);

  useEffect(() => {
    let timer;
    if (isRecording) {
      timer = setInterval(() => {
        setRecordingTime((t) => t + 1);
      }, 1000);
    } else {
      setRecordingTime(0);
    }
    return () => clearInterval(timer);
  }, [isRecording]);

  useEffect(() => {
    setCurrentQuestion(0);
    setIsRecording(false);
  }, [activePart]);

  const formatTime = (seconds) => {
    const mins = Math.floor(seconds / 60);
    const secs = seconds % 60;
    return `${mins.toString().padStart(2, "0")}:${secs
      .toString()
      .padStart(2, "0")}`;
  };

  const handleNextQuestion = () => {
    if (
      currentPart.questions &&
      currentQuestion < currentPart.questions.length - 1
    ) {
      setCurrentQuestion((prev) => prev + 1);
      setIsRecording(false);
    }
  };

  const handleNextPart = () => {
    if (activePart < 3) {
      onNextPart(activePart + 1);
    }
  };

  const isLastQuestion = currentPart.questions
    ? currentQuestion === currentPart.questions.length - 1
    : false;
  const isLastPart = activePart === 3;

  return (
    <div className="flex-1 flex items-start justify-center bg-white px-6 pt-8 pb-24 overflow-y-auto">
      <div className="w-full max-w-3xl">
        <div className="w-full max-w-3xl text-center">
          {" "}
          {/* ✅ căn giữa */}
          {/* ✅ Part Title + Subtitle ở giữa */}
          <h1 className="text-[#4A5568] text-xl font-bold mb-0.5">
            {currentPart.title}
          </h1>
          <p className="text-slate-500 text-xs mb-6">{currentPart.subtitle}</p>
          <div className="flex items-start gap-6">
            {/* Left: Video */}
            <div className="flex-shrink-0 w-80">
              <div className="bg-slate-100 rounded-lg overflow-hidden shadow-md">
                <div className="aspect-video bg-gradient-to-br from-slate-200 to-slate-300 flex items-center justify-center">
                  <div className="text-center">
                    <div className="w-20 h-20 mx-auto mb-3 bg-white rounded-full flex items-center justify-center shadow-lg">
                      <div className="w-16 h-16 bg-slate-300 rounded-full"></div>
                    </div>
                    <p className="text-slate-500 text-xs">Video Preview</p>
                  </div>
                </div>
              </div>
            </div>

            {/* Right: Part Info and Question */}
            <div className="flex-1">
              {/* Render Question Based on Part */}
              {renderQuestionByPart(currentPart, currentQuestion)}
            </div>
          </div>
          {/* Recording Controls */}
          <div className="flex flex-col items-center gap-3 mt-8 mb-6">
            {/* Audio Waveform with Mic Button */}
            <div className="flex items-center gap-3 w-full max-w-md">
              {/* Left line */}
              <div className="flex-1 h-0.5 bg-slate-300 relative overflow-hidden">
                {isRecording && (
                  <div className="absolute inset-0 bg-[#C76378] animate-pulse"></div>
                )}
              </div>

              {/* Mic Button */}
              <button
                onClick={() => setIsRecording(!isRecording)}
                className={`w-14 h-14 rounded-full flex items-center justify-center transition-all shadow-lg flex-shrink-0 ${
                  isRecording
                    ? "bg-[#C76378]"
                    : "bg-white border-2 border-[#C76378] hover:bg-[#C76378] hover:bg-opacity-10"
                }`}
              >
                <Mic
                  className={`w-7 h-7 ${
                    isRecording ? "text-white" : "text-[#C76378]"
                  }`}
                />
              </button>

              {/* Right line */}
              <div className="flex-1 h-0.5 bg-slate-300 relative overflow-hidden">
                {isRecording && (
                  <div className="absolute inset-0 bg-[#C76378] animate-pulse"></div>
                )}
              </div>
            </div>

            {/* Recording Time */}
            <div className="text-[#C76378] text-base font-semibold">
              {formatTime(recordingTime)}
            </div>
          </div>
          {/* Next Question / Next Part Button */}
          <div className="flex justify-center">
            {currentPart.id === 2 ? (
              // Part 2 always shows Next Part button
              !isLastPart && (
                <button
                  onClick={handleNextPart}
                  className="bg-[#C76378] text-white px-6 py-2.5 rounded-full font-medium hover:bg-[#B35567] transition-all shadow-md text-sm"
                >
                  Next part →
                </button>
              )
            ) : (
              // Part 1 & 3
              <>
                {!isLastQuestion ? (
                  <button
                    onClick={handleNextQuestion}
                    className="bg-[#C76378] text-white px-6 py-2.5 rounded-full font-medium hover:bg-[#B35567] transition-all shadow-md text-sm"
                  >
                    Next question →
                  </button>
                ) : (
                  !isLastPart && (
                    <button
                      onClick={handleNextPart}
                      className="bg-[#C76378] text-white px-6 py-2.5 rounded-full font-medium hover:bg-[#B35567] transition-all shadow-md text-sm"
                    >
                      Next part →
                    </button>
                  )
                )}
              </>
            )}
          </div>
        </div>
      </div>
    </div>
  );
};

// =================== MAIN PAGE ===================
const SpeakingTestPage = () => {
  const [activePart, setActivePart] = useState(1);

  const parts = [
    { id: 1, label: "Part 1" },
    { id: 2, label: "Part 2" },
    { id: 3, label: "Part 3" },
  ];

  return (
    <div
      className="w-screen h-screen flex flex-col bg-slate-50 overflow-hidden"
      style={{ fontFamily: "Be Vietnam Pro, sans-serif" }}
    >
      <TopNavbar />

      <SpeakingContent activePart={activePart} onNextPart={setActivePart} />

      {/* FOOTER */}
      <footer className="fixed bottom-0 left-0 w-full bg-white border-t border-slate-300 shadow-md z-[999]">
        <div className="flex items-center h-16 px-4 gap-3">
          {parts.map((p) => (
            <button
              key={p.id}
              onClick={() => setActivePart(p.id)}
              className={`flex-1 py-3 rounded-xl border transition-all duration-300 text-base font-semibold
                ${
                  activePart === p.id
                    ? "border-[#C76378] text-[#C76378] bg-pink-50"
                    : "border-slate-300 text-slate-700 hover:bg-slate-100 hover:border-[#C76378]"
                }`}
              style={{ fontFamily: "Be Vietnam Pro, sans-serif" }}
            >
              {p.label}
            </button>
          ))}
        </div>
      </footer>
    </div>
  );
};

export default SpeakingTestPage;
