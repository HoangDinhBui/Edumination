namespace Edumination.Api.Features.Leaderboard.Dtos;

public record LeaderboardEntryDto(
    long UserId,
    string FullName,
    string Email,
    decimal BestOverallBand,
    DateTime BestAt
);
