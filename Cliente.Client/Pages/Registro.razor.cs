using Cliente.Shared;
using FluentValidation;
using MudBlazor;
using System.Net.Http.Json;
using System.Text.Json;

namespace Cliente.Client.Pages
{
    public partial class Registro
    {
        public MudForm form = new ();
        public ModelUsuario model = new();
        public ModelValidator validator = new ();
        public bool DatosCorrectos { get; set; }
        public bool Enviando { get; set; }
        public string Error { get; set; } = string.Empty;

        public async Task RegistrarUsuario()
        {
            await form.Validate();

            if (form.IsValid)
            {
                Enviando = true;
                var data = await Http.PostAsJsonAsync("/api/usuario", model.GetDTO());
                var content = await data.Content.ReadFromJsonAsync<JsonElement>();
                Enviando = false;
                if (data.IsSuccessStatusCode)
                {
                    form.Reset();
                    _navigationManager.NavigateTo("/login");
                }
                else
                {
                    try { Error = content.GetProperty("error").ToString(); }
                    catch (Exception) { Error = "Error desconocido!"; }
                }
                
            }
        }
        public class ModelUsuario
        {
            public string Nombres { get; set; } = string.Empty;
            public string Apellidos { get; set; } = string.Empty;
            public string Cedula { get; set; } = string.Empty;
            public string Celular { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
            public string Rol { get; set; } = string.Empty;


            public InsertarComandoDTO GetDTO()
            { 
                return new InsertarComandoDTO(
                    Nombres, 
                    Apellidos,
                    Cedula,
                    Email,
                    Password,
                    Celular,
                    Rol
                );
            }
        }

        public class ModelValidator: AbstractValidator<ModelUsuario>{
            public ModelValidator()
            {
                RuleFor(x => x.Nombres)
                    .NotEmpty()
                    .Must(SoloLetras)
                    .WithMessage("Solo se adminten letras")
                    .Must(x => x.Split(" ").Length == 2)
                    .WithMessage("Debe ingresar los dos nombres")
                    .Length(4, 50);

                RuleFor(x => x.Apellidos)
                    .NotEmpty()
                    .Must(SoloLetras)
                    .WithMessage("Solo se admiten letras")
                    .Must(x => x.Split(" ").Length == 2)
                    .WithMessage("Debe ingresar los dos apellidos")
                    .Length(4, 50);

                RuleFor(x => x.Celular)
                    .NotEmpty()
                    .Must(SoloNumeros)
                    .WithMessage("Solo se admiten números")
                    .Length(10);

                RuleFor(x => x.Cedula)
                    .NotEmpty()
                    .Must(ValidarCedula)
                    .WithMessage("La cédula no es válida");

                RuleFor(x => x.Email)
                    .NotEmpty()
                    .MaximumLength(50)
                    .EmailAddress();

                RuleFor(x => x.Password)
                    .NotEmpty()
                    .Length(8, 30);

                RuleFor(x => x.Rol)
                    .NotEmpty();
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

            private bool SoloNumeros(string cadena)
            {
                for (int i = 0; i < cadena.Length; i++)
                {
                    if (!char.IsNumber(cadena[i]))
                    {
                        return false;
                    }
                }
                return true;
            }

            private bool ValidarCedula(string cedula)
            {
                if (!SoloNumeros(cedula))
                {
                    return false;
                }

                if (cedula.Length != 10 && cedula.Length != 13)
                {
                    return false;
                }

                List<int> digitos = new();
                for (int i = 0; i < cedula.Length; i++)
                {
                    digitos.Add(int.Parse(cedula[i].ToString()));
                }
                List<int> posicionesImpares = new List<int>();
                List<int> posicionesPares = new List<int>();

                for (int i = 0; i < 9; i++)
                {
                    if (i % 2 == 0)
                    {
                        if ((digitos[i] * 2) > 9)
                        {
                            posicionesImpares.Add((digitos[i]*2)-9);
                        }
                        else
                        {
                            posicionesImpares.Add(digitos[i] * 2);
                        }

                    }
                    else
                    {
                        posicionesPares.Add(digitos[i]);
                    }
                }

                int suma = posicionesPares.Sum() + posicionesImpares.Sum();
                int modulo = suma % 10;
                int digitoVerificador = 0;
                if (modulo > 0)
                {
                    digitoVerificador = 10 - modulo;
                }

                if (digitos[9] != digitoVerificador)
                {
                    return false;
                }

                if (cedula.Length == 13)
                {
                    if (digitos[10] != 0 || digitos[11] != 0 || digitos[12] != 1)
                    {
                        return false;
                    }
                }

                return true;
            }

            public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
            {
                var result = await ValidateAsync(ValidationContext<ModelUsuario>.CreateWithOptions((ModelUsuario)model, x => x.IncludeProperties(propertyName)));
                if (result.IsValid)
                    return Array.Empty<string>();
                return result.Errors.Select(e => e.ErrorMessage);
            };
        }
    }
}
