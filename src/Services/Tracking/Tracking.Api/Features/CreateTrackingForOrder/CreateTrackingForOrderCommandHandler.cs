
using SharedKernel.CQRS;
using Tracking.Api.Domain.CargoTracking;

namespace Tracking.Api.Features.CreateTrackingForOrder
{

    public record CreateTrackingForOrderCommand(CreateTrackingForOrderDto dto) : ICommand<CreateTrackingForOrderCommandResponse>;
    public record CreateTrackingForOrderCommandResponse(bool trackingLogIsCreated);
    public class CreateTrackingForOrderCommandHandler(AggregateRepository<CargoTracking> _repository) : ICommandHandler<CreateTrackingForOrderCommand, CreateTrackingForOrderCommandResponse>
    {
        public async Task<CreateTrackingForOrderCommandResponse> Handle(CreateTrackingForOrderCommand request, CancellationToken cancellationToken)
        {
            try
            {
               
                var cargo = CargoTracking.Create(request.dto.OrderId, request.dto.OriginLocation, request.dto.Timestamp);

                await _repository.SaveAsync(cargo,cancellationToken);
                return new CreateTrackingForOrderCommandResponse(true);
            }
            catch (Exception)
            {
               
                throw; 
            }
        }
    }
    
}
