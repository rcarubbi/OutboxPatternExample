using DomainLayer.Entities;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace InfrastructureLayer.Database;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
         : base(options)
    {

    }
    
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>();
        modelBuilder.AddTransactionalOutboxEntities();
    }
}
