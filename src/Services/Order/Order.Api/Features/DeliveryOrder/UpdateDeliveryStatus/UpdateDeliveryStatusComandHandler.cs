using Order.Api.Enums;

namespace Order.Api.Features.DeliveryOrder.UpdateDeliveryStatus
{
    public record UpdateDeliveryStatusCommand(Guid Id, DeliveryStatus status) : ICommand<UpdateDeliveryStatusCommandResponse>;
    public record UpdateDeliveryStatusCommandResponse(bool IsSuccess);
    public class UpdateDeliveryStatusComandHandler(OrderDbContext _context) : ICommandHandler<UpdateDeliveryStatusCommand, UpdateDeliveryStatusCommandResponse>
    {
        public async Task<UpdateDeliveryStatusCommandResponse> Handle(UpdateDeliveryStatusCommand request, CancellationToken cancellationToken)
        {
            var deliveryOrder = await _context.DeliveryOrders
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (deliveryOrder is null)
            {
                throw new NotFoundException(nameof(deliveryOrder), request.Id.ToString());
            }

            deliveryOrder.UpdateStatus(request.status);
            await _context.SaveChangesAsync(cancellationToken);

            return new UpdateDeliveryStatusCommandResponse(true);
        }
    }

}
