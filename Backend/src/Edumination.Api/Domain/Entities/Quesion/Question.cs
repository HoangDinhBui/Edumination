using System.ComponentModel.DataAnnotations.Schema;
using Edumination.Domain.Entities;

namespace Edumination.Api.Domain.Entities; // Di chuyển vào namespace chính

public class Question
{
    public long Id { get; set; }

    [ForeignKey(nameof(Section))]
    public long SectionId { get; set; }

    [ForeignKey(nameof(Passage))]
    public long? PassageId { get; set; } // Thay đổi thành nullable nếu phù hợp

    public string Qtype { get; set; } // Xóa giá trị mặc định
    public string Stem { get; set; } // Xóa giá trị mặc định
    public string? MetaJson { get; set; }
    public int Position { get; set; }
    public DateTime CreatedAt { get; set; } // Xóa giá trị mặc định, để database xử lý

    [ForeignKey(nameof(Exercise))]
    public long? ExerciseId { get; set; }

    public virtual Passage? Passage { get; set; }
    public virtual Exercise? Exercise { get; set; }
    public virtual TestSection? Section { get; set; }
    public virtual ICollection<QuestionChoice>? QuestionChoices { get; set; } = new List<QuestionChoice>();
    public virtual QuestionAnswerKey? QuestionAnswerKey { get; set; }
}