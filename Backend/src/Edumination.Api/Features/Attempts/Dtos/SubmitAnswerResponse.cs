namespace Edumination.Api.Features.Attempts.Dtos;

public record SubmitAnswerResponse(long Id, long QuestionId, bool? IsCorrect, decimal? EarnedScore);