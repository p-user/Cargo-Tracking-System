
using SharedKernel.Core.DDD;

namespace Tracking.Api.Domain.CargoTracking.Events
{
    public record CargoDelivered(Guid OrderId, string CargoTrackingId, string FinalLocation, DateTime Timestamp): IDomainEvent; 

}
