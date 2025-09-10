using System.ComponentModel.DataAnnotations.Schema;

namespace Edumination.Api.Domain.Entities;

[Table("course_band_rules")]
public class CourseBandRule
{
    public long Id { get; set; }
    [Column("course_id")] public long CourseId { get; set; }
    [Column("band_min", TypeName = "decimal(3,1)")] public decimal BandMin { get; set; }
    [Column("band_max", TypeName = "decimal(3,1)")] public decimal BandMax { get; set; }

    // optional navigation
    public Course? Course { get; set; }
}
