using Aplicacion.Dominio.Entidades;
using Aplicacion.Persistencia;
using AutoMapper;
using Cliente.Shared;
using Cliente.Shared.Jugador;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Throw;

namespace Aplicacion.Caracteristicas.Categoria
{
    public class Eliminar
    {
        public record Comando(int Id):IRequest<CategoriaDTO>;
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
                var categoria = context.Categorias.Include(x => x.ListaEquipos).FirstOrDefault(x => x.Id == request.Id);
                categoria.ThrowIfNull("El id del jugador no existe!");
                categoria.ListaEquipos.Count.Throw("No puede eliminar una categoría que tiene equipos!").IfTrue(x => x > 0);
                context.Categorias.Remove(categoria);
                var saveChanges = await context.SaveChangesAsync(cancellationToken);
                saveChanges.Throw("Huvo un problema para eliminar la categoria").IfEquals(0);

                return mapper.Map<CategoriaDTO>(categoria);
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
