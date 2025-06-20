using Microsoft.EntityFrameworkCore;
using SharedKernel.Core.DefaultEntities;



namespace SharedKernel.Core.Data.DbContext
{
    public interface IApplicationDbContext
    {
        DbSet<OutboxMessage> OutboxMessages { get; set; }
        DbSet<InboxMessage> InboxMessages { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
