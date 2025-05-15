using Microsoft.EntityFrameworkCore;
using SharedKernel.Data.OutBox;


namespace SharedKernel.Data.DbContext
{
    public interface IApplicationDbContext
    {
        DbSet<OutboxMessage> OutboxMessages { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
