using Cliente.Shared;
using System.Net.NetworkInformation;
using System.Text.Json;

namespace Cliente.Client.Store
{
    public class ErrorState
    {
        public string Mensaje { get; set; } = string.Empty;
        public event Action? ActualizarEstado;

        public async Task ActualizarMensaje(HttpResponseMessage response)
        {
            var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
            var error = JsonSerializer.Deserialize<ErrorResponseDTO>(await response.Content.ReadAsStringAsync(), options);
            this.Mensaje = error != null ? error.Error : "Error desconocido";
            Notificar();
        }
        public void RestablecerError()
        {
            Mensaje = string.Empty;
            Notificar();
        }
        private void Notificar()
        {
            ActualizarEstado?.Invoke();
        }
    }
}
