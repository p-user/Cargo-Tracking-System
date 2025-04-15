using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Routing.gRPC.Data
{
    public class RoutingDbContext : DbContext
    {

        public DbSet<Models.Route> Routes { get; set; } = default!;


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
