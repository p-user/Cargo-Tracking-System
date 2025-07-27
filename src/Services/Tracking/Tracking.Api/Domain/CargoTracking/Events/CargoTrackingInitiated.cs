using SharedKernel.Core.DDD;

namespace Tracking.Api.Domain.CargoTracking.Events
{
    public record CargoTrackingInitiated(Guid  CargoTrackingId, Guid OrderId, string OriginLocation, DateTime Timestamp): IDomainEvent;
}
