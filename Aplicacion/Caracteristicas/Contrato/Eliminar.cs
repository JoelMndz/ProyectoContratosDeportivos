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
    public class Eliminar
    {
        public record Comando(int Id) : IRequest<ContratoDTO>;
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
                var contrato = context.Contratos.FirstOrDefault(x => x.Id == request.Id);
                contrato.ThrowIfNull("El id del contrato no existe!");

                context.Contratos.Remove(contrato);
                var saveChanges = await context.SaveChangesAsync(cancellationToken);
                saveChanges.Throw("Huvo un problema para eliminar el contrato").IfEquals(0);

                return mapper.Map<ContratoDTO>(contrato);
            }
        }
        public class MapRespuesta : Profile
        {
            public MapRespuesta()
            {
                CreateMap<Contratos, ContratoDTO>();
            }
        }
    }
}
