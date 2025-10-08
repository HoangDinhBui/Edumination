namespace Edumination.Api.Features.Attempts.Dtos;

public record SubmitTestRequest
{
    public bool ConfirmSubmission { get; set; } // Xác nhận nộp toàn bộ bài thi
}