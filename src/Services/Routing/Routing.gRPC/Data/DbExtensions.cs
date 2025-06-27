using Microsoft.EntityFrameworkCore;

namespace Routing.gRPC.Data
{
    public static class DbExtensions
    {

        public static IApplicationBuilder EnsureSeedData(this IApplicationBuilder app)
        {
            using var initialScope = app.ApplicationServices.CreateScope();
            var serviceProvider = initialScope.ServiceProvider;
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var logger = serviceProvider.GetRequiredService<ILogger<Program>>();


            var optionsBuilder = new DbContextOptionsBuilder<RoutingDbContext>();

            try
            {
                string dbPath;
                var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

                if (environment == "Local")
                {
                    var appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                    var localDbFolder = Path.Combine(appDataFolder, "CargoTrackingSystem");
                    Directory.CreateDirectory(localDbFolder);
                    dbPath = Path.Combine(localDbFolder, "routingDb.sqlite");
                }
                else
                {
                    var dataFolder = Path.Combine(AppContext.BaseDirectory, "data");
                    Directory.CreateDirectory(dataFolder);
                    dbPath = Path.Combine(dataFolder, "routingDb.sqlite");
                }
                var connectionString = $"Data Source={dbPath}";

                // Use the options builder to configure the database provider.
                optionsBuilder.UseSqlite(connectionString);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to configure database connection string for startup migration.");
                throw;
            }

           
            using var dbContext = new RoutingDbContext(optionsBuilder.Options);

          
            try
            {
                logger.LogInformation("Applying database migrations at startup...");
                dbContext.Database.Migrate();
                logger.LogInformation("Database migrations applied successfully.");

              
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while applying database migrations.");
                throw;
            }

            return app;

        }
    }
}
