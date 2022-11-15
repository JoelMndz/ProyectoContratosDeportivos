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
    public class ContratosConfiguracion : IEntityTypeConfiguration<Contratos>
    {
        public void Configure(EntityTypeBuilder<Contratos> entity)
        {
            entity.Property(x => x.NumeroJugador)
                .IsRequired(true);

            entity.Property(x => x.IdEquipo)
                .IsRequired(true);

            entity.Property(x => x.IdJugador)
                .IsRequired(true);
            
            entity.HasOne<Equipos>(x => x.Equipo)
                .WithMany(x => x.ListaContratos)
                .HasForeignKey(x => x.IdEquipo)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne<Jugadores>(x => x.Jugador)
                .WithMany(x => x.ListaContratos)
                .HasForeignKey(x => x.IdJugador)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
