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


            using var scope = app.ApplicationServices.CreateScope();
            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger<TContext>>();

            try
            {
                logger.LogInformation("Applying database migrations for {DbContextName}...", typeof(TContext).Name);

               
                var dbContext = services.GetRequiredService<TContext>();

                dbContext.Database.Migrate();

                logger.LogInformation("Database migrations applied successfully for {DbContextName}.", typeof(TContext).Name);
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
