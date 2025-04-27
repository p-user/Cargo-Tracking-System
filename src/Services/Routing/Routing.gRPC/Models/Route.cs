using Routing.gRPC.Enums;
using SharedKernel.DDD;

namespace Routing.gRPC.Models
{
    public class Route : Aggregate<Guid>
    {
        public string OrderId { get; private set; } //reference to the order
        public string Origin { get; private set; } = default!;
        public string Destination { get; private set; } = default!;
        public double? DistanceInKm { get; private set; } //ToDo : integrate with google 
        public TimeSpan? EstimatedTime { get; private set; }

        public DateTime? EstimatedDeliveryDate { get; private set; }

        public Enums.RouteStatus Status { get; private set; } = Enums.RouteStatus.Planned;

        private Route() { }

        private Route(string orderId, string origin, string destination, double? distanceInKm, TimeSpan? estimatedTime)
        {
            Id = Guid.NewGuid();
            OrderId = orderId;
            Origin = origin;
            Destination = destination;
            DistanceInKm = distanceInKm;
            EstimatedTime = estimatedTime;
            EstimatedDeliveryDate = CalculateEstimatedDeliveryTime(DateTime.Now, EstimatedTime);
            Status = Enums.RouteStatus.Planned;
        }

        public static Route Create(string orderId, string origin, string destination, double? distanceInKm, TimeSpan? estimatedTime)
        {
            if (string.IsNullOrWhiteSpace(orderId))
            {
                throw new ArgumentException("Order ID cannot be empty.");
            }
            if (string.IsNullOrWhiteSpace(origin))
            {
                throw new ArgumentException("Origin cannot be empty.");
            }
            if (string.IsNullOrWhiteSpace(destination))
            {
                throw new ArgumentException("Destination cannot be empty.");
            }

            return new Route(orderId, origin, destination, distanceInKm, estimatedTime);
        }


        public static DateTime CalculateEstimatedDeliveryTime(DateTime startTime, TimeSpan? estimatedDuration)
        {  
            if (estimatedDuration.HasValue)
            {
                return startTime.Add(estimatedDuration.Value);
            }

            else
            {
                return startTime.AddDays(7);
            }
        }
    }
}
