using Data.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

using static System.Net.Mime.MediaTypeNames;

using System.Data;

namespace CAS.Services.Database;

internal class AppDBContext : DbContext
{
    private readonly IConfiguration _configuration;

    public AppDBContext(DbContextOptions<AppDBContext> options, IConfiguration configuration) : base(options)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        string? connectionString = _configuration.GetConnectionString("Development");
        if (connectionString != null)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }
        else
        {
            throw new Exception("Connectionstring doesn't work");
        }
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<JWTToken> jWTTokens { get; set; } = null!;

}
