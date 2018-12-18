using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using PCME.Domain.AggregatesModel.ExamResultAggregates;
using PCME.Domain.AggregatesModel.HomeWorkAggregates;
using PCME.Domain.AggregatesModel.InvigilateAggregates;
using PCME.Domain.AggregatesModel.TestAggregates;
using PCME.Infrastructure.EntityConfigurations;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCME.Infrastructure
{
    public class TestDBContext : DbContext
    {
        public DbSet<TestLibrary> TestLibrary { get; set; }
        public DbSet<TestConfig> TestConfig { get; set; }
        public DbSet<TestPaper> TestPaper { get; set; }
        public DbSet<TestType> TestType { get; set; }

        public DbSet<InvigilateForRoomPlant> InvigilateForRoomPlant { get; set; }
        public DbSet<InvigilateForStudent> InvigilateForStudent { get; set; }
        public DbSet<ExamResult> ExamResult { get; set; }

        public DbSet<HomeWorkResult> HomeWorkResult { get; set; }
        public TestDBContext(DbContextOptions<TestDBContext> options):base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new TestLibraryEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new TestConfigEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new TestTypeEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ExamResultEntityTypeConfiguration());

            //base.OnModelCreating(modelBuilder);
        }
        public class TestDBContextFactory : IDesignTimeDbContextFactory<TestDBContext>
        {
            public TestDBContext CreateDbContext(string[] args)
            {
                var optionsBuilder = new DbContextOptionsBuilder<TestDBContext>()
                    .UseSqlServer(@"Server=.;database=PCME_TEST;uid=sa;pwd=sa@28122661");

                return new TestDBContext(optionsBuilder.Options);
                //return new ApplicationDbContext(optionsBuilder.Options,new NoMediator());
            }
        }
    }
}
