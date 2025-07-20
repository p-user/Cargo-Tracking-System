using SharedKernel.Core.DDD;

namespace Tracking.Api.Models.Events
{
    public record CargoStatusUpdated(Guid CargoId, string TrackingId, string NewStatus, string NewLocation, DateTime Timestamp, string Remarks): IDomainEvent;

}
