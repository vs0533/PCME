using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PCME.Domain.AggregatesModel.WorkUnitAccountAggregates;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCME.Infrastructure.EntityConfigurations
{
    public class WorkUnitAccountTypeEntityTypeConfiguration: IEntityTypeConfiguration<WorkUnitAccountType>
    {
        public void Configure(EntityTypeBuilder<WorkUnitAccountType> builder)
        {
            builder.ToTable("WorkUnitAccountType");

            builder.HasKey(o => o.Id);

            builder.Property(o => o.Id)
                .HasDefaultValue(1)
                .ValueGeneratedNever()
                .IsRequired();

            builder.Property(o => o.Name)
                .HasMaxLength(200)
                .IsRequired();
        }
    }
}
