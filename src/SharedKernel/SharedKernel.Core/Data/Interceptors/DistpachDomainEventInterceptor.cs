using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Core.DDD;


namespace SharedKernel.Core.Data.Interceptors
{
    public class DispatchDomainEventInterceptor(IServiceProvider serviceProvider) : SaveChangesInterceptor 
    {
       

        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            //DispatchDomainEvents(eventData.Context as TContext).GetAwaiter().GetResult();
            return base.SavingChanges(eventData, result);
        }

        public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,InterceptionResult<int> result,CancellationToken cancellationToken = default)
        {
            await DispatchDomainEvents(eventData.Context);
            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private async Task DispatchDomainEvents(DbContext dbContext)
        {
            if (dbContext == null) return;

            var _publishEndpoint = serviceProvider.GetRequiredService<IPublishEndpoint>();

            // Retrieve entities with domain events
            var aggregates = dbContext.ChangeTracker
                .Entries<IAggregate>()
                .Where(entry => entry.Entity.DomainEvents.Any())
                .Select(entry => entry.Entity)
                .ToList();

            var domainEvents = aggregates.SelectMany(agg => agg.DomainEvents).ToList();

            // Clear domain events
            foreach (var aggregate in aggregates)
            {
                aggregate.ClearDomainEvents();
            }

            foreach (var domainEvent in domainEvents)
            {
                await  _publishEndpoint.Publish(domainEvent);
            }
        }
    }
}
