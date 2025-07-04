

namespace Order.Api.Features.DeliveryOrder.GetDeliveryOrderById
{

    public record GetDeliveryOrderByIdQuery(Guid Id) : IQuery<GetDeliveryOrderByIdResponse>;
    public record GetDeliveryOrderByIdResponse(ViewDeliveryOrderDto dto);

    public class GetDeliveryOrderByIdQueryHandler(IMapper _mapper, OrderDbContext _context) : IQueryHandler<GetDeliveryOrderByIdQuery, GetDeliveryOrderByIdResponse>
    {
        public async Task<GetDeliveryOrderByIdResponse> Handle(GetDeliveryOrderByIdQuery request, CancellationToken cancellationToken)
        {


            var deliveryOrder = await _context.DeliveryOrders
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (deliveryOrder == null)
            {
                throw new NotFoundException(nameof(deliveryOrder), request.Id.ToString());
            }

            var deliveryOrderDto = _mapper.Map<ViewDeliveryOrderDto>(deliveryOrder);
            return new GetDeliveryOrderByIdResponse(deliveryOrderDto);
        }

    }

}
