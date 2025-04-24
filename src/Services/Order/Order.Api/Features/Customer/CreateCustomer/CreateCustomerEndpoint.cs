
using Order.Api.Features.DeliveryOrder.CreateDeliveryOrder;

namespace Order.Api.Features.Customer.CreateCustomer
{
    public class CreateCustomerEndpoint : ICarterModule
    {
        
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/order/customers", CreateCustomer)
             .WithName(RouteNames.CreateCustomer)
             .Produces<CreateCustomerCommandResponse>(StatusCodes.Status201Created)
             .Produces(StatusCodes.Status400BadRequest)
             .WithTags(TagNames.DeliveryOrders);
        }


        private async Task<IResult> CreateCustomer(CreateCustomerDto dto, ISender sender)
        {
            var command = new CreateCustomerCommand(dto);
            var response = await sender.Send(command);
            return Results.CreatedAtRoute(RouteNames.GetCustomerById, new { id = response.customerId }, response);
        }
    }
}
