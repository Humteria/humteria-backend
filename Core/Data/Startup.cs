using Humteria.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Humteria.Data;

public static class Startup
{
    public static IServiceCollection AddDatabaseServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options => 
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        return services;
    }

    public static IServiceProvider ConfigureDatabaseServices(this IServiceProvider provider)
    {
        // Migrate and create database
        {
            using IServiceScope serviceScope = provider.GetRequiredService<IServiceScopeFactory>().CreateScope();
        
            // Gets the context
            ApplicationDbContext context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            // Execute Migrations
            context.Database.Migrate();
        }

        return provider;
    }
}
