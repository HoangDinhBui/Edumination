namespace Edumination.WinForms.Dto.Papers
{
    public class SectionCreateRequest
    {
        public string? Skill { get; set; } // ENUM: LISTENING, READING, WRITING, SPEAKING
        public int SectionNo { get; set; }
        public int TimeLimitSec { get; set; }
        public long? AudioAssetId { get; set; }
    }
}