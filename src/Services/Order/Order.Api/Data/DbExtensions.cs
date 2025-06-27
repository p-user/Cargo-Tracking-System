namespace Order.Api.Data
{
    public static class DbExtensions
    {
        /// <summary>
        /// Applies EF Core migrations at startup. This method creates an isolated,
        /// temporary DbContext to avoid triggering runtime services like MassTransit.
        /// </summary>
        public static IApplicationBuilder EnsureSeedData<TContext>(this IApplicationBuilder app) where TContext : DbContext
        {

            var configuration = app.ApplicationServices.GetRequiredService<IConfiguration>();
            var loggerFactory = app.ApplicationServices.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger("MigrationExtensions");


            var services = new ServiceCollection();

            services.AddDbContext<TContext>(options =>
            {

                var connectionString = configuration.GetConnectionString("DefaultConnection");
                options.UseSqlServer(connectionString);

            });

            var serviceProvider = services.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<TContext>();

            try
            {
                logger.LogInformation("Applying database migrations for {DbContextName}...", typeof(TContext).Name);

               
                dbContext.Database.Migrate();

                logger.LogInformation("Database migrations applied successfully.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while applying database migrations for {DbContextName}.", typeof(TContext).Name);
                throw;
            }

            return app;
        }
    }
}
