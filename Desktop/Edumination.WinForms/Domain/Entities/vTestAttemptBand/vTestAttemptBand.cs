
namespace Edumination.Api.Domain.Entities;

public class vTestAttemptBand
    {
        public long TestAttemptId { get; set; }
        public long UserId { get; set; }
        public long PaperId { get; set; }
        public decimal? OverallBand { get; set; }
    }