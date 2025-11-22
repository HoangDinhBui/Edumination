using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edumination.WinForms.Domain.Entities.TestPaper
{
    public class PaperItem
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Taken { get; set; }
    }
}
