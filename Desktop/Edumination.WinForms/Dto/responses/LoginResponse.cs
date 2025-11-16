using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edumination.WinForms.Dto.responses
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
    }
}
