using MassTransit;
using SharedKernel.Core.Data.DbContext;

namespace Routing.gRPC.Data
{
    public class RoutingDbContext : DbContext, IApplicationDbContext
    {

        public virtual DbSet<Models.Route> Routes { get; set; } = default!;

        public RoutingDbContext(DbContextOptions<RoutingDbContext> options) : base(options)
        {


        }

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
