import React, { useState, useRef } from "react";
import axios from "axios";

interface SpeakingRecorderProps {
  attemptId: number;
  sectionId: number;
  questionId?: number;
  promptText: string;
  onSubmitSuccess: (result: SpeakingSubmissionResult) => void;
}

interface SpeakingSubmissionResult {
  id: number;
  audioUrl: string;
  transcribedText: string;
  scores: {
    overallScore: number;
    fluencyScore: number;
    grammarScore: number;
    vocabularyScore: number;
    pronunciationScore: number;
    bandLevel: string;
  };
  feedback: {
    summary: string;
    strengths: string[];
    improvements: string[];
  };
  status: string;
  gradedAt: string;
}

export const SpeakingRecorder: React.FC<SpeakingRecorderProps> = ({
  attemptId,
  sectionId,
  questionId,
  promptText,
  onSubmitSuccess,
}) => {
  const [isRecording, setIsRecording] = useState(false);
  const [audioBlob, setAudioBlob] = useState<Blob | null>(null);
  const [audioUrl, setAudioUrl] = useState<string | null>(null);
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const mediaRecorderRef = useRef<MediaRecorder | null>(null);
  const chunksRef = useRef<Blob[]>([]);

  const startRecording = async () => {
    try {
      const stream = await navigator.mediaDevices.getUserMedia({ audio: true });
      const mediaRecorder = new MediaRecorder(stream, {
        mimeType: "audio/webm;codecs=opus",
      });

      mediaRecorderRef.current = mediaRecorder;
      chunksRef.current = [];

      mediaRecorder.ondataavailable = (event) => {
        if (event.data.size > 0) {
          chunksRef.current.push(event.data);
        }
      };

      mediaRecorder.onstop = () => {
        const blob = new Blob(chunksRef.current, { type: "audio/webm" });
        setAudioBlob(blob);
        setAudioUrl(URL.createObjectURL(blob));

        // Stop all tracks
        stream.getTracks().forEach((track) => track.stop());
      };

      mediaRecorder.start();
      setIsRecording(true);
      setError(null);
    } catch (err) {
      setError("Failed to access microphone. Please check permissions.");
      console.error("Error starting recording:", err);
    }
  };

  const stopRecording = () => {
    if (mediaRecorderRef.current && isRecording) {
      mediaRecorderRef.current.stop();
      setIsRecording(false);
    }
  };

  const submitRecording = async () => {
    if (!audioBlob) {
      setError("No audio recorded");
      return;
    }

    setIsSubmitting(true);
    setError(null);

    try {
      const formData = new FormData();
      formData.append("AudioFile", audioBlob, "recording.webm");
      formData.append("PromptText", promptText);
      if (questionId) {
        formData.append("QuestionId", questionId.toString());
      }

      const response = await axios.post<SpeakingSubmissionResult>(
        `/api/v1/attempts/${attemptId}/sections/${sectionId}/speaking`,
        formData,
        {
          headers: {
            "Content-Type": "multipart/form-data",
          },
        }
      );

      onSubmitSuccess(response.data);
    } catch (err: any) {
      const errorMessage =
        err.response?.data?.error || "Failed to submit recording";
      setError(errorMessage);
      console.error("Error submitting recording:", err);
    } finally {
      setIsSubmitting(false);
    }
  };

  const resetRecording = () => {
    setAudioBlob(null);
    if (audioUrl) {
      URL.revokeObjectURL(audioUrl);
    }
    setAudioUrl(null);
    setError(null);
  };

  return (
    <div className="speaking-recorder">
      <div className="prompt-section">
        <h3>Speaking Task</h3>
        <p className="prompt-text">{promptText}</p>
      </div>

      <div className="recording-controls">
        {!audioBlob ? (
          <div className="record-section">
            {!isRecording ? (
              <button
                onClick={startRecording}
                className="btn btn-primary btn-record"
                disabled={isSubmitting}
              >
                🎤 Start Recording
              </button>
            ) : (
              <div className="recording-indicator">
                <button
                  onClick={stopRecording}
                  className="btn btn-danger btn-stop"
                >
                  ⏹️ Stop Recording
                </button>
                <span className="recording-pulse">🔴 Recording...</span>
              </div>
            )}
          </div>
        ) : (
          <div className="playback-section">
            <audio src={audioUrl!} controls className="audio-player" />

            <div className="action-buttons">
              <button
                onClick={resetRecording}
                className="btn btn-secondary"
                disabled={isSubmitting}
              >
                🔄 Re-record
              </button>

              <button
                onClick={submitRecording}
                className="btn btn-success"
                disabled={isSubmitting}
              >
                {isSubmitting ? "⏳ Grading..." : "✅ Submit for Grading"}
              </button>
            </div>
          </div>
        )}
      </div>

      {error && (
        <div className="alert alert-danger" role="alert">
          {error}
        </div>
      )}

      {isSubmitting && (
        <div className="grading-progress">
          <div className="spinner-border text-primary" role="status">
            <span className="visually-hidden">Processing...</span>
          </div>
          <p className="grading-text">
            Transcribing and grading your response using AI...
            <br />
            This may take 30-60 seconds.
          </p>
        </div>
      )}
    </div>
  );
};

// Result Display Component
export const SpeakingResult: React.FC<{ result: SpeakingSubmissionResult }> = ({
  result,
}) => {
  return (
    <div className="speaking-result">
      <div className="result-header">
        <h3>Your Speaking Result</h3>
        <div className="overall-band">
          <span className="band-label">Overall Band:</span>
          <span className="band-score">{result.scores.bandLevel}</span>
        </div>
      </div>

      <div className="transcript-section">
        <h4>Transcript</h4>
        <p className="transcript-text">{result.transcribedText}</p>
      </div>

      <div className="scores-breakdown">
        <h4>Detailed Scores</h4>
        <div className="score-items">
          <ScoreItem
            label="Fluency & Coherence"
            score={result.scores.fluencyScore}
          />
          <ScoreItem
            label="Lexical Resource"
            score={result.scores.vocabularyScore}
          />
          <ScoreItem
            label="Grammatical Range"
            score={result.scores.grammarScore}
          />
          <ScoreItem
            label="Pronunciation"
            score={result.scores.pronunciationScore}
          />
        </div>
      </div>

      <div className="feedback-section">
        <div className="strengths">
          <h4>✅ Strengths</h4>
          <ul>
            {result.feedback.strengths.map((strength, index) => (
              <li key={index}>{strength}</li>
            ))}
          </ul>
        </div>

        <div className="improvements">
          <h4>📈 Areas for Improvement</h4>
          <ul>
            {result.feedback.improvements.map((improvement, index) => (
              <li key={index}>{improvement}</li>
            ))}
          </ul>
        </div>
      </div>

      <div className="feedback-summary">
        <h4>AI Feedback</h4>
        <p>{result.feedback.summary}</p>
      </div>

      <div className="audio-playback">
        <h4>Your Recording</h4>
        <audio src={result.audioUrl} controls />
      </div>
    </div>
  );
};

const ScoreItem: React.FC<{ label: string; score: number }> = ({
  label,
  score,
}) => (
  <div className="score-item">
    <span className="score-label">{label}</span>
    <div className="score-bar">
      <div className="score-fill" style={{ width: `${(score / 9) * 100}%` }} />
      <span className="score-value">{score.toFixed(1)}</span>
    </div>
  </div>
);
