namespace Edumination.WinForms.Dto.Papers
{
    public class BandScaleCreateRequest
    {
        public string Skill { get; set; } // ENUM: LISTENING, READING, WRITING, SPEAKING
        public List<BandScaleRange> Ranges { get; set; }

        public class BandScaleRange
        {
            public int RawMin { get; set; }
            public int RawMax { get; set; }
            public decimal Band { get; set; }
        }
    }
}

