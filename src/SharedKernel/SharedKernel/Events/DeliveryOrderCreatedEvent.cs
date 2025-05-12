using SharedKernel.DDD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedKernel.Events
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
