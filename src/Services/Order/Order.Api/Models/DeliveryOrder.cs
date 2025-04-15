using Order.Api.Enums;
using SharedKernel.DDD;

namespace Order.Api.Models
{
    public class DeliveryOrder : Aggregate<Guid>
    {
        public string SenderName { get; private set; }
        public string SenderContact { get; private set; }
        public string ReceiverName { get; private set; }
        public string ReceiverContact { get; private set; }

        public string PickupAddress { get; private set; }
        public string DeliveryAddress { get; private set; }

        public CargoDetails Cargo { get; private set; }

        public DeliveryStatus Status { get; private set; }
        public DateTime? DeliveredAt { get; private set; }

        public Guid? TrackingId { get; private set; }

        public DeliveryOrder()
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
         string senderName,
         string senderContact,
         string receiverName,
         string receiverContact,
         string pickupAddress,
         string deliveryAddress,
         CargoDetails cargo)
        {

            if (string.IsNullOrWhiteSpace(senderName)) throw new ArgumentException("Sender name is required.");
            if (cargo == null) throw new ArgumentNullException(nameof(cargo));

            return new DeliveryOrder
            {
                Id = Guid.NewGuid(),
                SenderName = senderName,
                SenderContact = senderContact,
                ReceiverName = receiverName,
                ReceiverContact = receiverContact,
                PickupAddress = pickupAddress,
                DeliveryAddress = deliveryAddress,
                Cargo = cargo,
                Status = DeliveryStatus.Created,
                DeliveredAt = null,
            };
        }


        //ToDo: set tracking id from external service


    }
}
