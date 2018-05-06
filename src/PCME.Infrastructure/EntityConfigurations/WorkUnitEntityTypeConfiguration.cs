using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PCME.Domain.AggregatesModel.UnitAggregates;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCME.Infrastructure.EntityConfigurations
{
    public class WorkUnitEntityTypeConfiguration : IEntityTypeConfiguration<WorkUnit>
    {
        public void Configure(EntityTypeBuilder<WorkUnit> builder)
        {
            builder.ToTable("WorkUnit");

            builder
                .HasOne(o => o.Parent)
                .WithMany(o => o.Childs)
                .HasForeignKey(d=>d.PID);
            

            builder.Ignore(o => o.DomainEvents);

            builder.Property(o => o.Name)
                .HasMaxLength(60)
                .IsRequired();

            //builder.HasIndex(o => o.Name).IsUnique();
            builder.HasIndex(o => o.Code).IsUnique();

            builder.Property(o => o.Code)
                .HasMaxLength(50)
                .IsRequired();
            
            

            builder.Property(o => o.Level)
                .IsRequired();

            builder.HasOne(o => o.WorkUnitNature)
                .WithMany()
                //.OnDelete(DeleteBehavior.Cascade)
                .HasForeignKey("WorkUnitNatureId");
                
        }
    }
}
