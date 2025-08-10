using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace SharedKernel.Core.DDD
{
    public class DomainEventDispatcher<TDbContext> where TDbContext : DbContext
    {
        private readonly TDbContext _dbContext;
        private readonly IPublishEndpoint _publishEndpoint;

        public DomainEventDispatcher(TDbContext dbContext, IPublishEndpoint publishEndpoint)
        {
            _dbContext = dbContext;
            _publishEndpoint = publishEndpoint;
        }

        public async Task DispatchAndClearEventsAsync()
        {
            var aggregatesWithEvents = _dbContext.ChangeTracker
                .Entries<IAggregate>()
                .Where(entry => entry.Entity.DomainEvents.Any())
                .Select(entry => entry.Entity)
                .ToList();

            if (!aggregatesWithEvents.Any())
            {
                return;
            }

            var domainEvents = aggregatesWithEvents
                .SelectMany(agg => agg.DomainEvents)
                .ToList();

            foreach (var aggregate in aggregatesWithEvents)
            {
                aggregate.ClearDomainEvents();
            }

            foreach (var domainEvent in domainEvents)
            {
                await _publishEndpoint.Publish(domainEvent, CancellationToken.None);
            }
        }
    }
}
