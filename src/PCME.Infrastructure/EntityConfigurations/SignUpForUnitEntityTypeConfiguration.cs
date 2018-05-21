using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PCME.Domain.AggregatesModel.SignUpAggregates;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCME.Infrastructure.EntityConfigurations
{
    public class SignUpForUnitEntityTypeConfiguration : IEntityTypeConfiguration<SignUpForUnit>
    {
        public void Configure(EntityTypeBuilder<SignUpForUnit> builder)
        {
            builder.HasIndex(t => t.Code).IsUnique();
        }
    }
}
