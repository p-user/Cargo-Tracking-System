
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Core.Data;
using SharedKernel.Core.DDD;

namespace SharedKernel.Core.Extensions
{
    public static class UnitOfWorkExtensions
    {
        public static IServiceCollection AddUnitOfWorkWithOutbox<TDbContext>(this IServiceCollection services) where TDbContext : DbContext
        {
            services.AddScoped<DomainEventDispatcher<TDbContext>>();
            services.AddScoped<IUnitOfWork, EfUnitOfWork<TDbContext>>();
            services.Decorate<IUnitOfWork, OutboxUnitOfWorkDecorator<TDbContext>>();

            return services;
        }
    }
}
