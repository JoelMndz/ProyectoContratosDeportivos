using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Dominio.Entidades
{
    public partial class Jugadores
    {
        public string NombreRepresentante => $"{Representante.Nombres} {Representante.Apellidos}";
    }
}
