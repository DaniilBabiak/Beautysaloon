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
    public DbSet<BestWork> BestWorks { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Master> Masters { get; set; }
    public DbSet<Schedule> Schedules { get; set; }
    public DbSet<WorkingDay> WorkingDays { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyBeautySaloonEntitiesConfigurations();

        base.OnModelCreating(modelBuilder);
    }
}