using Aplicacion.Helper.Comunes.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Dominio.Entidades
{
    public partial class Usuarios : IEntity
    {
        public int Id { get; set; }
        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string Cedula { get; set; } = string.Empty; 
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Celular { get; set; } = string.Empty;
        public string? CodigoRecuperacion { get; set; } = default;
        public DateTime FechaRegistro { get; set; }
        public Roles Rol { get; set; }
        public List<Equipos> ListaEquipos { get; set; } = new List<Equipos>();
        public List<Jugadores> ListaJugadores { get; set; } = new List<Jugadores>();
    }

    public enum Roles
    {
        Administrador,
        Representante,
        Gerente
    }
}
