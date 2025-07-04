﻿using Routing.gRPC.Protos;
using SharedKernel.Core.Exeptions;

namespace Routing.gRPC.Services
{
    public class RoutingApplicationService : IRoutingApplicationService
        {
            private readonly ILogger<RoutingService> _logger;
            private readonly RoutingDbContext _context;
            private readonly GoogleMapsService _googleMapsService;

            public RoutingApplicationService(ILogger<RoutingService> logger, RoutingDbContext context, GoogleMapsService googleMapsService)
            {
                _logger = logger;
                _context = context;
                _googleMapsService=googleMapsService;
            }
        public async Task<RouteResponse> GetRoute(RouteRequest request)
        {
            var id = new Guid(request.RouteId);
            var route = await _context.Routes.FindAsync(id);
            return MapToRouteResponse(route);
        }

        public async Task<RouteResponse> CreateRoute(CreateRouteRequest request)
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


        public async Task<RouteResponse> UpdateRoute(UpdateRouteRequest request)
        {
            var id = new Guid(request.RouteId);
            var route = await _context.Routes.FindAsync(id, CancellationToken.None);

            if (route == null)
            {
                throw new NotFoundException(nameof(route), request.RouteId);
            }

            var (distanceInKm, estimatedTime) = await _googleMapsService.GetRouteInfoAsync(request.Origin, request.Destination, CancellationToken.None);


            route.UpdateRoute(
                request.Origin,
                request.Destination,
                distanceInKm,
                estimatedTime
            );

            _context.Routes.Update(route);
            await _context.SaveChangesAsync();

            return MapToRouteResponse(route);
        }

        public async Task<RouteResponse> UpdateStatus(UpdateStatusRequest request)
        {

            var id = new Guid(request.RouteId);
            var route = await _context.Routes.FindAsync(id, CancellationToken.None);

            if (route == null)
            {
                throw new NotFoundException(nameof(route), request.RouteId);
            }





            route.UpdateStatus((Enums.RouteStatus)request.Status);

            _context.Routes.Update(route);
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
                Status = (Protos.RouteStatus)route.Status
            };


            return response;
        }

    }
 }

