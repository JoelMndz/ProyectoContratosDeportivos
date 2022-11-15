using Aplicacion.Dominio.Entidades;
using Aplicacion.Persistencia;
using AutoMapper;
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
    public class Eliminar
    {
        public record Comando(int Id):IRequest<JugadorDTO>;

        public class Handler : IRequestHandler<Comando, JugadorDTO>
        {
            private readonly Contexto context;
            private readonly IMapper mapper;
            public Handler(Contexto context, IMapper mapper)
            {
                this.context = context;
                this.mapper = mapper;
            }
            public async Task<JugadorDTO> Handle(Comando request, CancellationToken cancellationToken)
            {
                var jugador = context.Jugadores.Include(x => x.ListaContratos).FirstOrDefault(x => x.Id == request.Id);
                jugador.ThrowIfNull("El id del jugador no existe!");
                jugador.ListaContratos.Count.Throw("No puede eliminar un jugador que tiene contratos!").IfTrue(x => x > 0);
                context.Jugadores.Remove(jugador);
                var saveChanges = await context.SaveChangesAsync(cancellationToken);
                saveChanges.Throw("Huvo un problema para eliminar el jugador").IfEquals(0);

                return mapper.Map<JugadorDTO>(jugador);
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
