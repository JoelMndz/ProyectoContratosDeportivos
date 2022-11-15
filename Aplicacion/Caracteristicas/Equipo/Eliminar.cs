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

namespace Aplicacion.Caracteristicas.Equipo
{
    public class Eliminar
    {
        public record Comando(int Id):IRequest<EquipoDTO>;

        public class Handler : IRequestHandler<Comando, EquipoDTO>
        {
            private readonly Contexto context;
            private readonly IMapper mapper;
            public Handler(Contexto context, IMapper mapper)
            {
                this.context = context;
                this.mapper = mapper;
            }
            public async Task<EquipoDTO> Handle(Comando request, CancellationToken cancellationToken)
            {
                var equipo = context.Equipos.Include(x => x.ListaContratos).FirstOrDefault(x => x.Id == request.Id);
                equipo.ThrowIfNull("El id del jugador no existe!");
                equipo.ListaContratos.Count.Throw("No puede eliminar un equipo que tiene contratos!").IfTrue(x => x > 0);
                context.Equipos.Remove(equipo);
                var saveChanges = await context.SaveChangesAsync(cancellationToken);
                saveChanges.Throw("Huvo un problema para eliminar el jugador").IfEquals(0);

                return mapper.Map<EquipoDTO>(equipo);
            }
        }
        public class MapRespuesta : Profile
        {
            public MapRespuesta()
            {
                CreateMap<Equipos, EquipoDTO>();
            }
        }
    }
}
