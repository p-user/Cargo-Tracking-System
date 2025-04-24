using Order.Api.Enums;

namespace Order.Api.Dtos
{
    public record UpdateDeliveryStatusDto
    {
        public DeliveryStatus status { get; init; }
    }
}
