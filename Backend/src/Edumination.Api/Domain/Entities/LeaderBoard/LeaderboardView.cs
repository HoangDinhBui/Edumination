namespace Edumination.Api.Domain.Entities.Leaderboard;

// Keyless dùng cho projection từ SQL
public class LeaderboardRow
{
    public long UserId { get; set; }
    public string Email { get; set; } = default!;
    public string FullName { get; set; } = default!;
    public decimal BestOverallBand { get; set; }
    public DateTime BestAt { get; set; }
}
