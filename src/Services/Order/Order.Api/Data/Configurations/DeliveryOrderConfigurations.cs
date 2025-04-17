using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Order.Api.Data.Configurations
{
    public class DeliveryOrderConfigurations : IEntityTypeConfiguration<DeliveryOrder>
    {
        public void Configure(EntityTypeBuilder<DeliveryOrder> builder)
        {
            builder.OwnsOne(d => d.Cargo, cargo =>
            {
                cargo.Property(c => c.Description).HasColumnName("CargoDescription");
                cargo.Property(c => c.WeightKg).HasColumnName("CargoWeightKg");

            });
        }
    }
}
