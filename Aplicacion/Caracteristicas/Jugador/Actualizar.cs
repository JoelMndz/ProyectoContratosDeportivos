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
    public class Actualizar
    {
        public record Comando(
            string Id,
            string Nombres,
            string Apellidos,
            string Posicion,
            string IdRepresentante): IRequest<JugadorDTO>;

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
                var jugador = mapper.Map<Jugadores>(request);

                var jugadorActualizado = context.Jugadores.Include(x => x.Representante).FirstOrDefault(x => x.Id == jugador.Id);
                jugadorActualizado.ThrowIfNull("El id del jugador no existe!");
                
                var representante = context.Usuarios.FirstOrDefault(x => x.Id == jugador.IdRepresentante);
                representante.ThrowIfNull("No existe el id del representante");

                jugadorActualizado.Representante = representante;
                jugadorActualizado.Nombres = jugador.Nombres;
                jugadorActualizado.Apellidos = jugador.Apellidos;
                jugadorActualizado.Posicion = jugador.Posicion;

                context.Jugadores.Update(jugadorActualizado);

                var saveChanges = await context.SaveChangesAsync(cancellationToken);
                saveChanges.Throw("Huvo un problema para actualizar el jugador").IfEquals(0);

                return mapper.Map<JugadorDTO>(jugadorActualizado);
            }

        }
        public class MapRespuesta : Profile
        {
            public MapRespuesta()
            {
                CreateMap<Comando, Jugadores>();
                CreateMap<Jugadores, JugadorDTO>();
            }
        }
    }
}
