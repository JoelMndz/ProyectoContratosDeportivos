using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cliente.Shared
{
    public class EnviarCodigoDTO : IRequest<int>
    {
        public string Email { get; set; } = string.Empty;
    }
}
