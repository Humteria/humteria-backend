using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Humteria.Data.Context;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
        : base(options) { }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<JwtToken> JwtTokens { get; set; } = null!;

}
