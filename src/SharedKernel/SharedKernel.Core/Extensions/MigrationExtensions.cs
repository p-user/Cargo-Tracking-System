using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace SharedKernel.Extensions
{
    public static class MigrationExtensions
    {
        public static IApplicationBuilder ApplyMigrations<TContext>(this IApplicationBuilder app) where TContext : DbContext
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<TContext>();
                context.Database.Migrate();
            }
            return app;
        }
    }
}
