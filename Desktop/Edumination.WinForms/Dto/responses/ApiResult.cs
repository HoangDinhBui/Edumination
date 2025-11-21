using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edumination.WinForms.Dto.responses
{
    public class ApiResult<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; } = default!;
        public string? Message { get; set; }
    }
}
