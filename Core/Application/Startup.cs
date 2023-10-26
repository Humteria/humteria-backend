global using Humteria.Data.Models.Bases;
global using System.ComponentModel.DataAnnotations;
global using AutoMapper;
using Humteria.Application.Profiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Humteria.Application;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration) =>
        Configuration = configuration;

    public static void ConfigureServices(IServiceCollection services) => services.AddAutoMapper(typeof(AutoMapperProfile));
}
