namespace Routing.gRPC.Dtos
{
    public record RouteDto
    {
        public Guid Id { get; init; }
        public string OrderId { get; init; }
        public string Origin { get; init; }
        public string Destination { get; init; }
        public double? DistanceInKm { get; init; }
        public TimeSpan? EstimatedTime { get; init; }
        public DateTime? EstimatedDeliveryDate { get; init; }
    }


    public record CreateRouteDto : RouteDto
    {

    }

    public record ViewRouteDto : RouteDto
    {

    }
}
