using Aplicacion.Helper.Comunes.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Dominio.Entidades
{
    public partial class Jugadores:IEntity
    {
        public int Id { get; set; }
        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string Posicion { get; set; } = string.Empty;
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        public int IdRepresentante { get; set; }
        public Usuarios Representante { get; set; } = new Usuarios();
        public List<Contratos> ListaContratos { get; set; } = new List<Contratos>();
    }
}
