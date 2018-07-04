using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using PCME.Domain.AggregatesModel.AdmissionTicketAggregates;
using PCME.Domain.AggregatesModel.AdmissionTicketLogAggregates;
using PCME.Domain.AggregatesModel.AuditStatusAggregates;
using PCME.Domain.AggregatesModel.BookAggregates;
using PCME.Domain.AggregatesModel.CreditExamAggregates;
using PCME.Domain.AggregatesModel.ExaminationRoomAggregates;
using PCME.Domain.AggregatesModel.ExaminationRoomPlanAggregates;
using PCME.Domain.AggregatesModel.ExamOpenInfoAggregates;
using PCME.Domain.AggregatesModel.ExamSubjectAggregates;
using PCME.Domain.AggregatesModel.PaperAggregates;
using PCME.Domain.AggregatesModel.ProfessionalTitleAggregates;
using PCME.Domain.AggregatesModel.ScientificPayoffsAggregates;
using PCME.Domain.AggregatesModel.SignUpAggregates;
using PCME.Domain.AggregatesModel.StudentAggregates;
using PCME.Domain.AggregatesModel.TrainingCenterAggregates;
using PCME.Domain.AggregatesModel.UnitAggregates;
using PCME.Domain.AggregatesModel.WorkUnitAccountAggregates;
using PCME.Infrastructure.EntityConfigurations;
using System.Threading;
using System.Threading.Tasks;

namespace PCME.Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<WorkUnitNature> WorkUnitNature { get; set; }

        public DbSet<WorkUnit> WorkUnits { get; set; }
        public DbSet<Level> Levels { get; set; }
        public DbSet<ProfessionalTitle> ProfessionalTitles { get; set; }
        public DbSet<Series> Seriess { get; set; }
        public DbSet<Specialty> Specialtys { get; set; }
        public DbSet<WorkUnitAccount> WorkUnitAccounts { get; set; }
        public DbSet<WorkUnitAccountType> WorkUnitAccountType { get; set; }
        public DbSet<StudentType> StudentTypes { get; set; }
        public DbSet<Sex> Sex { get; set; }
        public DbSet<StudentStatus> StudentStatus { get; set; }
        public DbSet<TrainingCenter> TrainingCenter { get; set; }
		public DbSet<ExamSubject> ExamSubjects { get; set; }
		public DbSet<ExamSubjectStatus> ExamSubjectStatuses { get; set; }
        public DbSet<ExamType> ExamTypes { get; set; }
        public DbSet<OpenType> OpenTypes { get; set; }
        public DbSet<AuditStatus> AuditStatus { get; set; }
        public DbSet<ExamSubjectOpenInfo> ExamSubjectOpenInfo { get; set; }
        public DbSet<PromoteType> PromoteTypes { get; private set; }
        public DbSet<ProfessionalInfo> ProfessionalInfos { get; set; }
        public DbSet<DutyLevel> DutyLevels { get; set; }
		public DbSet<CivilServantInfo> CivilServantInfos { get; set; }

        public DbSet<SignUp> SignUp { get; set; }
        public DbSet<SignUpForUnit> SignUpForUnit { get; set; }
        public DbSet<SignUpCollection> SignUpCollections { get; set; }
        public DbSet<ExaminationRoom> ExaminationRooms { get; set; }
        public DbSet<ExaminationRoomPlan> ExaminationRoomPlans { get; set; }
        public DbSet<PlanStatus> PlanStatus { get; set; }
        public DbSet<AdmissionTicket> AdmissionTickets { get; set; }
        public DbSet<AdmissionTicketLogs> AdmissionTicketLogs { get; set; }
        public DbSet<Book> Books { get; set; }


        #region 学分申报类
        public DbSet<CreditExam> CreditExams { get; set; }
        public DbSet<Paper> Paper { get; set; }
        public DbSet<AreaLevel> AreaLevels { get; set; }
        public DbSet<AwardPaperLevel> AwardPaperLevels { get; set; }
        public DbSet<Periodical> Periodicals { get; set; }
        public DbSet<PublishType> PublishTypes { get; set; }

        public DbSet<ScientificPayoffs> ScientificPayoffs { get; set; }
        public DbSet<AwardSPLevel> AwardSPLevels { get; set; }
        #endregion


        //private readonly IMediator _mediator;

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(@"Server=.;Initial Catalog=PCME;Integrated Security=true");
        //}
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        //private ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,IMediator mediator) : base(options)
        //{
        //    _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        //    System.Diagnostics.Debug.WriteLine("OrderingContext::ctor ->" + this.GetHashCode());
        //}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new WorkUnitEntityTypeConfiguration());
            builder.ApplyConfiguration(new WorkUnitNatureEntityTypeConfiguration());
            builder.ApplyConfiguration(new SexEntityTypeConfiguration());
            builder.ApplyConfiguration(new StudentTypeEntityTypeConfiguration());
            builder.ApplyConfiguration(new ProfessionalTitleEntityTypeConfiguration());
            builder.ApplyConfiguration(new WorkUnitAccountTypeEntityTypeConfiguration());
            builder.ApplyConfiguration(new StudentStatusEntityTypeConfiguration());
			builder.ApplyConfiguration(new ExamSubjectStatusEntityTypeConfiguration());
			builder.ApplyConfiguration(new ExamTypeEntityTypeConfiguration());
			builder.ApplyConfiguration(new OpenTypeEntityTypeConfiguration());
            builder.ApplyConfiguration(new AuditStatusEntityTypeConfiguration());
            builder.ApplyConfiguration(new PromoteTypeEntityTypeConfiguration());
            builder.ApplyConfiguration(new ExamSubjectOpenInfoEntityTypeConfiguration());
            builder.ApplyConfiguration(new StudentEntityTypeConfiguration());
            builder.ApplyConfiguration(new SignUpCollectionEntityTypeConfiguration());
            builder.ApplyConfiguration(new SignUpEntityTypeConfiguration());
            builder.ApplyConfiguration(new PlanStatusEntityTypeConfiguration());
            builder.ApplyConfiguration(new AdmissionTicketEntityTypeConfiguration());
            builder.ApplyConfiguration(new CreditExamEntityTypeConfiguration());

            builder.ApplyConfiguration(new AreLevelTypeEntityTypeConfiguration());
            builder.ApplyConfiguration(new AwardPaperLevelEntityTypeConfiguration());
            builder.ApplyConfiguration(new AwardSPLevelEntityTypeConfiguration());
            builder.ApplyConfiguration(new PublishTypeEntityTypeConfiguration());

        }
        //public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken))
        //{ 
        //    await _mediator.DispatchDomainEventsAsync(this);
        //    var result = await base.SaveChangesAsync();
        //    return true;
        //}
        public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
        {
            public ApplicationDbContext CreateDbContext(string[] args) {
                var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
					.UseSqlServer(@"Server=.;database=PCME;uid=sa;pwd=Abc@28122661");

                return new ApplicationDbContext(optionsBuilder.Options);
                //return new ApplicationDbContext(optionsBuilder.Options,new NoMediator());
            }

            class NoMediator : IMediator
            {
                public Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default(CancellationToken)) where TNotification : INotification
                {
                    return Task.CompletedTask;
                }

                public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default(CancellationToken))
                {
                    return Task.FromResult<TResponse>(default(TResponse));
                }

                public Task Send(IRequest request, CancellationToken cancellationToken = default(CancellationToken))
                {
                    return Task.CompletedTask;
                }
            }
        }
    }
}
