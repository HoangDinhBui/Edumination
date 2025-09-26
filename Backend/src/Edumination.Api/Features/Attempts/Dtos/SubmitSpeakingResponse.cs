namespace Edumination.Api.Features.Attempts.Dtos;

public record SubmitSpeakingResponse
{
    public long SubmissionId { get; set; }
    public long SectionAttemptId { get; set; }
    public long AudioAssetId { get; set; }
    public string Status { get; set; }

    public SubmitSpeakingResponse(long submissionId, long sectionAttemptId, long audioAssetId, string status)
    {
        SubmissionId = submissionId;
        SectionAttemptId = sectionAttemptId;
        AudioAssetId = audioAssetId;
        Status = status;
    }
}