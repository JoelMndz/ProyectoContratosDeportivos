using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cliente.Shared
{
    public class EquipoDTO:IRequest<EquipoDTO>
    {
        public string? Id { get; set; } = default;
        public string Nombre { get; set; } = string.Empty;
        public string PresupuestoAnual { get; set; } = string.Empty;
        public string FechaRegistro { get; set; } = string.Empty;
        public string IdCategoria { get; set; } = string.Empty; 
        public string IdGerente { get; set; } = string.Empty;
        public string? NombreCategoria { get; set; } = default;
        public string? NombreGerente { get; set; }
    }
}
