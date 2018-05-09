using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace PCME.MOPDB
{
    public partial class MOPDBContext : DbContext
    {
        public virtual DbSet<Account> Account { get; set; }
        public virtual DbSet<AccountType> AccountType { get; set; }
        public virtual DbSet<AuditState> AuditState { get; set; }
        public virtual DbSet<BmdAccount> BmdAccount { get; set; }
        public virtual DbSet<BmdUnitInfo> BmdUnitInfo { get; set; }
        public virtual DbSet<BookNote> BookNote { get; set; }
        public virtual DbSet<BookPostNote> BookPostNote { get; set; }
        public virtual DbSet<Cc> Cc { get; set; }
        public virtual DbSet<CivilServantDutyLevel> CivilServantDutyLevel { get; set; }
        public virtual DbSet<CivilTrainAudit> CivilTrainAudit { get; set; }
        public virtual DbSet<CivilTrainType> CivilTrainType { get; set; }
        public virtual DbSet<Consume> Consume { get; set; }
        public virtual DbSet<CreateMagCardNote> CreateMagCardNote { get; set; }
        public virtual DbSet<DirectoryZwClass> DirectoryZwClass { get; set; }
        public virtual DbSet<DirectoryZwName> DirectoryZwName { get; set; }
        public virtual DbSet<ExamAudit> ExamAudit { get; set; }
        public virtual DbSet<ExamAuditTemp> ExamAuditTemp { get; set; }
        public virtual DbSet<ExamOrder> ExamOrder { get; set; }
        public virtual DbSet<ExamOrderBelongExamStation> ExamOrderBelongExamStation { get; set; }
        public virtual DbSet<ExamScore> ExamScore { get; set; }
        public virtual DbSet<ExamSerialNumber> ExamSerialNumber { get; set; }
        public virtual DbSet<ExamStation> ExamStation { get; set; }
        public virtual DbSet<ExamStationRoom> ExamStationRoom { get; set; }
        public virtual DbSet<ExamSubject> ExamSubject { get; set; }
        public virtual DbSet<ExamSubjectZwClass> ExamSubjectZwClass { get; set; }
        public virtual DbSet<FruitAudit> FruitAudit { get; set; }
        public virtual DbSet<FruitCount> FruitCount { get; set; }
        public virtual DbSet<HomeWorkCtr> HomeWorkCtr { get; set; }
        public virtual DbSet<Kdaccount> Kdaccount { get; set; }
        public virtual DbSet<KdunitInfo> KdunitInfo { get; set; }
        public virtual DbSet<LevelChange> LevelChange { get; set; }
        public virtual DbSet<Money> Money { get; set; }
        public virtual DbSet<MoneyNote> MoneyNote { get; set; }
        public virtual DbSet<PaperAudit> PaperAudit { get; set; }
        public virtual DbSet<PaperCount> PaperCount { get; set; }
        public virtual DbSet<PayCard> PayCard { get; set; }
        public virtual DbSet<Person> Person { get; set; }
        public virtual DbSet<PersonCivilServant> PersonCivilServant { get; set; }
        public virtual DbSet<PersonClassCard> PersonClassCard { get; set; }
        public virtual DbSet<PersonIdentity> PersonIdentity { get; set; }
        public virtual DbSet<PersonKsresultUp> PersonKsresultUp { get; set; }
        public virtual DbSet<PersonPhoto> PersonPhoto { get; set; }
        public virtual DbSet<PersonSignIn> PersonSignIn { get; set; }
        public virtual DbSet<PersonTechnician> PersonTechnician { get; set; }
        public virtual DbSet<Print> Print { get; set; }
        public virtual DbSet<Publication> Publication { get; set; }
        public virtual DbSet<PxdExamSubjectRoom> PxdExamSubjectRoom { get; set; }
        public virtual DbSet<PxdOpenSubject> PxdOpenSubject { get; set; }
        public virtual DbSet<PxdOpenSubjectbak> PxdOpenSubjectbak { get; set; }
        public virtual DbSet<PxdUnitInfo> PxdUnitInfo { get; set; }
        public virtual DbSet<RegisterSetting> RegisterSetting { get; set; }
        public virtual DbSet<RegisterTable> RegisterTable { get; set; }
        public virtual DbSet<RegisterTablecollection> RegisterTablecollection { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<SelectCcnote> SelectCcnote { get; set; }
        public virtual DbSet<SysAccount> SysAccount { get; set; }
        public virtual DbSet<TeachBook> TeachBook { get; set; }
        public virtual DbSet<TrainApply> TrainApply { get; set; }
        public virtual DbSet<TrainApply2> TrainApply2 { get; set; }
        public virtual DbSet<TrainAudit> TrainAudit { get; set; }
        public virtual DbSet<TrainCount> TrainCount { get; set; }
        public virtual DbSet<TrainStation> TrainStation { get; set; }
        public virtual DbSet<TrainStationBelong> TrainStationBelong { get; set; }
        public virtual DbSet<TrainType> TrainType { get; set; }
        public virtual DbSet<Unit> Unit { get; set; }
        public virtual DbSet<UnitApply> UnitApply { get; set; }
        public virtual DbSet<UnitDept> UnitDept { get; set; }
        public virtual DbSet<UnitType> UnitType { get; set; }
        public virtual DbSet<PxdAccount> PxdAccount { get; set; }

        // Unable to generate entity type for table 'dbo.cc_history'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.personksresult_up_history'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.bk2014'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.ApplyPerson'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.SignUpManage'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.pxdAccount'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.Opinion'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.PaperAudit_Temp'. Please see the warning messages.

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer(@"Server=.;Database=MOPDB;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PxdAccount>(entity => {
                entity.ToTable("pxdAccount");
            });
            modelBuilder.Entity<Account>(entity =>
            {
                entity.Property(e => e.AccountId).HasColumnName("accountID");

                entity.Property(e => e.AccountName)
                    .IsRequired()
                    .HasColumnName("accountName")
                    .HasMaxLength(17)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TypeId).HasColumnName("typeID");

                entity.Property(e => e.UnitId)
                    .IsRequired()
                    .HasColumnName("unitID")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.HasOne(d => d.Unit)
                    .WithMany(p => p.Account)
                    .HasForeignKey(d => d.UnitId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Account_Unit");
            });

            modelBuilder.Entity<AccountType>(entity =>
            {
                entity.ToTable("accountType");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.TypeName)
                    .IsRequired()
                    .HasColumnName("typeName")
                    .HasColumnType("nchar(15)");
            });

            modelBuilder.Entity<AuditState>(entity =>
            {
                entity.HasKey(e => e.StateId);

                entity.Property(e => e.StateId)
                    .HasColumnName("stateID")
                    .ValueGeneratedNever();

                entity.Property(e => e.StateName)
                    .IsRequired()
                    .HasColumnName("stateName")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<BmdAccount>(entity =>
            {
                entity.ToTable("bmdAccount");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.LogName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.LogPassWord)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.Property(e => e.UnitId).HasColumnName("UnitID");

                entity.Property(e => e.Usbkey)
                    .IsRequired()
                    .HasColumnName("usbkey")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<BmdUnitInfo>(entity =>
            {
                entity.ToTable("bmdUnitInfo");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.BmdAddress)
                    .IsRequired()
                    .HasColumnName("bmdAddress")
                    .HasMaxLength(50);

                entity.Property(e => e.BmdName)
                    .IsRequired()
                    .HasColumnName("bmdName")
                    .HasMaxLength(50);

                entity.Property(e => e.PxdId).HasColumnName("pxdID");
            });

            modelBuilder.Entity<BookNote>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.BookId).HasColumnName("BookID");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.PersonId)
                    .IsRequired()
                    .HasColumnName("PersonID")
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.Property(e => e.PxdId).HasColumnName("pxdID");

                entity.Property(e => e.SubjectId)
                    .HasColumnName("SubjectID")
                    .HasMaxLength(4)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<BookPostNote>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.PostBookId).HasColumnName("PostBookID");

                entity.Property(e => e.PostPersonId)
                    .IsRequired()
                    .HasColumnName("PostPersonID")
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.Property(e => e.PostbmdAccountId).HasColumnName("PostbmdAccountID");
            });

            modelBuilder.Entity<Cc>(entity =>
            {
                entity.ToTable("CC");

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

                entity.Property(e => e.IsSelectDate).HasColumnType("datetime");

                entity.Property(e => e.KdunitId).HasColumnName("KDUnitID");

                entity.Property(e => e.Ksdate)
                    .HasColumnName("KSDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Rs).HasColumnName("RS");

                entity.Property(e => e.Xh)
                    .IsRequired()
                    .HasColumnName("XH")
                    .HasMaxLength(8)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CivilServantDutyLevel>(entity =>
            {
                entity.HasKey(e => e.DutyId);

                entity.Property(e => e.DutyId)
                    .HasColumnName("DutyID")
                    .HasColumnType("char(3)")
                    .ValueGeneratedNever();

                entity.Property(e => e.DutyName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CivilTrainAudit>(entity =>
            {
                entity.HasKey(e => e.TrainId);

                entity.Property(e => e.TrainId).HasColumnName("TrainID");

                entity.Property(e => e.FrontUnit)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PersonId)
                    .HasColumnName("PersonID")
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.Property(e => e.Result).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.TrainDate).HasColumnType("datetime");

                entity.Property(e => e.TrainDuty)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TrainName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TrainTypeId).HasColumnName("TrainTypeID");

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.CivilTrainAudit)
                    .HasForeignKey(d => d.PersonId)
                    .HasConstraintName("FK_CivilTrainAudit_Person_CivilServant");

                entity.HasOne(d => d.TrainType)
                    .WithMany(p => p.CivilTrainAudit)
                    .HasForeignKey(d => d.TrainTypeId)
                    .HasConstraintName("FK_CivilTrainAudit_CivilTrainType");
            });

            modelBuilder.Entity<CivilTrainType>(entity =>
            {
                entity.HasKey(e => e.TypeId);

                entity.Property(e => e.TypeId)
                    .HasColumnName("TypeID")
                    .ValueGeneratedNever();

                entity.Property(e => e.TypeName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Consume>(entity =>
            {
                entity.HasIndex(e => new { e.Id, e.ConsumeName, e.ConsumeTime, e.ConsumeMoney, e.ConsumeHandle, e.ConsumeType, e.Type, e.ConsumePersonId })
                    .HasName("_dta_index_Consume_7_1533248517__K4_1_2_3_5_6_7_8");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ConsumeHandle)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ConsumeMoney).HasColumnType("money");

                entity.Property(e => e.ConsumeName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ConsumePersonId)
                    .IsRequired()
                    .HasColumnName("ConsumePersonID")
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.Property(e => e.ConsumeTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ConsumeType)
                    .IsRequired()
                    .HasColumnType("char(1)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Type).HasColumnName("type");
            });

            modelBuilder.Entity<CreateMagCardNote>(entity =>
            {
                entity.HasIndex(e => e.PersonId)
                    .HasName("IX_CreateMagCardNote");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreateAdmin)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.IscCivilServant).HasColumnName("IScCivilServant");

                entity.Property(e => e.Istechnician).HasColumnName("ISTechnician");

                entity.Property(e => e.Isunit).HasColumnName("ISUnit");

                entity.Property(e => e.MagCard)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PersonId)
                    .IsRequired()
                    .HasColumnName("PersonID")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<DirectoryZwClass>(entity =>
            {
                entity.HasKey(e => e.ClassId);

                entity.ToTable("Directory_ZwClass");

                entity.Property(e => e.ClassId)
                    .HasColumnName("ClassID")
                    .HasColumnType("char(2)")
                    .ValueGeneratedNever();

                entity.Property(e => e.ClassName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Pxclass)
                    .HasColumnName("pxclass")
                    .HasColumnType("nchar(10)");
            });

            modelBuilder.Entity<DirectoryZwName>(entity =>
            {
                entity.ToTable("Directory_ZwName");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ClassName)
                    .IsRequired()
                    .HasColumnType("char(2)");

                entity.Property(e => e.Promotionway)
                    .HasColumnName("promotionway")
                    .HasColumnType("nchar(10)");

                entity.Property(e => e.ZcJb)
                    .IsRequired()
                    .HasColumnType("nchar(2)");

                entity.Property(e => e.ZwName)
                    .IsRequired()
                    .HasMaxLength(15);

                entity.Property(e => e.Zy)
                    .HasColumnName("ZY")
                    .HasColumnType("nchar(10)");

                entity.HasOne(d => d.ClassNameNavigation)
                    .WithMany(p => p.DirectoryZwName)
                    .HasForeignKey(d => d.ClassName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Directory_ZwName_Directory_ZwClass");
            });

            modelBuilder.Entity<ExamAudit>(entity =>
            {
                entity.ToTable("examAudit");

                entity.HasIndex(e => new { e.CreditHour, e.PersonId, e.ExamDate })
                    .HasName("_dta_index_examAudit_7_1876201734__K2_K7_4");

                entity.HasIndex(e => new { e.ExamId, e.PersonId, e.ResultState, e.Id })
                    .HasName("_dta_index_examAudit_7_1876201734__K2_K5_K6_1");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ExamDate).HasColumnType("datetime");

                entity.Property(e => e.ExamId)
                    .IsRequired()
                    .HasColumnName("examID")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.PersonId)
                    .IsRequired()
                    .HasColumnName("PersonID")
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.Property(e => e.SumResult).HasColumnType("decimal(10, 2)");
            });

            modelBuilder.Entity<ExamAuditTemp>(entity =>
            {
                entity.ToTable("examAudit_Temp");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ExamDate).HasColumnType("datetime");

                entity.Property(e => e.ExamId)
                    .IsRequired()
                    .HasColumnName("examID")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Idcard)
                    .IsRequired()
                    .HasColumnName("IDCard")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.InsertState).HasDefaultValueSql("((0))");

                entity.Property(e => e.PersonName)
                    .HasColumnName("personName")
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.SumResult).HasColumnType("decimal(10, 2)");
            });

            modelBuilder.Entity<ExamOrder>(entity =>
            {
                entity.HasKey(e => e.OrderId);

                entity.Property(e => e.OrderId)
                    .HasColumnName("OrderID")
                    .HasColumnType("char(2)")
                    .ValueGeneratedNever();

                entity.Property(e => e.ExamEndDate).HasColumnType("datetime");

                entity.Property(e => e.ExamStartDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<ExamOrderBelongExamStation>(entity =>
            {
                entity.HasKey(e => new { e.ExamStationId, e.ExamOrderId, e.SubjectId });

                entity.ToTable("ExamOrderBelong_ExamStation");

                entity.Property(e => e.ExamStationId)
                    .HasColumnName("ExamStationID")
                    .HasColumnType("char(2)");

                entity.Property(e => e.ExamOrderId)
                    .HasColumnName("ExamOrderID")
                    .HasColumnType("char(2)");

                entity.Property(e => e.SubjectId)
                    .HasColumnName("SubjectID")
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.EndRoomNumber)
                    .IsRequired()
                    .HasColumnType("char(2)");

                entity.Property(e => e.StartRoomNumber)
                    .IsRequired()
                    .HasColumnType("char(2)");
            });

            modelBuilder.Entity<ExamScore>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Examscore)
                    .HasColumnName("examscore")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.PersonId)
                    .IsRequired()
                    .HasColumnName("PersonID")
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.Property(e => e.SubjectId)
                    .IsRequired()
                    .HasColumnName("SubjectID")
                    .HasColumnType("char(10)");
            });

            modelBuilder.Entity<ExamSerialNumber>(entity =>
            {
                entity.HasKey(e => new { e.ExamStationId, e.OrderId, e.RoomNumber, e.SeatNumber });

                entity.Property(e => e.ExamStationId)
                    .HasColumnName("ExamStationID")
                    .HasColumnType("char(2)");

                entity.Property(e => e.OrderId)
                    .HasColumnName("OrderID")
                    .HasColumnType("char(2)");

                entity.Property(e => e.RoomNumber).HasColumnType("char(2)");

                entity.Property(e => e.SeatNumber).HasColumnType("char(2)");

                entity.Property(e => e.ExamSerialNumber1)
                    .IsRequired()
                    .HasColumnName("ExamSerialNumber")
                    .HasMaxLength(12)
                    .IsUnicode(false);

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

                entity.HasOne(d => d.ExamStation)
                    .WithMany(p => p.ExamSerialNumber)
                    .HasForeignKey(d => d.ExamStationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ExamSerialNumber_ExamStation");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.ExamSerialNumber)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ExamSerialNumber_ExamOrder");
            });

            modelBuilder.Entity<ExamStation>(entity =>
            {
                entity.HasKey(e => e.StationId);

                entity.Property(e => e.StationId)
                    .HasColumnName("StationID")
                    .HasColumnType("char(2)")
                    .ValueGeneratedNever();

                entity.Property(e => e.StationAddress)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.StationName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TrainStationId)
                    .HasColumnName("TrainStationID")
                    .HasColumnType("char(4)");
            });

            modelBuilder.Entity<ExamStationRoom>(entity =>
            {
                entity.HasKey(e => new { e.OrderId, e.ExamStationId, e.RoomId });

                entity.Property(e => e.OrderId)
                    .HasColumnName("OrderID")
                    .HasColumnType("char(2)");

                entity.Property(e => e.ExamStationId)
                    .HasColumnName("ExamStationID")
                    .HasColumnType("char(2)");

                entity.Property(e => e.RoomId)
                    .HasColumnName("RoomID")
                    .HasColumnType("char(2)");

                entity.Property(e => e.SubjectId)
                    .IsRequired()
                    .HasColumnName("SubjectID")
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.HasOne(d => d.ExamStation)
                    .WithMany(p => p.ExamStationRoom)
                    .HasForeignKey(d => d.ExamStationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ExamStationRoom_ExamStation");

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.ExamStationRoom)
                    .HasForeignKey(d => d.SubjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ExamStationRoom_ExamSubject");
            });

            modelBuilder.Entity<ExamSubject>(entity =>
            {
                entity.HasKey(e => e.SubjectId);

                entity.Property(e => e.SubjectId)
                    .HasColumnName("SubjectID")
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.ApplyCost).HasColumnType("money");

                entity.Property(e => e.ApplyEndDate).HasColumnType("datetime");

                entity.Property(e => e.ApplyStartDate).HasColumnType("datetime");

                entity.Property(e => e.ClassId)
                    .HasColumnName("classID")
                    .HasColumnType("char(2)");

                entity.Property(e => e.ExamCost).HasColumnType("money");

                entity.Property(e => e.Ksdate)
                    .HasColumnName("KSDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Ksokline)
                    .HasColumnName("KSOKLINE")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Kssj)
                    .HasColumnName("kssj")
                    .HasMaxLength(50);

                entity.Property(e => e.Kstype)
                    .HasColumnName("KSType")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Mscount)
                    .HasColumnName("MSCount")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Pxsj)
                    .HasColumnName("pxsj")
                    .HasMaxLength(50);

                entity.Property(e => e.Pxtype)
                    .HasColumnName("pxtype")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.SubjectName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TrainDate).HasColumnType("datetime");

                entity.Property(e => e.TrainStationId)
                    .HasColumnName("TrainStationID")
                    .HasColumnType("char(4)");

                entity.Property(e => e.TypeId).HasColumnName("TypeID");

                entity.Property(e => e.Yxline)
                    .HasColumnName("yxline")
                    .HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<ExamSubjectZwClass>(entity =>
            {
                entity.ToTable("ExamSubject_ZwClass");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ClassId)
                    .IsRequired()
                    .HasColumnName("ClassID")
                    .HasColumnType("char(3)");

                entity.Property(e => e.Isenable).HasDefaultValueSql("((1))");

                entity.Property(e => e.SubjectId)
                    .IsRequired()
                    .HasColumnName("SubjectID")
                    .HasMaxLength(4)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<FruitAudit>(entity =>
            {
                entity.HasKey(e => e.FruitId);

                entity.ToTable("fruitAudit");

                entity.HasIndex(e => new { e.CreditHour, e.AuditState, e.PersonId, e.CompleteDate })
                    .HasName("_dta_index_fruitAudit_7_1316199739__K9_K2_K4_8");

                entity.Property(e => e.FruitId).HasColumnName("fruitID");

                entity.Property(e => e.AreaLevel)
                    .IsRequired()
                    .HasColumnName("areaLevel")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.AuditAccount)
                    .HasMaxLength(17)
                    .IsUnicode(false);

                entity.Property(e => e.AuditState)
                    .HasColumnName("auditState")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.AwardLevel)
                    .IsRequired()
                    .HasColumnName("awardLevel")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.CompleteDate)
                    .HasColumnName("completeDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.CreditHour).HasColumnName("creditHour");

                entity.Property(e => e.FruitName)
                    .IsRequired()
                    .HasColumnName("fruitName")
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.JoinLevel).HasColumnName("joinLevel");

                entity.Property(e => e.PersonId)
                    .IsRequired()
                    .HasColumnName("personID")
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.FruitAudit)
                    .HasForeignKey(d => d.PersonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_fruitAudit_Person_Technician");
            });

            modelBuilder.Entity<FruitCount>(entity =>
            {
                entity.ToTable("fruitCount");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AreaLevel)
                    .IsRequired()
                    .HasColumnName("areaLevel")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.AwardLevel)
                    .IsRequired()
                    .HasColumnName("awardLevel")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreditHour).HasColumnName("creditHour");
            });

            modelBuilder.Entity<HomeWorkCtr>(entity =>
            {
                entity.HasIndex(e => e.WorkState)
                    .HasName("_dta_index_HomeWorkCtr_7_1373247947__K6");

                entity.HasIndex(e => new { e.Id, e.PersonId, e.SubjectId })
                    .HasName("_dta_index_HomeWorkCtr_5_1373247947__K2_K4_1");

                entity.HasIndex(e => new { e.HomeWorkCtr1, e.OverDateTime, e.PersonId, e.SubjectId, e.Id })
                    .HasName("_dta_index_HomeWorkCtr_7_1373247947__K2_K4_K1_3_5");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.HomeWorkCtr1).HasColumnName("HomeWorkCtr");

                entity.Property(e => e.OverDateTime).HasColumnType("datetime");

                entity.Property(e => e.PersonId)
                    .IsRequired()
                    .HasColumnName("PersonID")
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.Property(e => e.SubjectId)
                    .IsRequired()
                    .HasColumnName("SubjectID")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.WorkState)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('default')");
            });

            modelBuilder.Entity<Kdaccount>(entity =>
            {
                entity.ToTable("KDAccount");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.LogName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.LogPassWord)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.Property(e => e.UnitId).HasColumnName("UnitID");
            });

            modelBuilder.Entity<KdunitInfo>(entity =>
            {
                entity.ToTable("KDUnitInfo");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Kdaddress)
                    .IsRequired()
                    .HasColumnName("KDAddress")
                    .HasMaxLength(50);

                entity.Property(e => e.Kdcode)
                    .IsRequired()
                    .HasColumnName("KDCode")
                    .HasColumnType("char(2)");

                entity.Property(e => e.Kdname)
                    .IsRequired()
                    .HasColumnName("KDName")
                    .HasMaxLength(50);

                entity.Property(e => e.PersonCount)
                    .HasColumnName("personCount")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.PxdId).HasColumnName("pxdID");
            });

            modelBuilder.Entity<LevelChange>(entity =>
            {
                entity.HasKey(e => e.ChangeId);

                entity.Property(e => e.ChangeId)
                    .HasColumnName("changeID")
                    .ValueGeneratedNever();

                entity.Property(e => e.ChangeDate)
                    .HasColumnName("changeDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.PersonId)
                    .IsRequired()
                    .HasColumnName("personID")
                    .HasMaxLength(17)
                    .IsUnicode(false);

                entity.Property(e => e.TechDuty).HasColumnName("techDuty");
            });

            modelBuilder.Entity<Money>(entity =>
            {
                entity.HasIndex(e => new { e.Id, e.MoneyActual, e.PersonId })
                    .HasName("_dta_index_Money_5_2036202304__K2_1_4");

                entity.HasIndex(e => new { e.MoneyVirtual, e.MoneyActual, e.PersonId, e.Id })
                    .HasName("_dta_index_Money_7_2036202304__K2_K1_3_4");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.MoneyActual)
                    .HasColumnType("money")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.MoneyVirtual)
                    .HasColumnType("money")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.PersonId)
                    .IsRequired()
                    .HasColumnName("personID")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<MoneyNote>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AdminId)
                    .IsRequired()
                    .HasColumnName("AdminID")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.IsOverDate).HasColumnType("datetime");

                entity.Property(e => e.MacId)
                    .IsRequired()
                    .HasColumnName("MacID")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Money).HasColumnType("money");

                entity.Property(e => e.OverAddress)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.PersonId)
                    .IsRequired()
                    .HasColumnName("PersonID")
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.Property(e => e.Type).HasColumnName("type");
            });

            modelBuilder.Entity<PaperAudit>(entity =>
            {
                entity.HasKey(e => e.PaperId);

                entity.ToTable("paperAudit");

                entity.HasIndex(e => new { e.CreditHour, e.AuditState, e.PublishDate, e.PersonId })
                    .HasName("_dta_index_paperAudit_7_1815677516__K12_K6_K2_11");

                entity.Property(e => e.PaperId).HasColumnName("paperID");

                entity.Property(e => e.AreaLevel)
                    .HasColumnName("areaLevel")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.AuditAccount)
                    .HasColumnName("auditAccount")
                    .HasMaxLength(17)
                    .IsUnicode(false);

                entity.Property(e => e.AuditState)
                    .HasColumnName("auditState")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.AwardLevel)
                    .HasColumnName("awardLevel")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.CreditHour).HasColumnName("creditHour");

                entity.Property(e => e.JoinCount).HasColumnName("joinCount");

                entity.Property(e => e.JoinLevel).HasColumnName("joinLevel");

                entity.Property(e => e.PaperName)
                    .IsRequired()
                    .HasColumnName("paperName")
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.PaperType)
                    .IsRequired()
                    .HasColumnName("paperType")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.PersonId)
                    .IsRequired()
                    .HasColumnName("personID")
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.Property(e => e.PublicationId).HasColumnName("publicationID");

                entity.Property(e => e.PublishDate)
                    .HasColumnName("publishDate")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<PaperCount>(entity =>
            {
                entity.ToTable("paperCount");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AreaLevel)
                    .IsRequired()
                    .HasColumnName("areaLevel")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.AwardLevel)
                    .IsRequired()
                    .HasColumnName("awardLevel")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreditHour).HasColumnName("creditHour");

                entity.Property(e => e.PaperType)
                    .IsRequired()
                    .HasColumnName("paperType")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PayCard>(entity =>
            {
                entity.HasKey(e => e.CardId);

                entity.Property(e => e.CardId)
                    .HasColumnName("CardID")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.CanUseDate).HasColumnType("datetime");

                entity.Property(e => e.Cost).HasColumnType("money");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SerialNumber)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Type)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserPersonId)
                    .HasColumnName("UserPersonID")
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.HasOne(d => d.UserPerson)
                    .WithMany(p => p.PayCard)
                    .HasForeignKey(d => d.UserPersonId)
                    .HasConstraintName("FK_PayCard_Person");
            });

            modelBuilder.Entity<Person>(entity =>
            {
                entity.HasIndex(e => e.Idcard)
                    .HasName("UQ_IDCard")
                    .IsUnique();

                entity.HasIndex(e => new { e.PersonId, e.PersonName, e.Idcard, e.DeptId, e.PersonIdentityId, e.WorkUnitId })
                    .HasName("_dta_index_Person_7_955150448__K11_1_2_3_12_26");

                entity.HasIndex(e => new { e.PersonName, e.Sex, e.Birthday, e.GraduateSchool, e.GraduateDate, e.GraduateSpecialty, e.WorkSpecialty, e.WorkDate, e.WorkUnitId, e.DeptId, e.Phone, e.Mobile, e.Email, e.Address, e.AccountMoney, e.PxdId, e.Pxdidgwy, e.Idcard, e.Password, e.PersonId, e.PersonIdentityId })
                    .HasName("_dta_index_Person_7_955150448__K3_K17_K1_K26_2_4_5_6_7_8_9_10_11_12_18_19_20_21_23_28_29");

                entity.Property(e => e.PersonId)
                    .HasColumnName("personID")
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.AccountMoney).HasColumnType("money");

                entity.Property(e => e.Address)
                    .HasColumnName("address")
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.Birthday)
                    .HasColumnName("birthday")
                    .HasColumnType("datetime");

                entity.Property(e => e.DeptId).HasColumnName("deptID");

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.GraduateDate)
                    .HasColumnName("graduateDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.GraduateSchool)
                    .HasColumnName("graduateSchool")
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.GraduateSpecialty)
                    .HasColumnName("graduateSpecialty")
                    .HasColumnType("char(20)");

                entity.Property(e => e.Idcard)
                    .HasColumnName("IDCard")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Mobile)
                    .HasColumnName("mobile")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasColumnName("password")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.PersonIdentityId)
                    .HasColumnName("PersonIdentityID")
                    .HasColumnType("char(2)");

                entity.Property(e => e.PersonName)
                    .IsRequired()
                    .HasColumnName("personName")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .HasColumnName("phone")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.PxdId).HasColumnName("pxdID");

                entity.Property(e => e.Pxdidgwy).HasColumnName("pxdidgwy");

                entity.Property(e => e.Sex)
                    .HasColumnName("sex")
                    .HasColumnType("char(2)");

                entity.Property(e => e.WorkDate)
                    .HasColumnName("workDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.WorkSpecialty)
                    .HasColumnName("workSpecialty")
                    .HasColumnType("char(20)");

                entity.Property(e => e.WorkUnitId)
                    .HasColumnName("workUnitID")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.HasOne(d => d.PersonIdentity)
                    .WithMany(p => p.Person)
                    .HasForeignKey(d => d.PersonIdentityId)
                    .HasConstraintName("FK_Person_PersonIdentity");

                entity.HasOne(d => d.WorkUnit)
                    .WithMany(p => p.Person)
                    .HasForeignKey(d => d.WorkUnitId)
                    .HasConstraintName("FK_Person_Unit");
            });

            modelBuilder.Entity<PersonCivilServant>(entity =>
            {
                entity.HasKey(e => e.PersonId);

                entity.ToTable("Person_CivilServant");

                entity.Property(e => e.PersonId)
                    .HasColumnName("PersonID")
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.AuditUnitId)
                    .HasColumnName("AuditUnitID")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.ChiefDuty)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DutyLevel)
                    .IsRequired()
                    .HasColumnType("char(3)");

                entity.Property(e => e.RawQualification)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RepresentDate).HasColumnType("datetime");

                entity.Property(e => e.TopQualification)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.DutyLevelNavigation)
                    .WithMany(p => p.PersonCivilServant)
                    .HasForeignKey(d => d.DutyLevel)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Person_CivilServant_CivilServantDutyLevel");

                entity.HasOne(d => d.Person)
                    .WithOne(p => p.PersonCivilServant)
                    .HasForeignKey<PersonCivilServant>(d => d.PersonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Person_CivilServant_Person");
            });

            modelBuilder.Entity<PersonClassCard>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.PersonId)
                    .IsRequired()
                    .HasColumnName("PersonID")
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.Property(e => e.PxdExamSubjectRoomId).HasColumnName("PxdExamSubjectRoomID");

                entity.Property(e => e.TicketCode)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<PersonIdentity>(entity =>
            {
                entity.HasKey(e => e.IdentityId);

                entity.Property(e => e.IdentityId)
                    .HasColumnName("IdentityID")
                    .HasColumnType("char(2)")
                    .ValueGeneratedNever();

                entity.Property(e => e.IdentityName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PersonKsresultUp>(entity =>
            {
                entity.ToTable("PersonKSResult_UP");

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
                    .HasColumnType("char(4)");

                entity.Property(e => e.Zkzcode)
                    .IsRequired()
                    .HasColumnName("ZKZCode")
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PersonPhoto>(entity =>
            {
                entity.ToTable("personPhoto");

                entity.HasIndex(e => new { e.PhotoUrl, e.PersonId })
                    .HasName("_dta_index_personPhoto_7_461244698__K2_3");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Createdate)
                    .HasColumnName("createdate")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsOk).HasColumnName("IsOK");

                entity.Property(e => e.PersonId)
                    .IsRequired()
                    .HasColumnName("personID")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.PhotoUrl)
                    .IsRequired()
                    .HasColumnName("photoURL")
                    .HasMaxLength(225);
            });

            modelBuilder.Entity<PersonSignIn>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.PersonClassCardId).HasColumnName("PersonClassCardID");

                entity.Property(e => e.SignInTime).HasColumnType("datetime");

                entity.Property(e => e.SignOutTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<PersonTechnician>(entity =>
            {
                entity.HasKey(e => e.PersonId);

                entity.ToTable("Person_Technician");

                entity.Property(e => e.PersonId)
                    .HasColumnName("PersonID")
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.AuditCategory)
                    .IsRequired()
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.AuditUnitId)
                    .IsRequired()
                    .HasColumnName("AuditUnitID")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.CountDate).HasColumnType("datetime");

                entity.Property(e => e.CreditHours).HasColumnName("creditHours");

                entity.Property(e => e.DutyId).HasColumnName("DutyID");

                entity.Property(e => e.RepresentDate).HasColumnType("datetime");

                entity.Property(e => e.TopQualification)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.AuditUnit)
                    .WithMany(p => p.PersonTechnician)
                    .HasForeignKey(d => d.AuditUnitId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Person_Technician_Unit");

                entity.HasOne(d => d.Duty)
                    .WithMany(p => p.PersonTechnician)
                    .HasForeignKey(d => d.DutyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Person_Technician_Directory_ZwName");

                entity.HasOne(d => d.Person)
                    .WithOne(p => p.PersonTechnician)
                    .HasForeignKey<PersonTechnician>(d => d.PersonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Person_Technician_Person");
            });

            modelBuilder.Entity<Print>(entity =>
            {
                entity.HasKey(e => e.PersonId);

                entity.Property(e => e.PersonId)
                    .HasColumnName("PersonID")
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Idcard)
                    .IsRequired()
                    .HasColumnName("IDCard")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.PrintOrder)
                    .IsRequired()
                    .HasColumnType("char(2)");

                entity.Property(e => e.PrintState)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Printtime)
                    .HasColumnName("printtime")
                    .HasColumnType("datetime");

                entity.Property(e => e.Selecttime)
                    .HasColumnName("selecttime")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<Publication>(entity =>
            {
                entity.Property(e => e.PublicationId).HasColumnName("publicationID");

                entity.Property(e => e.AreaLevel)
                    .IsRequired()
                    .HasColumnType("char(6)");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PublicationName)
                    .IsRequired()
                    .HasMaxLength(60)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PxdExamSubjectRoom>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.EndTime).HasColumnType("datetime");

                entity.Property(e => e.IsActive).HasColumnName("isActive");

                entity.Property(e => e.Pxaddress)
                    .IsRequired()
                    .HasColumnName("PXAddress")
                    .HasMaxLength(255);

                entity.Property(e => e.Pxcontent)
                    .IsRequired()
                    .HasColumnName("PXContent")
                    .HasMaxLength(255);

                entity.Property(e => e.PxdUnitId).HasColumnName("PxdUnitID");

                entity.Property(e => e.Pxtime)
                    .HasColumnName("PXTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.SerialNumber)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.StartTime).HasColumnType("datetime");

                entity.Property(e => e.SubjectId)
                    .IsRequired()
                    .HasColumnName("SubjectID")
                    .HasMaxLength(4)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PxdOpenSubject>(entity =>
            {
                entity.ToTable("pxdOpenSubject");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Is2).HasColumnName("is2");

                entity.Property(e => e.Isenable).HasDefaultValueSql("((1))");

                entity.Property(e => e.PxdId).HasColumnName("pxdID");

                entity.Property(e => e.SubjectId)
                    .IsRequired()
                    .HasColumnName("SubjectID")
                    .HasMaxLength(4)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PxdOpenSubjectbak>(entity =>
            {
                entity.ToTable("pxdOpenSubjectbak");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.PxdId).HasColumnName("pxdID");

                entity.Property(e => e.SubjectId)
                    .IsRequired()
                    .HasColumnName("SubjectID")
                    .HasMaxLength(4)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PxdUnitInfo>(entity =>
            {
                entity.ToTable("pxdUnitInfo");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.PxdAddress)
                    .HasColumnName("pxdAddress")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PxdClass)
                    .HasColumnName("pxdClass")
                    .HasMaxLength(50);

                entity.Property(e => e.PxdEmail)
                    .HasColumnName("pxdEmail")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PxdLinkMan)
                    .HasColumnName("pxdLinkMan")
                    .HasMaxLength(50);

                entity.Property(e => e.PxdName)
                    .IsRequired()
                    .HasColumnName("pxdName")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PxdPhone)
                    .HasColumnName("pxdPhone")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Type).HasColumnName("type");
            });

            modelBuilder.Entity<RegisterSetting>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.IsActive).HasColumnName("isActive");

                entity.Property(e => e.SubjectId)
                    .IsRequired()
                    .HasColumnName("SubjectID")
                    .HasMaxLength(4)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<RegisterTable>(entity =>
            {
                entity.HasIndex(e => new { e.UnitId, e.SubjectId })
                    .HasName("IX_RegisterTable")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.IsLocked).HasColumnName("isLocked");

                entity.Property(e => e.Num)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SubjectId)
                    .IsRequired()
                    .HasColumnName("SubjectID")
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.UnitId)
                    .IsRequired()
                    .HasColumnName("UnitID")
                    .HasMaxLength(15)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<RegisterTablecollection>(entity =>
            {
                entity.HasIndex(e => new { e.RegisterTableId, e.PersonId })
                    .HasName("IX_RegisterTablecollection")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.PersonId)
                    .IsRequired()
                    .HasColumnName("PersonID")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RegisterTableId).HasColumnName("RegisterTableID");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.Popedom)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SelectCcnote>(entity =>
            {
                entity.ToTable("SelectCCNote");

                entity.HasIndex(e => new { e.Ccid, e.Ksstate })
                    .HasName("_dta_index_SelectCCNote_7_989246579__K4_K7");

                entity.HasIndex(e => new { e.CreateDate, e.Zkzcode, e.PersonId, e.Ccid, e.SubjectId })
                    .HasName("_dta_index_SelectCCNote_7_989246579__K2_K4_K3_5_11");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Ccid).HasColumnName("CCID");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.GoToTime).HasColumnType("datetime");

                entity.Property(e => e.Ksstate)
                    .IsRequired()
                    .HasColumnName("KSState")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('default')");

                entity.Property(e => e.OverKsdate)
                    .HasColumnName("OverKSDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.PersonId)
                    .IsRequired()
                    .HasColumnName("PersonID")
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.Property(e => e.StartKsdate)
                    .HasColumnName("StartKSDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.SubjectId)
                    .IsRequired()
                    .HasColumnName("SubjectID")
                    .HasColumnType("char(4)");

                entity.Property(e => e.Zkzcode)
                    .HasColumnName("ZKZCode")
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SysAccount>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.Property(e => e.SysLogName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.SysLogPass)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TeachBook>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Agio)
                    .HasColumnType("money")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.BookCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Dmoney)
                    .HasColumnName("DMoney")
                    .HasColumnType("money")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.IsSelect).HasDefaultValueSql("((1))");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Publisher).HasMaxLength(50);

                entity.Property(e => e.SubjectId)
                    .IsRequired()
                    .HasColumnName("SubjectID")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TrueMoney).HasColumnType("money");
            });

            modelBuilder.Entity<TrainApply>(entity =>
            {
                entity.HasKey(e => new { e.PersonId, e.SubjectId });

                entity.Property(e => e.PersonId)
                    .HasColumnName("PersonID")
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.Property(e => e.SubjectId)
                    .HasColumnName("SubjectID")
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.TrainStationId)
                    .IsRequired()
                    .HasColumnName("TrainStationID")
                    .HasColumnType("char(4)");

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.TrainApply)
                    .HasForeignKey(d => d.SubjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TrainApply_ExamSubject");
            });

            modelBuilder.Entity<TrainApply2>(entity =>
            {
                entity.HasIndex(e => e.PersonId)
                    .HasName("PK_TrainApply22");

                entity.HasIndex(e => new { e.Id, e.SubjectId, e.PersonId })
                    .HasName("_dta_index_TrainApply2_5_416720537__K3_K2_1");

                entity.HasIndex(e => new { e.TrainStationId, e.CreateDate, e.Arrearage, e.IsEnable2, e.PersonId, e.Isenable, e.Id, e.SubjectId })
                    .HasName("_dta_index_TrainApply2_7_416720537__K2_K7_K1_K3_4_6_9_10");

                entity.HasIndex(e => new { e.TrainStationId, e.IsConfirm, e.CreateDate, e.Isenable, e.WorkScore, e.Arrearage, e.IsEnable2, e.Yx, e.PersonId, e.SubjectId, e.Id })
                    .HasName("_dta_index_TrainApply2_7_416720537__K2_K3_K1_4_5_6_7_8_9_10_12");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Arrearage)
                    .HasColumnType("money")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Isenable).HasDefaultValueSql("((1))");

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

                entity.Property(e => e.TrainStationId).HasColumnName("TrainStationID");

                entity.Property(e => e.WorkScore).HasDefaultValueSql("((0))");

                entity.Property(e => e.Yx).HasColumnName("yx");
            });

            modelBuilder.Entity<TrainAudit>(entity =>
            {
                entity.HasKey(e => e.TrainId);

                entity.HasIndex(e => new { e.CreditHour, e.AuditState, e.PersonId, e.TrainDate })
                    .HasName("_dta_index_TrainAudit_7_1556200594__K9_K2_K5_8");

                entity.HasIndex(e => new { e.CreditHour, e.AuditState, e.TrainDate, e.PersonId })
                    .HasName("_dta_index_TrainAudit_7_1556200594__K9_K5_K2_8");

                entity.Property(e => e.TrainId).HasColumnName("trainID");

                entity.Property(e => e.AuditAccount)
                    .HasColumnName("auditAccount")
                    .HasMaxLength(17)
                    .IsUnicode(false);

                entity.Property(e => e.AuditState)
                    .HasColumnName("auditState")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.CreditHour).HasColumnName("creditHour");

                entity.Property(e => e.FrontUnit)
                    .IsRequired()
                    .HasColumnName("frontUnit")
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.PersonId)
                    .IsRequired()
                    .HasColumnName("personID")
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.Property(e => e.TrainContent)
                    .IsRequired()
                    .HasColumnName("trainContent")
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.TrainDate)
                    .HasColumnName("trainDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.TrainPeriod)
                    .HasColumnName("trainPeriod")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.TrainType)
                    .IsRequired()
                    .HasColumnName("trainType")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.TrainAudit)
                    .HasForeignKey(d => d.PersonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TrainAudit_Person_Technician");
            });

            modelBuilder.Entity<TrainCount>(entity =>
            {
                entity.HasKey(e => e.TrainId);

                entity.Property(e => e.TrainId).HasColumnName("TrainID");

                entity.Property(e => e.TrainName)
                    .IsRequired()
                    .HasMaxLength(60)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TrainStation>(entity =>
            {
                entity.HasKey(e => e.StationId);

                entity.Property(e => e.StationId)
                    .HasColumnName("StationID")
                    .HasColumnType("char(4)")
                    .ValueGeneratedNever();

                entity.Property(e => e.StationName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TrainStationBelong>(entity =>
            {
                entity.HasKey(e => new { e.SubjectId, e.TrainStationId });

                entity.Property(e => e.SubjectId)
                    .HasColumnName("SubjectID")
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.TrainStationId)
                    .HasColumnName("TrainStationID")
                    .HasColumnType("char(4)");

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.TrainStationBelong)
                    .HasForeignKey(d => d.SubjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TrainStationBelong_ExamSubject");

                entity.HasOne(d => d.TrainStation)
                    .WithMany(p => p.TrainStationBelong)
                    .HasForeignKey(d => d.TrainStationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TrainStationBelong_TrainStation");
            });

            modelBuilder.Entity<TrainType>(entity =>
            {
                entity.ToTable("trainType");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.TypeName)
                    .IsRequired()
                    .HasColumnName("typeName")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Unit>(entity =>
            {
                entity.Property(e => e.UnitId)
                    .HasColumnName("unitID")
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Address)
                    .HasColumnName("address")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Linkman)
                    .HasColumnName("linkman")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Mobile)
                    .HasColumnName("mobile")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .HasColumnName("phone")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TypeId).HasColumnName("typeID");

                entity.Property(e => e.UnitName)
                    .IsRequired()
                    .HasColumnName("unitName")
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.Unit)
                    .HasForeignKey(d => d.TypeId)
                    .HasConstraintName("FK_Unit_UnitType");
            });

            modelBuilder.Entity<UnitApply>(entity =>
            {
                entity.HasKey(e => e.ApplyId);

                entity.Property(e => e.ApplyId)
                    .HasColumnName("ApplyID")
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Amount).HasColumnType("money");

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.SubjectId)
                    .IsRequired()
                    .HasColumnName("SubjectID")
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.UnitId)
                    .IsRequired()
                    .HasColumnName("UnitID")
                    .HasMaxLength(15)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UnitDept>(entity =>
            {
                entity.HasKey(e => e.DeptId);

                entity.ToTable("unitDept");

                entity.HasIndex(e => e.UnitId)
                    .HasName("IX_unitDept");

                entity.Property(e => e.DeptId).HasColumnName("deptID");

                entity.Property(e => e.DeptName)
                    .IsRequired()
                    .HasColumnName("deptName")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UnitId)
                    .IsRequired()
                    .HasColumnName("unitID")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.HasOne(d => d.Unit)
                    .WithMany(p => p.UnitDept)
                    .HasForeignKey(d => d.UnitId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_unitDept_Unit");
            });

            modelBuilder.Entity<UnitType>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.TypeName)
                    .IsRequired()
                    .HasColumnName("typeName")
                    .HasColumnType("char(20)");
            });
        }
    }
}
