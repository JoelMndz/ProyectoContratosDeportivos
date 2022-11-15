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
    public class Actualizar
    {
        public record Comando(
            string Id,
            string Nombre,
            string Descripcion):IRequest<CategoriaDTO>;
        public class Handler : IRequestHandler<Comando, CategoriaDTO>
        {
            private readonly Contexto context;
            private readonly IMapper mapper;

            public Handler(Contexto context, IMapper mapper)
            {
                this.context = context;
                this.mapper = mapper;
            }

            public async Task<CategoriaDTO> Handle(Comando request, CancellationToken cancellationToken)
            {
                var categoria = mapper.Map<Categorias>(request);

                var categoriaActualizada = context.Categorias.FirstOrDefault(x => x.Id == categoria.Id);
                categoriaActualizada.ThrowIfNull("El id de la categoría no existe!");

                categoriaActualizada.Nombre = categoria.Nombre;
                categoriaActualizada.Descripcion = categoria.Descripcion;

                context.Categorias.Update(categoriaActualizada);

                var saveChanges = await context.SaveChangesAsync(cancellationToken);
                saveChanges.Throw("Huvo un problema para actualizar el jugador").IfEquals(0);

                return mapper.Map<CategoriaDTO>(categoriaActualizada);
            }

        }
        public class MapRespuesta : Profile
        {
            public MapRespuesta()
            {
                CreateMap<Comando, Categorias>();
                CreateMap<CategoriaDTO, Categorias>().ReverseMap();
            }
        }
    }
}
