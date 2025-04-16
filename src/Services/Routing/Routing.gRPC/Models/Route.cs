using Routing.gRPC.Enums;

namespace Routing.gRPC.Models
{
    public class Route
    {
        public Guid Id { get; private set; }
        public string OrderId { get; private set; } //reference to the order
        public string Origin { get; private set; } = default!;
        public string Destination { get; private set; } = default!;
        public double? DistanceInKm { get; private set; } //ToDo : integrate with google 
        public TimeSpan? EstimatedTime { get; private set; }

        private List<Waypoint> _locations = new();
        public IReadOnlyList<Waypoint> Waypoints => _locations.AsReadOnly();
        public DateTime? EstimatedDeliveryDate { get; private set; }

        public RouteStatus Status { get; private set; } = RouteStatus.Planned;

        private Route() { }

        private Route(string orderId, string origin, string destination, double? distanceInKm, TimeSpan? estimatedTime, DateTime estimatedDeliveryDate)
        {
            Id = Guid.NewGuid();
            OrderId = orderId;
            Origin = origin;
            Destination = destination;
            DistanceInKm = distanceInKm;
            EstimatedTime = estimatedTime;
            EstimatedDeliveryDate = estimatedDeliveryDate;
            Status = RouteStatus.Planned;
        }

        public static Route Create(string orderId, string origin, string destination, double? distanceInKm, TimeSpan? estimatedTime, DateTime estimatedDeliveryDate)
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

            return new Route(orderId, origin, destination, distanceInKm, estimatedTime, estimatedDeliveryDate);
        }
    }
}
