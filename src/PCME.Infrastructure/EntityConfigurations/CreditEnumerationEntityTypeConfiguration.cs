using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PCME.Domain.AggregatesModel.PaperAggregates;
using PCME.Domain.AggregatesModel.ScientificPayoffsAggregates;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCME.Infrastructure.EntityConfigurations
{
    public class AreLevelTypeEntityTypeConfiguration : IEntityTypeConfiguration<AreaLevel>
    {
        public void Configure(EntityTypeBuilder<AreaLevel> builder)
        {
            builder.ToTable("AreaLevel");

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
    public class AwardPaperLevelEntityTypeConfiguration : IEntityTypeConfiguration<AwardPaperLevel>
    {
        public void Configure(EntityTypeBuilder<AwardPaperLevel> builder)
        {
            builder.ToTable("AwardPaperLevel");

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
    public class PublishTypeEntityTypeConfiguration : IEntityTypeConfiguration<PublishType>
    {
        public void Configure(EntityTypeBuilder<PublishType> builder)
        {
            builder.ToTable("PublishType");

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
    public class AwardSPLevelEntityTypeConfiguration : IEntityTypeConfiguration<AwardSPLevel>
    {
        public void Configure(EntityTypeBuilder<AwardSPLevel> builder)
        {
            builder.ToTable("AwardSPLevel");

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
