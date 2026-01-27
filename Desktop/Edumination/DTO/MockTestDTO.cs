using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IELTS.DTO
{
    public class MockTestDTO
    {
        public long Id { get; set; }
        public int Year { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public bool? IsPublished { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }

        public List<TestPaperDTO> Papers { get; set; } = new();
    }



}
