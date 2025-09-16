namespace Edumination.Api.Features.Attempts.Dtos;

public record SubmitAnswerRequest(long QuestionId, object AnswerJson);