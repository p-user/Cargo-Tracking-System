using JasperFx.Events;
using Marten;
using Marten.Linq.Parsing.Operators;
using SharedKernel.CQRS;
using System.IO;
using Tracking.Api.Domain.CargoTracking;
using Tracking.Api.Domain.CargoTracking.Events;

namespace Tracking.Api.Features.GetTrackingByOrderId
{
    public record GetTrackingByOrderIdQuery(Guid OrderId) : IQuery<GetTrackingByOrderIdQueryResponse>;
    public record GetTrackingByOrderIdQueryResponse(TrackingDetailsDto? dto);
    public class GetTrackingByOrderIdQueryHandler(IDocumentSession _documentSession) : IQueryHandler<GetTrackingByOrderIdQuery, GetTrackingByOrderIdQueryResponse>
    {
        public async Task<GetTrackingByOrderIdQueryResponse> Handle(GetTrackingByOrderIdQuery request, CancellationToken cancellationToken)
        {
            var trackingId = await _documentSession.Events
                .QueryRawEventDataOnly<CargoTrackingInitiated>()
                .Where(x => x.OrderId == request.OrderId)
                .Select(x => x.CargoTrackingId)
                .FirstOrDefaultAsync();

            if (trackingId == Guid.Empty)
            {
                return new GetTrackingByOrderIdQueryResponse(null);
            }

            var aggregate = await _documentSession.Events.AggregateStreamAsync<CargoTracking>(trackingId);

            var dto = new TrackingDetailsDto(
             Id: trackingId,
             OrderId: aggregate.OrderId.ToString(),
             CurrentStatus: aggregate.CurrentStatus.ToString(),
             OriginLocation: aggregate.CurrentLocation);



            return new GetTrackingByOrderIdQueryResponse(dto);

        }
    }
}
