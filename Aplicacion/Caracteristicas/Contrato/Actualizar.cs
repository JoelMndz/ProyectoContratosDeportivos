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

namespace Aplicacion.Caracteristicas.Contrato
{
    public class Actualizar
    {
        public record Comando(
            string Id,
            string Precio,
            string FechaInicio,
            string FechaFin,
            string Descripcion,
            string NumeroJugador,
            string IdEquipo,
            string IdJugador) :IRequest<ContratoDTO>;

        public class Handler : IRequestHandler<Comando, ContratoDTO>
        {
            private readonly Contexto context;
            private readonly IMapper mapper;

            public Handler(Contexto context, IMapper mapper)
            {
                this.context = context;
                this.mapper = mapper;
            }

            public async Task<ContratoDTO> Handle(Comando request, CancellationToken cancellationToken)
            {
                var contrato = mapper.Map<Contratos>(request);

                var contratoActualizado = context.Contratos
                    .FirstOrDefault(x => x.Id == contrato.Id);
                contratoActualizado.ThrowIfNull("El id del jugador no existe!");

                contratoActualizado.Equipo = context.Equipos.FirstOrDefault(x => x.Id == contrato.IdEquipo).ThrowIfNull("El id no pertecene a ningún equipo");
                contratoActualizado.Jugador = context.Jugadores.FirstOrDefault(x => x.Id == contrato.IdJugador).ThrowIfNull("El id no pertecene a ningún jugador");
                contratoActualizado.Precio = contrato.Precio;
                contratoActualizado.FechaInicio = contrato.FechaInicio;
                contratoActualizado.FechaFin = contrato.FechaFin;
                contratoActualizado.NumeroJugador = contrato.NumeroJugador;
                contratoActualizado.Descripcion = contrato.Descripcion;

                context.Contratos.Update(contratoActualizado);

                var saveChanges = await context.SaveChangesAsync(cancellationToken);
                saveChanges.Throw("Huvo un problema para actualizar el jugador").IfEquals(0);

                return mapper.Map<ContratoDTO>(contratoActualizado);
            }

        }
        class MapRespuesta : Profile
        {
            public MapRespuesta()
            {
                CreateMap<Comando, Contratos>();
                CreateMap<Contratos, ContratoDTO>();
            }
        }
    }
}
