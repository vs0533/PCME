using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using PCME.Domain.AggregatesModel;
using PCME.Domain.AggregatesModel.ProfessionalTitleAggregates;
using PCME.Domain.AggregatesModel.StudentAggregates;
using PCME.Domain.AggregatesModel.TrainingCenterAggregates;
using PCME.Domain.AggregatesModel.UnitAggregates;
using PCME.Domain.AggregatesModel.WorkUnitAccountAggregates;
using PCME.Infrastructure.EntityConfigurations;
using System;
using System.Collections.Generic;
using System.Text;
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
                .UseSqlServer(@"Server=.;Initial Catalog=PCME;Integrated Security=true");

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
