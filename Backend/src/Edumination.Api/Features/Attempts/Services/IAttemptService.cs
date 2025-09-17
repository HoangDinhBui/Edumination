using Edumination.Api.Features.Attempts.Dtos;

namespace Edumination.Api.Features.Attempts.Services;

public interface IAttemptService
{
    Task<StartAttemptResponse> StartAsync(long userId, StartAttemptRequest req, CancellationToken ct);
    Task<SubmitAnswerResponse> SubmitAnswerAsync(long attemptId, long sectionId, SubmitAnswerRequest request, long userId, CancellationToken ct);
    Task<SubmitSectionResponse> SubmitSectionAsync(long attemptId, long sectionId, SubmitSectionRequest request, long userId, CancellationToken ct);
}
