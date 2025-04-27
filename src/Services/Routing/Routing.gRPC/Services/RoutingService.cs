namespace Routing.gRPC.Services
{
    public class RoutingService : RoutingProtoService.RoutingProtoServiceBase
    {
        private readonly ILogger<RoutingService> _logger;
        private readonly RoutingDbContext _context;
        private readonly GoogleMapsService _googleMapsService;

        public RoutingService(ILogger<RoutingService> logger, RoutingDbContext context, GoogleMapsService googleMapsService)
        {
            _logger = logger;
            _context = context;
            _googleMapsService=googleMapsService;
        }
        public override async Task<RouteResponse> GetRoute(RouteRequest request, ServerCallContext context)
        {
            var id = new Guid(request.RouteId);
            var route = await _context.Routes.FindAsync(id);
            return MapToRouteResponse(route);
        }

        public override async Task<RouteResponse> CreateRoute(CreateRouteRequest request, ServerCallContext context)
        {

            var (distanceInKm, estimatedTime) = await _googleMapsService.GetRouteInfoAsync(request.Origin, request.Destination, CancellationToken.None);


            var route = Models.Route.Create(
                request.OrderId,
                request.Origin,
                request.Destination,
                distanceInKm,
                estimatedTime
            );

            await _context.Routes.AddAsync(route);
            await _context.SaveChangesAsync();

            return MapToRouteResponse(route);
        }

        private RouteResponse MapToRouteResponse(Models.Route route)
        {
            var response = new RouteResponse
            {
                RouteId = route.Id.ToString(),
                OrderId = route.OrderId,
                Origin = route.Origin,
                Destination = route.Destination,
                DistanceInKm = route.DistanceInKm ?? 0,
                EstimatedTime = route.EstimatedTime?.ToString() ?? "",
                EstimatedDeliveryDate = route.EstimatedDeliveryDate?.ToString() ?? "",
                Status = (RouteStatus)route.Status
            };


            return response;
        }

    }
}
