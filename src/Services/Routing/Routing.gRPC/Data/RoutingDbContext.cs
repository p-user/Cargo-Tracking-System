using Microsoft.EntityFrameworkCore;

namespace Routing.gRPC.Data
{
    public class RoutingDbContext : DbContext
    {

        public DbSet<Models.Route> Routes { get; set; } = default!;


        public RoutingDbContext(DbContextOptions<RoutingDbContext> options) : base(options)
        {

        }

    }
}
