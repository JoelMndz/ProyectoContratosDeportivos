using Cliente.Shared;
using Cliente.Shared.Jugador;
using System.Net.Http.Json;
using System.Text.Json;

namespace Cliente.Client.Store
{
    public class CategoriaState
    {
        public List<CategoriaDTO> ListaCategorias { get; set; } = new();
        public event Action? ActualizarEstado;
        private readonly string ENDPOINT = "/api/categoria";
        public async Task ObtenerTodos(HttpClient Http)
        {
            var data = await Http.GetFromJsonAsync<List<CategoriaDTO>>(ENDPOINT);
            if (data != null) ListaCategorias = data;
            else ListaCategorias = new();
            NotificarEstado();
        }

        public async Task Agregar(HttpClient Http, AppState appState, CategoriaDTO categoria)
        {
            var data = await Http.PostAsJsonAsync(ENDPOINT, categoria);
            if (data.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                var categoriaIngresada = JsonSerializer.Deserialize<CategoriaDTO>(await data.Content.ReadAsStringAsync(), options);
                ListaCategorias.Add(categoriaIngresada!);
                NotificarEstado();
            }
            else
            {
                await appState.ErrorState.ActualizarMensaje(data);
            }
        }
        public async Task Actualizar(HttpClient Http, AppState appState, CategoriaDTO categoria)
        {
            var data = await Http.PutAsJsonAsync(ENDPOINT, categoria);
            if (data.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                var categoriaIngresada = JsonSerializer.Deserialize<CategoriaDTO>(await data.Content.ReadAsStringAsync(), options);
                var indice = ListaCategorias.FindIndex(x => x.Id == categoriaIngresada?.Id);
                if (indice != -1) ListaCategorias[indice] = categoriaIngresada!;
                NotificarEstado();
            }
            else
            {
                await appState.ErrorState.ActualizarMensaje(data);
            }
        }

        public async Task Eliminar(HttpClient Http, AppState appState, string id)
        {
            var data = await Http.DeleteAsync($"{ENDPOINT}/{id}");
            if (data.IsSuccessStatusCode)
            {
                ListaCategorias.Remove(ListaCategorias.First(x => x.Id == id));
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
