using Aplicacion.Dominio.Entidades;
using Aplicacion.Persistencia;
using AutoMapper;
using Cliente.Shared;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Throw;

namespace Aplicacion.Caracteristicas.Usuario
{
    public class Insertar
    {
        public class Validator: AbstractValidator<InsertarComandoDTO>
        {
            public Validator()
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
                            posicionesImpares.Add((digitos[i] * 2) - 9);
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
        }
        public class Handler : IRequestHandler<InsertarComandoDTO, UsuarioDTO>
        {
            private readonly Contexto context;
            private readonly IMapper mapper;

            public Handler(Contexto context, IMapper mapper)
            {
                this.context = context;
                this.mapper = mapper;
            }

            public async Task<UsuarioDTO> Handle(InsertarComandoDTO request, CancellationToken cancellationToken)
            {
                await ValidarUsuario(request);

                var usuario = mapper.Map<Usuarios>(request);

                if (usuario.Rol == Roles.Administrador)
                {
                    var existe = context.Usuarios.Where(x => x.Rol == Roles.Administrador).FirstOrDefault();
                    if (existe != null)
                    {
                        throw new Exception("Solo se puede registrar un administrador!");
                    }
                }

                usuario.Email = usuario.Email.ToLower();
                usuario.Nombres = usuario.Nombres.ToLower();
                usuario.Apellidos = usuario.Apellidos.ToLower();

                await context.Usuarios.AddAsync(usuario, cancellationToken);
                var saveChanges = await context.SaveChangesAsync(cancellationToken);
                saveChanges.Throw("Huvo un problema para ingresar el usuario").IfEquals(0);
                
                return mapper.Map<UsuarioDTO>(usuario);
            }

            private async Task ValidarUsuario(InsertarComandoDTO request)
            {
                var existeEmail = await context.Usuarios.FirstOrDefaultAsync(x => x.Email == request.Email.ToLower());
                existeEmail?.Throw("El email ya está registrado").IfTrue(true);

                var existeCedula = await context.Usuarios.FirstOrDefaultAsync(x => x.Cedula == request.Cedula);
                existeCedula?.Throw("La cédula o ruc ya está registrado").IfTrue(true);

                var existeCelular = await context.Usuarios.FirstOrDefaultAsync(x => x.Celular == request.Celular);
                existeCedula?.Throw("La celular ya está registrado").IfTrue(true);

            }
        }

        public class MapRespuesta: Profile
        {
            public MapRespuesta()
            {
                CreateMap<InsertarComandoDTO, Usuarios>();
            }
        }
    }
}
