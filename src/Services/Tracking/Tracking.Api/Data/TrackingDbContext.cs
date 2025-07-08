using MassTransit;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Tracking.Api.Models;

namespace Tracking.Api.Data
{
    public class TrackingDbContext : DbContext
    {
        public TrackingDbContext(DbContextOptions options) : base(options)
        {
        }

        

        public virtual DbSet<CargoTracking> CargoTracking { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.AddOutboxStateEntity();
            modelBuilder.AddInboxStateEntity();
            modelBuilder.AddOutboxMessageEntity();
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());



        }

    }
}
