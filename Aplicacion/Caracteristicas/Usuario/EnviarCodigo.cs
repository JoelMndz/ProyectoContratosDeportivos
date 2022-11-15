using Aplicacion.Persistencia;
using Cliente.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Throw;

namespace Aplicacion.Caracteristicas.Usuario
{
    public class EnviarCodigo
    {
        public class Handler:IRequestHandler<EnviarCodigoDTO, int>
        {
            private readonly Contexto context;
            private string emailOrigen = "asistente.recovery@gmail.com";
            private string password = "pofsqiuhtdiosvjx";

            public Handler(Contexto context)
            {
                this.context = context;
            }

            public async Task<int> Handle(EnviarCodigoDTO request, CancellationToken cancellationToken)
            {
                var usuario = context.Usuarios.Where(x => x.Email == request.Email).FirstOrDefault();
                usuario.ThrowIfNull("El email no existe!");

                usuario.CodigoRecuperacion = GenerarCodigo();
                SendEmail(request.Email, usuario.CodigoRecuperacion);

                context.Usuarios.Update(usuario);
                var saveChanges = await context.SaveChangesAsync(cancellationToken);
                saveChanges.Throw("Huvo un problema en el servidor").IfEquals(0);
                return 1;
            }
            private void SendEmail(string emailDestino, string codigo)
            {
                MailMessage mailMessage = new (emailOrigen, emailDestino, "Código de recuperación", $"<p><strong>{codigo}</strong> es tu código para recuperar la cuenta</p>");
                mailMessage.IsBodyHtml = true;
                var smtpClient = new SmtpClient
                {
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    Host = "smtp.gmail.com",
                    Port = 587,
                    Credentials = new NetworkCredential { UserName = emailOrigen, Password = password }
                };
                smtpClient.Send(mailMessage);
            }
            private string GenerarCodigo()
            {
                string codigo = string.Empty;
                Random random = new();
                for (int i = 0; i < 6; i++)
                {
                    codigo += random.Next(1, 9);
                }
                return codigo;
            }
        }
    }
}
