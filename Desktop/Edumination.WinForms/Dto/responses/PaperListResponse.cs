using Edumination.WinForms.Domain.Entities.TestPaper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edumination.WinForms.Dto.responses
{
    public class PaperListResponse
    {
        public string Title { get; set; } = default!;
        public List<PaperItem> Items { get; set; } = new List<PaperItem>();
    }
}
