using Aplicacion.Dominio.Entidades;
using Aplicacion.Persistencia;
using AutoMapper;
using Cliente.Shared;
using Cliente.Shared.Jugador;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Throw;

namespace Aplicacion.Caracteristicas.Categoria
{
    public class Insertar
    {
        public class Handler : IRequestHandler<InsertarCategoriaDTO, CategoriaDTO>
        {
            private readonly Contexto context;
            private readonly IMapper mapper;

            public Handler(Contexto context, IMapper mapper)
            {
                this.context = context;
                this.mapper = mapper;
            }

            public async Task<CategoriaDTO> Handle(InsertarCategoriaDTO request, CancellationToken cancellationToken)
            {
                var categoria = mapper.Map<Categorias>(request);

                await context.Categorias.AddAsync(categoria, cancellationToken);
                var saveChanges = await context.SaveChangesAsync(cancellationToken);
                saveChanges.Throw("Huvo un problema para ingresar la categoria").IfEquals(0);

                return mapper.Map<CategoriaDTO>(categoria);
            }

        }

        public class MapRespuesta : Profile
        {
            public MapRespuesta()
            {
                CreateMap<Categorias, CategoriaDTO>();
                CreateMap<InsertarCategoriaDTO, Categorias>();
            }
        }
    }
}
