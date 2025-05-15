using SharedKernel.Data.DbContext;
using SharedKernel.Data.OutBox;
using System.Reflection;

namespace Routing.gRPC.Data
{
    public class RoutingDbContext : DbContext, IApplicationDbContext
    {

        public virtual DbSet<Models.Route> Routes { get; set; } = default!;
        public virtual DbSet<OutboxMessage> OutboxMessages { get ; set ; } = default!;

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
