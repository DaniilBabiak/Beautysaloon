﻿using BeautySaloon.API.Entities.BeautySaloonContextEntities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace BeautySaloon.API.Entities.Contexts.ContextConfigurations.BeautySaloonContextConfigurations;

public class ServiceConfiguration : IEntityTypeConfiguration<Service>
{
    public void Configure(EntityTypeBuilder<Service> builder)
    {
    }
}
