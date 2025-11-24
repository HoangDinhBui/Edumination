using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Edumination.Domain.Entities;

namespace Edumination.Api.Domain.Entities;

public class Question
{
    public long Id { get; set; }

    [ForeignKey(nameof(Section))]
    public long SectionId { get; set; }

    [ForeignKey(nameof(Passage))]
    public long? PassageId { get; set; }

    [Required]
    public string Qtype { get; set; }

    [Required]
    public string Stem { get; set; }

    public string MetaJson { get; set; }
    public int Position { get; set; }
    
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime CreatedAt { get; set; }

    [ForeignKey(nameof(Exercise))]
    public long? ExerciseId { get; set; }

    // Navigation properties
    public virtual Passage? Passage { get; set; }
    public virtual Exercise? Exercise { get; set; }
    public virtual TestSection Section { get; set; }
    public virtual ICollection<QuestionChoice> QuestionChoices { get; set; } = new List<QuestionChoice>();
    public virtual QuestionAnswerKey QuestionAnswerKey { get; set; }
    public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();

    // Computed properties for UI compatibility
    [NotMapped]
    public string Text
    {
        get => Stem;
        set => Stem = value;
    }

    [NotMapped]
    public string Type
    {
        get => Qtype;
        set => Qtype = value;
    }
}