using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IELTS.DTO
{
    public class TestPaperDTO
    {
        public long Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsPublished { get; set; }

        public long CreatedBy { get; set; }
        public string? CreatedByName { get; set; }
        public DateTime CreatedAt { get; set; }

        // PDF info
        public string? PdfFileName { get; set; }
        public string? PdfFilePath { get; set; }

        // Navigation properties
        public List<TestSectionDTO> Sections { get; set; } = new();

        public override string ToString() => $"{Code} - {Title}";
    }

}
