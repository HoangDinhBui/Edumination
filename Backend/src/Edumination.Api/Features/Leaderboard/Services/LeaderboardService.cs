using Edumination.Api.Features.Leaderboard.Dtos;
using Edumination.Api.Features.Leaderboard.Interfaces;
using Edumination.Api.Infrastructure.Persistence;
using Edumination.Api.Domain.Entities.Leaderboard;
using Microsoft.EntityFrameworkCore;

namespace Edumination.Api.Features.Leaderboard.Services;

public sealed class LeaderboardService : ILeaderboardService
{
    private readonly AppDbContext _db;
    public LeaderboardService(AppDbContext db) => _db = db;

    public async Task<IReadOnlyList<LeaderboardEntryDto>> GetLeaderboardAsync(long paperId, int limit, CancellationToken ct = default)
    {
        var q =
            from lb in _db.LeaderboardEntries.AsNoTracking()
            join u in _db.Users.AsNoTracking() on lb.UserId equals u.Id
            where lb.PaperId == paperId && u.IsActive
            orderby lb.BestOverallBand descending, lb.BestAt descending
            select new LeaderboardEntryDto(
                u.Id,
                u.FullName,
                u.Email,
                lb.BestOverallBand,
                lb.BestAt
            );

        return await q.Take(limit <= 0 ? 100 : limit).ToListAsync(ct);
    }
}
