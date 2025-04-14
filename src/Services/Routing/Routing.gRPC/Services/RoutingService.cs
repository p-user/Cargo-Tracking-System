using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Routing.gRPC.Data;
using Routing.gRPC.Models;
using Routing.gRPC.Protos;

namespace Routing.gRPC.Services
{
    public class RoutingService(ILogger<RoutingService> _logger, RoutingDbContext _context) : RoutingProtoService.RoutingProtoServiceBase
    {
        public override Task<RouteResponse> CreateRoute(CreateRouteRequest request, ServerCallContext context)
        {
            return base.CreateRoute(request, context);
        }

        public override Task<DeleteRouteResponse> DeleteRoute(DeleteRouteRequest request, ServerCallContext context)
        {
            return base.DeleteRoute(request, context);
        }

        public override async Task<RouteResponse> GetRoute(RouteRequest request, ServerCallContext context)
        {
            var route = await _context.Routes.FirstOrDefaultAsync(s => s.OrderId == request.OrderId);

            var routeResposnse = new RouteResponse
            {
                OrderId = route.OrderId,

            };
            routeResposnse.Waypoints.AddRange((IEnumerable<Waypoint>)route.Waypoints.Select(w => new Location
            {
                Latitude = w.Latitude,
                Longitude = w.Longitude,

            }));

            return routeResposnse;

        }

        public override Task<RouteResponse> UpdateRoute(UpdateRouteRequest request, ServerCallContext context)
        {
            return base.UpdateRoute(request, context);
        }

    }

}
