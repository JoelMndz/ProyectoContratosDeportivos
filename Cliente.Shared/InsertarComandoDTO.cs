using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cliente.Shared
{
    public record InsertarComandoDTO(
            string Nombres,
            string Apellidos,
            string Cedula,
            string Email,
            string Password,
            string Celular,
            string Rol
    ) : IRequest<UsuarioDTO>;
}
