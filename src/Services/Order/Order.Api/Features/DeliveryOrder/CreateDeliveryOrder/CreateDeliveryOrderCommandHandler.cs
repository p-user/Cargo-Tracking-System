using SharedKernel.Core.Data;

namespace Order.Api.Features.DeliveryOrder.CreateDeliveryOrder
{

    public record CreateDeliveryOrderCommand(CreateDeliveryOrderDto dto) : ICommand<CreateDeliveryOrderCommandResponse>;

    public record CreateDeliveryOrderCommandResponse(Guid Id);

    public class CreateDeliveryOrderCommandHandler(OrderDbContext _context, IMapper _mapper, ISender _sender, IUnitOfWork _unitOfWork) : ICommandHandler<CreateDeliveryOrderCommand, CreateDeliveryOrderCommandResponse>
    {
        public async Task<CreateDeliveryOrderCommandResponse> Handle(CreateDeliveryOrderCommand request, CancellationToken cancellationToken)
        {
            //verify customer 
            var customerVM = await _sender.Send(new GetCustomerByEmailQuery(request.dto.Customer.Email));
            var customer = _mapper.Map<Models.Customer>(customerVM.viewCustomerDto);

            var cargo = _mapper.Map<CargoDetails>(request.dto.Cargo);

            var deliveryOrder = CreateDeliveryOrder(request.dto, cargo, customer);

           
          
            try
            {
                await  _context.DeliveryOrders.AddAsync(deliveryOrder, cancellationToken);

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return new CreateDeliveryOrderCommandResponse(deliveryOrder.Id);
            }

            catch (Exception ex)
            {
               
                return new CreateDeliveryOrderCommandResponse(Guid.Empty);
            }

        }


      
        private Models.DeliveryOrder CreateDeliveryOrder(DeliveryOrderDto dto, CargoDetails cargo, Models.Customer customer)
        {
            return Models.DeliveryOrder.Create(customer, dto.ReceiverName, dto.ReceiverContact, dto.PickupAddress, dto.DeliveryAddress, cargo);

        }
    }

}
