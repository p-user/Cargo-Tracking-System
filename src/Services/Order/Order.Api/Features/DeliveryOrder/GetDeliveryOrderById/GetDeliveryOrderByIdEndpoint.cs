
using Microsoft.AspNetCore.Mvc;

namespace Order.Api.Features.DeliveryOrder.GetDeliveryOrderById
{
    public class GetDeliveryOrderByIdEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/order/delivery-orders/{id:guid}", GetDeliveryOrderById)
                .WithName(RouteNames.GetDeliveryOrderById)
                .Produces<GetDeliveryOrderByIdResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .WithTags(TagNames.DeliveryOrders);
        }

        private async Task<IResult> GetDeliveryOrderById([FromRoute] Guid id, ISender sender)
        {
            var query = new GetDeliveryOrderByIdQuery(id);
            var response = await sender.Send(query);
            return Results.Ok(response.dto);
        }
    }
}
