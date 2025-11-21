namespace Edumination.Api.Features.Attempts.Dtos;

public class SectionResultDto
{
    public string PaperTitle { get; set; } = string.Empty;
    public string CandidateName { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
    public string BandScore { get; set; } = "0";
    public string RawScore { get; set; } = "0/0"; // Format: "35/40"
    public string TimeTaken { get; set; } = "00:00";
    
    // Danh sách câu trả lời để hiển thị
    public List<QuestionResultDto> Questions { get; set; } = new();
}

public class QuestionResultDto
{
    public long Id { get; set; }
    public int Position { get; set; } // Số thứ tự (1, 2, 3...)
    public string UserAnswerText { get; set; } = string.Empty; // Nội dung user chọn (A, B, hoặc chữ)
    public string CorrectAnswerText { get; set; } = string.Empty; // Đáp án đúng
    public bool IsCorrect { get; set; }
    public string PartTitle { get; set; } = string.Empty; // Ví dụ: "Questions 1-10"
}