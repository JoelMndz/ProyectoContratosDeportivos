using Cliente.Shared;
using FluentValidation;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Cliente.Client.Shared.Componentes
{
    public partial class DialogFormEquipo
    {
        [CascadingParameter]
        MudDialogInstance? MudDialog { get; set; }
        [Parameter]
        public EquipoDTO model { get; set; } = new();
        [Parameter]
        public bool Editar { get; set; } = false;
        public MudForm? form;
        public Validacion validacion = new();
        public bool cargando;
        public string Titulo => Editar ? "Actualizar Equipo" : "Agregar Equipo";

        protected override async Task OnInitializedAsync()
        {
            appState.CategoriaState.ActualizarEstado += StateHasChanged;
            await appState.CategoriaState.ObtenerTodos(Http);
            var usuario = await authStateProvider.GetUsuario();
            if (usuario != null) model.IdGerente = usuario.Id;

        }
        public async Task ProcesarFormulario()
        {
            await form!.Validate();

            if (!form.IsValid) return;

            appState.ErrorState.RestablecerError();
            cargando = true;
            if(Editar)
                await appState.EquipoState.Actualizar(Http, appState, model);
            else
                await appState.EquipoState.Agregar(Http, appState, model);
            cargando = false;

            if (string.IsNullOrEmpty(appState.ErrorState.Mensaje))
            {
                form.Reset();
                model = new();
                Snackbar.Add(Editar ? "Equipo actualizado" : "Equipo agregado!", MudBlazor.Severity.Success);
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
        public class Validacion : AbstractValidator<EquipoDTO>
        {
            public Validacion()
            {
                RuleFor(x => x.Nombre)
                    .NotEmpty()
                    .Must(SoloLetras)
                    .WithMessage("Solo se aceptan letras")
                    .Length(4, 50);

                RuleFor(x => x.PresupuestoAnual)
                    .NotEmpty()
                    .Must(ValidarPresupuesto)
                    .WithMessage("El presupuesto no puede ser mayor a 150 Millones");

                RuleFor(x => x.IdGerente)
                    .NotEmpty();

                RuleFor(x => x.IdGerente)
                    .NotEmpty();

                RuleFor(x => x.IdCategoria)
                    .NotEmpty();
            }
            private bool ValidarPresupuesto(string cadena)
            {
                try
                {
                    int numero = int.Parse(cadena);
                    if (numero > 150000000) return false;
                }
                catch (Exception)
                {
                    return false;
                }
                return true;
            }
            private bool SoloLetras(string cadena)
            {
                for (int i = 0; i < cadena.Length; i++)
                {
                    if (cadena[i] != ' ' && !char.IsLetter(cadena[i]))
                    {
                        return false;
                    }
                }
                return true;
            }
            public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
            {
                var result = await ValidateAsync(ValidationContext<EquipoDTO>.CreateWithOptions((EquipoDTO)model, x => x.IncludeProperties(propertyName)));
                if (result.IsValid)
                    return Array.Empty<string>();
                return result.Errors.Select(e => e.ErrorMessage);
            };
        }
    }
}
