using Microsoft.EntityFrameworkCore;

namespace SharedKernel.Core.Data
{
   
    public class EfUnitOfWork<TDbContext>(TDbContext dbContext) : IUnitOfWork     where TDbContext : DbContext
    {
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return dbContext.SaveChangesAsync(cancellationToken);
        }
    }
    
}
