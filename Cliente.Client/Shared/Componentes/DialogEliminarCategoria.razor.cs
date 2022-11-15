using Cliente.Shared;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Cliente.Client.Shared.Componentes
{
    public partial class DialogEliminarCategoria
    {
        [CascadingParameter]
        MudDialogInstance? MudDialog { get; set; }
        [Parameter]
        public CategoriaDTO Categoria { get; set; } = new();
        private bool cargando;

        private void Cancelar()
        {
            MudDialog?.Cancel();
        }

        private async Task Ok()
        {
            appState.ErrorState.RestablecerError();
            cargando = true;
            await appState.CategoriaState.Eliminar(Http, appState, Categoria.Id!);
            cargando = false;
            if (string.IsNullOrEmpty(appState.ErrorState.Mensaje))
                Snackbar.Add("Categoría eliminado!", Severity.Success);
            else
                Snackbar.Add(appState.ErrorState.Mensaje, Severity.Error);
            MudDialog?.Close(DialogResult.Ok(true));
        }
    }
}
