using BeautySaloon.API.Entities.BeautySaloonContextEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeautySaloon.API.Entities.Contexts.ContextConfigurations.BeautySaloonContextConfigurations;

public class ScheduleConfiguration : IEntityTypeConfiguration<Schedule>
{
    public void Configure(EntityTypeBuilder<Schedule> builder)
    {
        builder.HasMany(s => s.WorkingDays)
               .WithOne(wd => wd.Schedule)
               .HasForeignKey(w => w.ScheduleId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
