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
    public long? PassageId { get; set; } // Giữ nullable nếu phù hợp

    [Required]
    public string Qtype { get; set; } // Thêm [Required] nếu bắt buộc

    [Required]
    public string Stem { get; set; } // Thêm [Required] nếu bắt buộc

    public string MetaJson { get; set; }
    public int Position { get; set; }
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime CreatedAt { get; set; } // Để database tự gán

    [ForeignKey(nameof(Exercise))]
    public long? ExerciseId { get; set; }

    public virtual Passage? Passage { get; set; }
    public virtual Exercise? Exercise { get; set; }
    public virtual TestSection Section { get; set; } // Loại bỏ ? nếu SectionId là bắt buộc
    public virtual ICollection<QuestionChoice> QuestionChoices { get; set; } = new List<QuestionChoice>();
    public virtual QuestionAnswerKey QuestionAnswerKey { get; set; }
    public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>(); // Thêm quan hệ với Answer
}