using Microsoft.EntityFrameworkCore.Diagnostics;
using SharedKernel.Data.DbContext;
using SharedKernel.Data.OutBox;
using SharedKernel.DDD;
using System.Text.Json;


namespace SharedKernel.Data.Interceptors
{
    public class DispatchDomainEventInterceptor<TContext> : SaveChangesInterceptor where TContext : Microsoft.EntityFrameworkCore.DbContext, IApplicationDbContext
    {
      
       

        public DispatchDomainEventInterceptor()
        {
            
        }

        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            DispatchDomainEvents(eventData.Context as TContext).GetAwaiter().GetResult();
            return base.SavingChanges(eventData, result);
        }

        public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            await DispatchDomainEvents(eventData.Context as TContext);
            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private async Task DispatchDomainEvents(TContext? dbContext)
        {
            if (dbContext == null) return;

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

            foreach(var domainEvent in domainEvents)
            {
                var outboxMessage = new OutboxMessage
                {
                    Id = Guid.NewGuid(),
                    Type =domainEvent.GetType().AssemblyQualifiedName!,
                    Content = JsonSerializer.Serialize(domainEvent, domainEvent.GetType()),
                    OccuredOn = DateTime.UtcNow
                };

                await dbContext.OutboxMessages.AddAsync(outboxMessage);
            }
        }
    }
}
