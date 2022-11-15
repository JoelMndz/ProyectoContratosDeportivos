using Aplicacion.Dominio.Entidades;
using Aplicacion.Persistencia.Configuracion;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Aplicacion.Persistencia;

public partial class Contexto : DbContext
{
	public Contexto() { }
	public Contexto(DbContextOptions<Contexto> options)
		:base(options) { }

    
	public virtual DbSet<Categorias> Categorias { get; set; }
    public virtual DbSet<Contratos> Contratos { get; set; }
    public virtual DbSet<Equipos> Equipos { get; set; }
    public virtual DbSet<Jugadores> Jugadores { get; set; }
	public virtual DbSet<Usuarios> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CategoriasConfiguracion());
        modelBuilder.ApplyConfiguration(new ContratosConfiguracion());
        modelBuilder.ApplyConfiguration(new EquiposConfiguracion());
        modelBuilder.ApplyConfiguration(new JugadoresConfiguracion());
        modelBuilder.ApplyConfiguration(new UsuariosConfiguracion());

        OnModelCreatingPartial(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => base.OnConfiguring(optionsBuilder);

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);


}