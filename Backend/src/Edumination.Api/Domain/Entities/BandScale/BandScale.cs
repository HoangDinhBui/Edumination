using System.ComponentModel.DataAnnotations.Schema;
using Edumination.Api.Domain.Entities;

namespace Edumination.Domain.Entities;

public class BandScale
{
    [Column("id")]
    public long Id { get; set; }

    [Column("paper_id")]
    public long PaperId { get; set; }

    [Column("skill")]
    public string Skill { get; set; } // ENUM: LISTENING, READING, WRITING, SPEAKING

    [Column("raw_min")]
    public int RawMin { get; set; }

    [Column("raw_max")]
    public int RawMax { get; set; }

    [Column("band")]
    public decimal Band { get; set; }

    // Navigation property
    [ForeignKey("PaperId")]
    public virtual TestPaper TestPaper { get; set; }
}