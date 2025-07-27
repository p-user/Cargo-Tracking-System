using SharedKernel.Core.DDD;

namespace Tracking.Api.Domain.CargoTracking.Events
{
    public record CargoStatusUpdated(Guid OrderId, Guid CargoTrackingId, string NewStatus, string NewLocation, DateTime Timestamp, string Remarks): IDomainEvent;

}
