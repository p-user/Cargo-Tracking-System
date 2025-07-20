using SharedKernel.Core.DDD;

namespace Tracking.Api.Models.Events
{
    public record CargoDelivered(Guid CargoId, string TrackingId, string FinalLocation, DateTime Timestamp): IDomainEvent; 

}
