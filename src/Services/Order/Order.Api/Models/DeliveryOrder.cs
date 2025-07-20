
namespace Order.Api.Models
{
    public class DeliveryOrder : Aggregate<Guid>
    {
        public Guid CustomerId { get; private set; }
        public Customer Customer { get; private set; }
        public string ReceiverName { get; private set; }
        public string ReceiverContact { get; private set; }

        public string PickupAddress { get; private set; }
        public string DeliveryAddress { get; private set; }

        public CargoDetails Cargo { get; private set; }

        public DeliveryStatus Status { get; private set; }
        public DateTime? DeliveredAt { get; private set; }

        public Guid? TrackingId { get; private set; }

        private DeliveryOrder()
        {

        }

        public void UpdateStatus(DeliveryStatus newStatus)
        {
            Status = newStatus;

            if (newStatus == DeliveryStatus.Delivered)
            {
                DeliveredAt = DateTime.UtcNow;
            }
        }


        public static DeliveryOrder Create(
         Customer customer,
         string receiverName,
         string receiverContact,
         string pickupAddress,
         string deliveryAddress,
         CargoDetails cargo)
        {

           
            if (cargo == null) throw new ArgumentNullException(nameof(cargo));

            var deliveryOrder =  new DeliveryOrder
            {
                Id = Guid.NewGuid(),
                CustomerId = customer.Id,
                ReceiverName = receiverName,
                ReceiverContact = receiverContact,
                PickupAddress = pickupAddress,
                DeliveryAddress = deliveryAddress,
                Cargo = cargo,
                Status = DeliveryStatus.Created,
                DeliveredAt = null,
            };

            deliveryOrder.AddDomainEvent(new DeliveryOrderCreatedIntegrationEvent(deliveryOrder.Id, deliveryOrder.PickupAddress, deliveryOrder.DeliveryAddress));

            return deliveryOrder;
        }


        //ToDo: set tracking id from external service




        public void SetStatus(DeliveryStatus status)
        {
            if (status == DeliveryStatus.Cancelled && Status == DeliveryStatus.Delivered)
            {
                throw new InvalidOperationException("Cannot cancel a delivered order.");
            }
            if (status == DeliveryStatus.Dispatched && Status != DeliveryStatus.Created)
            {
                throw new InvalidOperationException("Order must be created to be dispatched.");
            }
            if (status == DeliveryStatus.Delivered && Status != DeliveryStatus.InTransit)
            {
                throw new InvalidOperationException("Order must be in transit to be delivered.");
            }
            if (status == DeliveryStatus.InTransit && Status != DeliveryStatus.PickedUp)
            {
                throw new InvalidOperationException("Order must be picked up to be in transit.");
            }
            Status = status;
        }
    }
}
