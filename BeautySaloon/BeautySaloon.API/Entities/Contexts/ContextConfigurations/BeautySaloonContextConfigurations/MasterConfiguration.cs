using BeautySaloon.API.Entities.BeautySaloonContextEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeautySaloon.API.Entities.Contexts.ContextConfigurations.BeautySaloonContextConfigurations;

public class MasterConfiguration : IEntityTypeConfiguration<Master>
{
    public void Configure(EntityTypeBuilder<Master> builder)
    {
        builder.HasMany(m => m.Services)
               .WithMany(s => s.Masters);

        builder.HasOne(m => m.Schedule)
               .WithOne(s => s.Master)
               .HasForeignKey<Schedule>(s => s.MasterId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
