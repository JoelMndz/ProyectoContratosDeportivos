using Cliente.Client.Shared.Componentes;
using Cliente.Shared;
using FluentValidation;
using MudBlazor;
using System.Net.Http.Json;
using System.Reflection;
using System.Text.Json;

namespace Cliente.Client.Pages
{
    public partial class RecuperarPassword
    {
        public MudForm? form;
        public EnviarCodigoDTO model { get; set; } = new();
        private Validacion validator = new();
        public bool Enviando;

        public async Task EnviarCodigo()
        {
            await form!.Validate();

            if (!form.IsValid) return;
            Enviando= true;
            var data = await Http.PutAsJsonAsync("/api/usuario/send", model);
            Enviando= false;
            if (data.IsSuccessStatusCode)
            {
                Snackbar.Add("Código enviado!", MudBlazor.Severity.Success);
                DialogOptions dialogOptions = new() { FullWidth = true };
                var parametros = new DialogParameters { ["model"] = new RecuperarPasswordDTO { Email = model.Email } };
                DialogService.Show<DialogFormRecuperarPassword>("Recovery", parametros, dialogOptions);
            }
            else
            {
                var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                var error = JsonSerializer.Deserialize<ErrorResponseDTO>(await data.Content.ReadAsStringAsync(), options);
                Snackbar.Add(error != null ? error.Error : "Error desconocido", MudBlazor.Severity.Error);
            }
        }

        public class Validacion : AbstractValidator<EnviarCodigoDTO>
        {
            public Validacion()
            {
                RuleFor(x => x.Email)
                    .NotEmpty()
                    .EmailAddress()
                    .MaximumLength(50);
            }
            public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
            {
                var result = await ValidateAsync(ValidationContext<EnviarCodigoDTO>.CreateWithOptions((EnviarCodigoDTO)model, x => x.IncludeProperties(propertyName)));
                if (result.IsValid)
                    return Array.Empty<string>();
                return result.Errors.Select(e => e.ErrorMessage);
            };
        }
    }
}
