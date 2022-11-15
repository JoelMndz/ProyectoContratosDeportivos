using Aplicacion.Dominio.Entidades;
using Aplicacion.Persistencia;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Cliente.Shared.Jugador;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Throw;

namespace Aplicacion.Caracteristicas.Jugador
{
    public class ObtenerTodoPorRepresentante
    {
        public record Comando(int IdRepresentante):IRequest<IReadOnlyCollection<JugadorDTO>>;

        public class Handler : IRequestHandler<Comando, IReadOnlyCollection<JugadorDTO>>
        {
            private readonly Contexto context;
            private readonly IMapper mapper;

            public Handler(Contexto context, IMapper mapper)
            {
                this.context = context;
                this.mapper = mapper;
            }

            public async Task<IReadOnlyCollection<JugadorDTO>> Handle(Comando request,
            CancellationToken cancellationToken)
            {
                IReadOnlyCollection<JugadorDTO> respuesta = await context.Jugadores
                    .Include(x => x.Representante)
                    .Where(x => x.IdRepresentante == request.IdRepresentante)
                    .ProjectTo<JugadorDTO>(mapper.ConfigurationProvider)
                    .ToArrayAsync(cancellationToken);

                return respuesta;
            }
        }
        public class MapRespuesta : Profile
        {
            public MapRespuesta()
            {
                CreateMap<Jugadores, JugadorDTO>();
            }
        }
    }
}
