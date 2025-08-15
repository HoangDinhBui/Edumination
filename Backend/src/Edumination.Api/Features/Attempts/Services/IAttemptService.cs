using Edumination.Api.Features.Attempts.Dtos;

namespace Edumination.Api.Features.Attempts.Services;

public interface IAttemptService
{
    Task<StartAttemptResponse> StartAsync(long userId, StartAttemptRequest req, CancellationToken ct);
}
