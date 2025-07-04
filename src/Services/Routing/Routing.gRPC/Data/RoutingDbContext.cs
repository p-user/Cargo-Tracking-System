using MassTransit;

namespace Routing.gRPC.Data
{
    public class RoutingDbContext : DbContext
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
