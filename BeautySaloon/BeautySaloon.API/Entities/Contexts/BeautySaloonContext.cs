using BeautySaloon.API.Entities.BeautySaloonContextEntities;
using BeautySaloon.API.Entities.Contexts.ContextConfigurations;
using Microsoft.EntityFrameworkCore;

namespace BeautySaloon.API.Entities.Contexts;

public class BeautySaloonContext : DbContext
{
    public BeautySaloonContext(DbContextOptions<BeautySaloonContext> options) : base(options)
    {
    }

    public DbSet<Comment> Comments { get; set; }
    public DbSet<ServiceCategory> ServiceCategories { get; set; }
    public DbSet<Service> Services { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyBeautySaloonEntitiesConfigurations();

        base.OnModelCreating(modelBuilder);
    }
}