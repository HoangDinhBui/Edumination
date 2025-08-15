namespace Edumination.Api.Domain.Entities;

public class TestAttempt
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public long PaperId { get; set; }
    public int AttemptNo { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? FinishedAt { get; set; }
    public string Status { get; set; } = "IN_PROGRESS";
}
