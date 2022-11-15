using Cliente.Client.Auth;
using Cliente.Shared;
using System.Collections.Generic;
using System.Net.Http.Json;
using static System.Net.WebRequestMethods;

namespace Cliente.Client.Store
{
    public class UsuarioState
    {
        public UsuarioDTO UsuarioActual { get; set; } = new("", "", "", "", "", "", "");
        public List<UsuarioDTO>? ListaUsuarios { get; set; } = new();
        public event Action? ActualizarEstado;

        public void ActualizarUsuario(UsuarioDTO? usuario)
        {
            if (usuario != null)
            {
                UsuarioActual = usuario;
                NotificarEstado();
            }
        }
        public async Task ObtenerTodos(HttpClient Http)
        {
            ListaUsuarios = await Http.GetFromJsonAsync<List<UsuarioDTO>>("/api/usuario");
        }

        public async Task<List<UsuarioDTO>> ObtenerRepresentantes(HttpClient Http)
        {
            var lista = await Http.GetFromJsonAsync<List<UsuarioDTO>>("/api/usuario");
            if (lista == null) return new List<UsuarioDTO>();
            return lista.FindAll(x => x.Rol == "Representante");
        }

        public async Task<List<UsuarioDTO>> ObtenerGerentes(HttpClient Http)
        {
            var lista = await Http.GetFromJsonAsync<List<UsuarioDTO>>("/api/usuario");
            if (lista == null) return new List<UsuarioDTO>();
            return lista.FindAll(x => x.Rol == "Gerente");
        }

        private void NotificarEstado()
        {
            ActualizarEstado?.Invoke();
        }
    }
}
