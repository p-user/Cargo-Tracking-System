namespace Tracking.Api.Features.GetAllOrderTrackingHistory
{
    public record TrackingHistoryItemDto(
     string Status,
     string Location,
     DateTime Timestamp,
     string? Remarks
 );
}
