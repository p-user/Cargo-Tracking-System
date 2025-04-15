using Routing.gRPC.Data;
using Routing.gRPC.Enums;
using Routing.gRPC.Models;
//using Routing.gRPC.Protos;

namespace Routing.gRPC.Services
{
    public class RoutingService(ILogger<RoutingService> _logger, RoutingDbContext _context) //: RoutingProtoService.RoutingProtoServiceBase
    {
        //public override async Task<RouteResponse> GetRoute(RouteRequest request, ServerCallContext context)
        //{
        //    var route = await _context.Routes.FirstOrDefaultAsync(Guid.Parse(request.RouteId));
        //    return MapToRouteResponse(route);
        //}

        //public override async Task<RouteResponse> CreateRoute(CreateRouteRequest request, ServerCallContext context)
        //{
        //    var route = new Route(
        //        Guid.NewGuid(),
        //        request.OrderId,
        //        request.Origin,
        //        request.Destination,
        //        request.DistanceInKm,
        //        TimeSpan.Parse(request.EstimatedTime),
        //        DateTime.Parse(request.EstimatedDeliveryDate),
        //        request.Waypoints.Select(w => new Waypoint(
        //            Guid.NewGuid(),
        //            w.LocationName,
        //            w.Latitude,
        //            w.Longitude,
        //            w.Sequence,
        //            DateTime.Parse(w.EstimatedArrival),
        //            DateTime.Parse(w.EstimatedDeparture),
        //            (Enums.WaypointStatus)w.Status
        //        )).ToList()
        //    );
        //    await _context.Routes.AddAsync(route);
        //    await _context.SaveChangesAsync();
        //    return MapToRouteResponse(route);
        //}

        //private RouteResponse MapToRouteResponse(Route route)
        //{
        //    var response = new RouteResponse
        //    {
        //        RouteId = route.Id.ToString(),
        //        OrderId = route.OrderId,
        //        Origin = route.Origin,
        //        Destination = route.Destination,
        //        DistanceInKm = route.DistanceInKm ?? 0,
        //        EstimatedTime = route.EstimatedTime?.ToString() ?? "",
        //        EstimatedDeliveryDate = route.EstimatedDeliveryDate.ToString("o"),
        //        Status = (RouteStatus)route.Status
        //    };
        //    response.Waypoints.AddRange(route.Waypoints.Select(w => new Waypoint
        //    {
        //        Id = w.Id.ToString(),
        //        RouteId = route.Id.ToString(),
        //        LocationName = w.LocationName,
        //        Latitude = w.Latitude,
        //        Longitude = w.Longitude,
        //        Sequence = w.Sequence,
        //        EstimatedArrival = w.EstimatedArrival?.ToString("o") ?? string.Empty,
        //        EstimatedDeparture = w.EstimatedDeparture?.ToString("o") ?? string.Empty,
        //        Status = (WaypointStatus)w.Status
        //    }));
        //    return response;
        //}

    }

}
