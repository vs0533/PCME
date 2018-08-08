using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PCME.Domain.AggregatesModel.TestAggregates;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCME.Infrastructure.EntityConfigurations
{
    public class TestLibraryEntityTypeConfiguration : IEntityTypeConfiguration<TestLibrary>
    {
        public void Configure(EntityTypeBuilder<TestLibrary> builder)
        {
            //builder.HasOne(t => t.ExamSubject)
            //    .WithMany()
            //    .HasForeignKey(t => t.ExamSubjectId)
            //    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
