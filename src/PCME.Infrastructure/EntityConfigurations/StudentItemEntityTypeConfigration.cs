using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PCME.Domain.AggregatesModel.ApplicationForm;

namespace PCME.Infrastructure.EntityConfigurations
{
    public class StudentItemEntityTypeConfigration:IEntityTypeConfiguration<StudentItem>
    {

        public void Configure(EntityTypeBuilder<StudentItem> builder)
        {
            builder.HasOne(t => t.ApplyTable)
                .WithMany(t => t.StudentItems)
                .HasForeignKey(k => k.ApplyTableId);

            builder.HasOne(t=>t.Student)
                .WithMany()
                .HasForeignKey(t=>t.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(c => new { c.ApplyTableId, c.StudentId }).IsUnique();
        }
    }
}
