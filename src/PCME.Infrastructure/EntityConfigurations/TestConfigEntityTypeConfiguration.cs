using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PCME.Domain.AggregatesModel.TestAggregates;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCME.Infrastructure.EntityConfigurations
{
    public class TestConfigEntityTypeConfiguration : IEntityTypeConfiguration<TestConfig>
    {
        public void Configure(EntityTypeBuilder<TestConfig> builder)
        {
            //builder.HasMany(t => t.TestPaper)
            //    .WithOne(t => t.TestConfig)
            //    .HasForeignKey(t => t.TestConfigId);

            //builder.HasOne(t => t.ExamSubject)
            //    .WithMany()
            //    .HasForeignKey(t => t.ExamSubjectId)
            //    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
