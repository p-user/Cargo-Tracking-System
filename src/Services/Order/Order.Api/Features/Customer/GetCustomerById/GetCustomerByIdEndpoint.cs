
using Microsoft.AspNetCore.Mvc;

namespace Order.Api.Features.Customer.GetCustomerById
{
    public class GetCustomerByIdEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/order/customers/{id:guid}", GetCustomer)
             .WithName(RouteNames.GetCustomerById)
             .Produces<GetCustomerByIdQueryResponse>(StatusCodes.Status201Created)
             .Produces(StatusCodes.Status400BadRequest)
             .WithTags(TagNames.Customers);
        }

        private async Task<IResult> GetCustomer([FromRoute]Guid id, ISender sender)
        {
            var command = new GetCustomerByIdQuery(id);
            var response = await sender.Send(command);
            return Results.Ok(response.customer);
        }
    }
}
