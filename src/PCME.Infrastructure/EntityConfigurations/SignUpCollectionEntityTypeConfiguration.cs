using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PCME.Domain.AggregatesModel.SignUpAggregates;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCME.Infrastructure.EntityConfigurations
{
    public class SignUpCollectionEntityTypeConfiguration : IEntityTypeConfiguration<SignUpCollection>
    {
        public void Configure(EntityTypeBuilder<SignUpCollection> builder)
        {
            builder.HasOne(t => t.SignUpForUnit)
                .WithMany(t=>t.SignUpCollection)
                .HasForeignKey(t => t.SignUpForUnitId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(t => new { t.ExamSubjectId, t.StudentId }).IsUnique();
            
            //builder.HasKey(c => new { c.ExamSubjectId, c.StudentId });
            //builder.Property(t => t.ExamSubjectId).ValueGeneratedNever();
            //builder.Property(t => t.StudentId).ValueGeneratedNever();
        }
    }
}
