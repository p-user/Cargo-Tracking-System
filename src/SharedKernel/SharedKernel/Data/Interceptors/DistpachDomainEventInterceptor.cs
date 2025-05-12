using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.DDD;

namespace SharedKernel.Data.Interceptors
{
    public class DistpachDomainEventInterceptor : SaveChangesInterceptor
    {
        private readonly IServiceProvider _serviceProvider;

        public DistpachDomainEventInterceptor(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            DispatchDomainEvents(eventData.Context).GetAwaiter().GetResult();
            return base.SavingChanges(eventData, result);
        }

        public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            await DispatchDomainEvents(eventData.Context);
            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private async Task DispatchDomainEvents(DbContext dbContext)
        {
            if (dbContext == null) { return; }


            //retrive entitties with domain events
            var aggregates = dbContext.ChangeTracker
                .Entries<IAggregate>()
                .Where(s => s.Entity.DomainEvents.Any())
                .Select(s => s.Entity);

           

            var domainEvents = aggregates.SelectMany(s => s.DomainEvents).ToList();

            //clear events in the entitities
            foreach (var item in aggregates)
            {
                item.ClearDomainEvents();
            }

            //dispatch using massTransit
            using var scope = _serviceProvider.CreateScope();
            var _publishEndpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();
            foreach (var domainEvent in domainEvents)
            {
                await _publishEndpoint.Publish(domainEvent);
            }
        }

    }
}
