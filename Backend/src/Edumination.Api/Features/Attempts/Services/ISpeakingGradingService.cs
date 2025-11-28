using System.Threading;
using System.Threading.Tasks;
using Edumination.Api.Features.Attempts.Dtos;

namespace Edumination.Api.Features.Attempts.Services;

public interface ISpeakingGradingService
{
    Task<string> TranscribeAudioAsync(string audioUrl, CancellationToken ct = default);
    Task<SpeakingGradingResult> GradeSpeakingAsync(string transcript, CancellationToken ct = default);
    Task<SpeakingGradingResult> ProcessSpeakingSubmissionAsync(string audioUrl, CancellationToken ct = default);
}
