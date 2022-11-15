using Cliente.Shared;
using Cliente.Shared.Jugador;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Cliente.Client.Shared.Componentes
{
    public partial class TablaEquipos
    {
        [Parameter]
        public string Rol { get; set; } = string.Empty;
        public bool loading = false;
        protected override async Task OnInitializedAsync()
        {
            appState.EquipoState.ActualizarEstado += StateHasChanged;
            loading = true;
            if (Rol == "Gerente")
            {
                var usuario = await authStateProvider.GetUsuario();
                if (usuario != null)
                {
                    Console.WriteLine("** CONSULTABNDO EQUIPOS POR GERENTE**");
                    var idGerente = usuario.Id;
                    await appState.EquipoState.ObtenerTodoPorGerente(Http, idGerente);
                }
            }
            else
            {
                await appState.EquipoState.ObtenerTodos(Http);
            }
            loading = false;
        }
        public void Agregar()
        {
            DialogOptions dialogOptions = new() { FullWidth = true };
            var parametros = new DialogParameters { };
            DialogService.Show<DialogFormEquipo>("Equipo", parametros, dialogOptions);
        }
        public void Editar(EquipoDTO equipo)
        {
            DialogOptions dialogOptions = new() { FullWidth = true };
            var parametros = new DialogParameters { ["model"] = equipo, ["Editar"] = true};
            DialogService.Show<DialogFormEquipo>("Equipo", parametros, dialogOptions);
        }

        public void Confirmar(EquipoDTO equipo)
        {
            DialogOptions dialogOptions = new() { FullWidth = true };
            var parametros = new DialogParameters { ["Equipo"] = equipo };
            DialogService.Show<DialogEliminarEquipo>("Eliminar equipo", parametros, dialogOptions);
        }

        public void Dispose()
        {
            appState.EquipoState.ActualizarEstado -= StateHasChanged;
        }
    }
}
