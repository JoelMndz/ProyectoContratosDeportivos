using Aplicacion.Dominio.Entidades;
using Aplicacion.Persistencia;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Cliente.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Caracteristicas.Usuario
{
    public class ObtenerTodo
    {
        public record Consulta: IRequest<IReadOnlyCollection<UsuarioDTO>>;

        public class Handler : IRequestHandler<Consulta, IReadOnlyCollection<UsuarioDTO>>
        {
            private readonly Contexto context;
            private readonly IMapper mapper;

            public Handler(Contexto context, IMapper mapper)
            {
                this.context = context;
                this.mapper = mapper;
            }

            public async Task<IReadOnlyCollection<UsuarioDTO>> Handle(Consulta request, CancellationToken cancellationToken)
            {

                IReadOnlyCollection<UsuarioDTO> respuesta = await context.Usuarios
                    .ProjectTo<UsuarioDTO>(mapper.ConfigurationProvider)
                    .ToArrayAsync(cancellationToken);

                return respuesta;
            }
        }

        public class MapRespuesta : Profile
        {
            public MapRespuesta()
            {
                CreateMap<Usuarios, UsuarioDTO>();
            }
        }
    }
}
