

namespace SharedKernel.Messaging.Events
{
    public record CargoStatusUpdatedIntegrationEvent(Guid CargoId, string TrackingId, string NewStatus, string CurrentLocation) : BaseIntegrationEvent;
    
}
