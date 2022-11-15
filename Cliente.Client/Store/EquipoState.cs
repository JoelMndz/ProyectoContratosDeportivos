using Cliente.Shared;
using Cliente.Shared.Jugador;
using System.Net.Http.Json;
using System.Text.Json;

namespace Cliente.Client.Store
{
    public class EquipoState
    {
        public List<EquipoDTO> ListaEquipos { get; set; } = new();
        public EquipoDTO EquipoActual { get; set; } = new();
        public event Action? ActualizarEstado;
        private readonly string ENDPOINT = "/api/equipo";

        public async Task ObtenerTodos(HttpClient Http)
        {
            var data = await Http.GetFromJsonAsync<List<EquipoDTO>>(ENDPOINT);
            if (data != null) ListaEquipos = data;
            else ListaEquipos = new();
            NotificarEstado();
        }
        public async Task ObtenerTodoPorGerente(HttpClient Http, string id)
        {
            var data = await Http.GetFromJsonAsync<List<EquipoDTO>>($"{ENDPOINT}/{id}");
            Console.WriteLine(data);
            if (data != null) ListaEquipos = data;
            else ListaEquipos = new();
            NotificarEstado();
        }
        public async Task Agregar(HttpClient Http, AppState appState, EquipoDTO equipo)
        {
            var data = await Http.PostAsJsonAsync(ENDPOINT, equipo);
            if (data.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                var equipoNuevo = JsonSerializer.Deserialize<EquipoDTO>(await data.Content.ReadAsStringAsync(), options);
                ListaEquipos.Add(equipoNuevo!);
                NotificarEstado();
            }
            else
            {
                await appState.ErrorState.ActualizarMensaje(data);
            }
        }

        public async Task Actualizar(HttpClient Http, AppState appState, EquipoDTO equipo)
        {
            var data = await Http.PutAsJsonAsync(ENDPOINT, equipo);
            if (data.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                var nuevoEquipo = JsonSerializer.Deserialize<EquipoDTO>(await data.Content.ReadAsStringAsync(), options);
                var indice = ListaEquipos.FindIndex(x => x.Id == nuevoEquipo?.Id);
                if (indice != -1) ListaEquipos[indice] = nuevoEquipo!;
                NotificarEstado();
            } else {
                await appState.ErrorState.ActualizarMensaje(data);
            }
        }

        public async Task Eliminar(HttpClient Http, AppState appState, string id)
        {
            var data = await Http.DeleteAsync($"{ENDPOINT}/{id}");
            if (data.IsSuccessStatusCode)
            {
                ListaEquipos.Remove(ListaEquipos.First(x => x.Id == id));
                NotificarEstado();
            }
            else
                await appState.ErrorState.ActualizarMensaje(data);
        }

        private void NotificarEstado()
        {
            ActualizarEstado?.Invoke();
        }
    }
}
