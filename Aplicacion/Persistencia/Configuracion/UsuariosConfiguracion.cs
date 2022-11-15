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
    public class UsuariosConfiguracion : IEntityTypeConfiguration<Usuarios>
    {
        public void Configure(EntityTypeBuilder<Usuarios> entity)
        {
            entity.Property(x => x.Nombres)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired();

            entity.Property(x => x.Apellidos)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired();

            entity.Property(x => x.Cedula)
                .HasMaxLength(13)
                .IsUnicode(false)
                .IsRequired(true);

            entity.Property(x => x.Celular)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsRequired();

            entity.Property(x => x.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired(true);

            entity.Property(x => x.Password)
                .IsUnicode(false)
                .IsRequired(true);

            entity.Property(x => x.Rol)
                .IsUnicode(false)
                .IsRequired(true);


            entity.HasIndex(x => x.Email)
                .IsUnique();

            entity.HasIndex(x => x.Cedula)
                .IsUnique();

            entity.HasIndex(x => x.Celular)
                .IsUnique();
        }
    }
}
