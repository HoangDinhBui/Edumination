using Edumination.Api.Features.Speaking;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Edumination.Api.Features.Speaking.Interfaces
{
    public interface ISpeakingGradingService
    {
        Task<GroqGradingResponse> GradeSubmissionAsync(
            Stream audioStream, 
            string promptText, 
            string? questionContext = null,
            CancellationToken cancellationToken = default);

        Task<GroqGradingResponse> RetryGradingAsync(
            long submissionId,
            CancellationToken cancellationToken = default);

        Task<string?> TranscribeOnlyAsync(
            Stream audioStream,
            CancellationToken cancellationToken = default);

        Task<(bool IsValid, string? ErrorMessage)> ValidateAudioAsync(Stream audioStream);
    }
}