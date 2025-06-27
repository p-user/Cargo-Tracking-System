using Microsoft.EntityFrameworkCore.Design;

namespace Routing.gRPC.Data
{
    public class RoutingDbContextFactory : IDesignTimeDbContextFactory<RoutingDbContext>
    {
        public RoutingDbContext CreateDbContext(string[] args)
        {

            // This factory provides a simplified way to create the DbContext
            // for design-time tools, without running the full application host.

            var optionsBuilder = new DbContextOptionsBuilder<RoutingDbContext>();
            var appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var localDbFolder = Path.Combine(appDataFolder, "CargoTrackingSystem");
            Directory.CreateDirectory(localDbFolder);
            var dbPath = Path.Combine(localDbFolder, "routingDb.sqlite");
            var connectionString = $"Data Source={dbPath}";

            optionsBuilder.UseSqlite(connectionString);
            return new RoutingDbContext(optionsBuilder.Options);
        }
    }
}
