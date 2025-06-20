
using SharedKernel.Core.DDD;

namespace SharedKernel.Messaging.Events
{
    public record DeliveryOrderCreatedEvent : IDomainEvent
    {
        
        public Guid OrderId { get; set; }

        public string Origin { get; set; }
        public string Destination { get; set; }

        public DeliveryOrderCreatedEvent(Guid orderId, string origin, string destination)
        {
            OrderId=orderId;
            Origin=origin;
            Destination=destination;
        }

        public DeliveryOrderCreatedEvent() { }
    }
}
