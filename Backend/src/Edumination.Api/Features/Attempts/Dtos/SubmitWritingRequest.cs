namespace Edumination.Api.Features.Attempts.Dtos;

public record SubmitWritingRequest
{
    public string ContentText { get; set; } // Nội dung văn bản của bài Writing
    public IFormFile? File { get; set; } // File đính kèm (PDF, JPG, PNG, tùy chọn)
    public string? PromptText { get; set; } // Prompt của câu hỏi (tùy chọn)
    public bool ConfirmSubmission { get; set; } // Xác nhận nộp bài
}