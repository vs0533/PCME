using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PCME.Domain.AggregatesModel.ExamSubjectAggregates;

namespace PCME.Infrastructure.EntityConfigurations
{
	public class OpenTypeEntityTypeConfiguration : IEntityTypeConfiguration<OpenType>
	{
		public void Configure(EntityTypeBuilder<OpenType> builder)
		{
			builder.ToTable("OpenType");

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
