
using MassTransit;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Order.Api.Features.DeliveryOrder.CreateDeliveryOrder
{

    public record CreateDeliveryOrderCommand(CreateDeliveryOrderDto dto) : ICommand<CreateDeliveryOrderCommandResponse>;

    public record CreateDeliveryOrderCommandResponse(Guid Id);

    public class CreateDeliveryOrderCommandHandler(OrderDbContext _context, IMapper _mapper, ISender _sender, IPublishEndpoint publishEndpoint) : ICommandHandler<CreateDeliveryOrderCommand, CreateDeliveryOrderCommandResponse>
    {
        public async Task<CreateDeliveryOrderCommandResponse> Handle(CreateDeliveryOrderCommand request, CancellationToken cancellationToken)
        {
            //verify customer 
            var customerVM = await _sender.Send(new GetCustomerByEmailCommand(request.dto.Customer));
            var customer = _mapper.Map<Models.Customer>(customerVM.viewCustomerDto);

            var cargo = _mapper.Map<CargoDetails>(request.dto.Cargo);

            var deliveryOrder = CreateDeliveryOrder(request.dto, cargo, customer);

           
            await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                await  _context.DeliveryOrders.AddAsync(deliveryOrder, cancellationToken);
               
                //On savechanges the domain messages will be saved on the outbox table
                await _context.SaveChangesAsync(cancellationToken);

                await transaction.CommitAsync(cancellationToken);
               return new CreateDeliveryOrderCommandResponse(deliveryOrder.Id);
            }

            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                return new CreateDeliveryOrderCommandResponse(Guid.Empty);
            }

        }


      
        private Models.DeliveryOrder CreateDeliveryOrder(DeliveryOrderDto dto, CargoDetails cargo, Models.Customer customer)
        {
            return Models.DeliveryOrder.Create(customer, dto.ReceiverName, dto.ReceiverContact, dto.PickupAddress, dto.DeliveryAddress, cargo);

        }
    }

}
