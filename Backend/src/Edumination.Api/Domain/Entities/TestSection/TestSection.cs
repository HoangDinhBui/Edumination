namespace Edumination.Api.Domain.Entities;

public class TestSection
{
    public long Id { get; set; }
    public long PaperId { get; set; }
    public string Skill { get; set; } = default!;
    public int SectionNo { get; set; }
    public int? TimeLimitSec { get; set; }
    public bool IsPublished { get; set; }
}
