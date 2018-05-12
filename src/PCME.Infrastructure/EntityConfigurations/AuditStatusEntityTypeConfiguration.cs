using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PCME.Domain.AggregatesModel.AuditStatusAggregates;

namespace PCME.Infrastructure.EntityConfigurations
{
	public class AuditStatusEntityTypeConfiguration : IEntityTypeConfiguration<AuditStatus>
	{
		public void Configure(EntityTypeBuilder<AuditStatus> builder)
		{
			builder.ToTable("AuditStatus");

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
