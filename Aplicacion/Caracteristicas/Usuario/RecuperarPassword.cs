using Aplicacion.Persistencia;
using Cliente.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Throw;

namespace Aplicacion.Caracteristicas.Usuario
{
    public class RecuperarPassword
    {
        public class Handler : IRequestHandler<RecuperarPasswordDTO, int>
        {
            private readonly Contexto context;
            public Handler(Contexto context)
            {
                this.context = context;
            }
            public async Task<int> Handle(RecuperarPasswordDTO request, CancellationToken cancellationToken)
            {
                var usuario = context.Usuarios
                    .Where(x => x.Email == request.Email && x.CodigoRecuperacion == request.Codigo)
                    .FirstOrDefault();
                usuario.ThrowIfNull("Código incorrecto!");

                usuario.CodigoRecuperacion = null;
                usuario.Password = request.Password;

                context.Usuarios.Update(usuario);
                var saveChanges = await context.SaveChangesAsync(cancellationToken);
                saveChanges.Throw("Huvo un problema en el servidor").IfEquals(0);

                return 1;
            }
        }
    }
}
