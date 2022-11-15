using Cliente.Shared;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Cliente.Client.Shared.Componentes
{
    public partial class DialogEliminarContrato
    {
        [CascadingParameter]
        MudDialogInstance? MudDialog { get; set; }
        [Parameter]
        public ContratoDTO Contrato { get; set; } = new();
        private bool cargando;

        private void Cancelar()
        {
            MudDialog?.Cancel();
        }

        private async Task Ok()
        {
            appState.ErrorState.RestablecerError();
            cargando = true;
            await appState.ContratosState.Eliminar(Http, appState, Contrato.Id!);
            cargando = false;
            if (string.IsNullOrEmpty(appState.ErrorState.Mensaje))
                Snackbar.Add("Contrato eliminado!", Severity.Success);
            else
                Snackbar.Add(appState.ErrorState.Mensaje, Severity.Error);
            MudDialog?.Close(DialogResult.Ok(true));
        }
    }
}
