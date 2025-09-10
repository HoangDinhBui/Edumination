using Edumination.Api.Features.Stats.Services;
using Edumination.Api.Features.Stats.Dtos;
using Edumination.Api.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Edumination.Api.Features.Stats.Services;

public class UserStatsService : IUserStatsService
{
    private readonly AppDbContext _db;

    public UserStatsService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<UserStatsDto?> GetUserStatsAsync(long userId, CancellationToken ct = default)
    {
        var stats = await _db.UserStats.FirstOrDefaultAsync(x => x.UserId == userId);
        if (stats == null) return null;

        return new UserStatsDto
        {
            UserId = stats.UserId,
            TotalTests = stats.TotalTests,
            BestBand = stats.BestBand,
            WorstBand = stats.WorstBand,
            BestSkill = stats.BestSkill,
            WorstSkill = stats.WorstSkill,
            AvgListeningBand = stats.AvgListeningBand,
            AvgReadingBand = stats.AvgReadingBand,
            AvgWritingBand = stats.AvgWritingBand,
            AvgSpeakingBand = stats.AvgSpeakingBand,
            UpdatedAt = stats.UpdatedAt
        };
    }
}