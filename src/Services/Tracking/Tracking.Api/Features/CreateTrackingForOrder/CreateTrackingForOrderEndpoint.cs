
using MediatR;

namespace Tracking.Api.Features.CreateTrackingForOrder
{
    public class CreateTrackingForOrderEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/tracking", CreateTracking)
             .WithName("Create Tracking")
             .Produces<CreateTrackingForOrderCommandResponse>(StatusCodes.Status201Created)
             .Produces(StatusCodes.Status400BadRequest)
             .WithTags("Tracking");
        }

        private async Task<IResult> CreateTracking(CreateTrackingForOrderDto dto, ISender sender) 
        {
            var command = new CreateTrackingForOrderCommand(dto);
            var response = await sender.Send(command);
            return Results.Ok(response);
        }
    }
}
