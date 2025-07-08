using SharedKernel.CQRS;
using Tracking.Api.Dtos;

namespace Tracking.Api.Features.CreateTrackingForOrder
{

    public record CreateTrackingForOrderCommand(CreateTrackingForOrderDto dto) : ICommand<CreateTrackingForOrderCommandResponse>;
    public record CreateTrackingForOrderCommandResponse(bool trackingLogIsCreated);
    public class CreateTrackingForOrderCommandHandler : ICommandHandler<CreateTrackingForOrderCommand, CreateTrackingForOrderCommandResponse>
    {
        public Task<CreateTrackingForOrderCommandResponse> Handle(CreateTrackingForOrderCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
