using Microsoft.EntityFrameworkCore;

namespace Routing.gRPC.Data
{
    public static class DbExtensions
    {

        public static IApplicationBuilder EnsureSeedData(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            using var dbcontext = scope.ServiceProvider.GetRequiredService<RoutingDbContext>();
            dbcontext.Database.MigrateAsync();

            return app;

        }
    }
}
