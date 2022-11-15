using Aplicacion.Dominio.Entidades;
using Aplicacion.Persistencia;
using AutoMapper;
using Cliente.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Throw;

namespace Aplicacion.Caracteristicas.Usuario
{
    public class ObtenerUsuarioPorToken
    {
        public class Handler : IRequestHandler<TokenDTO, UsuarioDTO>
        {
            private readonly Contexto context;
            private readonly IMapper mapper;

            public Handler(Contexto context, IMapper mapper)
            {
                this.context = context;
                this.mapper = mapper;
            }
            public async Task<UsuarioDTO> Handle(TokenDTO request, CancellationToken cancellationToken)
            {
                request.Token.Throw("Debe envía el token").IfEmpty();
                var identity = new ClaimsIdentity();
                identity = new ClaimsIdentity(ParseClaimsFromJwt(request.Token), "jwt");
                var id = identity.Claims.FirstOrDefault(x => x.Type == "id")?.Value;
                id.ThrowIfNull("Token incorrecto!");

                var usuario = context.Usuarios.FirstOrDefault(x => x.Id == int.Parse(id));
                usuario.ThrowIfNull("Token incorrecto!");
                return mapper.Map<UsuarioDTO>(usuario);
            }

            private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
            {
                var claims = new List<Claim>();
                var payload = jwt.Split('.')[1];
                var jsonBytes = ParseBase64WithoutPadding(payload);
                var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
                return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));
            }
            private byte[] ParseBase64WithoutPadding(string base64)
            {
                switch (base64.Length % 4)
                {
                    case 2: base64 += "=="; break;
                    case 3: base64 += "="; break;
                }
                return Convert.FromBase64String(base64);
            }
        }
        class MapRespuesta : Profile
        {
            public MapRespuesta()
            {
                CreateMap<Usuarios,UsuarioDTO>();
            }
        }
    }
}
