using BeautySaloon.API.Entities.BeautySaloonContextEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeautySaloon.API.Entities.Contexts.ContextConfigurations.BeautySaloonContextConfigurations;

public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
{
    public void Configure(EntityTypeBuilder<Reservation> builder)
    {

    }
}
