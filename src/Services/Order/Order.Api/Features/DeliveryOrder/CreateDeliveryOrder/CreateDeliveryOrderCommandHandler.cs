
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

                // Write all  message to the outbox
                var outboxMsgs = new List<OutboxMessage>();

                foreach (var item in deliveryOrder.DomainEvents)
                {
                    var outboxMessage = new OutboxMessage
                    {
                        Id = Guid.NewGuid(),
                        Type =item.GetType().AssemblyQualifiedName!,
                        Content = JsonSerializer.Serialize(item, item.GetType()),
                        OccuredOn = DateTime.UtcNow
                    };

                    outboxMsgs.Add(outboxMessage);
                }

                await  _context.DeliveryOrders.AddAsync(deliveryOrder, cancellationToken);
                await _context.OutboxMessages.AddRangeAsync(outboxMsgs, cancellationToken);

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
