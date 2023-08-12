using BeautySaloon.API.Entities.BeautySaloonContextEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeautySaloon.API.Entities.Contexts.ContextConfigurations.BeautySaloonContextConfigurations;

public class ServiceCategoryConfiguration : IEntityTypeConfiguration<ServiceCategory>
{
    public void Configure(EntityTypeBuilder<ServiceCategory> builder)
    {
        builder
            .HasMany(category => category.Services)
            .WithOne(service => service.Category)
            .HasForeignKey(category => category.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
