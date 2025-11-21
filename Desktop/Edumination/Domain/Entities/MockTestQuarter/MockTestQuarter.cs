using Edumination.Api.Domain.Entities;

namespace Edumination.Api.Domain.Entities;
public class MockTestQuarter
{
    public long Id { get; set; }
    public long MockTestId { get; set; }
    public string Quarter { get; set; } = string.Empty; // "Q1", "Q2", "Q3", "Q4"
    public int SetNumber { get; set; }
    public long? ListeningPaperId { get; set; }
    public long? ReadingPaperId { get; set; }
    public long? WritingPaperId { get; set; }
    public long? SpeakingPaperId { get; set; }
    public string Status { get; set; } = string.Empty; // "DRAFT", "PUBLISHED", "ARCHIVED"
    public DateTime CreatedAt { get; set; }
    public DateTime? PublishedAt { get; set; }

    // --- Navigation Properties ---
    
    // Đến MockTest (cha)
    public virtual MockTest MockTest { get; set; }

    // Đến các TestPapers (liên kết)
    public virtual TestPaper ListeningPaper { get; set; }
    public virtual TestPaper ReadingPaper { get; set; }
    public virtual TestPaper WritingPaper { get; set; }
    public virtual TestPaper SpeakingPaper { get; set; }
}