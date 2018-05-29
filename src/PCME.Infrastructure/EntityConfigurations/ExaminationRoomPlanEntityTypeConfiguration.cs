using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PCME.Domain.AggregatesModel.ExaminationRoomPlanAggregates;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCME.Infrastructure.EntityConfigurations
{
    public class ExaminationRoomPlanEntityTypeConfiguration : IEntityTypeConfiguration<ExaminationRoomPlan>
    {
        public void Configure(EntityTypeBuilder<ExaminationRoomPlan> builder)
        {
            builder.HasIndex(c => c.Num).IsUnique();
        }
    }
}
