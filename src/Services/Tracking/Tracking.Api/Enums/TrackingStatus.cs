using System.ComponentModel;

namespace Tracking.Api.Enums
{
    public enum TrackingStatus
    {
        [Description("Pending")]
        Pending = 1,

        [Description("At Origin Facility")]
            AtOriginFacility = 2,
    
        [Description("In Transit")]
            InTransit = 3,
    
        [Description("At Destination Hub")]
            AtDestinationHub = 4,
    
        [Description("Out for Delivery")]
            OutForDelivery = 5,
    
        [Description("Delivered")]
            Delivered = 6,
    
        [Description("Failed Delivery Attempt")]
            FailedAttempt = 7,
    
        [Description("Lost")]
            Lost = 8,
    }
}
