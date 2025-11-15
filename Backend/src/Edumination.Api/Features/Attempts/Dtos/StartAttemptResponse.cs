namespace Edumination.Api.Features.Attempts.Dtos;

public record StartAttemptResponse(long AttemptId, IEnumerable<SectionSummary> Sections);
public record SectionSummary(long Id, string Skill, int? TimeLimitSec);
