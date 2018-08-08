using Microsoft.EntityFrameworkCore;
using PCME.Domain.AggregatesModel.CoursewareAggregates;
using PCME.Domain.AggregatesModel.VideoAggregates;
using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
