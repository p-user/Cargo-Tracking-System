namespace Order.Api.Features.Customer.GetCustomerByEmail
{
    public record GetCustomerByEmailQuery(string email) : ICommand<GetCustomerByEmailQueryResponse>;
    public record GetCustomerByEmailQueryResponse(ViewCustomerDto viewCustomerDto);
    public class GetCustomerByEmailQueryHandler(OrderDbContext _context, IMapper _mapper) : ICommandHandler<GetCustomerByEmailQuery, GetCustomerByEmailQueryResponse>
    {
        public async  Task<GetCustomerByEmailQueryResponse> Handle(GetCustomerByEmailQuery request, CancellationToken cancellationToken)
        {
            var customer = await _context.Customers.Where(s=>s.Email==request.email).FirstOrDefaultAsync(cancellationToken);
            if (customer is null) { throw new NotFoundException(nameof(Customer), request.email);  }
             var mapped = _mapper.Map<ViewCustomerDto>(customer);
            return new GetCustomerByEmailQueryResponse(mapped);

        }
    }
}
