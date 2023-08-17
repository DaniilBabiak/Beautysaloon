using BeautySaloon.API.Entities.Contexts.ContextConfigurations.BeautySaloonContextConfigurations;
using Microsoft.EntityFrameworkCore;

namespace BeautySaloon.API.Entities.Contexts.ContextConfigurations;

public static class BeautySaloonContextConfiguration
{
    public static void ApplyBeautySaloonEntitiesConfigurations(this ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CommentConfiguration());
        modelBuilder.ApplyConfiguration(new ServiceCategoryConfiguration());
        modelBuilder.ApplyConfiguration(new ServiceConfiguration());
        modelBuilder.ApplyConfiguration(new MasterConfiguration());
    }
}
