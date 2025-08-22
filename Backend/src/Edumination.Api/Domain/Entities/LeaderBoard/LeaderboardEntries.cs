
namespace Edumination.Api.Domain.Entities.Leaderboard
{
    public class LeaderboardEntry
    {
        public long Id { get; set; }

        public long UserId { get; set; }

        public long PaperId { get; set; }

        public decimal BestOverallBand { get; set; }

        public DateTime BestAt { get; set; }
    }
}
