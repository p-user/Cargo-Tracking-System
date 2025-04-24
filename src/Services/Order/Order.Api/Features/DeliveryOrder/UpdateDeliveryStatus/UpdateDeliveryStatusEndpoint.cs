using Microsoft.AspNetCore.Mvc;
using Order.Api.Enums;

namespace Order.Api.Features.DeliveryOrder.UpdateDeliveryStatus
{
    public class UpdateDeliveryStatusEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/order/delivery-orders/{id:guid}/status", UpdateDeliveryStatus)
                .WithName(RouteNames.UpdateDeliveryOrderStatus)
                .Produces<UpdateDeliveryStatusCommandResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status404NotFound)
                .WithTags(TagNames.DeliveryOrders);
        }

        private async Task<IResult> UpdateDeliveryStatus([FromRoute] Guid id, [FromBody] UpdateDeliveryStatusDto statusDto, ISender sender)
        {
            var command = new UpdateDeliveryStatusCommand(id, statusDto.status);
            var response = await sender.Send(command);
            return Results.Ok(response);
        }
    }
}
