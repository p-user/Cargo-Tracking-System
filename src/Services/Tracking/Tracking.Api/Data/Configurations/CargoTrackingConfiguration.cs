using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tracking.Api.Models;

namespace Tracking.Api.Data.Configurations
{
    public class CargoTrackingConfiguration : IEntityTypeConfiguration<CargoTracking>
    {
        public void Configure(EntityTypeBuilder<CargoTracking> builder)
        {
            builder.OwnsMany(ct => ct.History, trackingEvent =>
            {
                trackingEvent.ToTable("TrackingEvents");

                trackingEvent.WithOwner().HasForeignKey("CargoTrackingId");

                trackingEvent.Property(te => te.Status)
                             .HasConversion<string>()
                             .IsRequired();

                trackingEvent.Property(te => te.Location)
                             .IsRequired();

                trackingEvent.Property(te => te.Timestamp)
                             .IsRequired();

                trackingEvent.Property(te => te.Remarks);

                trackingEvent.HasKey("CargoTrackingId", "Timestamp");
            });
        }
    }
}
