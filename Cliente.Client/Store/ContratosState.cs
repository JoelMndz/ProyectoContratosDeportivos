using Cliente.Shared;
using Cliente.Shared.Jugador;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace Cliente.Client.Store
{
    public class ContratosState
    {
        public List<ContratoDTO> ListaContratos { get; set; } = new();
        public event Action? ActualizarEstado;
        private readonly string ENDPOINT = "/api/contrato";
        private record Comando(string Id);

        public async Task ObtenerTodos(HttpClient http)
        {
            var data = await http.GetFromJsonAsync<List<ContratoDTO>>(ENDPOINT);
            if (data != null) ListaContratos = data;
            else ListaContratos = new();
            NotificarEstado();
        }
        public async Task ObtenerTodoPorGerente(HttpClient http, string id)
        {
            var data = await http.PutAsJsonAsync($"{ENDPOINT}/gerente/", new Comando(id));
            if (data.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                var lista = JsonSerializer.Deserialize<List<ContratoDTO>>(await data.Content.ReadAsStringAsync(), options);
                ListaContratos = lista == null ? new() : lista;
            }
            else
            {
                ListaContratos = new();
            }
            NotificarEstado();
        }
        public async Task ObtenerTodoPorRepresentante(HttpClient http, string id)
        {
            Console.WriteLine($"{ENDPOINT}/representante");
            var data = await http.PutAsJsonAsync($"{ENDPOINT}/representante",new Comando(id));
            if (data.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                var lista = JsonSerializer.Deserialize<List<ContratoDTO>>(await data.Content.ReadAsStringAsync(), options);
                ListaContratos = lista == null ? new() : lista;
            }
            else
            {
                ListaContratos = new();
            }
            NotificarEstado();
        }

        public async Task Agregar(HttpClient Http, AppState appState, ContratoDTO contrato)
        {
            var data = await Http.PostAsJsonAsync(ENDPOINT, contrato);
            if (data.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                var nuevoContrato = JsonSerializer.Deserialize<ContratoDTO>(await data.Content.ReadAsStringAsync(), options);
                ListaContratos.Add(nuevoContrato!);
                NotificarEstado();
            }
            else
            {
                await appState.ErrorState.ActualizarMensaje(data);
            }
        }

        public async Task Actualizar(HttpClient Http, AppState appState, ContratoDTO contrato)
        {
            var data = await Http.PutAsJsonAsync(ENDPOINT, contrato);
            if (data.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                var nuevoContrato = JsonSerializer.Deserialize<ContratoDTO>(await data.Content.ReadAsStringAsync(), options);
                var indice = ListaContratos.FindIndex(x => x.Id == nuevoContrato?.Id);
                if (indice != -1) ListaContratos[indice] = nuevoContrato!;
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
                ListaContratos.Remove(ListaContratos.First(x => x.Id == id));
                NotificarEstado();
            }
            else
                await appState.ErrorState.ActualizarMensaje(data);
        }

        private void NotificarEstado()
        {
            this.ActualizarEstado?.Invoke();
        }
    }
}
