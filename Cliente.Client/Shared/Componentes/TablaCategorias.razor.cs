using Cliente.Shared;
using Cliente.Shared.Jugador;
using MudBlazor;

namespace Cliente.Client.Shared.Componentes
{
    public partial class TablaCategorias
    {
        public bool loading = false;
        protected override async Task OnInitializedAsync()
        {
            appState.CategoriaState.ActualizarEstado += StateHasChanged;
            loading = true;
            await appState.CategoriaState.ObtenerTodos(Http);
            loading = false;
        }

        public void Confirmar(CategoriaDTO categoria)
        {
            DialogOptions dialogOptions = new() { FullWidth = true };
            var parametros = new DialogParameters { ["Categoria"] = categoria };
            DialogService.Show<DialogEliminarCategoria>("Eliminar categoria", parametros, dialogOptions);
        }
        public void Agregar()
        {
            DialogOptions dialogOptions = new() { FullWidth = true };
            var parametros = new DialogParameters {};
            DialogService.Show<DialogFormCategoria>("Categoría", parametros, dialogOptions);
        }
        public void Editar(CategoriaDTO categoria)
        {
            DialogOptions dialogOptions = new() { FullWidth = true };
            var parametros = new DialogParameters { ["model"] = categoria, ["Editar"] = true};
            DialogService.Show<DialogFormCategoria>("Categoría", parametros, dialogOptions);
        }
        public void Dispose()
        {
            appState.CategoriaState.ActualizarEstado -= StateHasChanged;
        }
    }
}
