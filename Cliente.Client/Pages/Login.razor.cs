using Cliente.Client.Auth;
using Cliente.Shared;
using FluentValidation;
using MudBlazor;
using System.Net.Http.Json;
using System.Text.Json;

namespace Cliente.Client.Pages
{
    public partial class Login
    {
        public MudForm? form;
        private Model model = new();
        private Validator validator = new();
        private bool DatosCorrectos { get; set; }
        private bool Enviando { get; set; }
        private string Error { get; set; } = string.Empty;

        private async Task LoginUsuario()
        {
            await form!.Validate();

            if (form.IsValid)
            {
                Enviando = true;
                var data = await Http.PostAsJsonAsync("/api/usuario/login", model.GetDTO());
                var content = await data.Content.ReadFromJsonAsync<JsonElement>();
                Enviando = false;
                Console.WriteLine(content);
                if (data.IsSuccessStatusCode)
                {
                    form.Reset();
                    await authStateProvider.Login(content.GetProperty("token").ToString());
                    var usuario = await authStateProvider.GetUsuario();
                    appState.UsuarioState.ActualizarUsuario(usuario);   
                    _navigationManager.NavigateTo("/");
                }
                else
                {
                    try { Error = content.GetProperty("error").ToString(); }
                    catch (Exception) { Error = "Error desconocido!"; }
                }

            }
        }

        private class Model
        {
            public string Email { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;

            public LoginDTO GetDTO()
            {
                return new LoginDTO(
                    Email,
                    Password
                );
            }
        }
        private class Validator:AbstractValidator<Model>
        {
            public Validator()
            {
                RuleFor(x => x.Email)
                    .NotEmpty()
                    .EmailAddress()
                    .MaximumLength(50);

                RuleFor(x => x.Password)
                    .NotEmpty()
                    .MinimumLength(8);
            }
            public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
            {
                var result = await ValidateAsync(ValidationContext<Model>.CreateWithOptions((Model)model, x => x.IncludeProperties(propertyName)));
                if (result.IsValid)
                    return Array.Empty<string>();
                return result.Errors.Select(e => e.ErrorMessage);
            };
        }
    }
}
