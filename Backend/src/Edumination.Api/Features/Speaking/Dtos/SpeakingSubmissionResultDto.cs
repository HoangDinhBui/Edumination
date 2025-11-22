public class SpeakingSubmissionResultDto
{
    public long SubmissionId { get; set; }
    public long SectionAttemptId { get; set; }
    public long? QuestionId { get; set; }
    public string PromptText { get; set; }
    public string AudioUrl { get; set; }  // From assets table
    public int DurationSec { get; set; }
    
    // Transcript
    public string TranscribedText { get; set; }
    public int WordsCount { get; set; }
    
    // Scores (from ai_evaluations)
    public decimal? OverallScore { get; set; }
    public decimal? BandScore { get; set; }
    
    // Detailed scores (from ai_evaluation_details)
    public Dictionary<string, DetailedScore> CriteriaScores { get; set; }
    
    // Feedback
    public string FeedbackSummary { get; set; }
    public List<string> Strengths { get; set; }
    public List<string> ImprovementAreas { get; set; }
    
    // Metadata
    public string Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? GradedAt { get; set; }
}

public class DetailedScore
{
    public string CriterionCode { get; set; }
    public string CriterionName { get; set; }
    public decimal Score { get; set; }
    public string Feedback { get; set; }
}