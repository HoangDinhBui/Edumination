import React, { useState, useEffect, useRef } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import { useReactMediaRecorder } from "react-media-recorder";
import {
  Clock,
  FileText,
  Menu,
  Loader2,
  Mic,
  AlertTriangle,
  Save,
  LogOut,
  UploadCloud,
} from "lucide-react";
import edmLogo from "../../assets/img/edm-logo.png";

// =================== IMPORT FONT ===================
const fontLink = document.createElement("link");
fontLink.href =
  "https://fonts.googleapis.com/css2?family=Be+Vietnam+Pro:wght@300;400;500;600;700&display=swap";
fontLink.rel = "stylesheet";
document.head.appendChild(fontLink);

// =================== COUNTDOWN HOOK ===================
function useCountdown(initialSeconds: number) {
  const [timeLeft, setTimeLeft] = useState(initialSeconds);
  useEffect(() => {
    setTimeLeft(initialSeconds);
  }, [initialSeconds]);
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

// =================== NAVBAR (SUBMIT BUTTON INTEGRATED) ===================
const TopNavbar = ({
  paperName,
  timeProps,
  onMiniMenuClick,
  onReviewClick,
  onSubmitClick,
}: any) => {
  const { mins, secs, isWarning, timeLeft } = timeProps;

  // Sử dụng useRef để chặn lần render đầu tiên nếu time = 0 do khởi tạo
  const isMounted = React.useRef(false);

  useEffect(() => {
    // Skip on initial render
    if (!isMounted.current) {
      isMounted.current = true;
      return;
    }

    // Auto-submit when time reaches zero
    if (timeLeft === 0) {
      alert("Time's up! The system will auto-submit your test...");
      onSubmitClick();
    }
  }, [timeLeft, onSubmitClick]);

  return (
    <nav className="w-full bg-white shadow-sm sticky top-0 z-50 border-b border-slate-200">
      <div className="grid grid-cols-3 items-center px-6 py-3 h-16">
        <div className="flex items-center">
          <img src={edmLogo} alt="Edumination Logo" className="h-6" />
          <span className="text-slate-700 font-medium text-sm hidden lg:block ml-4">
            {paperName || "Loading..."}
          </span>
        </div>
        <div className="flex justify-center items-center">
          <div
            className={`flex items-center gap-2 ${
              isWarning ? "text-red-600" : "text-[#C76378]"
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
          <button
            onClick={onReviewClick}
            className="flex items-center gap-2 text-slate-600 px-3 py-1.5 rounded-full hover:bg-slate-100 transition"
          >
            <FileText className="w-4 h-4" />
            <span>Review</span>
          </button>
          <button
            onClick={onMiniMenuClick}
            className="p-2 text-slate-600 cursor-pointer hover:bg-slate-100 rounded-full"
          >
            <Menu className="w-5 h-5" />
          </button>

          {/* NÚT SUBMIT GỌI API */}
          <button
            onClick={onSubmitClick}
            className="bg-[#C76378] text-white px-5 py-1.5 rounded-full flex items-center gap-2 hover:bg-[#b35567] transition shadow-sm active:scale-95"
          >
            Submit
          </button>
        </div>
      </div>
    </nav>
  );
};

// =================== MINI MENU POPUP ===================
const MiniMenuPopup = ({ isOpen, onClose, onSaveDraft, onExitTest }: any) => {
  if (!isOpen) return null;
  return (
    <div className="fixed inset-0 z-[998]" onClick={onClose}>
      <div
        className="absolute top-16 right-6 bg-white rounded-2xl shadow-lg p-3 w-48 border border-slate-200"
        onClick={(e) => e.stopPropagation()}
      >
        <button
          onClick={onSaveDraft}
          className="flex items-center gap-3 w-full text-left px-3 py-2.5 rounded-lg font-medium text-white bg-green-500 hover:bg-green-600"
        >
          <Save className="w-5 h-5" />
          <span>Save Draft</span>
        </button>
        <button
          onClick={onExitTest}
          className="flex items-center gap-3 w-full text-left px-3 py-2.5 rounded-lg font-medium text-slate-700 hover:bg-slate-100 mt-1"
        >
          <LogOut className="w-5 h-5" />
          <span>Exit Test</span>
        </button>
      </div>
    </div>
  );
};

// =================== EXIT MODAL ===================
const ExitModal = ({ isOpen, onClose, onConfirm }: any) => {
  if (!isOpen) return null;
  return (
    <div className="fixed inset-0 bg-black/30 backdrop-blur-sm z-[999] flex items-center justify-center">
      <div className="bg-white rounded-2xl shadow-xl p-8 max-w-sm w-full text-center">
        <div className="w-16 h-16 bg-pink-100 text-[#C76378] rounded-full flex items-center justify-center mx-auto">
          <AlertTriangle className="w-8 h-8" strokeWidth={2.5} />
        </div>
        <h2 className="text-xl font-bold text-slate-800 mt-6">
          Are you sure to exit the test?
        </h2>
        <p className="text-slate-600 mt-2 text-sm">
          Your answers will not be saved when exist. You might need to save the
          draft first.
        </p>
        <div className="flex gap-4 mt-8">
          <button
            onClick={onClose}
            className="flex-1 px-4 py-2.5 bg-white border border-slate-300 text-slate-700 rounded-full font-semibold hover:bg-slate-50"
          >
            Cancel
          </button>
          <button
            onClick={onConfirm}
            className="flex-1 px-4 py-2.5 bg-[#C76378] text-white rounded-full font-semibold hover:bg-[#b35567]"
          >
            Yes
          </button>
        </div>
      </div>
    </div>
  );
};

// =================== VIDEO PLAYER ===================
const VideoPlayer = ({ assetId }: { assetId?: number }) => {
  const videoUrl = assetId
    ? `http://localhost:8081/api/v1/assets/download/${assetId}`
    : null;

  return (
    <div className="flex-shrink-0 w-80">
      <div className="bg-slate-100 rounded-lg overflow-hidden shadow-md">
        <div className="aspect-video bg-gradient-to-br from-slate-200 to-slate-300 flex items-center justify-center">
          {videoUrl ? (
            <video controls className="w-full h-full" src={videoUrl}>
              Your browser does not support the video tag.
            </video>
          ) : (
            <div className="text-center">
              <div className="w-20 h-20 mx-auto mb-3 bg-white rounded-full flex items-center justify-center shadow-lg">
                <div className="w-16 h-16 bg-slate-300 rounded-full"></div>
              </div>
              <p className="text-slate-500 text-xs">Video Preview</p>
            </div>
          )}

          {/* video placeholder area; recordings-status moved to SpeakingContent */}
        </div>
      </div>
    </div>
  );
};

// =================== QUESTION DISPLAY ===================
const QuestionDisplay = ({ passage, currentQuestionIndex }: any) => {
  if (!passage) return null;

  if (passage.Position === 2) {
    // Part 2
    let instructions = passage.ContentText
      ? passage.ContentText.split("\n").filter(
          (line: string) => line.trim() !== ""
        )
      : [];
    // Remove first 3 lines if they include the broad header 'PART 1: Introduction and Interview'
    try {
      if (
        instructions.length >= 3 &&
        instructions[0].includes("PART 1: Introduction and Interview")
      ) {
        instructions = instructions.slice(3);
      }
    } catch {
      /* ignore */
    }
    return (
      <div className="bg-slate-50 border border-slate-200 rounded-lg p-5 text-left max-w-md">
        <h3 className="text-[#C76378] font-semibold text-sm mb-2">
          {passage.Title}
        </h3>
        <p className="text-slate-600 text-xs mb-2 italic">You should say:</p>
        <ul className="list-disc list-inside text-slate-700 text-xs space-y-1">
          {instructions.map((inst: string, idx: number) => (
            <li key={idx}>{inst}</li>
          ))}
        </ul>
      </div>
    );
  }

  // Part 1 & 3
  const question = passage.Questions?.[currentQuestionIndex];
  if (!question) return null;

  return (
    <div className="bg-slate-50 border border-slate-200 rounded-lg p-4 text-left max-w-md">
      <div className="flex items-start gap-2">
        <div className="w-7 h-7 rounded-full bg-[#C76378] text-white flex items-center justify-center text-xs font-bold flex-shrink-0">
          {question.Position}
        </div>
        <p className="text-slate-700 text-sm font-medium pt-1">
          {question.Stem}
        </p>
      </div>
    </div>
  );
};

// =================== MAIN SPEAKING CONTENT ===================
const SpeakingContent = ({
  paperData,
  activePart,
  onNextPart,
  attemptId,
  sectionId,
  setTriggerUploadAndGrade,
}: any) => {
  const [recordingTime, setRecordingTime] = useState(0);
  const [currentQuestion, setCurrentQuestion] = useState(0);
  const [isUploading, setIsUploading] = useState(false);
  const [gradingResult, setGradingResult] = useState<{
    overallBand: number | null;
    fluencyScore: number | null;
    lexicalScore: number | null;
    grammarScore: number | null;
    pronunciationScore: number | null;
    transcript: string | null;
    feedback: string | null;
  } | null>(null);
  const [recordingUrl, setRecordingUrl] = useState<string | null>(null);
  // Keep recordings per question: key = "part-question" (e.g. "1-0", "3-1")
  const [recordings, setRecordings] = useState<
    Record<
      string,
      {
        blob: Blob;
        url: string | null;
        grading?: any;
        partPosition: number;
        questionIndex: number;
      }
    >
  >({});
  const recordingsRef = useRef<
    Record<
      string,
      {
        blob: Blob;
        url: string | null;
        grading?: any;
        partPosition: number;
        questionIndex: number;
      }
    >
  >({});
  // keep playback url in sync with the active part + question's recording
  useEffect(() => {
          try {
            const key = `${activePart}-${currentQuestion}`;
            const url = recordings[key]?.url ?? null;
            setRecordingUrl((prev) => {
              if (prev && prev !== url) {
                try {
                  URL.revokeObjectURL(prev);
                } catch (err) {
                  console.warn(err);
                }
              }
              return url;
            });
          } catch (err) {
            console.warn(err);
          }
  }, [activePart, currentQuestion, recordings]);
  const lastBlobRef = useRef<Blob | null>(null);
  const recordingPartRef = useRef<number | null>(null);
  // Save current question index when recording starts
  const recordingQuestionRef = useRef<number | null>(null);

  // Prevent accidental auto-submit: this flag indicates the user actually pressed the record button
  const hasRecordedRef = useRef(false);

  // Get current passage by index (activePart 1,2,3 → index 0,1,2)
  const passages = paperData?.Sections?.[0]?.Passages ?? [];
  const currentPassage = passages[activePart - 1] ?? null;

  const videoAssetId = paperData?.Sections?.[0]?.AudioAssetId;

  // --- Logic Upload (POST .../speaking) ---
  // Upload a single recorded blob for a specific part. Returns parsed grading data or null.
  const handleAudioUpload = async (
    blob: Blob,
    partPosition?: number,
    questionIndex?: number
  ) => {
    // FIX LỖI: Nếu người dùng chưa bấm ghi âm bao giờ mà hàm này tự chạy -> Hủy
    if (!hasRecordedRef.current) {
      console.log("Skipping upload because user hasn't recorded.");
      return;
    }

    // 1. Kiểm tra file rác (Tăng lên 1024 bytes cho chắc)
    if (!blob || blob.size < 1024) {
      console.warn("Recorded file too small or empty, cancelling upload. Size:", blob?.size);
      hasRecordedRef.current = false; // Reset flag
      return;
    }

    if (!attemptId || !sectionId) {
      console.error("Missing ID:", { attemptId, sectionId });
      return;
    }

    setIsUploading(true);
    const TOKEN = localStorage.getItem("Token");

    const formData = new FormData();
    // derive filename extension from blob type to keep correct audio container
    const mimeType = blob?.type ?? "audio/webm";
    let ext = "webm";
    try {
      ext = mimeType.split("/")[1] || ext;
      if (ext.includes(";")) ext = ext.split(";")[0];
    } catch (e) {
      console.warn(e);
    }
    const filename = `part${partPosition ?? "x"}_q${questionIndex ?? 0}.${ext}`;
    formData.append("AudioFile", blob, filename);
    if (partPosition) {
      formData.append("PartPosition", partPosition.toString());
    }
    if (typeof questionIndex === "number") {
      formData.append("QuestionIndex", questionIndex.toString());
    }
    formData.append("PromptText", currentPassage?.Title || "Speaking Part");
    formData.append("ConfirmSubmission", "true");

    try {
      console.log("Uploading audio:", {
        filename,
        mimeType,
        size: blob.size,
        partPosition,
        questionIndex,
      });
      const response = await fetch(
        `http://localhost:8081/api/v1/attempts/${attemptId}/sections/${sectionId}/speaking`,
        {
          method: "POST",
          headers: { Authorization: `Bearer ${TOKEN}` },
          body: formData,
        }
      );

      if (!response.ok) {
        // FIX LỖI JSON: Xử lý backend trả về Text
        const errorText = await response.text();
        try {
          const errJson = JSON.parse(errorText);
          throw new Error(errJson.Title || errJson.Detail || "Upload failed");
        } catch {
          throw new Error(errorText || "Lỗi không xác định từ server");
        }
      }

      // Try to parse JSON grading response (backend may include grading result)
      let data: any = null;
      try {
        data = await response.json();
      } catch (e) {
        console.warn("Upload succeeded but response was not JSON.", e);
      }

      console.log("Upload audio thành công!", data);
      // If backend returned grading info, save it to state to show to user
      if (data) {
        setGradingResult({
          overallBand: data.OverallBand ?? data.overallBand ?? null,
          fluencyScore:
            data.FluencyScore ?? data.fluencyScore ?? data.Fluency ?? null,
          lexicalScore:
            data.LexicalScore ??
            data.lexicalScore ??
            data.VocabularyScore ??
            null,
          grammarScore:
            data.GrammarScore ?? data.grammarScore ?? data.Grammar ?? null,
          pronunciationScore:
            data.PronunciationScore ??
            data.pronunciationScore ??
            data.Pronunciation ??
            null,
          transcript:
            data.TranscribedText ??
            data.Transcript ??
            data.transcript ??
            data.AsrText ??
            null,
          feedback: data.AiFeedback ?? data.Feedback ?? data.feedback ?? null,
        });
      }

      // Return parsed grading data to caller so parent can decide next actions
      return data ?? null;

      // Reset cờ sau khi upload thành công
      hasRecordedRef.current = false;
    } catch (err: any) {
      console.error("Upload error:", err);
      alert(`Lỗi upload ghi âm: ${err.message}`);
      return null;
    } finally {
      setIsUploading(false);
    }
  };

  // --- Logic Ghi Âm ---
  // NOTE: we intentionally do not mirror activePart into a ref to avoid
  // synchronization delays; use `activePart` directly when needed.

  const { status, startRecording, stopRecording, clearBlobUrl } =
    useReactMediaRecorder({
      audio: true,
      onStop: (blobUrl, blob) => {
        try {
          // Use the pinned part/question for this recording session
          const partPos = recordingPartRef.current ?? activePart ?? 1;
          const questionIdx =
            recordingQuestionRef.current ?? currentQuestion ?? 0;
          const recordingKey = `${partPos}-${questionIdx}`;
          console.log(
            `Recorded blob: ${blob?.size} ${blob?.type} for Part ${partPos}, Question ${questionIdx}`
          );
          const url = blob ? URL.createObjectURL(blob) : null;

          // save into per-question recordings map
          setRecordings((prev) => {
            const next = {
              ...prev,
              [recordingKey]: {
                blob: blob as Blob,
                url,
                grading: prev[recordingKey]?.grading,
                partPosition: partPos,
                questionIndex: questionIdx,
              },
            };
            try {
              recordingsRef.current = next;
            } catch (e) {
              console.warn(e);
            }
            console.log("Saved recordings map:", recordingsRef.current);
            return next;
          });

          // also keep lastBlobRef for backward compatibility and playback url
          try {
            lastBlobRef.current = blob;
          } catch (e) {
            console.warn(e);
          }
          if (blob) {
            try {
              setRecordingUrl((r) => {
                if (r) {
                  URL.revokeObjectURL(r);
                }
                return url;
              });
            } catch (e) {
              console.warn(e);
            }
          }

          // clear the pinned recording-part/question now that the recording finished
          try {
            recordingPartRef.current = null;
            recordingQuestionRef.current = null;
          } catch (e) {
            /* non-blocking */
          }

          // NOTE: We intentionally do NOT auto-advance here to avoid unexpected navigation
        } catch (e) {
          console.warn(e);
        }
        // Do NOT auto-upload or call AI here. Upload and grading
        // will be triggered when the user clicks the Submit button.
      },
    });

  const isRecording = status === "recording";

  useEffect(() => {
    let timer: NodeJS.Timeout;
    if (isRecording) {
      timer = setInterval(() => setRecordingTime((t) => t + 1), 1000);
    } else {
      setRecordingTime(0);
    }
    return () => clearInterval(timer);
  }, [isRecording]);

  // Reset câu hỏi khi chuyển phần
  useEffect(() => {
    setCurrentQuestion(0);
    if (status === "recording") {
      stopRecording(); // Chỉ stop nếu đang ghi
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [activePart]);

  const formatTime = (seconds: number) => {
    const mins = Math.floor(seconds / 60);
    const secs = seconds % 60;
    return `${mins.toString().padStart(2, "0")}:${secs
      .toString()
      .padStart(2, "0")}`;
  };

  const handleNextQuestion = () => {
    if (
      currentPassage?.Questions &&
      currentQuestion < currentPassage.Questions.length - 1
    ) {
      setCurrentQuestion((prev) => prev + 1);
    }
  };

  const handleNextPart = () => {
    if (activePart < 3) {
      onNextPart(activePart + 1);
    }
  };

  const handleMicClick = () => {
    if (isRecording) {
      stopRecording(); // Dừng -> Upload
    } else {
      hasRecordedRef.current = true; // Đánh dấu là người dùng ĐÃ BẤM NÚT
      // Clear any previous last blob/url to avoid accidentally showing the same
      try {
        lastBlobRef.current = null;
      } catch (e) {
        /* non-blocking */
      }
      try {
        if (recordingUrl) {
          URL.revokeObjectURL(recordingUrl);
        }
        setRecordingUrl(null);
      } catch (e) {
        /* non-blocking */
      }
      clearBlobUrl();
      // pin current active part and question for this recording session
      try {
        const pinnedPart = typeof activePart === "number" ? activePart : 1;
        const pinnedQuestion =
          typeof currentQuestion === "number" ? currentQuestion : 0;
        recordingPartRef.current = pinnedPart;
        recordingQuestionRef.current = pinnedQuestion;
        console.log(
          `Starting record for Part ${pinnedPart}, Question ${pinnedQuestion}`
        );
      } catch (e) {
        /* non-blocking */
      }
      startRecording(); // Bắt đầu ghi
    }
  };

  // Expose a trigger function to parent so it can call upload+grade before final submit
  useEffect(() => {
    if (!setTriggerUploadAndGrade) return;
    // Expose a function that uploads & grades ALL recorded questions and returns an array of results
    setTriggerUploadAndGrade(() => async () => {
      const entries = Object.entries(recordingsRef.current);
      if (entries.length === 0) {
        alert("No recordings found to upload/grade.");
        return [];
      }
      const results: Array<{
        key: string;
        part: number;
        question: number;
        data: any;
      }> = [];
      for (const [key, rec] of entries) {
        if (!rec?.blob) continue;
        try {
          const data = await handleAudioUpload(
            rec.blob,
            rec.partPosition,
            rec.questionIndex
          );
          results.push({
            key,
            part: rec.partPosition,
            question: rec.questionIndex,
            data,
          });
          // attach grading to recordings state and ref for UI
          setRecordings((prev) => {
            const next = { ...prev, [key]: { ...prev[key], grading: data } };
            try {
              recordingsRef.current = next;
            } catch (e) {
              console.warn(e);
            }
            return next;
          });
        } catch (e) {
          console.warn("Upload/grade failed for", key, e);
        }
      }
      return results;
    });
    return () => {
      if (setTriggerUploadAndGrade) setTriggerUploadAndGrade(null);
    };
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [setTriggerUploadAndGrade]);

  const isLastQuestion = currentPassage?.Questions
    ? currentQuestion === currentPassage.Questions.length - 1
    : false;
  const isLastPart = activePart === 3;

  if (!currentPassage) {
    return <div className="text-center p-10">No Data Found.</div>;
  }

  return (
    <div className="flex-1 flex items-start justify-center bg-white px-6 pt-8 pb-24 overflow-y-auto">
      <div className="w-full max-w-3xl">
        <div className="w-full max-w-3xl text-center">
            <h1 className="text-[#4A5568] text-xl font-bold mb-0.5">
            {currentPassage.Title}
          </h1>
          <div className="flex items-start gap-6 mt-6">
            <VideoPlayer assetId={videoAssetId} />
            <div className="flex-1">
              <QuestionDisplay
                passage={currentPassage}
                currentQuestionIndex={currentQuestion}
              />
            </div>
          </div>

          {/* Per-part playback & status */}
          <div className="mt-4 max-w-3xl mx-auto text-left">
            <h4 className="text-sm font-semibold text-slate-700 mb-2">
              Recordings
            </h4>
            <div className="grid grid-cols-1 gap-3">
              {(paperData?.Sections?.[0]?.Passages || []).map((p: any) => (
                <div
                  key={p.Position}
                  className="flex items-center justify-between bg-white p-3 rounded-lg border"
                >
                  <div className="flex items-center gap-3">
                    <div className="text-sm font-medium text-slate-700">
                      Part {p.Position}
                    </div>
                    <div className="text-sm text-slate-500">{p.Title}</div>
                    {/* Show any recordings for this part (per-question) */}
                    <div className="ml-3 flex items-center gap-2">
                      {Object.keys(recordings)
                        .filter((k) => k.startsWith(`${p.Position}-`))
                        .map((key) => (
                          <audio
                            key={key}
                            src={recordings[key].url ?? undefined}
                            controls
                            className="rounded"
                          />
                        ))}
                    </div>
                  </div>
                  <div className="flex items-center gap-2">
                    <button
                      onClick={() => {
                        // clear existing recordings for this part and switch to it for re-record
                        setRecordings((prev) => {
                          const copy = { ...prev };
                          Object.keys(copy).forEach((k) => {
                            if (k.startsWith(`${p.Position}-`)) delete copy[k];
                          });
                          try {
                            recordingsRef.current = copy;
                          } catch (e) {
                            /* non-blocking */
                          }
                          return copy;
                        });
                        onNextPart(p.Position);
                      }}
                      className="px-3 py-1 text-sm bg-slate-100 rounded"
                    >
                      Re-record
                    </button>
                  </div>
                </div>
              ))}
            </div>
          </div>

          {/* CONTROL BAR */}
          <div className="flex flex-col items-center gap-3 mt-8 mb-6">
            <div className="flex items-center gap-3 w-full max-w-md">
              <div className="flex-1 h-0.5 bg-slate-300 relative overflow-hidden">
                {(isRecording || isUploading) && (
                  <div className="absolute inset-0 bg-[#C76378] animate-pulse"></div>
                )}
              </div>
              <button
                onClick={handleMicClick}
                disabled={isUploading}
                className={`w-14 h-14 rounded-full flex items-center justify-center transition-all shadow-lg flex-shrink-0 ${
                  isRecording
                    ? "bg-[#C76378]"
                    : "bg-white border-2 border-[#C76378] hover:bg-[#C76378] hover:bg-opacity-10"
                } ${
                  isUploading
                    ? "bg-slate-300 border-slate-300 cursor-not-allowed"
                    : ""
                }`}
              >
                {isUploading ? (
                  <UploadCloud className="w-7 h-7 text-slate-500 animate-pulse" />
                ) : (
                  <Mic
                    className={`w-7 h-7 ${
                      isRecording ? "text-white" : "text-[#C76378]"
                    }`}
                  />
                )}
              </button>
              {/* Playback for last recording */}
              {recordingUrl && (
                <div className="ml-4">
                  <audio src={recordingUrl} controls className="rounded-md" />
                </div>
              )}
              <div className="flex-1 h-0.5 bg-slate-300 relative overflow-hidden">
                {(isRecording || isUploading) && (
                  <div className="absolute inset-0 bg-[#C76378] animate-pulse"></div>
                )}
              </div>
            </div>
            <div className="text-[#C76378] text-base font-semibold">
              {isUploading ? "Uploading..." : formatTime(recordingTime)}
            </div>
          </div>

          {/* AI GRADING RESULT (Improved UI) */}
          {gradingResult && (
            <div className="mt-6 max-w-2xl mx-auto bg-gradient-to-br from-pink-50 to-white border-2 border-[#C76378] p-6 rounded-xl shadow-lg">
              <div className="flex items-center gap-2 mb-4">
                <div className="w-10 h-10 bg-[#C76378] rounded-full flex items-center justify-center">
                  <svg
                    className="w-6 h-6 text-white"
                    fill="none"
                    stroke="currentColor"
                    viewBox="0 0 24 24"
                  >
                    <path
                      strokeLinecap="round"
                      strokeLinejoin="round"
                      strokeWidth={2}
                      d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z"
                    />
                  </svg>
                </div>
                <h4 className="text-lg font-bold text-[#C76378]">
                  AI Grading Result
                </h4>
              </div>
              {/* Overall Band Score */}
              {gradingResult.overallBand && (
                <div className="bg-white rounded-lg p-4 mb-4 border border-pink-200">
                  <div className="flex items-center justify-between">
                    <span className="text-sm font-semibold text-slate-700">
                      Overall Band Score
                    </span>
                    <span className="text-3xl font-bold text-[#C76378]">
                      {gradingResult.overallBand.toFixed(1)}
                    </span>
                  </div>
                </div>
              )}
              {/* Detailed Scores Grid */}
              {(gradingResult.fluencyScore ||
                gradingResult.lexicalScore ||
                gradingResult.grammarScore ||
                gradingResult.pronunciationScore) && (
                <div className="grid grid-cols-2 gap-3 mb-4">
                  {gradingResult.fluencyScore && (
                    <div className="bg-white rounded-lg p-3 border border-pink-100">
                      <div className="text-xs text-slate-500 mb-1">
                        Fluency & Coherence
                      </div>
                      <div className="text-2xl font-bold text-[#C76378]">
                        {gradingResult.fluencyScore.toFixed(1)}
                      </div>
                    </div>
                  )}
                  {gradingResult.lexicalScore && (
                    <div className="bg-white rounded-lg p-3 border border-pink-100">
                      <div className="text-xs text-slate-500 mb-1">
                        Lexical Resource
                      </div>
                      <div className="text-2xl font-bold text-[#C76378]">
                        {gradingResult.lexicalScore.toFixed(1)}
                      </div>
                    </div>
                  )}
                  {gradingResult.grammarScore && (
                    <div className="bg-white rounded-lg p-3 border border-pink-100">
                      <div className="text-xs text-slate-500 mb-1">Grammar</div>
                      <div className="text-2xl font-bold text-[#C76378]">
                        {gradingResult.grammarScore.toFixed(1)}
                      </div>
                    </div>
                  )}
                  {gradingResult.pronunciationScore && (
                    <div className="bg-white rounded-lg p-3 border border-pink-100">
                      <div className="text-xs text-slate-500 mb-1">
                        Pronunciation
                      </div>
                      <div className="text-2xl font-bold text-[#C76378]">
                        {gradingResult.pronunciationScore.toFixed(1)}
                      </div>
                    </div>
                  )}
                </div>
              )}
              {/* Transcript Section */}
              {gradingResult.transcript && (
                <div className="bg-white rounded-lg p-4 mb-4 border border-pink-100">
                  <div className="text-sm font-semibold text-slate-700 mb-2 flex items-center gap-2">
                    <svg
                      className="w-4 h-4 text-[#C76378]"
                      fill="none"
                      stroke="currentColor"
                      viewBox="0 0 24 24"
                    >
                      <path
                        strokeLinecap="round"
                        strokeLinejoin="round"
                        strokeWidth={2}
                        d="M7 8h10M7 12h4m1 8l-4-4H5a2 2 0 01-2-2V6a2 2 0 012-2h14a2 2 0 012 2v8a2 2 0 01-2 2h-3l-4 4z"
                      />
                    </svg>
                    Transcript
                  </div>
                  <div className="text-sm text-slate-600 leading-relaxed whitespace-pre-wrap max-h-40 overflow-y-auto">
                    {gradingResult.transcript}
                  </div>
                </div>
              )}
              {/* Feedback Section */}
              {gradingResult.feedback && (
                <div className="bg-white rounded-lg p-4 border border-pink-100">
                  <div className="text-sm font-semibold text-slate-700 mb-2 flex items-center gap-2">
                    <svg
                      className="w-4 h-4 text-[#C76378]"
                      fill="none"
                      stroke="currentColor"
                      viewBox="0 0 24 24"
                    >
                      <path
                        strokeLinecap="round"
                        strokeLinejoin="round"
                        strokeWidth={2}
                        d="M9.663 17h4.673M12 3v1m6.364 1.636l-.707.707M21 12h-1M4 12H3m3.343-5.657l-.707-.707m2.828 9.9a5 5 0 117.072 0l-.548.547A3.374 3.374 0 0014 18.469V19a2 2 0 11-4 0v-.531c0-.895-.356-1.754-.988-2.386l-.548-.547z"
                      />
                    </svg>
                    AI Feedback
                  </div>
                  <div className="text-sm text-slate-600 leading-relaxed whitespace-pre-wrap max-h-32 overflow-y-auto">
                    {gradingResult.feedback}
                  </div>
                </div>
              )}
            </div>
          )}

          {/* NAVIGATION BUTTONS */}
          <div className="flex justify-center">
            {currentPassage.Position === 2 ? (
              !isLastPart && (
                <button
                  onClick={handleNextPart}
                  className="bg-[#C76378] text-white px-6 py-2.5 rounded-full font-medium hover:bg-[#B35567] transition-all shadow-md text-sm"
                >
                  Next part →
                </button>
              )
            ) : (
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

// =================== MAIN PAGE CONTROLLER ===================
const SpeakingTestPage = () => {
  const [activePart, setActivePart] = useState(1);
  const handleSetActivePart = (part: number) => {
    console.log("🔵 handleSetActivePart called with part:", part);
    setActivePart(part);
    console.log("✅ setActivePart executed");
  };
  const location = useLocation();
  const navigate = useNavigate();
  const { paperId, paperName } = location.state || {};
  const [paperData, setPaperData] = useState<any>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [timeLimit, setTimeLimit] = useState(0);

  const [attemptId, setAttemptId] = useState<number | null>(null);
  const [sectionId, setSectionId] = useState<number | null>(null);

  const [isSubmittingTest, setIsSubmittingTest] = useState(false);
  const [submitResultModal, setSubmitResultModal] = useState<{
    visible: boolean;
    band: number | null;
    graded: boolean;
    message?: string | null;
  }>({ visible: false, band: null, graded: false, message: null });
  const [triggerUploadAndGrade, setTriggerUploadAndGrade] = useState<
    (() => Promise<any>) | null
  >(null);

  const timeProps = useCountdown(timeLimit);
  const [isExitModalOpen, setIsExitModalOpen] = useState(false);
  const [isMiniMenuOpen, setIsMiniMenuOpen] = useState(false);

  // Derived value: parts for footer navigation
  const parts = paperData?.Sections?.[0]?.Passages ?? [];
  console.log("📊 Parts array:", parts, "paperData:", paperData);

  // --- INITIALIZATION ---
  useEffect(() => {
    if (!paperId) {
      setError("Test not found. Please return to the library.");
      setIsLoading(false);
      return;
    }
    const TOKEN = localStorage.getItem("Token");
    if (!TOKEN) {
      navigate("/signin", { state: { from: location } });
      return;
    }

    const startAndFetchTest = async () => {
      try {
        // 1. Bắt đầu bài thi (POST /attempts)
        const startResponse = await fetch(
          `http://localhost:8081/api/v1/attempts`,
          {
            method: "POST",
            headers: {
              Authorization: `Bearer ${TOKEN}`,
              "Content-Type": "application/json",
            },
            body: JSON.stringify({ PaperId: paperId }),
          }
        );
        if (!startResponse.ok) {
          // Handle text errors from backend
          const errText = await startResponse.text();
          try {
            const errJson = JSON.parse(errText);
            throw new Error(errJson.Title || errJson.Detail || "Error starting test");
          } catch {
            throw new Error(errText || "Unable to start the test.");
          }
        }

        const startData = await startResponse.json();
        const currentAttemptId = startData.AttemptId;
        const speakingSection = startData.Sections?.find(
          (s: any) => s.Skill === "SPEAKING"
        );

        if (!speakingSection || !currentAttemptId) {
          throw new Error("Speaking section or AttemptId not found.");
        }

        setAttemptId(currentAttemptId);
        setSectionId(speakingSection.Id);

        // 2. Lấy nội dung đề (GET /papers/:id)
        const paperResponse = await fetch(
          `http://localhost:8081/api/v1/papers/${paperId}`,
          { headers: { Authorization: `Bearer ${TOKEN}` } }
        );
        if (!paperResponse.ok)
          throw new Error(`API error: ${paperResponse.statusText}`);

        const paperData = await paperResponse.json();
        setPaperData(paperData);

        const totalTimeInSeconds =
          paperData.Sections?.[0]?.TimeLimitSec || 15 * 60;
        setTimeLimit(totalTimeInSeconds);
      } catch (err: any) {
        console.error("Error fetching:", err);
        setError((err as Error).message);
      } finally {
        setIsLoading(false);
      }
    };

    startAndFetchTest();
  }, [paperId, navigate, location]);

  // --- FUNCTION: NỘP BÀI TỔNG (API /submit) ---
  const handleFinalSubmit = async () => {
    if (!attemptId) return;

    const confirm = window.confirm(
      "Are you sure you want to submit and finish the test?"
    );
    if (!confirm) return;

    setIsSubmittingTest(true);
    const TOKEN = localStorage.getItem("Token");

    try {
      // Ask child to upload & grade all recorded parts (exposed by setTriggerUploadAndGrade)
      let gradingResults: Array<{ part: number; data: any }> = [];
      if (triggerUploadAndGrade) {
        try {
          const res = await triggerUploadAndGrade();
          if (Array.isArray(res)) gradingResults = res;
        } catch (e) {
          console.warn("Upload/grade before final submit failed:", e);
          alert(
            "Warning: failed to upload/grade your recordings before final submit. Proceeding to submit."
          );
        }
      }

      const response = await fetch(
        `http://localhost:8081/api/v1/attempts/${attemptId}/submit`,
        {
          method: "POST",
          headers: {
            Authorization: `Bearer ${TOKEN}`,
            "Content-Type": "application/json",
          },
          body: JSON.stringify({ ConfirmSubmission: true }),
        }
      );

      if (!response.ok) {
        // Handle backend returning text errors
        const errorText = await response.text();
        try {
          const errJson = JSON.parse(errorText);
          throw new Error(errJson.Title || errJson.Detail || "Submit failed");
        } catch {
          throw new Error(errorText || "Unknown error from server");
        }
      }

      const result = await response.json();
      console.log("Nộp bài thành công:", result);

      // Aggregate bands from gradingResults if present (average), else fall back to submit API
      const bands: number[] = [];
      for (const g of gradingResults) {
        const v = g?.data?.OverallBand ?? g?.data?.overallBand ?? null;
        if (typeof v === "number") bands.push(v);
      }
      let displayedBand: number | null = null;
      if (bands.length > 0) {
        const sum = bands.reduce((s, x) => s + x, 0);
        displayedBand = Math.round((sum / bands.length) * 10) / 10; // one decimal
      } else {
        displayedBand = result?.OverallBand ?? result?.overallBand ?? null;
      }

      const graded = displayedBand !== null && displayedBand !== undefined;
      setSubmitResultModal({
        visible: true,
        band: displayedBand ?? null,
        graded,
        message: graded
          ? "The test was graded by AI."
          : "Test submitted. Scores will be updated when results are available.",
      });
    } catch (err: any) {
      console.error("Submit error:", err);
      alert(`Error: Unable to submit the test. ${err.message}`);
    } finally {
      setIsSubmittingTest(false);
    }
  };

  // Helper Functions
  const handleSaveDraft = () => {
    alert("Draft saved! (feature in development)");
    setIsMiniMenuOpen(false);
  };
  const handleExitClick = () => {
    setIsMiniMenuOpen(false);
    setIsExitModalOpen(true);
  };
  const handleExitConfirm = () => {
    setIsExitModalOpen(false);
    navigate("/library");
  };

  if (isLoading) {
    return (
      <div className="w-screen h-screen flex flex-col items-center justify-center bg-slate-50 text-[#C76378]">
        <Loader2 className="w-12 h-12 animate-spin mb-4" />
        <p className="font-medium text-lg">Loading test...</p>
      </div>
    );
  }
  if (error) {
    return (
      <div className="w-screen h-screen flex flex-col items-center justify-center bg-slate-50 text-red-600">
        <p className="font-medium text-lg">Error: {error}</p>
        <button
          onClick={() => navigate("/library")}
          className="mt-4 px-4 py-2 bg-blue-600 text-white rounded"
        >
          Back to library
        </button>
      </div>
    );
  }

  return (
    <div
      className="w-screen h-screen flex flex-col bg-slate-50 overflow-hidden"
      style={{ fontFamily: "Be Vietnam Pro, sans-serif" }}
    >
      <TopNavbar
        paperName={paperName}
        timeProps={timeProps}
        onMiniMenuClick={() => setIsMiniMenuOpen(true)}
        onReviewClick={() => alert("Review clicked!")}
        onSubmitClick={handleFinalSubmit}
      />

      <SpeakingContent
        paperData={paperData}
        activePart={activePart}
        onNextPart={handleSetActivePart}
        attemptId={attemptId}
        sectionId={sectionId}
        setTriggerUploadAndGrade={setTriggerUploadAndGrade}
      />

      <footer className="fixed bottom-0 left-0 w-full bg-white border-t border-slate-300 shadow-md z-[1050]">
        <div className="flex items-center h-16 px-4 gap-3">
          {parts?.map((p: any, index: number) => (
            <button
              key={p.Id}
              onClick={() => handleSetActivePart(index + 1)}
              className={`flex-1 py-3 rounded-xl border transition-all duration-300 text-base font-semibold
                ${
                  activePart === index + 1
                    ? "border-[#C76378] text-[#C76378] bg-pink-50"
                    : "border-slate-300 text-slate-700 hover:bg-slate-100 hover:border-[#C76378]"
                }`}
              style={{ fontFamily: "Be Vietnam Pro, sans-serif" }}
            >
              {p.Title}
            </button>
          ))}
        </div>
      </footer>

      {isSubmittingTest && (
        <div className="fixed inset-0 bg-black/50 z-[1000] flex items-center justify-center flex-col text-white">
          <Loader2 className="w-12 h-12 animate-spin mb-4" />
          <p className="text-lg font-semibold">Submitting test...</p>
        </div>
      )}

      <MiniMenuPopup
        isOpen={isMiniMenuOpen}
        onClose={() => setIsMiniMenuOpen(false)}
        onSaveDraft={handleSaveDraft}
        onExitTest={handleExitClick}
      />
      <ExitModal
        isOpen={isExitModalOpen}
        onClose={() => setIsExitModalOpen(false)}
        onConfirm={handleExitConfirm}
      />
      {/* Centered Submit Result Modal */}
      {submitResultModal.visible && (
        <div className="fixed inset-0 z-[1100] flex items-center justify-center bg-black/40">
          <div className="bg-white rounded-xl shadow-xl p-6 w-full max-w-md text-center">
            <h3 className="text-xl font-bold text-slate-800 mb-2">
              Submission Successful
            </h3>
            {submitResultModal.graded ? (
              <div className="mb-4">
                <div className="text-sm text-slate-500">Official Score</div>
                <div className="text-4xl font-extrabold text-[#C76378]">
                  {submitResultModal.band?.toFixed(1)}
                </div>
                <div className="text-sm text-slate-600 mt-2">
                  {submitResultModal.message}
                </div>
              </div>
            ) : (
              <div className="mb-4">
                <div className="text-sm text-slate-600">
                  {submitResultModal.message}
                </div>
              </div>
            )}
            <div className="flex justify-center gap-3 mt-4">
              <button
                onClick={() => {
                  setSubmitResultModal({
                    ...submitResultModal,
                    visible: false,
                  });
                  navigate("/library");
                }}
                className="px-4 py-2 bg-[#C76378] text-white rounded-full"
              >
                OK
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default SpeakingTestPage;
