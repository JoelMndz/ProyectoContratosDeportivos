using Cliente.Shared;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Cliente.Client.Shared.Componentes
{
    public partial class TablaContratos
    {
        [Parameter]
        public string Rol { get; set; } = string.Empty;
        public bool loading = false;
        protected override async Task OnInitializedAsync()
        {
            appState.ContratosState.ActualizarEstado += StateHasChanged;
            loading = true;
            if (Rol == "Administrador")
            {
                await appState.ContratosState.ObtenerTodos(Http);
            }
            else
            {
                var usuario = await authStateProvider.GetUsuario();
                var id = usuario!.Id;
                Console.WriteLine($"ID -> {id}");
                //Gerente
                if (Rol == "Gerente")
                {
                    Console.WriteLine("Gerente");
                    await appState.ContratosState.ObtenerTodoPorGerente(Http,id);
                }
                //Representante
                else if (Rol == "Representante")
                {
                    Console.WriteLine("Representante");
                    await appState.ContratosState.ObtenerTodoPorRepresentante(Http, id);
                }
            }
            loading = false;
        }
        public string FormatearFecha(string fecha)
        {
            return fecha.Split(" ")[0];
        }
        public void Confirmar(ContratoDTO contrato)
        {
            DialogOptions dialogOptions = new() { FullWidth = true };
            var parametros = new DialogParameters { ["Contrato"] = contrato };
            DialogService.Show<DialogEliminarContrato>("Eliminar contrato", parametros, dialogOptions);
        }
        public void Agregar()
        {
            DialogOptions dialogOptions = new() { FullWidth = true };
            var parametros = new DialogParameters { };
            DialogService.Show<DialogFormContrato>("Contrato", parametros, dialogOptions);
        }
        public void Editar(ContratoDTO equipo)
        {
            DialogOptions dialogOptions = new() { FullWidth = true };
            var parametros = new DialogParameters { ["model"] = equipo, ["Editar"] = true };
            DialogService.Show<DialogFormContrato>("Contrato", parametros, dialogOptions);
        }

        public void Dispose()
        {
            appState.ContratosState.ActualizarEstado -= StateHasChanged;
        }
    }
}
