using Aplicacion.Dominio.Entidades;
using Aplicacion.Persistencia;
using AutoMapper;
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

namespace Aplicacion.Caracteristicas.Jugador
{
    public class Insertar
    {
        public class Validacion : ValidacionJugador { }
        public class Handler : IRequestHandler<JugadorDTO, JugadorDTO>
        {
            private readonly Contexto context;
            private readonly IMapper mapper;

            public Handler(Contexto context, IMapper mapper)
            {
                this.context = context;
                this.mapper = mapper;
            }

            public async Task<JugadorDTO> Handle(JugadorDTO request, CancellationToken cancellationToken)
            {
                if (!string.IsNullOrEmpty(request.Id)) request.Id = null;

                var jugador = mapper.Map<Jugadores>(request);

                var representante = context.Usuarios.FirstOrDefault(x => x.Id == jugador.IdRepresentante);
                representante.ThrowIfNull("No existe el id del representante");

                jugador.Representante = representante;
                await context.Jugadores.AddAsync(jugador, cancellationToken);
                var saveChanges = await context.SaveChangesAsync(cancellationToken);
                saveChanges.Throw("Huvo un problema para ingresar el jugador").IfEquals(0);

                return mapper.Map<JugadorDTO>(jugador);
            }

        }

        public class MapRespuesta : Profile
        {
            public MapRespuesta()
            {
                CreateMap<JugadorDTO, Jugadores>();
                CreateMap<Jugadores, JugadorDTO>();
            }
        }
    }
}
