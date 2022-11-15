using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Helper.Comunes.Interfaces
{
    public interface IServicioUsuarioActual
    {
        public string IdUsuario { get; }
        public string Correo { get; }
        public string NombreMostrar { get; }
    }
}
