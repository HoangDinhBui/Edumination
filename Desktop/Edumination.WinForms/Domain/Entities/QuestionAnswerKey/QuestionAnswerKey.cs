using Edumination.Api.Domain.Entities;

namespace Edumination.Domain.Entities;

public class QuestionAnswerKey
{
    public long Id { get; set; }
    public long QuestionId { get; set; }
    public string? KeyJson { get; set; }

    // Navigation property
    public virtual Question? Question { get; set; }
}