using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PCME.Domain.AggregatesModel.UnitAggregates;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCME.Infrastructure.EntityConfigurations
{
    public class UnitEntityTypeConfiguration : IEntityTypeConfiguration<WorkUnit>
    {
        public void Configure(EntityTypeBuilder<WorkUnit> builder)
        {
            builder.ToTable("Unit");

            builder
                .HasOne(o => o.Parent)
                .WithMany(o => o.Childs);

            builder.Ignore(o => o.DomainEvents);

            builder.Property(o => o.Name)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(o => o.Code)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(o => o.PassWord)
                .IsRequired();

            builder.Property(o => o.Level)
                .IsRequired();

            builder.HasOne(o => o.UnitNature)
                .WithMany()
                //.OnDelete(DeleteBehavior.Cascade)
                .HasForeignKey("WorkUnitNatureId");
                
        }
    }
}
