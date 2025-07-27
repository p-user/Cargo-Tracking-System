using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tracking.Api.Features.CreateTrackingForOrder;

namespace Tracking.Api.Features.GetTrackingByOrderId
{
    public class GetTrackingByOrderIdEndpoint: ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/tracking/{orderId}", GetTracking)
             .WithName("Get Tracking")
             .Produces<GetTrackingByOrderIdQueryResponse>(StatusCodes.Status200OK)
             .Produces(StatusCodes.Status400BadRequest)
             .WithTags("Tracking");
        }

        private async Task<IResult> GetTracking([FromRoute] Guid orderId, ISender sender)
        {
            var command = new GetTrackingByOrderIdQuery(orderId);
            var response = await sender.Send(command);
            return Results.Ok(response);
        }
    }
}
