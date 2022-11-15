using Cliente.Shared.Jugador;
using Cliente.Shared;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Cliente.Client.Shared.Componentes
{
    public partial class DialogFormJugador
    {
        [CascadingParameter]
        MudDialogInstance? MudDialog { get; set; }
        [Parameter]
        public bool EsAdmin { get; set; } = false;
        [Parameter]
        public bool Editar { get; set; } = false;
        [Parameter]
        public JugadorDTO model { get; set; } = new();
        public MudForm? form;
        public bool enviando = false;
        public List<UsuarioDTO> ListaRepresentantes = new();
        public ValidacionJugador validacion = new();
        public List<string> ListaPosiciones = new() { "Portero", "Defensa", "Defensa lateral", "Carrilero", "Centrocampista defensivo", "Centro campista interior","Media punta","Mediocampista externo","Delantero extremo","Segundo delantero","Delantero centro" };
        public string Titulo => Editar ? "Actualizar Jugador" : "Agregar Jugador";
        protected override async Task OnInitializedAsync()
        {
            if (!EsAdmin)
            {
                var usuario = await authStateProvider.GetUsuario();
                if(usuario != null) model.IdRepresentante = usuario.Id;
            }
            ListaRepresentantes = await appState.UsuarioState.ObtenerRepresentantes(Http);
        }

        public async Task Procesar()
        {
            await form!.Validate();

            if (!form.IsValid) return;

            appState.ErrorState.RestablecerError();
            enviando = true;
            if (Editar)
                await appState.JugadoreState.Actualizar(Http, appState, model);
            else
                await appState.JugadoreState.Agregar(Http, appState, model);

            enviando = false;

            if (string.IsNullOrEmpty(appState.ErrorState.Mensaje))
            {
                form.Reset();
                model = new();
                Snackbar.Add(Editar ? "Jugador actualizado!":"Jugador agregado!", Severity.Success);
            }
            else
            {
                Snackbar.Add(appState.ErrorState.Mensaje, Severity.Error);
            }
            MudDialog?.Close(DialogResult.Ok(true));
        }
        private void Cancelar()
        {
            MudDialog?.Cancel();
        }

    }
}