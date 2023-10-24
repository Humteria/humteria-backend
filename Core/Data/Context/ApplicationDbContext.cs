using Humteria.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Humteria.Data.Context;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
        : base(options) { }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<JwtToken> JwtTokens { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasOne(u => u.JwtToken)
            .WithOne()
            .HasForeignKey<JwtToken>(j => j.Id);
    }

}
