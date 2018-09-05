using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PCME.Domain.AggregatesModel.CertificateAggregates;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCME.Infrastructure.EntityConfigurations
{
    public class CertificateCategoryEntityTypeConfiguration : IEntityTypeConfiguration<CertificateCategory>
    {
        public void Configure(EntityTypeBuilder<CertificateCategory> builder)
        {
            builder.ToTable("CertificateCategory");

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
