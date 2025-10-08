namespace Edumination.Api.Features.Attempts.Dtos;

public record SubmitTestResponse
{
    public long AttemptId { get; set; }
    public decimal OverallBand { get; set; }
    public IEnumerable<SectionBandSummary> SectionBands { get; set; } = new List<SectionBandSummary>();
    public string Status { get; set; } = string.Empty;
    public bool IsBestAttempt { get; set; } // True nếu đây là best score cho paper này

    public SubmitTestResponse(long attemptId, decimal overallBand, IEnumerable<SectionBandSummary> sectionBands, string status, bool isBestAttempt)
    {
        AttemptId = attemptId;
        OverallBand = overallBand;
        SectionBands = sectionBands;
        Status = status;
        IsBestAttempt = isBestAttempt;
    }
}