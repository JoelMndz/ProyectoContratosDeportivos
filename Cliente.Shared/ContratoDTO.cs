using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cliente.Shared
{
    public class ContratoDTO
    {
        public string? Id { get; set; } = default;
        public string Precio { get; set; } = string.Empty;
        public string FechaInicio { get; set; } = string.Empty;
        public string FechaFin { get; set; } = string.Empty;
        public string? Documento { get; set; } = default;
        public string Descripcion { get; set; } = string.Empty;
        public string NumeroJugador { get; set; } = string.Empty;
        public string IdEquipo { get; set; } = string.Empty;
        public string IdJugador { get; set; } = string.Empty;
        public string? NombreEquipo { get; set; } = default;
        public string? NombreJugador { get; set; } = default;
    }
}