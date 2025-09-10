using Edumination.Api.Features.Stats.Dtos;

namespace Edumination.Api.Features.Stats.Services;

public interface IUserStatsService
{
    Task<UserStatsDto?> GetUserStatsAsync(long userId, CancellationToken ct = default);
}