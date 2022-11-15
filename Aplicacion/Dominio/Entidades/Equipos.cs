using Aplicacion.Helper.Comunes.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Dominio.Entidades
{
    public partial class Equipos : IEntity
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int PresupuestoAnual { get; set; }
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        public int IdCategoria { get; set; }
        public int IdGerente { get; set; }
        public Categorias Categoria { get; set; } = new Categorias();
        public Usuarios Gerente { get; set; } = new Usuarios();
        public List<Contratos> ListaContratos { get; set; } = new List<Contratos>();
    }
}
