using System;
using Microsoft.AspNetCore.Http;

namespace Edumination.Api.Features.Speaking
{
    public class SubmitSpeakingRequest
    {
        public long? QuestionId { get; set; }
        public string? PromptText { get; set; }
        public IFormFile AudioFile { get; set; } = null!;
    }

    public class SpeakingSubmissionResultDto
    {
        public long Id { get; set; }
        public long SectionAttemptId { get; set; }
        public long? QuestionId { get; set; }
        public string? PromptText { get; set; }
        public string AudioUrl { get; set; } = string.Empty;
        public string? TranscribedText { get; set; }
        public int? WordsCount { get; set; }
        public int? DurationSeconds { get; set; }
        public SpeakingScoresDto Scores { get; set; } = new();
        public SpeakingFeedbackDto Feedback { get; set; } = new();
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? GradedAt { get; set; }
    }

    public class SpeakingScoresDto
    {
        public decimal? OverallScore { get; set; }
        public decimal? FluencyScore { get; set; }
        public decimal? GrammarScore { get; set; }
        public decimal? VocabularyScore { get; set; }
        public decimal? PronunciationScore { get; set; }
        public string? BandLevel { get; set; } // "Band 6.5", "Band 7.0", etc.
    }

    public class SpeakingFeedbackDto
    {
        public string? Summary { get; set; }
        public string[] Strengths { get; set; } = Array.Empty<string>();
        public string[] Improvements { get; set; } = Array.Empty<string>();
        public string? AIModel { get; set; }
        public string? AIProvider { get; set; }
    }

    public class GroqGradingRequest
    {
        public string AudioBase64 { get; set; } = string.Empty;
        public string PromptText { get; set; } = string.Empty;
        public string QuestionContext { get; set; } = string.Empty;
    }

    public class GroqGradingResponse
    {
        public bool Success { get; set; }
        public string? Transcript { get; set; }
        public int? WordsCount { get; set; }
        public GroqScoresDto Scores { get; set; } = new();
        public string? FeedbackSummary { get; set; }
        public string[] Strengths { get; set; } = Array.Empty<string>();
        public string[] Improvements { get; set; } = Array.Empty<string>();
        public string? ErrorMessage { get; set; }
        public string? Model { get; set; }
        public string? Provider { get; set; }
    }

    public class GroqScoresDto
    {
        public decimal Overall { get; set; }
        public decimal Fluency { get; set; }
        public decimal Grammar { get; set; }
        public decimal Vocabulary { get; set; }
        public decimal Pronunciation { get; set; }
    }
}