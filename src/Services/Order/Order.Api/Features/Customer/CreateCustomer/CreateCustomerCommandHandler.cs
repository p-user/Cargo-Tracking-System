
using Order.Api.Features.Customer.GetCustomerByEmail;

namespace Order.Api.Features.Customer.CreateCustomer
{
    public record CreateCustomerCommand(CreateCustomerDto dto) : ICommand<CreateCustomerCommandResponse>;
    public record CreateCustomerCommandResponse(string customerId);
    public class CreateCustomerCommandHandler(OrderDbContext _context, ISender _sender, IMapper _mapper) : ICommandHandler<CreateCustomerCommand, CreateCustomerCommandResponse>
    {
        public async Task<CreateCustomerCommandResponse> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var validCustomer = await _sender.Send(new GetCustomerByEmailCommand(request.dto));
            if (validCustomer.viewCustomerDto != null)
            {
                throw new Exception("Another customer with this email address already registered!");
            }
            var customer = CreateCustomer(request.dto);
            await _context.Customers.AddAsync(customer, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return new CreateCustomerCommandResponse(customer.Id.ToString());
        }

        private Models.Customer CreateCustomer(CreateCustomerDto dto)
        {
            return Models.Customer.Create(dto.FullName, dto.Email, dto.Phone, dto.Address.Street, dto.Address.City, dto.Address.ZipCode, dto.Address.Country);
        }
    }
}
