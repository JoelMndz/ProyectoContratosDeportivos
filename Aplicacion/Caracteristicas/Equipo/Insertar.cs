using Aplicacion.Dominio.Entidades;
using Aplicacion.Persistencia;
using AutoMapper;
using Cliente.Shared;
using MediatR;
using Throw;

namespace Aplicacion.Caracteristicas.Equipo
{
    public class Insertar
    {
        public class Handler : IRequestHandler<InsertarEquipoDTO, EquipoDTO>
        {
            private readonly Contexto context;
            private readonly IMapper mapper;

            public Handler(Contexto context, IMapper mapper)
            {
                this.context = context;
                this.mapper = mapper;
            }

            public async Task<EquipoDTO> Handle(InsertarEquipoDTO request, CancellationToken cancellationToken)
            {
                var equipo = mapper.Map<Equipos>(request);

                equipo.Gerente = context.Usuarios.FirstOrDefault(x => x.Id == equipo.IdGerente).ThrowIfNull("El Id del Gerente no existe");
                equipo.Categoria = context.Categorias.FirstOrDefault(x => x.Id == equipo.IdCategoria).ThrowIfNull("El Id de la categoria no existe");

                await context.Equipos.AddAsync(equipo, cancellationToken);
                var saveChanges = await context.SaveChangesAsync(cancellationToken);
                saveChanges.Throw("Huvo un problema para ingresar el equipo").IfEquals(0);

                return mapper.Map<EquipoDTO>(equipo);
            }

        }

        public class MapRespuesta : Profile
        {
            public MapRespuesta()
            {
                CreateMap<Equipos, EquipoDTO>();
                CreateMap<InsertarEquipoDTO, Equipos>();
            }
        }
    }
}
