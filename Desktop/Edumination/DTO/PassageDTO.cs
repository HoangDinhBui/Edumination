using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IELTS.DTO
{
    public class PassageDTO
    {
        public long Id { get; set; }
        public long SectionId { get; set; }
        public string Title { get; set; }
        public string ContentText { get; set; }
        public string AudioFilePath { get; set; }
        public string TranscriptFilePath { get; set; }
        public int Position { get; set; }
        public DateTime CreatedAt { get; set; }

        public override string ToString()
        {
            return Title ?? $"Passage {Position}";
        }
    }
}
