using Cliente.Shared;
using FluentValidation;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Cliente.Client.Shared.Componentes
{
    public partial class DialogFormCategoria
    {
        [CascadingParameter]
        MudDialogInstance? MudDialog { get; set; }
        [Parameter]
        public CategoriaDTO model { get; set; } = new();
        [Parameter]
        public bool Editar { get; set; } = false;
        public MudForm? form;
        public Validacion validacion = new();
        public bool cargando;
        public string Titulo => Editar ? "Actualizar Categoría" : "Agregar Categoría";

        public async Task ProcesarFormulario()
        {
            await form!.Validate();

            if (!form.IsValid) return;

            appState.ErrorState.RestablecerError();
            cargando = true;
            if (Editar)
                await appState.CategoriaState.Actualizar(Http, appState, model);
            else
                await appState.CategoriaState.Agregar(Http, appState, model);
            cargando = false;

            if (string.IsNullOrEmpty(appState.ErrorState.Mensaje))
            {
                form.Reset();
                model = new();
                Snackbar.Add(Editar ? "Categoría actualizada" : "Categoría agregada!", MudBlazor.Severity.Success);
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
        public class Validacion : AbstractValidator<CategoriaDTO>
        {
            public Validacion()
            {
                RuleFor(x => x.Nombre)
                    .NotEmpty()
                    .Length(4, 50);

                RuleFor(x => x.Descripcion)
                    .NotEmpty()
                    .Length(4, 100);
            }
            public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
            {
                var result = await ValidateAsync(ValidationContext<CategoriaDTO>.CreateWithOptions((CategoriaDTO)model, x => x.IncludeProperties(propertyName)));
                if (result.IsValid)
                    return Array.Empty<string>();
                return result.Errors.Select(e => e.ErrorMessage);
            };
        }
    }
}
