using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Edumination.Api.Domain.Entities;

namespace Edumination.Domain.Entities;

public class QuestionChoice
{
    public long Id { get; set; }

    [ForeignKey(nameof(Question))]
    public long QuestionId { get; set; }

    [Required]
    [StringLength(1000)]
    public string? Content { get; set; }

    public int Position { get; set; }

    [Required]
    public bool IsCorrect { get; set; }
    [JsonIgnore]

    public virtual Question? Question { get; set; }
}