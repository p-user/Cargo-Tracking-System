using Microsoft.AspNetCore.Mvc;
using Order.Api.Features.Customer.GetCustomerById;

namespace Order.Api.Features.Customer.GetCustomerByEmail
{
    public class GetCustomerByEmailEndpoint : ICarterModule
    {

        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/order/customers", GetCustomerByEmail)
             .WithName(RouteNames.GetCustomerByEmail)
             .Produces<GetCustomerByEmailQueryResponse>(StatusCodes.Status200OK)
             .Produces(StatusCodes.Status400BadRequest)
             .WithTags(TagNames.Customers);
        }

        private async Task<IResult> GetCustomerByEmail([FromQuery] string email, ISender sender)
        {
            var command = new GetCustomerByEmailQuery(email);
            var response = await sender.Send(command);
            return Results.Ok(response);
        }
    }
}
