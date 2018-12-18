using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using PCME.Domain.AggregatesModel.CoursewareAggregates;
using PCME.Domain.AggregatesModel.VideoAggregates;

namespace PCME.Infrastructure
{
    public class VideoDbContext:DbContext
    {
        public DbSet<Video> Video { get; set; }
        public DbSet<VideoCollection> VideoCollection { get; set; }
        public VideoDbContext(DbContextOptions<VideoDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
        }
        public class VideoDbContextFactory : IDesignTimeDbContextFactory<VideoDbContext>
        {
            public VideoDbContext CreateDbContext(string[] args)
            {
                var optionsBuilder = new DbContextOptionsBuilder<VideoDbContext>()
                    .UseSqlServer(@"Server=.;database=PCME_Video;uid=sa;pwd=sa@28122661");

                return new VideoDbContext(optionsBuilder.Options);
                //return new ApplicationDbContext(optionsBuilder.Options,new NoMediator());
            }
        }
    }
}
