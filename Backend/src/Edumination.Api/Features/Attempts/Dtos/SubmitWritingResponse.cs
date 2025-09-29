namespace Edumination.Api.Features.Attempts.Dtos;

public record SubmitWritingResponse
{
    public long SubmissionId { get; set; }
    public long SectionAttemptId { get; set; }
    public string Status { get; set; }

    public SubmitWritingResponse(long submissionId, long sectionAttemptId, long? assetId, string status)
    {
        SubmissionId = submissionId;
        SectionAttemptId = sectionAttemptId;
        Status = status;
    }
}