


namespace Order.Api.Features.Customer.GetCustomerById
{
    public record GetCustomerByIdQuery(Guid id) : IQuery<GetCustomerByIdQueryResponse>;
    public record GetCustomerByIdQueryResponse(ViewCustomerDto customer);
    public class GetCustomerByIdQueryHandler(OrderDbContext _context, IMapper _mapper) : IQueryHandler<GetCustomerByIdQuery, GetCustomerByIdQueryResponse>
    {
        public async Task<GetCustomerByIdQueryResponse> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
        {
            var customer = await _context.Customers.FindAsync(request.id, cancellationToken);
            if (customer == null) 
            { 
                throw new NotFoundException(nameof(customer), request.id.ToString()); 
            }

            var mapped = _mapper.Map<ViewCustomerDto>(customer);
             return new GetCustomerByIdQueryResponse(mapped);
        }
    }
}
