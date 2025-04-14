namespace Routing.gRPC.Models
{
    public class Route
    {
        public Guid Id { get; set; }
        public required string OrderId { get; set; } //reference to the order
        public List<Location> Waypoints { get; set; } = new();
        public TimeSpan? EstimatedTime { get; set; }
        public double? DistanceInKm { get; set; } //ToDo : integrate with google 
        public decimal? EstimatedCost { get; set; }
    }
}
