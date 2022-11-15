using Cliente.Shared;
using FluentValidation;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Cliente.Client.Shared.Componentes
{
    public partial class DialogFormContrato
    {
        [CascadingParameter]
        MudDialogInstance? MudDialog { get; set; }
        [Parameter]
        public ContratoDTO model { get; set; } = new();
        [Parameter]
        public bool Editar { get; set; } = false;
        public MudForm? form;
        public Validacion validacion = new();
        public bool cargando;
        public string Titulo => Editar ? "Actualizar Contrato" : "Agregar Contrato";


        protected override async Task OnInitializedAsync()
        {
            appState.EquipoState.ActualizarEstado += StateHasChanged;
            appState.JugadoreState.ActualizarEstado += StateHasChanged;
            var usuario = await authStateProvider.GetUsuario();
            if (usuario != null)
                await appState.EquipoState.ObtenerTodoPorGerente(Http, usuario.Id);
            await appState.JugadoreState.ObtenerTodos(Http);
        }

        public async Task ProcesarFormulario()
        {
            await form!.Validate();

            if (!form.IsValid) return;

            appState.ErrorState.RestablecerError();
            cargando = true;
            if(Editar)
                await appState.ContratosState.Actualizar(Http, appState, model);
            else
                await appState.ContratosState.Agregar(Http, appState, model);
            cargando = false;

            if (string.IsNullOrEmpty(appState.ErrorState.Mensaje))
            {
                form.Reset();
                model = new();
                Snackbar.Add(Editar ? "Contrato actualizado!" : "Contrato agregado!", MudBlazor.Severity.Success);
            }
            else
            {
                Snackbar.Add(appState.ErrorState.Mensaje, MudBlazor.Severity.Error);
            }

            MudDialog?.Close(DialogResult.Ok(true));
        }
        private void Cancelar()
        {
            MudDialog?.Cancel();
        }
        public void Dispose()
        {
            appState.EquipoState.ActualizarEstado += StateHasChanged;
            appState.JugadoreState.ActualizarEstado += StateHasChanged;
        }

        public class Validacion : AbstractValidator<ContratoDTO>
        {
            public Validacion()
            {
                RuleFor(x => x.FechaInicio)
                    .NotEmpty();

                RuleFor(x => x.FechaFin)
                    .NotEmpty();

                RuleFor(x => x.NumeroJugador)
                    .NotEmpty()
                    .Must(ValidarNumero)
                    .WithMessage("Ingres un numero válido")
                    .Length(1, 3);

                RuleFor(x => x.IdEquipo)
                    .NotEmpty();

                RuleFor(x => x.IdJugador)
                    .NotEmpty();

                RuleFor(x => x.Precio)
                    .NotEmpty()
                    .Must(ValidarNumero)
                    .WithMessage("El precio no es válido");

                RuleFor(x => x.Descripcion)
                    .NotEmpty()
                    .Length(4, 100);
            }

            private bool ValidarNumero(string cadena)
            {
                try
                {
                    int numero = int.Parse(cadena);
                    if (numero <= 0) return false;
                }
                catch (Exception)
                {
                    return false;
                }
                return true;
            }
            public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
            {
                var result = await ValidateAsync(ValidationContext<ContratoDTO>.CreateWithOptions((ContratoDTO)model, x => x.IncludeProperties(propertyName)));
                if (result.IsValid)
                    return Array.Empty<string>();
                return result.Errors.Select(e => e.ErrorMessage);
            };
        }
    }
}
