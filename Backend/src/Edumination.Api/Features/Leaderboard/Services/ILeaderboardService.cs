using Edumination.Api.Features.Leaderboard.Dtos;

namespace Edumination.Api.Features.Leaderboard.Interfaces;

public interface ILeaderboardService
{
    Task<IReadOnlyList<LeaderboardEntryDto>> GetLeaderboardAsync(long paperId, int limit, CancellationToken ct = default);
}
