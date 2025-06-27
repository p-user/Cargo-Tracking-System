using Microsoft.EntityFrameworkCore.Design;
using SharedKernel.Core.Data.DbContext;

namespace Order.Api.Data
{
    public class OrderDbContextFactory : IDesignTimeDbContextFactory<OrderDbContext>
    {
        public OrderDbContext CreateDbContext(string[] args)
        {
            // This factory provides a simplified way to create the DbContext
            // for design-time tools, without running the full application host.


            IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.Develoment.json", optional: true)
            .AddJsonFile("appsettings.Local.json" )
            .Build();


            var optionsBuilder = new DbContextOptionsBuilder<OrderDbContext>();


            var connectionString = configuration.GetConnectionString("DefaultConnection");


            optionsBuilder.UseSqlServer(connectionString, sqlServerOptions =>
            {

                sqlServerOptions.MigrationsAssembly(typeof(OrderDbContext).Assembly.FullName);
            });


            return new OrderDbContext(optionsBuilder.Options);
        }
    }
}
