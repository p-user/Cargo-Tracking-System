using Elastic.CommonSchema;
using Tracking.Api.Enums;

namespace Tracking.Api.Models.ValueObjects
{
    /// <summary>
    /// Represents a single tracking event in a order delivery's history
    /// </summary>
    /// <param name="Status"></param>
    /// <param name="Location"></param>
    /// <param name="Timestamp"></param>
    /// <param name="Remarks"></param>
    public record TrackingEvent(TrackingStatus Status,string Location,DateTime Timestamp, string? Remarks);
}
