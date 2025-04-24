using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Order.Api.Data.Configurations
{
    public class CustomerConfigurations : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.OwnsOne(d => d.Address, address =>
            {
                address.Property(c => c.Street).HasColumnName("AddressStreet");
                address.Property(c => c.ZipCode).HasColumnName("AddressZipCode");
                address.Property(c => c.City).HasColumnName("AddressCity");
                address.Property(c => c.Country).HasColumnName("AddressCountry");

            });
        }
    }
}
