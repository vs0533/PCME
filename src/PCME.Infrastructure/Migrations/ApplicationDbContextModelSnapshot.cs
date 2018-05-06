﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using PCME.Infrastructure;
using System;

namespace PCME.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.2-rtm-10011")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("PCME.Domain.AggregatesModel.ProfessionalTitleAggregates.Level", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Levels");
                });

            modelBuilder.Entity("PCME.Domain.AggregatesModel.ProfessionalTitleAggregates.ProfessionalTitle", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("LevelId");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<int?>("SeriesId");

                    b.Property<int?>("SpecialtyId");

                    b.HasKey("Id");

                    b.HasIndex("LevelId");

                    b.HasIndex("SeriesId");

                    b.HasIndex("SpecialtyId");

                    b.ToTable("ProfessionalTitle");
                });

            modelBuilder.Entity("PCME.Domain.AggregatesModel.ProfessionalTitleAggregates.Series", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Seriess");
                });

            modelBuilder.Entity("PCME.Domain.AggregatesModel.ProfessionalTitleAggregates.Specialty", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Specialtys");
                });

            modelBuilder.Entity("PCME.Domain.AggregatesModel.StudentAggregates.Sex", b =>
                {
                    b.Property<int>("Id")
                        .HasDefaultValue(1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.HasKey("Id");

                    b.ToTable("Sex");
                });

            modelBuilder.Entity("PCME.Domain.AggregatesModel.StudentAggregates.Student", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address");

                    b.Property<decimal>("Balance");

                    b.Property<DateTime?>("BirthDay");

                    b.Property<string>("Email");

                    b.Property<bool>("EmailIsValid");

                    b.Property<string>("GraduationSchool");

                    b.Property<string>("IDCard");

                    b.Property<string>("Name");

                    b.Property<string>("OfficeName");

                    b.Property<string>("Password");

                    b.Property<string>("Photo");

                    b.Property<bool>("PhotoIsValid");

                    b.Property<int>("ProfessionalTitleId");

                    b.Property<int?>("SexId");

                    b.Property<string>("Specialty");

                    b.Property<int?>("TypeId");

                    b.Property<DateTime?>("WorkDate");

                    b.Property<int>("WorkUnitId");

                    b.HasKey("Id");

                    b.HasIndex("SexId");

                    b.HasIndex("TypeId");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("PCME.Domain.AggregatesModel.StudentAggregates.StudentType", b =>
                {
                    b.Property<int>("Id")
                        .HasDefaultValue(1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.HasKey("Id");

                    b.ToTable("StudentType");
                });

            modelBuilder.Entity("PCME.Domain.AggregatesModel.UnitAggregates.WorkUnit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("Email");

                    b.Property<int>("Level");

                    b.Property<string>("LinkMan");

                    b.Property<string>("LinkPhone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(60);

                    b.Property<int?>("PID");

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<int>("WorkUnitNatureId");

                    b.HasKey("Id");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.HasIndex("PID");

                    b.HasIndex("WorkUnitNatureId");

                    b.ToTable("WorkUnit");
                });

            modelBuilder.Entity("PCME.Domain.AggregatesModel.UnitAggregates.WorkUnitNature", b =>
                {
                    b.Property<int>("Id")
                        .HasDefaultValue(1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.HasKey("Id");

                    b.ToTable("WorkUnitNature");
                });

            modelBuilder.Entity("PCME.Domain.AggregatesModel.WorkUnitAccountAggregates.WorkUnitAccount", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AccountName")
                        .IsRequired();

                    b.Property<string>("HolderName");

                    b.Property<string>("PassWord")
                        .IsRequired();

                    b.Property<int>("WorkUnitAccountTypeId");

                    b.Property<int>("WorkUnitId");

                    b.HasKey("Id");

                    b.HasIndex("WorkUnitAccountTypeId");

                    b.HasIndex("WorkUnitId");

                    b.ToTable("WorkUnitAccounts");
                });

            modelBuilder.Entity("PCME.Domain.AggregatesModel.WorkUnitAccountAggregates.WorkUnitAccountType", b =>
                {
                    b.Property<int>("Id")
                        .HasDefaultValue(1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.HasKey("Id");

                    b.ToTable("WorkUnitAccountType");
                });

            modelBuilder.Entity("PCME.Domain.AggregatesModel.ProfessionalTitleAggregates.ProfessionalTitle", b =>
                {
                    b.HasOne("PCME.Domain.AggregatesModel.ProfessionalTitleAggregates.Level", "Level")
                        .WithMany()
                        .HasForeignKey("LevelId");

                    b.HasOne("PCME.Domain.AggregatesModel.ProfessionalTitleAggregates.Series", "Series")
                        .WithMany()
                        .HasForeignKey("SeriesId");

                    b.HasOne("PCME.Domain.AggregatesModel.ProfessionalTitleAggregates.Specialty", "Specialty")
                        .WithMany()
                        .HasForeignKey("SpecialtyId");
                });

            modelBuilder.Entity("PCME.Domain.AggregatesModel.StudentAggregates.Student", b =>
                {
                    b.HasOne("PCME.Domain.AggregatesModel.StudentAggregates.Sex", "Sex")
                        .WithMany()
                        .HasForeignKey("SexId");

                    b.HasOne("PCME.Domain.AggregatesModel.StudentAggregates.StudentType", "Type")
                        .WithMany()
                        .HasForeignKey("TypeId");
                });

            modelBuilder.Entity("PCME.Domain.AggregatesModel.UnitAggregates.WorkUnit", b =>
                {
                    b.HasOne("PCME.Domain.AggregatesModel.UnitAggregates.WorkUnit", "Parent")
                        .WithMany("Childs")
                        .HasForeignKey("PID");

                    b.HasOne("PCME.Domain.AggregatesModel.UnitAggregates.WorkUnitNature", "WorkUnitNature")
                        .WithMany()
                        .HasForeignKey("WorkUnitNatureId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("PCME.Domain.AggregatesModel.WorkUnitAccountAggregates.WorkUnitAccount", b =>
                {
                    b.HasOne("PCME.Domain.AggregatesModel.WorkUnitAccountAggregates.WorkUnitAccountType", "WorkUnitAccountType")
                        .WithMany()
                        .HasForeignKey("WorkUnitAccountTypeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("PCME.Domain.AggregatesModel.UnitAggregates.WorkUnit", "WorkUnit")
                        .WithMany("Accounts")
                        .HasForeignKey("WorkUnitId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
