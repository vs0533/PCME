using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PCME.Domain.AggregatesModel.ExamOpenInfoAggregates;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCME.Infrastructure.EntityConfigurations
{
    public class ExamSubjectOpenInfoEntityTypeConfiguration : IEntityTypeConfiguration<ExamSubjectOpenInfo>
    {
        public void Configure(EntityTypeBuilder<ExamSubjectOpenInfo> builder)
        {
            builder.HasOne(t => t.TrainingCenter)
                .WithMany()
                .HasForeignKey(t => t.TrainingCenterId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(t => t.Pirce)
                .HasDefaultValue(0);
        }
    }
}
