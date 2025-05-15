using SharedKernel.Data.DbContext;
using SharedKernel.Data.OutBox;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());



        }
    }

}
