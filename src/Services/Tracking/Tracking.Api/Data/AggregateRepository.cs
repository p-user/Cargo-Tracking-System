using Marten;
using SharedKernel.Core.DDD;

namespace Tracking.Api.Data
{
    public sealed class AggregateRepository<TAggregate> where TAggregate : Aggregate<Guid>
    {
        private readonly IDocumentSession _documentSession;
        private readonly TrackingDbContext _dbContext;
        private readonly IPublishEndpoint _publishEndpoint;

        public AggregateRepository(IDocumentSession documentSession, TrackingDbContext dbContext, IPublishEndpoint publishEndpoint)
        {
            _documentSession = documentSession;
            _dbContext = dbContext;
            _publishEndpoint=publishEndpoint;
        }

        public async Task SaveAsync(TAggregate aggregate, CancellationToken ct = default)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync(ct);

            try
            {
                // Save domain events to Marten
                var events = aggregate.DomainEvents;
                _documentSession.Events.Append(aggregate.Id, events);

                await _documentSession.SaveChangesAsync(ct);

                // publish to outbox table using  EF Core 
                foreach (var domainEvent in events)
                {

                    await _publishEndpoint.Publish((object)domainEvent, ct);
                }


                await _dbContext.SaveChangesAsync(ct);
                

                await transaction.CommitAsync(ct);

                // Clear aggregate's uncommitted events after success
                aggregate.ClearDomainEvents();
            }
            catch
            {
                await transaction.RollbackAsync(ct);
                throw;
            }
        }

        public async Task<TAggregate> LoadAsync(Guid id, CancellationToken ct = default)
        {
            var aggregate = await _documentSession.Events.AggregateStreamAsync<TAggregate>(id, token: ct);
            return aggregate ?? throw new InvalidOperationException($"No aggregate found with ID {id}");
        }
    }

}
