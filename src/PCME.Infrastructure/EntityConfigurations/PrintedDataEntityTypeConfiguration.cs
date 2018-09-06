using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PCME.Domain.AggregatesModel.CertificateAggregates;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCME.Infrastructure.EntityConfigurations
{
    public class PrintedDataEntityTypeConfiguration : IEntityTypeConfiguration<PrintedData>
    {
        public void Configure(EntityTypeBuilder<PrintedData> builder)
        {
            builder.HasIndex(t => t.Num).IsUnique();
        }
    }
}
