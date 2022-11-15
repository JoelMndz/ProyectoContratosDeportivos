using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cliente.Shared
{
    public class RecuperarPasswordDTO:IRequest<int>
    {
        public string Email { get; set; }
        public string Password { get; set; } = string.Empty;
        public string Codigo { get; set; } = string.Empty;
    }
}
