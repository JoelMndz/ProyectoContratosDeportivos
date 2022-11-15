using Aplicacion.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Aplicacion.Persistencia.Configuracion
{
    public class EquiposConfiguracion : IEntityTypeConfiguration<Equipos>
    {
        public void Configure(EntityTypeBuilder<Equipos> entity)
        {
            entity.Property(x => x.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired(true);

            entity.HasOne<Usuarios>(x => x.Gerente)
                .WithMany(x => x.ListaEquipos)
                .HasForeignKey(x => x.IdGerente);

            entity.HasOne<Categorias>(x => x.Categoria)
                .WithMany(x => x.ListaEquipos)
                .HasForeignKey(x => x.IdCategoria);
        }
    }
}
