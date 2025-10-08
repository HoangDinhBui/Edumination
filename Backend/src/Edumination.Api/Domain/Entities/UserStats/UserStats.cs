
using System.ComponentModel.DataAnnotations.Schema;
using Edumination.Api.Domain.Enums;

namespace Edumination.Api.Domain.Entities;
public class UserStats
{
    public long UserId { get; set; }
    public int TotalTests { get; set; } = 0;
    public decimal? BestBand { get; set; }
    public decimal? WorstBand { get; set; }
    public Skill? BestSkill { get; set; }
    public Skill? WorstSkill { get; set; }
    public decimal? AvgListeningBand { get; set; }
    public decimal? AvgReadingBand { get; set; }
    public decimal? AvgWritingBand { get; set; }
    public decimal? AvgSpeakingBand { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation
     [ForeignKey("UserId")]
    public User? User { get; set; }
}