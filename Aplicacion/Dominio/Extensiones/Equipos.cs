using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Dominio.Entidades
{
    public partial class Equipos
    {
        public string NombreCategoria => Categoria.Nombre;
        public string NombreGerente => $"{Gerente.Nombres} {Gerente.Apellidos}";
    }
}
