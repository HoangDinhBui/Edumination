namespace Edumination.Api.Features.Papers.Dtos
{
    public class SectionCreateRequest
    {
        public string Skill { get; set; } // ENUM: LISTENING, READING, WRITING, SPEAKING
        public int SectionNo { get; set; }
        public int TimeLimitSec { get; set; }
        public long? AudioAssetId { get; set; }
    }
}