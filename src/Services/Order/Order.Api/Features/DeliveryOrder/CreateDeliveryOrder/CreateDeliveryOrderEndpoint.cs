
namespace Order.Api.Features.DeliveryOrder.CreateDeliveryOrder
{
    public class CreateDeliveryOrderEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/order/delivery-orders", CreateDeliveryOrder)
             .WithName(RouteNames.CreateDeliveryOrder)
             .Produces<CreateDeliveryOrderCommandResponse>(StatusCodes.Status201Created)
             .Produces(StatusCodes.Status400BadRequest)
             .WithTags(TagNames.DeliveryOrders);
        }


        private async Task<IResult> CreateDeliveryOrder(CreateDeliveryOrderDto dto, ISender sender)
        {
            var command = new CreateDeliveryOrderCommand(dto);
            var response = await sender.Send(command);
            return Results.CreatedAtRoute(RouteNames.GetDeliveryOrderById, new { id = response.Id }, response);
        }
    }
}
