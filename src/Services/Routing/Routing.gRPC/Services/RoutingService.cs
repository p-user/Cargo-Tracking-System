using Grpc.Core;
using Routing.gRPC.Data;
using Routing.gRPC.Protos;

namespace Routing.gRPC.Services
{
    public class RoutingService(ILogger<RoutingService> _logger, RoutingDbContext _context) : RoutingProtoService.RoutingProtoServiceBase
    {
        public override async Task<RouteResponse> GetRoute(RouteRequest request, ServerCallContext context)
        {
            var id = new Guid(request.RouteId);
            var route = await _context.Routes.FindAsync(id);
            return MapToRouteResponse(route);
        }

        public override async Task<RouteResponse> CreateRoute(CreateRouteRequest request, ServerCallContext context)
        {
            var route = Models.Route.Create(
                request.OrderId,
                request.Origin,
                request.Destination,
                request.DistanceInKm,
                TimeSpan.Parse(request.EstimatedTime),
                DateTime.Parse(request.EstimatedDeliveryDate)
            );

            //add waypoints

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

            response.Waypoints.AddRange(route.Waypoints.Select(w => new Waypoint
            {
                Id = w.Id.ToString(),
                RouteId = route.Id.ToString(),
                LocationName = w.LocationName,
                Latitude = w.Latitude,
                Longitude = w.Longitude,
                Sequence = w.Sequence,
                Status = (WaypointStatus)w.Status
            }));

            return response;
        }

    }

}
