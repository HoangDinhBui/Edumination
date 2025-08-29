using Education.Domain.Entities;
using Edumination.Domain.Entities;

namespace Edumination.Api.Domain.Entities;

public class TestSection
{
    public long Id { get; set; }
    public long PaperId { get; set; }
    public string Skill { get; set; } = default!; // Có thể thay bằng enum: LISTENING, READING, WRITING, SPEAKING
    public int SectionNo { get; set; }
    public int? TimeLimitSec { get; set; }
    public bool IsPublished { get; set; }
    public long? AudioAssetId { get; set; } // Liên kết với Asset (audio cho Listening)

    // Navigation properties
    public virtual TestPaper TestPaper { get; set; } = default!;
    public virtual Asset? AudioAsset { get; set; } // Liên kết với Asset
    public virtual ICollection<Passage> Passages { get; set; } = new List<Passage>();
}