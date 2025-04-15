using AutoMapper;
using Order.Api.Dtos;
using SharedKernel.CQRS;

namespace Order.Api.Features.DeliveryOrder.CreateDeliveryOrder
{

    public record CreateDeliveryOrderCommand(DeliveryOrderDto dto) : ICommand<CreateDeliveryOrderCommandResponse>;

    public record CreateDeliveryOrderCommandResponse(Guid Id);

    public class CreateDeliveryOrderCommandHandler(OrderDbContext context, IMapper mapper) : ICommandHandler<CreateDeliveryOrderCommand, CreateDeliveryOrderCommandResponse>
    {
        public async Task<CreateDeliveryOrderCommandResponse> Handle(CreateDeliveryOrderCommand request, CancellationToken cancellationToken)
        {
            var cargo = mapper.Map<CargoDetails>(request.dto.Cargo);
            var deliveryOrder = CreateDeleveryOrder(request.dto, cargo);

            await context.DeliveryOrders.AddAsync(deliveryOrder, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            return new CreateDeliveryOrderCommandResponse(deliveryOrder.Id);
        }

        private Models.DeliveryOrder CreateDeleveryOrder(DeliveryOrderDto dto, CargoDetails cargo)
        {
            return Models.DeliveryOrder.Create(dto.SenderName, dto.SenderContact, dto.ReceiverName, dto.ReceiverContact, dto.PickupAddress, dto.DeliveryAddress, cargo);

        }
    }

}
