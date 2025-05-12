
namespace Routing.gRPC.Services
{
    public interface IRoutingApplicationService
    {
        Task<RouteResponse> CreateRoute(CreateRouteRequest request);
        Task<RouteResponse> GetRoute(RouteRequest request);
        Task<RouteResponse> UpdateRoute(UpdateRouteRequest request);
        Task<RouteResponse> UpdateStatus(UpdateStatusRequest request);
    }
}