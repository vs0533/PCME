using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PCME.Domain.AggregatesModel.SignUpStudentAggregates;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCME.Infrastructure.EntityConfigurations
{
    public class SignUpStudentEntityTypeConfiguration : IEntityTypeConfiguration<SignUpStudent>
    {
        public void Configure(EntityTypeBuilder<SignUpStudent> builder)
        {
            builder.HasIndex(i => i.Code).IsUnique();
            builder.HasMany(o => o.Collection)
                .WithOne(o => o.SignUpStudent)
                .HasForeignKey(o => o.SignUpStudentId);
        }
    }
}
