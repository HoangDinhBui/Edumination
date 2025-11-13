import React, { useState, useEffect, useRef } from "react";

import { useLocation, useNavigate } from "react-router-dom";

import { Clock, FileText, Loader2, CheckSquare, MousePointerClick } from "lucide-react";

import edmLogo from "../../assets/img/edm-logo.png";



// =================== IMPORT FONT ===================

const fontLink = document.createElement("link");

fontLink.href =

  "https://fonts.googleapis.com/css2?family=Be+Vietnam+Pro:wght@300;400;500;600;700&display=swap";

fontLink.rel = "stylesheet";

document.head.appendChild(fontLink);



// =================== COUNTDOWN HOOK ===================

function useCountdown(initialSeconds) {

  const [timeLeft, setTimeLeft] = useState(initialSeconds);



  useEffect(() => {

    setTimeLeft(initialSeconds);

  }, [initialSeconds]);



  useEffect(() => {

    if (timeLeft <= 0) return;

    const timer = setInterval(() => {

      setTimeLeft((t) => Math.max(t - 1, 0));

    }, 1000);

    return () => clearInterval(timer);

  }, [timeLeft]);



  const mins = Math.floor(timeLeft / 60);

  const secs = timeLeft % 60;

  const isWarning = mins < 5;



  return { mins, secs, isWarning, timeLeft };

}



// =================== NAVBAR ===================

// (Giữ nguyên, không thay đổi)

const TopNavbar = ({ paperName, timeProps }) => {

  const { mins, secs, isWarning, timeLeft } = timeProps;

  const [volume, setVolume] = useState(70);

  const audioRef = useRef<HTMLAudioElement | null>(null);



  useEffect(() => {

    if (timeLeft === 0) {

      alert("Time's up! Your test will be submitted automatically.");

    }

  }, [timeLeft]);



  // Cập nhật âm lượng

  useEffect(() => {

    if (audioRef.current) {

      audioRef.current.volume = volume / 100;

    }

  }, [volume, audioRef]);

  

  // Gán audioRef từ component con (MaterialViewer)

  const setAudioRef = (el: HTMLAudioElement) => {

    audioRef.current = el;

    if (el) {

      el.volume = volume / 100;

    }

  };



  return (

    <nav className="w-full bg-white shadow-sm sticky top-0 z-50 border-b border-slate-200">

      <div className="grid grid-cols-3 items-center px-6 py-3 h-16">

        <div className="flex items-center gap-4">

          <img src={edmLogo} alt="Edumination Logo" className="h-6" />

          <span className="text-slate-700 font-medium text-sm hidden lg:block">

            {paperName || "Loading..."}

          </span>

        </div>

        <div className="flex justify-center items-center">

          <div

            className={`flex items-center gap-2 ${

              isWarning ? "text-red-600" : "text-[#2986B7]"

            } font-medium transition-colors`}

          >

            <Clock className="w-5 h-5" />

            <span className="text-2xl font-semibold">

              {mins.toString().padStart(2, "0")}:

              {secs.toString().padStart(2, "0")}

            </span>

          </div>

        </div>

        <div className="flex justify-end items-center gap-4">

          {/* Component âm lượng giờ sẽ điều khiển audio trong MaterialViewer */}

          {/* <Volume2 className="w-5 h-5 text-[#2986B7]" /> */}

          <input

            type="range"

            min="0"

            max="100"

            value={volume}

            onChange={(e) => setVolume(Number(e.target.value))}

            className="w-24 h-1 accent-[#2986B7]"

          />

          <button className="bg-[#2986B7] text-white px-5 py-1.5 rounded-full flex items-center gap-2 hover:bg-blue-700 transition">

            Submit

          </button>

        </div>

      </div>

      {/* Truyền hàm setAudioRef xuống cho con */}

      <div style={{ display: 'none' }}>

        <MaterialViewer paperData={null} audioRefCallback={setAudioRef} />

      </div>

    </nav>

  );

};



// =================== LEFT COLUMN (ĐÃ NÂNG CẤP) ===================

/**

 * Nâng Cấp:

 * 1. Hiển thị CẢ audio VÀ PDF cùng lúc.

 * 2. Audio ở trên, PDF ở dưới.

 * 3. Thêm prop `audioRefCallback` để Navbar có thể điều khiển âm lượng.

 */
const MaterialViewer = ({ paperData, audioRefCallback }) => {
  if (!paperData) return null; 

  const firstSection = paperData?.Sections?.[0];
  
  // Đọc ID (với chữ hoa) từ API
  const { AudioAssetId } = firstSection || {};
  const { PdfAssetId } = paperData;
  
  const API_BASE_URL = "http://localhost:8081";

  // SỬA LỖI: Xây dựng URL tải về chính xác
  // Dựa trên JSON của bạn, URL đúng là "/api/v1/assets/download/{id}"
  
  const audioUrl = AudioAssetId 
    ? `${API_BASE_URL}/api/v1/assets/download/${AudioAssetId}` 
    : null;
    
  const pdfUrl = PdfAssetId 
    ? `${API_BASE_URL}/api/v1/assets/download/${PdfAssetId}` 
    : null;

  return (
    <div className="h-full bg-[#F2F8FC] flex flex-col gap-4"> 
      
      {/* 1. Audio Player */}
      {audioUrl && (
        <div className="p-6 bg-white rounded-lg shadow-inner">
          <audio 
            ref={audioRefCallback}
            controls 
            className="w-full" 
            src={audioUrl} // <-- URL này giờ đã đúng
          >
            Your browser does not support the audio element.
          </audio>
        </div>
      )}

      {/* 2. PDF Viewer */}
      {pdfUrl && (
        <div className="flex-1 w-full h-full bg-white rounded-lg shadow-inner overflow-hidden">
          <iframe
            src={pdfUrl} // <-- URL này giờ đã đúng
            className="w-full h-full border-0"
            title="Test Questions"
          />
        </div>
      )}
      
      {/* 3. Fallback */}
      {!audioUrl && !pdfUrl && (
        <div className="flex flex-col items-center justify-center h-full text-slate-500">
          <FileText className="w-16 h-16 text-[#7AAEDB] mb-4" />
          <p>Không tìm thấy tài liệu (Audio/PDF) cho bài test này.</p>
        </div>
      )}
    </div>
  );
};


// =================== RENDER QUESTION (ĐÃ NÂNG CẤP) ===================

/**

 * Nâng Cấp:

 * 1. Thêm case "MULTI_SELECT" để render checkboxes.

 * 2. Cập nhật logic để xử lý state cho multi-select (lưu dưới dạng mảng).

 * 3. Giữ nguyên "MCQ" (dropdown) và "FILL_BLANK" (input).

 */

const QuestionRenderer = ({ question, answer, onAnswerChange }) => {

  const { Id, Qtype, Stem, Choices, Position } = question;



  let inputComponent;



  switch (Qtype.toUpperCase()) {

    

    // Case 1: Điền vào chỗ trống (Giống ảnh Questions 1-6)

    case "FILL_BLANK":

      inputComponent = (

        <input

          type="text"

          value={answer || ""}

          onChange={(e) => onAnswerChange(Id, e.target.value)}

          className="w-full border-b-2 border-slate-300 focus:border-[#2986B7] outline-none px-1 py-1 text-slate-700"

          autoComplete="off"

        />

      );

      break;



    // Case 2: Chọn 1 đáp án (Giống ảnh Questions 15-18)

    case "MCQ":

      inputComponent = (

        <select

          value={answer || ""}

          onChange={(e) => onAnswerChange(Id, e.target.value)}

          className="w-full border border-slate-300 rounded-full px-3 py-1.5 text-slate-700 text-sm focus:ring-2 focus:ring-[#2986B7] outline-none"

        >

          <option value="">Select answer</option>

          {Choices?.map((choice, index) => (

            <option key={index} value={choice.Content}>

              {choice.Content}

            </option>

          ))}

        </select>

      );

      break;

    

    // Case 3: Chọn nhiều đáp án (MỚI - Giống ảnh Questions 11-12)

    case "MULTI_SELECT":

      // Đảm bảo 'answer' là một mảng

      const selectedAnswers = Array.isArray(answer) ? answer : [];



      const handleMultiSelectChange = (choiceContent: string) => {

        let newAnswers;

        if (selectedAnswers.includes(choiceContent)) {

          // Bỏ chọn: lọc nó ra khỏi mảng

          newAnswers = selectedAnswers.filter(a => a !== choiceContent);

        } else {

          // Chọn mới: thêm nó vào mảng

          newAnswers = [...selectedAnswers, choiceContent];

        }

        onAnswerChange(Id, newAnswers);

      };



      inputComponent = (

        <div className="flex flex-wrap gap-4">

          {Choices?.map((choice, index) => (

            <label 

              key={index} 

              className="flex items-center gap-2 cursor-pointer p-2 rounded-md hover:bg-slate-50"

            >

              <input

                type="checkbox"

                className="h-4 w-4 text-[#2986B7] focus:ring-[#2986B7]"

                checked={selectedAnswers.includes(choice.Content)}

                onChange={() => handleMultiSelectChange(choice.Content)}

              />

              <span className="font-medium text-slate-700">{choice.Content}</span>

            </label>

          ))}

        </div>

      );

      break;



    // Trường hợp mặc định

    default:

      inputComponent = (

        <input

          type="text"

          value={answer || ""}

          onChange={(e) => onAnswerChange(Id, e.target.value)}

          className="w-full border-b-2 border-slate-300 focus:border-[#2986B7] outline-none px-1 py-1 text-slate-700"

          autoComplete="off"

        />

      );

  }



  // --- Render Câu Hỏi (chung cho mọi loại) ---

  // Thiết kế này không có Stem (đề bài) vì đề bài nằm bên PDF

  if (!Stem) {

    return (

      <div className="flex items-center gap-3 mb-4">

        <div className="w-7 h-7 rounded-full bg-[#2986B7] text-white flex items-center justify-center text-sm font-semibold flex-shrink-0 mt-0.5">

          {Position}

        </div>

        <div className="flex-1">

          {inputComponent}

        </div>

      </div>

    );

  }

  

  // (Giữ lại logic cũ nếu bạn có Stem)

  return (

    <div className="flex items-start gap-3 mb-4">

      <div className="w-7 h-7 rounded-full bg-[#2986B7] text-white flex items-center justify-center text-sm font-semibold flex-shrink-0 mt-0.5">

        {Position}

      </div>

      <div className="flex-1">

        {Stem && (

          <p

            className="text-slate-800 text-sm mb-2"

            dangerouslySetInnerHTML={{ __html: Stem }}

          />

        )}

        {inputComponent}

      </div>

    </div>

  );

};





// =================== MAIN PAGE ===================

const ListeningTestPage = () => {

  const [dividerX, setDividerX] = useState(50);

  const [isDragging, setIsDragging] = useState(false);

  const containerRef = useRef(null);

  const [activePart, setActivePart] = useState(1);

  const audioControlRef = useRef<HTMLAudioElement | null>(null);



  const location = useLocation();

  const navigate = useNavigate();

  

  const { paperId, paperName } = location.state || {};



  const [paperData, setPaperData] = useState(null);

  const [isLoading, setIsLoading] = useState(true);

  const [error, setError] = useState(null);

  const [timeLimit, setTimeLimit] = useState(0);

  

  // State `answers` giờ sẽ lưu:

  // { 1: "answer text", 11: ["A", "C"] }

  const [answers, setAnswers] = useState({});



  const timeProps = useCountdown(timeLimit);



  // Logic này giờ đã hỗ trợ cả string (Fill_Blank) và array (Multi_Select)

  const handleAnswerChange = (questionId, answer) => {

    setAnswers((prevAnswers) => ({

      ...prevAnswers,

      [questionId]: answer,

    }));

  };



  // Kéo thanh chia đôi

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



  // === useEffect GỌI API (Giữ nguyên code đã sửa) ===

  useEffect(() => {

    if (!paperId) {

      setError("Không tìm thấy bài test. Vui lòng quay lại trang thư viện.");

      setIsLoading(false);

      return;

    }



    const TOKEN = localStorage.getItem("Token");

    if (!TOKEN) {

      navigate("/signin", { state: { from: location } });

      return;

    }



    const fetchPaper = async () => {

      try {

        const response = await fetch(

          `http://localhost:8081/api/v1/papers/${paperId}`,

          {

            headers: {

              Authorization: `Bearer ${TOKEN}`,

              "Content-Type": "application/json",

            },

          }

        );



        if (response.status === 401) {

          navigate("/signin");

          return;

        }

        if (!response.ok) {

          throw new Error(`Lỗi API: ${response.statusText}`);

        }



        const data = await response.json();

        

        if (!data.Sections || !Array.isArray(data.Sections)) {

           console.error("Dữ liệu trả về không có 'Sections' (hoặc không phải mảng).", data);

           setError("Lỗi dữ liệu: Bài test này không có nội dung.");

        }

        

        setPaperData(data);

        const firstSectionTime = data.Sections?.[0]?.TimeLimitSec || 3600;

        setTimeLimit(firstSectionTime);



      } catch (err) {

        console.error("Lỗi khi fetch:", err);

        setError(err.message);

      } finally {

        setIsLoading(false);

      }

    };



    fetchPaper();

  }, [paperId, navigate, location]);



  // === Xử lý trạng thái Loading và Error ===

  if (isLoading) {

    return (

      <div className="w-screen h-screen flex flex-col items-center justify-center bg-slate-50 text-[#2986B7]">

        <Loader2 className="w-12 h-12 animate-spin mb-4" />

        <p className="font-medium text-lg">Đang tải bài test...</p>

      </div>

    );

  }



  if (error) {

    return (

      <div className="w-screen h-screen flex flex-col items-center justify-center bg-slate-50 text-red-600">

        <p className="font-medium text-lg">Lỗi: {error}</p>

        <button

          onClick={() => navigate("/library")}

          className="mt-4 px-4 py-2 bg-blue-600 text-white rounded"

        >

          Quay lại thư viện

        </button>

      </div>

    );

  }



  // === Render khi có dữ liệu (ĐÃ SỬA) ===

  return (

    <div className="w-screen h-screen flex flex-col bg-slate-50 font-['Be_Vietnam_Pro'] overflow-hidden">

      

      {/* Navbar sẽ nhận audio ref từ MaterialViewer thông qua callback */}

      <TopNavbar 

        paperName={paperName} 

        timeProps={timeProps} 

      />



      <div

        ref={containerRef}

        className="flex flex-1 overflow-hidden relative bg-[#F8FAFC]"

      >

        {/* LEFT: PDF/AUDIO VIEWER (ĐÃ SỬA) */}

        <div

          className="border-r border-slate-200 overflow-y-auto"

          style={{ width: `${dividerX}%` }}

        >

          {paperData && (

            <MaterialViewer 

              paperData={paperData} 

              audioRefCallback={(el) => (audioControlRef.current = el)}

            />

          )}

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



        {/* RIGHT: QUESTIONS (ĐÃ SỬA) */}

        <div className="flex-1 overflow-y-auto px-10 py-8 bg-white">

          <div className="max-w-xl">

            {paperData?.Sections?.map((section) => (

              <div key={section.Id}>

                {/* LƯU Ý VỀ DATA:

                  Code này lặp qua 'Passages'. Để có các nhóm "Questions 1-6"

                  và "Questions 7-10" như ảnh, bạn chỉ cần đảm bảo trong

                  database, các câu 1-6 nằm trong một 'passage' có 

                  Title = "Questions 1-6", và các câu 7-10 nằm trong

                  'passage' thứ hai có Title = "Questions 7-10".

                  

                  Code React đã xử lý đúng logic này.

                */}

                {section?.Passages?.map((passage) => (

                  <div key={passage.Id} className="mb-8">

                    <h3 className="text-[#2986B7] font-semibold text-lg mb-4">

                      {passage.Title}

                    </h3>



                    {/* LƯU Ý VỀ DATA:

                      Nếu câu hỏi của bạn (như 1-6) không có 'Stem' (đề bài)

                      vì đề bài nằm bên PDF, thì QuestionRenderer sẽ tự động

                      ẩn 'Stem' và chỉ hiển thị số thứ tự + ô nhập.

                    */}

                    {passage?.Questions?.map((q) => (

                      <QuestionRenderer

                        key={q.Id}

                        question={q}

                        answer={answers[q.Id]}

                        onAnswerChange={handleAnswerChange}

                      />

                    ))}

                  </div>

                ))}

              </div>

            ))}

          </div>

        </div>

      </div>



      {/* FOOTER */}

      <footer className="fixed bottom-0 left-0 w-full bg-white border-t border-slate-300 shadow-md z-[999]">

        <div className="flex items-center h-16 px-4 gap-3">

          {[

            { id: 1, label: "Part 1" },

            { id: 2, label: "Part 2" },

            { id: 3, label: "Part 3" },

          ]?.map((p) => (

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