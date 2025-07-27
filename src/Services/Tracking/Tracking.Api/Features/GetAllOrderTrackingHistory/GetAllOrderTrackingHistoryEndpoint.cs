using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tracking.Api.Features.GetTrackingByOrderId;

namespace Tracking.Api.Features.GetAllOrderTrackingHistory
{
    public class GetAllOrderTrackingHistoryEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/tracking/tracking-history/{orderId}", GetTrackingHistory)
             .WithName("Get Tracking History For Order")
             .Produces<GetAllOrderTrackingHistoryQueryResponse>(StatusCodes.Status200OK)
             .Produces(StatusCodes.Status400BadRequest)
             .WithTags("Tracking");
        }

        private async Task<IResult> GetTrackingHistory([FromRoute] Guid orderId, ISender sender)
        {
            var command = new GetAllOrderTrackingHistoryQuery(orderId);
            var response = await sender.Send(command);
            return Results.Ok(response);
        }
    }

}
