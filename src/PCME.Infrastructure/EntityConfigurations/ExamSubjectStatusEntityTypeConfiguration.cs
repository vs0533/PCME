using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PCME.Domain.AggregatesModel.ExamSubjectAggregates;

namespace PCME.Infrastructure.EntityConfigurations
{
	public class ExamSubjectStatusEntityTypeConfiguration : IEntityTypeConfiguration<ExamSubjectStatus>
	{
		public void Configure(EntityTypeBuilder<ExamSubjectStatus> builder)
		{
			builder.ToTable("ExamSubjectStatus");

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
