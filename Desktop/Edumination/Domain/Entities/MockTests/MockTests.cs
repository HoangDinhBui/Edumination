using Edumination.Api.Domain.Entities;

namespace Edumination.Api.Domain.Entities;

public class MockTest
{
    public long Id { get; set; }
    public int Year { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty; // "DRAFT", "PUBLISHED", "ARCHIVED"
    public DateTime CreatedAt { get; set; }
    public DateTime? PublishedAt { get; set; }

    // Navigation property
    public virtual ICollection<MockTestQuarter> Quarters { get; set; } = new List<MockTestQuarter>();
}