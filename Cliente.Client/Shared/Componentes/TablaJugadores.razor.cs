using Cliente.Client.Store;
using Cliente.Shared;
using Cliente.Shared.Jugador;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Net.Http.Json;

namespace Cliente.Client.Shared.Componentes
{
    public partial class TablaJugadores
    {
        [Parameter]
        public string Rol { get; set; } = string.Empty;
        public bool loading = false;
        protected override async Task OnInitializedAsync()
        {
            appState.JugadoreState.ActualizarEstado += StateHasChanged;
            loading = true;
            if (Rol != "Representante")
                await appState.JugadoreState.ObtenerTodos(Http);
            else
            {
                var usuario = await authStateProvider.GetUsuario();
                if(usuario != null)
                {
                    var idRepresentante = usuario.Id;
                    await appState.JugadoreState.ObtenerTodosPorRepresentante(Http, idRepresentante);
                }
            }
            loading = false;
        }
        public void Confirmar(JugadorDTO jugador)
        {
            DialogOptions dialogOptions = new() { FullWidth = true };
            var parametros = new DialogParameters { ["Jugador"] = jugador };
            DialogService.Show<DialogEliminarJugador>("Eliminar jugador", parametros, dialogOptions);
        }
        public void Agregar()
        {
            DialogOptions dialogOptions = new() { FullWidth = true };
            var parametros = new DialogParameters{ ["EsAdmin"] = Rol == "Administrador" };
            DialogService.Show<DialogFormJugador>("Jugador", parametros, dialogOptions);
        }
        public void Editar(JugadorDTO jugador)
        {
            DialogOptions dialogOptions = new() { FullWidth = true };
            var parametros = new DialogParameters { ["model"] = jugador , ["Editar"] = true, ["EsAdmin"] = Rol == "Administrador" };
            DialogService.Show<DialogFormJugador>("Jugador", parametros, dialogOptions);
        }
        public void Dispose()
        {
            appState.JugadoreState.ActualizarEstado -= StateHasChanged;
        }
    }
}
