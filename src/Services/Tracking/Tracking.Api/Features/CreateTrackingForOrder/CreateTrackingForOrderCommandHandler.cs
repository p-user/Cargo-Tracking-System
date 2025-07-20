using Marten;
using Marten.Linq.Parsing.Operators;
using SharedKernel.CQRS;
using SharedKernel.Messaging.Events;
using Tracking.Api.Dtos;
using Tracking.Api.Models;

namespace Tracking.Api.Features.CreateTrackingForOrder
{

    public record CreateTrackingForOrderCommand(CreateTrackingForOrderDto dto) : ICommand<CreateTrackingForOrderCommandResponse>;
    public record CreateTrackingForOrderCommandResponse(bool trackingLogIsCreated);
    public class CreateTrackingForOrderCommandHandler (IDocumentSession _documentSession, TrackingDbContext _dbContext, IPublishEndpoint _publishEndpoint) : ICommandHandler<CreateTrackingForOrderCommand, CreateTrackingForOrderCommandResponse>
    {
        public async Task<CreateTrackingForOrderCommandResponse> Handle(CreateTrackingForOrderCommand request, CancellationToken cancellationToken)
        {
            var cargo = CargoTracking.Create(request.dto.OrderId, request.dto.OriginLocation, request.dto.Timestamp);

            // Commit Marten changes first
            _documentSession.Events.StartStream(cargo.Id, cargo.GetUncommittedEvents());
            await _documentSession.SaveChangesAsync(); 

            //// store integration event at outbox table
            //await _publishEndpoint.Publish(new CargoTrackingInitiatedIntegrationEvent(cargo.OrderId, cargo.TrackingId));
            //await _dbContext.SaveChangesAsync();

            return new CreateTrackingForOrderCommandResponse(true);
        }
    }
}
