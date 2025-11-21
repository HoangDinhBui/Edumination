using Edumination.Api.Domain.Entities;

namespace Edumination.Domain.Entities;

public class Exercise
{
    public long Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public long CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }

    // Quan hệ với User (nếu có)
    public User? CreatedByUser { get; set; }
}
