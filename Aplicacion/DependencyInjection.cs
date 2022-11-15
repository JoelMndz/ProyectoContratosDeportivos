
using Aplicacion.Helper.Comportamientos;
using Aplicacion.Persistencia;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Aplicacion;

public static class DependencyInjection
{
    public static IServiceCollection AddAplicacion(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddLogging();


        services.AddDbContext<Contexto>(options =>
        {
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                (a) => a.MigrationsAssembly("Cliente.Server"));
        },
        ServiceLifetime.Transient);

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(Validacion<,>));

        return services;
    }
}