using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Edumination.Api.Domain.MongoEntities;

public class SubmissionLog
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public long SectionAttemptId { get; set; } // Khóa ngoại "ảo" trỏ về MySQL
    public long UserId { get; set; }
    public long PaperId { get; set; }
    public long SectionId { get; set; }
    
    // Danh sách câu trả lời
    public List<StudentAnswerLog> Answers { get; set; } = new List<StudentAnswerLog>();
    
    public DateTime LastUpdatedAt { get; set; } = DateTime.UtcNow;
}

public class StudentAnswerLog
{
    public long QuestionId { get; set; }
    public BsonDocument? AnswerJson { get; set; } // Lưu nguyên cục JSON câu trả lời
    public bool? IsCorrect { get; set; }
    public decimal? EarnedScore { get; set; }
    public DateTime AnsweredAt { get; set; }
}