namespace Edumination.Api.Features.Attempts.Dtos;

public record SubmitSpeakingRequest
{
    public IFormFile AudioFile { get; set; } 
    public string? PromptText { get; set; } 
    public bool ConfirmSubmission { get; set; } 
}