using Aplicacion.Dominio.Entidades;
using Aplicacion.Persistencia;
using AutoMapper;
using Cliente.Shared;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Throw;

namespace Aplicacion.Caracteristicas.Usuario
{
    public class Login
    {
        public class Validator: AbstractValidator<LoginDTO>
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
        }

        public class Handler:IRequestHandler<LoginDTO, TokenDTO>
        {
            private readonly Contexto context;

            public Handler(Contexto context)
            {
                this.context = context;
            }

            public async Task<TokenDTO> Handle(LoginDTO request, CancellationToken cancellationToken)
            {

                var usuario = await context.Usuarios.FirstOrDefaultAsync(
                    x => x.Email.Equals(request.Email.ToLower()) && x.Password.Equals(request.Password)
                );

                usuario.ThrowIfNull("Las credencias son incorrectas!");



                return new TokenDTO(GenerarToken(usuario));
            }

            private string GenerarToken(Usuarios usuario)
            {

                var claims = new[]
                {
                    new Claim("id", usuario.Id.ToString()),
                    new Claim(ClaimTypes.Role, usuario.Rol.ToString())
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("jkhjrtretikklimnbmnFDGFKHJKLJKLVCVBCXZDSADHJGJJKJNMBMN"));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var expiration = DateTime.UtcNow.AddMonths(6);

                JwtSecurityToken token = new JwtSecurityToken(
                   issuer: null,
                   audience: null,
                   claims: claims,
                   expires: expiration,
                   signingCredentials: creds
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
        }
    }
}
