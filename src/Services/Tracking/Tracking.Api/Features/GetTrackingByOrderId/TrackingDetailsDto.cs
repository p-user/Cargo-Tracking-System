namespace Tracking.Api.Features.GetTrackingByOrderId
{
    public record TrackingDetailsDto(
     Guid Id,
     string OrderId,
     string CurrentStatus,
     string OriginLocation
     
    );


}
