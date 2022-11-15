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
    public class Actualizar
    {
        public record Comando(
            string Id,
            string Nombre,
            string PresupuestoAnual,
            string FechaRegistro,
            string IdCategoria,
            string IdGerente) :IRequest<EquipoDTO>;
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
                var equipo = mapper.Map<Equipos>(request);

                var equipoActualizado = context.Equipos.FirstOrDefault(x => x.Id == equipo.Id);
                equipoActualizado.ThrowIfNull("El id del equipo no existe!");

                var gerente = context.Usuarios.FirstOrDefault(x => x.Id == equipo.IdGerente);
                gerente.ThrowIfNull("No existe el id del representante");

                var categoria = context.Categorias.FirstOrDefault(x => x.Id == equipo.IdCategoria);
                categoria.ThrowIfNull("El id de la categoría no existe!");

                equipoActualizado.Gerente = gerente;
                equipoActualizado.Categoria = categoria;
                equipoActualizado.Nombre = equipo.Nombre;
                equipoActualizado.PresupuestoAnual = equipo.PresupuestoAnual;
                equipoActualizado.FechaRegistro = equipo.FechaRegistro;

                context.Equipos.Update(equipoActualizado);

                var saveChanges = await context.SaveChangesAsync(cancellationToken);
                saveChanges.Throw("Huvo un problema para actualizar el equipo").IfEquals(0);

                return mapper.Map<EquipoDTO>(equipoActualizado);
            }

        }
        public class MapRespuesta : Profile
        {
            public MapRespuesta()
            {
                CreateMap<Comando, Equipos>();
                CreateMap<Equipos, EquipoDTO>();
            }
        }
    }
}
