using Cliente.Shared;
using Cliente.Shared.Jugador;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Cliente.Client.Shared.Componentes
{
    public partial class DialogEliminarEquipo
    {
        [CascadingParameter]
        MudDialogInstance? MudDialog { get; set; }
        [Parameter]
        public EquipoDTO Equipo { get; set; } = new();
        private bool cargando;

        private void Cancelar()
        {
            MudDialog?.Cancel();
        }

        private async Task Ok()
        {
            appState.ErrorState.RestablecerError();
            cargando = true;
            await appState.EquipoState.Eliminar(Http, appState, Equipo.Id!);
            cargando = false;
            if (string.IsNullOrEmpty(appState.ErrorState.Mensaje))
                Snackbar.Add("Equipo eliminado!", Severity.Success);
            else
                Snackbar.Add(appState.ErrorState.Mensaje, Severity.Error);
            MudDialog?.Close(DialogResult.Ok(true));
        }
    }
}
