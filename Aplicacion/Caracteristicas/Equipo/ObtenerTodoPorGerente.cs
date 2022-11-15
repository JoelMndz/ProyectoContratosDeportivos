using Aplicacion.Dominio.Entidades;
using Aplicacion.Persistencia;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Cliente.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Aplicacion.Caracteristicas.Equipo
{
    public class ObtenerTodoPorGerente
    {
        public record Comando(int IdGerente) : IRequest<IReadOnlyCollection<EquipoDTO>>;
        public class Handler : IRequestHandler<Comando, IReadOnlyCollection<EquipoDTO>>
        {
            private readonly Contexto context;
            private readonly IMapper mapper;

            public Handler(Contexto context, IMapper mapper)
            {
                this.context = context;
                this.mapper = mapper;
            }

            public async Task<IReadOnlyCollection<EquipoDTO>> Handle(Comando request, CancellationToken cancellationToken)
            {
                IReadOnlyCollection<EquipoDTO> respuesta = await context.Equipos
                    .Include(x => x.Gerente)
                    .Include(x => x.Categoria)
                    .Where(x => x.IdGerente == request.IdGerente)
                    .ProjectTo<EquipoDTO>(mapper.ConfigurationProvider)
                    .ToArrayAsync(cancellationToken);
                return respuesta;
            }

        }

        public class MapRespuesta : Profile
        {
            public MapRespuesta()
            {
                CreateMap<Equipos, EquipoDTO>();
            }
        }
    }
}
