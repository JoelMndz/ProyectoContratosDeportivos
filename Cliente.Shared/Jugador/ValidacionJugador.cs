using FluentValidation;
using System.Reflection;

namespace Cliente.Shared.Jugador
{
    public class ValidacionJugador: AbstractValidator<JugadorDTO>
    {
        public ValidacionJugador()
        {
            RuleFor(x => x.Nombres)
                .NotEmpty()
                .Length(4,50)
                .Must(SoloLetras)
                .WithMessage("Solo se adminten letras")
                .Must(x => x.Split(" ").Length == 2 || x.Split(" ").Length == 1)
                .WithMessage("Debe ingresar los uno ó dos nombres")
                .Length(4, 50);

            RuleFor(x => x.Apellidos)
                .NotEmpty()
                .Must(SoloLetras)
                .WithMessage("Solo se adminten letras")
                .Must(x => x.Split(" ").Length == 2)
                .WithMessage("Debe ingresar los dos apellidos")
                .Length(4, 50);

            RuleFor(x => x.Posicion)
                .NotEmpty()
                .Must(SoloLetras)
                .WithMessage("Solo se adminten letras")
                .Length(4, 50);

            RuleFor(x => x.IdRepresentante)
                .NotEmpty()
                .Must(SoloNumeros)
                .WithMessage("El id debe ser numérico");

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

        public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
        {
            var result = await ValidateAsync(ValidationContext<JugadorDTO>.CreateWithOptions((JugadorDTO)model, x => x.IncludeProperties(propertyName)));
            if (result.IsValid)
                return Array.Empty<string>();
            return result.Errors.Select(e => e.ErrorMessage);
        };
    }
}
