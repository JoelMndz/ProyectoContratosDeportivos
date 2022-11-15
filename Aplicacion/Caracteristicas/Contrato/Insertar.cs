using Aplicacion.Dominio.Entidades;
using Aplicacion.Persistencia;
using AutoMapper;
using Cliente.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Throw;

namespace Aplicacion.Caracteristicas.Contrato
{
    public class Insertar
    {
        public record Comando(
            string Precio, 
            string FechaInicio,
            string FechaFin, 
            string Descripcion,
            string IdEquipo,
            string NumeroJugador,
            string IdJugador):IRequest<ContratoDTO>;

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

                contrato.Equipo = context.Equipos.FirstOrDefault(x => x.Id == contrato.IdEquipo).ThrowIfNull("El Id del Equipo no existe");
                contrato.Jugador = context.Jugadores.FirstOrDefault(x => x.Id == contrato.IdJugador).ThrowIfNull("El Id del jugador no existe");

                var filtro = context.Contratos
                    .Where(x => x.IdEquipo == contrato.IdEquipo)
                    .Where(x => x.NumeroJugador == contrato.NumeroJugador)
                    .FirstOrDefault();

                if (filtro != null) throw new Exception("El número ya está ocupado por otro jugador del equipo!");

                await context.Contratos.AddAsync(contrato, cancellationToken);
                var saveChanges = await context.SaveChangesAsync(cancellationToken);
                saveChanges.Throw("Huvo un problema para ingresar el contrato").IfEquals(0);

                return mapper.Map<ContratoDTO>(contrato);
            }

        }
        class MapRespuesta: Profile
        {
            public MapRespuesta()
            {
                CreateMap<Comando, Contratos>();
                CreateMap<Contratos, ContratoDTO>();
            }
        }
    }
}
