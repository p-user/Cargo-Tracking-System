using Routing.gRPC.Enums;

namespace Routing.gRPC.Models
{
    public class Route
    {
        public Guid Id { get; set; }
        public required string OrderId { get; set; } //reference to the order
        public string Origin { get; set; } = default!;
        public string Destination { get; set; } = default!;
        public double? DistanceInKm { get; set; } //ToDo : integrate with google 
        public TimeSpan? EstimatedTime { get; set; }

        private List<Waypoint> _locations = new();
        public IReadOnlyList<Waypoint> Waypoints => _locations.AsReadOnly();
        public DateTime EstimatedDeliveryDate { get; set; }

        public RouteStatus Status { get; set; } = RouteStatus.Planned;

        private Route() { }
    }
}
