﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PCME.Domain.AggregatesModel.StudentAggregates;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCME.Infrastructure.EntityConfigurations
{
    public class StudentStatusEntityTypeConfiguration : IEntityTypeConfiguration<StudentStatus>
    {
        public void Configure(EntityTypeBuilder<StudentStatus> builder)
        {
            builder.ToTable("StudentStatus");

            builder.HasKey(o => o.Id);

            builder.Property(o => o.Id)
                .HasDefaultValue(1)
                .ValueGeneratedNever()
                .IsRequired();

            builder.Property(o => o.Name)
                .HasMaxLength(200)
                .IsRequired();
        }
    }
}
