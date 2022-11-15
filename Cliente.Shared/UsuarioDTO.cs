using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cliente.Shared
{
    public record UsuarioDTO(
        string Id,
        string Nombres,
        string Apellidos,
        string Cedula,
        string Email,
        string Celular,
        string Rol
    );
}
