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

namespace Aplicacion.Caracteristicas.Contrato
{
    public class ObtenerTodo
    {
        public record Comando:IRequest<IReadOnlyCollection<ContratoDTO>>;
        public class Handler : IRequestHandler<Comando, IReadOnlyCollection<ContratoDTO>>
        {
            private readonly Contexto context;
            private readonly IMapper mapper;

            public Handler(Contexto context, IMapper mapper)
            {
                this.context = context;
                this.mapper = mapper;
            }

            public async Task<IReadOnlyCollection<ContratoDTO>> Handle(Comando request, CancellationToken cancellationToken)
            {
                IReadOnlyCollection<ContratoDTO> respuesta = await context.Contratos
                    .Include(x => x.Jugador)
                    .Include(x => x.Equipo)
                    .ProjectTo<ContratoDTO>(mapper.ConfigurationProvider)
                    .ToArrayAsync(cancellationToken);

                return respuesta;
            }

        }

        public class MapRespuesta : Profile
        {
            public MapRespuesta()
            {
                CreateMap<Contratos, ContratoDTO>();
            }
        }
    }
}
