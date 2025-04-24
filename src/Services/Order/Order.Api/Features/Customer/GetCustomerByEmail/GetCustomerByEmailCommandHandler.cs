namespace Order.Api.Features.Customer.GetCustomerByEmail
{
    public record GetCustomerByEmailCommand(CustomerDto customerDto) : ICommand<GetCustomerByEmailCommandResponse>;
    public record GetCustomerByEmailCommandResponse(ViewCustomerDto viewCustomerDto);
    public class GetCustomerByEmailCommandHandler(OrderDbContext _context, IMapper _mapper) : ICommandHandler<GetCustomerByEmailCommand, GetCustomerByEmailCommandResponse>
    {
        public async  Task<GetCustomerByEmailCommandResponse> Handle(GetCustomerByEmailCommand request, CancellationToken cancellationToken)
        {
            var customer = await _context.Customers.Where(s=>s.Email==request.customerDto.Email).FirstOrDefaultAsync(cancellationToken);
            if (customer is null) { throw new NotFoundException(nameof(Customer), request.customerDto.Email);  }
             var mapped = _mapper.Map<ViewCustomerDto>(customer);
            return new GetCustomerByEmailCommandResponse(mapped);

        }
    }
}
