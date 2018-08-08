using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace PCME.KSDB
{
    public partial class KSDBContext : DbContext
    {

        public virtual DbSet<CcCopy> CcCopy { get; set; }
        public virtual DbSet<InvigilatorNote> InvigilatorNote { get; set; }
        public virtual DbSet<PersonCopy> PersonCopy { get; set; }
        public virtual DbSet<PersonKsresult> PersonKsresult { get; set; }
        public virtual DbSet<Test> Test { get; set; }
        public virtual DbSet<TestConfigCopy> TestConfigCopy { get; set; }
        public virtual DbSet<UsemagCardComOnNote> UsemagCardComOnNote { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server =.;database=KSDB;uid=sa;pwd=Abc@28122661");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CcCopy>(entity =>
            {
                entity.ToTable("CC_copy");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreateAccount)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsOpen)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('Close')");

                entity.Property(e => e.IsOpenDate).HasColumnType("datetime");

                entity.Property(e => e.IsOverSelectDate).HasColumnType("datetime");

                entity.Property(e => e.IsRelax).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsSelectDate).HasColumnType("datetime");

                entity.Property(e => e.KdunitId).HasColumnName("KDUnitID");

                entity.Property(e => e.Ksdate)
                    .HasColumnName("KSDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.KsoverDate)
                    .HasColumnName("KSOverDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Rs).HasColumnName("RS");

                entity.Property(e => e.Xh)
                    .IsRequired()
                    .HasColumnName("XH")
                    .HasMaxLength(8)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<InvigilatorNote>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Ccxh)
                    .IsRequired()
                    .HasColumnName("ccxh")
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.InvigilatorTeacher)
                    .IsRequired()
                    .HasColumnName("invigilatorTeacher")
                    .HasMaxLength(50);

                entity.Property(e => e.Ksdate)
                    .HasColumnName("KSdate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Qkrs).HasColumnName("QKRS");

                entity.Property(e => e.Remark)
                    .IsRequired()
                    .IsUnicode(false)
                    .HasDefaultValueSql("('无')");

                entity.Property(e => e.SaveTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Skrs).HasColumnName("SKRS");

                entity.Property(e => e.Wjrs).HasColumnName("WJRS");
            });

            modelBuilder.Entity<PersonCopy>(entity =>
            {
                entity.ToTable("Person_copy");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Ccxh)
                    .IsRequired()
                    .HasColumnName("CCXH")
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.GoToTime).HasColumnType("datetime");

                entity.Property(e => e.Idcard)
                    .IsRequired()
                    .HasColumnName("IDCard")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.IsTk).HasColumnName("IsTK");

                entity.Property(e => e.IsZb).HasColumnName("IsZB");

                entity.Property(e => e.KdunitId).HasColumnName("KDUnitID");

                entity.Property(e => e.Ksstate)
                    .HasColumnName("KSState")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('default')");

                entity.Property(e => e.MagCard)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OverKstime)
                    .HasColumnName("OverKSTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.PassWord)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.PersonId)
                    .IsRequired()
                    .HasColumnName("PersonID")
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.Property(e => e.PersonName)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.PersonPhoto)
                    .IsRequired()
                    .HasColumnName("personPhoto")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.PersonStartKstime)
                    .HasColumnName("PersonStartKSTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.PersonZkz)
                    .IsRequired()
                    .HasColumnName("personZKZ")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Sex)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.StartKstime)
                    .HasColumnName("StartKSTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.SubjectId)
                    .IsRequired()
                    .HasColumnName("SubjectID")
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.SubjectName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.WorkUnitName)
                    .IsRequired()
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Workscore).HasColumnName("workscore");
            });

            modelBuilder.Entity<PersonKsresult>(entity =>
            {
                entity.ToTable("PersonKSResult");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Ccxh)
                    .IsRequired()
                    .HasColumnName("CCXH")
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.Createdate)
                    .HasColumnName("createdate")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ExamDate).HasColumnType("datetime");

                entity.Property(e => e.PersonId)
                    .IsRequired()
                    .HasColumnName("PersonID")
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.Property(e => e.SubjectId)
                    .IsRequired()
                    .HasColumnName("SubjectID")
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.Zkzcode)
                    .IsRequired()
                    .HasColumnName("ZKZCode")
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Test>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.SelectItem).HasColumnName("selectItem");

                entity.Property(e => e.SubjectId)
                    .HasColumnName("subjectID")
                    .HasMaxLength(255);

                entity.Property(e => e.Topic).HasColumnName("topic");

                entity.Property(e => e.Type).HasColumnName("type");
            });

            modelBuilder.Entity<TestConfigCopy>(entity =>
            {
                entity.ToTable("TestConfig_copy");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Age).HasColumnType("datetime");

                entity.Property(e => e.SubjectId)
                    .IsRequired()
                    .HasColumnName("SubjectID")
                    .HasMaxLength(4)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UsemagCardComOnNote>(entity =>
            {
                entity.ToTable("USEMagCardComOnNote");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Ccxh)
                    .HasColumnName("CCXH")
                    .HasMaxLength(8);

                entity.Property(e => e.Idcard)
                    .IsRequired()
                    .HasColumnName("IDCard")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Kdaccount)
                    .IsRequired()
                    .HasColumnName("KDAccount")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.KdunitId).HasColumnName("KDUnitID");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
