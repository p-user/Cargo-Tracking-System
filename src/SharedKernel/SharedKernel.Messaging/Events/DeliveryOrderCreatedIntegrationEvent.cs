
using SharedKernel.Core.DDD;

namespace SharedKernel.Messaging.Events
{
    public record DeliveryOrderCreatedIntegrationEvent : BaseIntegrationEvent
    {
        
        public Guid OrderId { get; set; }

        public string Origin { get; set; }
        public string Destination { get; set; }

        public DeliveryOrderCreatedIntegrationEvent(Guid orderId, string origin, string destination)
        {
            OrderId=orderId;
            Origin=origin;
            Destination=destination;
        }

        public DeliveryOrderCreatedIntegrationEvent() { }
    }
}
