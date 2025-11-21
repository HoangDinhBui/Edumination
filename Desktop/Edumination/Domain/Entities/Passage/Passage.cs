using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Education.Domain.Entities;
using Edumination.Api.Domain.Entities;

namespace Edumination.Domain.Entities;

public class Passage
{
    public long Id { get; set; }
    public long SectionId { get; set; }
    public string? Title { get; set; }
    public string? ContentText { get; set; } // MEDIUMTEXT trong database
    public long? AudioId { get; set; }
    public long? TranscriptId { get; set; }
    public int Position { get; set; }
    public DateTime CreatedAt { get; set; }

    [JsonIgnore]

    // Navigation properties
    public virtual TestSection? TestSection { get; set; }
    public virtual Asset? Audio { get; set; }
    public virtual Asset? Transcript { get; set; }
    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
}
