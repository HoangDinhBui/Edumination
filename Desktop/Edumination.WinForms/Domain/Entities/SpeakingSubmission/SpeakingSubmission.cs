using Education.Domain.Entities;

namespace Edumination.Api.Domain.Entities;

public class SpeakingSubmission
{
    public long Id { get; set; }
    public long SectionAttemptId { get; set; }
    public string? PromptText { get; set; }
    public long AudioAssetId { get; set; }
    public string? AsrText { get; set; }
    public int? WordsCount { get; set; }
    public int? DurationSec { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation properties
    public SectionAttempt SectionAttempt { get; set; }
    public Asset AudioAsset { get; set; }
}