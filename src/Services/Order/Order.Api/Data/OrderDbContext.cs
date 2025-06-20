using SharedKernel.Core.Data.DbContext;
using SharedKernel.Core.DefaultEntities;
using System.Reflection;

namespace Order.Api.Data
{
    public class OrderDbContext : DbContext, IApplicationDbContext
    {
        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
        {
        }
        public virtual DbSet<DeliveryOrder> DeliveryOrders { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<OutboxMessage> OutboxMessages { get; set ; }
        public virtual DbSet<InboxMessage> InboxMessages { get; set ; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());



        }
    }

}
