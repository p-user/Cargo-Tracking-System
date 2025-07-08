using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Tracking.Api.Data
{
    public class TrackingDbContextFactory:    IDesignTimeDbContextFactory<TrackingDbContext>
    {
        public TrackingDbContext CreateDbContext(string[] args)
        {
            // This factory provides a simplified way to create the DbContext
            // for design-time tools, without running the full application host.


            IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.Develoment.json", optional: true)
            .AddJsonFile("appsettings.Local.json")
            .Build();


            var optionsBuilder = new DbContextOptionsBuilder<TrackingDbContext>();


            var connectionString = configuration.GetConnectionString("DefaultConnection");


            optionsBuilder.UseNpgsql(connectionString, sqlServerOptions =>
            {

                sqlServerOptions.MigrationsAssembly(typeof(TrackingDbContext).Assembly.FullName);
            });


            return new TrackingDbContext(optionsBuilder.Options);
        }
    }
}
