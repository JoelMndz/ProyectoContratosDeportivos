using Cliente.Shared.Jugador;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Cliente.Client.Shared.Componentes
{
    public partial class DialogEliminarJugador
    {
        [CascadingParameter]
        MudDialogInstance? MudDialog { get; set; }
        [Parameter]
        public JugadorDTO Jugador { get; set; } = new ();
        private bool cargando;

        private void Cancelar()
        {
            MudDialog?.Cancel();
        }

        private async Task Ok()
        {
            cargando = true;
            await appState.JugadoreState.Eliminar(Http, appState, Jugador.Id);
            cargando = false;
            if (string.IsNullOrEmpty(appState.ErrorState.Mensaje))
                Snackbar.Add("Jugador eliminado!", Severity.Success);
            else
                Snackbar.Add(appState.ErrorState.Mensaje, Severity.Error);
            MudDialog?.Close(DialogResult.Ok(true));
        }
    }
}
