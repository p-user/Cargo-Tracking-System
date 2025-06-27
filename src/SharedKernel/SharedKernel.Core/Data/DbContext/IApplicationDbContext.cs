using Microsoft.EntityFrameworkCore;



namespace SharedKernel.Core.Data.DbContext
{
    public interface IApplicationDbContext
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
