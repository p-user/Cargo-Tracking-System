using Order.Api.Enums;

namespace Order.Api.Models
{
    public class DeliveryOrder // inherit from aggregate + id
    {
        public string SenderName { get; set; }
        public string SenderContact { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverContact { get; set; }

        public string PickupAddress { get; set; }
        public string DeliveryAddress { get; set; }

        public CargoDetails Cargo { get; set; }

        public DeliveryStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DeliveredAt { get; set; }

        public void UpdateStatus(DeliveryStatus newStatus)
        {
            Status = newStatus;

            if (newStatus == DeliveryStatus.Delivered)
            {
                DeliveredAt = DateTime.UtcNow;
            }
        }
    }
}
