using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PCME.Domain.AggregatesModel.AdmissionTicketAggregates;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCME.Infrastructure.EntityConfigurations
{
    public class AdmissionTicketCSEntityTypeConfiguration : IEntityTypeConfiguration<AdmissionTicketCS>
    {
        public void Configure(EntityTypeBuilder<AdmissionTicketCS> builder)
        {
            builder.HasIndex(t => t.Num).IsUnique();
            //builder.HasIndex(t => new { t.ExamSubjectId, t.StudentId }).IsUnique();
        }
    }
}
