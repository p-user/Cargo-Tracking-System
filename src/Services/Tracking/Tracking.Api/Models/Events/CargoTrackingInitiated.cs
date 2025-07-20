using SharedKernel.Core.DDD;

namespace Tracking.Api.Models.Events
{
    public record CargoTrackingInitiated(Guid CargoId, string TrackingId, string OriginLocation, DateTime Timestamp): IDomainEvent;
}
