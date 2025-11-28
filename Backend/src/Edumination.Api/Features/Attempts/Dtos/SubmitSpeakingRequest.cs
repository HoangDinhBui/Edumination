namespace Edumination.Api.Features.Attempts.Dtos;

public record SubmitSpeakingRequest
{
    public IFormFile AudioFile { get; set; } 
    public string? PromptText { get; set; } 
    public bool ConfirmSubmission { get; set; }
    
    // Optional: For tracking which part and question this submission belongs to
    public int? PartPosition { get; set; }
    public int? QuestionIndex { get; set; }
}