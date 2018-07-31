using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PCME.Domain.AggregatesModel.ExaminationRoomAccountAggregates;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCME.Infrastructure.EntityConfigurations
{
    public class ExaminationRoomAccountEntityTypeConfiguration : IEntityTypeConfiguration<ExaminationRoomAccount>
    {
        public void Configure(EntityTypeBuilder<ExaminationRoomAccount> builder)
        {
            builder.HasIndex(c => new { c.AccountName }).IsUnique();
        }
    }
}
