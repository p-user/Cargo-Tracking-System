
using Microsoft.EntityFrameworkCore;
using SharedKernel.Core.Data;

namespace SharedKernel.Core.DDD
{
    public class OutboxUnitOfWorkDecorator<TDbContext>(IUnitOfWork decorated,  DomainEventDispatcher<TDbContext> domainEventDispatcher) : IUnitOfWork
        where TDbContext : DbContext
    {
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            //Dispatch events
            await domainEventDispatcher.DispatchAndClearEventsAsync();

            //Call the inner SaveChanges, which saves entities AND the outbox messages in one transaction
            return await decorated.SaveChangesAsync(cancellationToken);
        }
    }
   
}
