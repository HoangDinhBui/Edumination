using Education.Domain.Entities;

namespace Edumination.Api.Domain.Entities;

public class WritingSubmission
{
    public long Id { get; set; }
    public long SectionAttemptId { get; set; }
    public string ContentText { get; set; } // Nội dung văn bản
    public string? PromptText { get; set; } // Prompt của câu hỏi (tùy chọn)
    public DateTime CreatedAt { get; set; }

    // Navigation properties
    public SectionAttempt SectionAttempt { get; set; }
}