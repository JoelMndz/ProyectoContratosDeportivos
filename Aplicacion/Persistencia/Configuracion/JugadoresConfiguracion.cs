using Aplicacion.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Persistencia.Configuracion
{
    public class JugadoresConfiguracion : IEntityTypeConfiguration<Jugadores>
    {
        public void Configure(EntityTypeBuilder<Jugadores> entity)
        {
            entity.Property(x => x.Nombres)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired(true);

            entity.Property(x => x.Apellidos)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired(true);

            entity.Property(x => x.Posicion)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired(true);

            entity.HasOne<Usuarios>(x => x.Representante)
                .WithMany(x => x.ListaJugadores)
                .HasForeignKey(x => x.IdRepresentante);
        }
    }
}
