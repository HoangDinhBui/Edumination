namespace Edumination.Api.Domain.Entities;

public class SectionAttempt
{
    public long Id { get; set; }
    public long TestAttemptId { get; set; }
    public long SectionId { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? FinishedAt { get; set; }
    public decimal? RawScore { get; set; }
    public decimal? ScaledBand { get; set; }
    public string Status { get; set; } = "IN_PROGRESS";
    public virtual TestAttempt TestAttempt { get; set; }
    public virtual TestSection TestSection { get; set; }
    public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>(); // Thêm quan hệ với Answer
    // Thêm UpdatedAt nếu cần
    // public DateTime? UpdatedAt { get; set; }
}