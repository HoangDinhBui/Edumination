namespace Edumination.Api.Features.Attempts.Dtos;
public class SubmitSectionResponse
{
    public long SectionAttemptId { get; set; }
    public decimal? RawScore { get; set; }
    public decimal? ScaledBand { get; set; }
    public string Status { get; set; }

    public SubmitSectionResponse(long sectionAttemptId, decimal? rawScore, decimal? scaledBand, string status)
    {
        SectionAttemptId = sectionAttemptId;
        RawScore = rawScore;
        ScaledBand = scaledBand;
        Status = status;
    }
}