
using Aplicacion.Helper.Comunes.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Dominio.Entidades
{
    public partial class Contratos
    {
        public string NombreEquipo => Equipo.Nombre;
        public string NombreJugador => $"{Jugador.Nombres} {Jugador.Apellidos}";
    }
}
