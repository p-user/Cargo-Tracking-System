
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tracking.Api.Models;

namespace Tracking.Api.Data.Configurations
{
    public class CargoTrackingConfiguration : IEntityTypeConfiguration<CargoTracking>
    {
       
        public void Configure(EntityTypeBuilder<CargoTracking> builder)
        {
           
        }
    }
}
