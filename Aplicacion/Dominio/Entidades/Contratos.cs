using Aplicacion.Helper.Comunes.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Dominio.Entidades
{
    public partial class Contratos:IEntity
    {
        public int Id { get; set; }
        public int Precio { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string UrlDocumento { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        public string NumeroJugador { get; set; } = string.Empty;
        public int IdEquipo { get; set; }
        public int IdJugador { get; set; }
        public Equipos Equipo { get; set; } = new Equipos();
        public Jugadores Jugador { get; set; } = new Jugadores();

    }
}
