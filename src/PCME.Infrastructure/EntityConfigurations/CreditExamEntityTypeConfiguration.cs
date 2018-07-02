using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PCME.Domain.AggregatesModel.CreditExamAggregates;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCME.Infrastructure.EntityConfigurations
{
    public class CreditExamEntityTypeConfiguration : IEntityTypeConfiguration<CreditExam>
    {
        public void Configure(EntityTypeBuilder<CreditExam> builder)
        {
            //builder.HasIndex(c => c.AdmissionTicketNum).IsUnique();
        }
    }
}
