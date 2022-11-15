using Aplicacion.Dominio.Entidades;
using Aplicacion.Persistencia;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Cliente.Shared;
using Cliente.Shared.Jugador;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Throw;

namespace Aplicacion.Caracteristicas.Categoria
{
    public class ObtenerTodo
    {
        public record Consulta:IRequest<IReadOnlyCollection<CategoriaDTO>>;
        public class Handler : IRequestHandler<Consulta, IReadOnlyCollection<CategoriaDTO>>
        {
            private readonly Contexto context;
            private readonly IMapper mapper;

            public Handler(Contexto context, IMapper mapper)
            {
                this.context = context;
                this.mapper = mapper;
            }

            public async Task<IReadOnlyCollection<CategoriaDTO>> Handle(Consulta request, CancellationToken cancellationToken)
            {
                IReadOnlyCollection<CategoriaDTO> respuesta = await context.Categorias
                    .ProjectTo<CategoriaDTO>(mapper.ConfigurationProvider)
                    .ToArrayAsync(cancellationToken);

                return respuesta;
            }

        }


        public class MapRespuesta : Profile
        {
            public MapRespuesta()
            {
                CreateMap<Categorias, CategoriaDTO>();
            }
        }
    }
}
