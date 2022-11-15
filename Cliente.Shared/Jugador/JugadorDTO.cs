using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Cliente.Shared.Jugador
{
    public class JugadorDTO:IRequest<JugadorDTO>
    {
        public string? Id { get; set; } = default;
        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string Posicion { get; set; } = string.Empty;
        public string IdRepresentante { get; set; } = string.Empty;
        public string? NombreRepresentante { get; set; } = default;
    }
}
