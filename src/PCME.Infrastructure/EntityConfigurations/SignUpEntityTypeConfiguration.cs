using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PCME.Domain.AggregatesModel.SignUpAggregates;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PCME.Infrastructure.EntityConfigurations
{
    public class SignUpEntityTypeConfiguration : IEntityTypeConfiguration<SignUp>
    {
        public void Configure(EntityTypeBuilder<SignUp> builder)
        {
            builder.HasKey(c => new { c.StudentId, c.ExamSubjectId });
            builder.Property(t => t.StudentId).ValueGeneratedNever();
            builder.Property(t => t.ExamSubjectId).ValueGeneratedNever();
        }
    }
}