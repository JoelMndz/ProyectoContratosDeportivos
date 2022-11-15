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
    public class CategoriasConfiguracion : IEntityTypeConfiguration<Categorias>
    {
        public void Configure(EntityTypeBuilder<Categorias> entity)
        {
            entity.Property(x => x.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired(true);

            entity.Property(x => x.Descripcion)
                .HasMaxLength(100)
                .IsUnicode(false)
                .IsRequired(true);
        }
    }
}
