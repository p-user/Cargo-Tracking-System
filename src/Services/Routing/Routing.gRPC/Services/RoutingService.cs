using SharedKernel.Exeptions;

namespace Routing.gRPC.Services
{
    public class RoutingService : RoutingProtoService.RoutingProtoServiceBase
    {
        private readonly ILogger<RoutingService> _logger;
        private readonly IRoutingApplicationService _routingservice;

        public RoutingService(ILogger<RoutingService> logger, IRoutingApplicationService routingservice)
        {
            _logger = logger;
            _routingservice=routingservice;
        }
        public override async Task<RouteResponse> GetRoute(RouteRequest request, ServerCallContext context)
        {

            return await _routingservice.GetRoute(request);
            
        }

        public override async Task<RouteResponse> CreateRoute(CreateRouteRequest request, ServerCallContext context)
        {

            return await _routingservice.CreateRoute(request);
        }


        public override async Task<RouteResponse> UpdateRoute(UpdateRouteRequest request, ServerCallContext context)
        {
            return await _routingservice.UpdateRoute(request);
        }

        public override async Task<RouteResponse> UpdateStatus(UpdateStatusRequest request, ServerCallContext context)
        {

            return await _routingservice.UpdateStatus(request);
        }

    }
}
