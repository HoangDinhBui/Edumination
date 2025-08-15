namespace Edumination.Api.Features.Attempts.Dtos;

public record StartAttemptResponse(long AttemptId, IEnumerable<SectionSummary> Sections);
public record SectionSummary(long SectionId, string Skill, int? TimeLimitSec);
