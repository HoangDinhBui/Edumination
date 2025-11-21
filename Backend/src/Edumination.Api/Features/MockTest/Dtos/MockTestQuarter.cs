namespace Edumination.Api.Features.MockTest.Dtos;
public class MockTestQuarter
    {
        public long Id { get; set; }
        public long MockTestId { get; set; }

        public string Quarter { get; set; }  // Q1, Q2, Q3, Q4

        public byte SetNumber { get; set; }

        public long? ListeningPaperId { get; set; }
        public long? ReadingPaperId { get; set; }

        public long? WritingPaperId { get; set; }

        public long? SpeakingPaperId { get; set; }

        public string Status { get; set; }  // DRAFT, PUBLISHED, ARCHIVED

        public DateTime CreatedAt { get; set; }

        public DateTime? PublishedAt { get; set; }
    }