using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PCME.Domain.AggregatesModel.ProfessionalTitleAggregates;
using PCME.Domain.AggregatesModel.UnitAggregates;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCME.Infrastructure.EntityConfigurations
{
    public class ProfessionalTitleEntityTypeConfiguration : IEntityTypeConfiguration<ProfessionalTitle>
    {
        public void Configure(EntityTypeBuilder<ProfessionalTitle> builder)
        {
            builder.ToTable("ProfessionalTitle");

            builder.HasKey(o => o.Id);
            builder.Property(o => o.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.HasOne(o => o.Level)
                .WithMany();

            builder.HasOne(o => o.Series)
                .WithMany();

            builder.HasOne(o => o.Specialty)
                .WithMany();
                
        }
    }
}
