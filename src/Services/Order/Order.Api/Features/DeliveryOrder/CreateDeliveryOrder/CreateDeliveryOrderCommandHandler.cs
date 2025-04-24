
using Order.Api.Features.Customer.GetCustomerByEmail;

namespace Order.Api.Features.DeliveryOrder.CreateDeliveryOrder
{

    public record CreateDeliveryOrderCommand(CreateDeliveryOrderDto dto) : ICommand<CreateDeliveryOrderCommandResponse>;

    public record CreateDeliveryOrderCommandResponse(Guid Id);

    public class CreateDeliveryOrderCommandHandler(OrderDbContext _context, IMapper _mapper, ISender _sender) : ICommandHandler<CreateDeliveryOrderCommand, CreateDeliveryOrderCommandResponse>
    {
        public async Task<CreateDeliveryOrderCommandResponse> Handle(CreateDeliveryOrderCommand request, CancellationToken cancellationToken)
        {
            //verify customer 
            var customerVM = await _sender.Send(new GetCustomerByEmailCommand(request.dto.Customer));
            var customer = _mapper.Map<Models.Customer>(customerVM.viewCustomerDto);

            var cargo = _mapper.Map<CargoDetails>(request.dto.Cargo);

            var deliveryOrder = CreateDeliveryOrder(request.dto, cargo, customer);

            await _context.DeliveryOrders.AddAsync(deliveryOrder, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return new CreateDeliveryOrderCommandResponse(deliveryOrder.Id);
        }


      
        private Models.DeliveryOrder CreateDeliveryOrder(DeliveryOrderDto dto, CargoDetails cargo, Models.Customer customer)
        {
            return Models.DeliveryOrder.Create(customer, dto.ReceiverName, dto.ReceiverContact, dto.PickupAddress, dto.DeliveryAddress, cargo);

        }
    }

}
