using Humteria.Application.Profiles;
using Humteria.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Humteria.Application;

public static class Startup
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration) =>
        services.AddAutoMapper(typeof(AutoMapperProfile))
            .AddDatabaseServices(configuration);
    
    public static IServiceProvider ConfigureApplicationServices(this IServiceProvider provider) =>
        provider.ConfigureDatabaseServices();
}
