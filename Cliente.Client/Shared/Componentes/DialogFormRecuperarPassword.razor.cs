using Cliente.Shared;
using FluentValidation;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Net.Http.Json;
using System.Text.Json;

namespace Cliente.Client.Shared.Componentes
{
    public partial class DialogFormRecuperarPassword
    {
        [CascadingParameter]
        MudDialogInstance? MudDialog { get; set; }
        [Parameter]
        public RecuperarPasswordDTO model { get; set; } = new();
        public MudForm? form;
        public bool Enviando;
        private Validacion validator = new();

        public async Task ProcesarFormulario()
        {
            await form!.Validate();

            if (!form.IsValid) return;

            Enviando = true;
            var data = await Http.PutAsJsonAsync("/api/usuario/recovery", model);
            Enviando = false;
            if (data.IsSuccessStatusCode)
            {
                form.Reset();
                model = new();
                Snackbar.Add("Contraseña restablecida!", MudBlazor.Severity.Success);
                MudDialog?.Close(DialogResult.Ok(true));
                _navigationManager.NavigateTo("/login");
            }
            else
            {
                var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                var error = JsonSerializer.Deserialize<ErrorResponseDTO>(await data.Content.ReadAsStringAsync(), options);
                Snackbar.Add(error != null ? error.Error : "Error desconocido", MudBlazor.Severity.Error);
            }
        }
        private void Cancelar()
        {
            MudDialog?.Cancel();
        }
        public class Validacion : AbstractValidator<RecuperarPasswordDTO>
        {
            public Validacion()
            {
                RuleFor(x => x.Email)
                    .NotEmpty()
                    .EmailAddress()
                    .MaximumLength(50);
                RuleFor(x => x.Password)
                    .NotEmpty()
                    .Length(8, 30);
                RuleFor(x => x.Codigo).NotEmpty().Length(6);
            }
            public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
            {
                var result = await ValidateAsync(ValidationContext<RecuperarPasswordDTO>.CreateWithOptions((RecuperarPasswordDTO)model, x => x.IncludeProperties(propertyName)));
                if (result.IsValid)
                    return Array.Empty<string>();
                return result.Errors.Select(e => e.ErrorMessage);
            };
        }
    }
}
