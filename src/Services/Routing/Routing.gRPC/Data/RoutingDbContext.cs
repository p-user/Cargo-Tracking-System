using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Routing.gRPC.Data
{
    public class RoutingDbContext : DbContext
    {

        public virtual DbSet<Models.Route> Routes { get; set; } = default!;
        public virtual DbSet<Models.OutboxMessage> OutboxMessages => Set<Models.OutboxMessage>();


        public RoutingDbContext(DbContextOptions<RoutingDbContext> options) : base(options)
        {


        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

    }
}
