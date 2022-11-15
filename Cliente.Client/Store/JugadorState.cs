using Cliente.Shared;
using Cliente.Shared.Jugador;
using System.Net.Http.Json;
using System.Reflection;
using System.Text.Json;

namespace Cliente.Client.Store
{
    public class JugadorState
    {
        public List<JugadorDTO> ListaJugadores { get; set; } = new();
        public event Action? ActualizarEstado;
        public async Task ObtenerTodos(HttpClient Http)
        {
            var data = await Http.GetFromJsonAsync<List<JugadorDTO>>("/api/jugador");
            if (data != null) ListaJugadores = data;
            else ListaJugadores = new();
            NotificarEstado();
        }
        public async Task ObtenerTodosPorRepresentante(HttpClient Http, string id)
        {
            var data = await Http.GetFromJsonAsync<List<JugadorDTO>>($"/api/jugador/{id}");
            if (data != null) ListaJugadores = data;
            else ListaJugadores = new();
            NotificarEstado();
        }

        public async Task Agregar(HttpClient Http, AppState appState, JugadorDTO jugador)
        {
            var data = await Http.PostAsJsonAsync("/api/jugador", jugador);
            if (data.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                var nuevoJugador = JsonSerializer.Deserialize<JugadorDTO>(await data.Content.ReadAsStringAsync(), options);
                ListaJugadores.Add(nuevoJugador!);
                NotificarEstado();
            }
            else
            {
                await appState.ErrorState.ActualizarMensaje(data);
            }
        }
        public async Task Actualizar(HttpClient Http, AppState appState, JugadorDTO jugador)
        {
            var data = await Http.PutAsJsonAsync("/api/jugador", jugador);
            if (data.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                var nuevoJugador = JsonSerializer.Deserialize<JugadorDTO>(await data.Content.ReadAsStringAsync(), options);
                var indice = ListaJugadores.FindIndex(x => x.Id == nuevoJugador?.Id);
                if (indice != -1) ListaJugadores[indice] = nuevoJugador!;
                NotificarEstado();
            }
            else
            {
                await appState.ErrorState.ActualizarMensaje(data);
            }
        }

        public async Task Eliminar(HttpClient Http, AppState appState, string id)
        {
            var data = await Http.DeleteAsync($"/api/jugador/{id}");
            if (data.IsSuccessStatusCode)
            {
                ListaJugadores.Remove(ListaJugadores.First(x => x.Id == id));
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
