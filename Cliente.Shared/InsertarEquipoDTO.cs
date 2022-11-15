using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cliente.Shared
{
    public record InsertarEquipoDTO(string Nombre, string PresupuestoAnual, string IdCategoria, string IdGerente):IRequest<EquipoDTO>;
}
