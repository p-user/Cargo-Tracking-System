using Marten;
using SharedKernel.CQRS;
using Tracking.Api.Domain.CargoTracking;
using Tracking.Api.Domain.CargoTracking.Enums;
using Tracking.Api.Domain.CargoTracking.Events;


namespace Tracking.Api.Features.GetAllOrderTrackingHistory
{

    public record GetAllOrderTrackingHistoryQuery(Guid OrderId): IQuery<GetAllOrderTrackingHistoryQueryResponse>;
    public record GetAllOrderTrackingHistoryQueryResponse(List<TrackingHistoryItemDto> TrackingHistoryItemDtos);
    public class GetAllOrderTrackingHistoryQueryHandler(IDocumentSession _documentSession) : IQueryHandler<GetAllOrderTrackingHistoryQuery, GetAllOrderTrackingHistoryQueryResponse>
    {
        public async  Task<GetAllOrderTrackingHistoryQueryResponse> Handle(GetAllOrderTrackingHistoryQuery request, CancellationToken cancellationToken)
        {
            var cargoInfo = await _documentSession.Query<CargoTracking>()
            .Where(ct => ct.OrderId == request.OrderId)
            .Select(ct => new { ct.Id })
            .FirstOrDefaultAsync(cancellationToken);
            if (cargoInfo is null)
            {
                return new GetAllOrderTrackingHistoryQueryResponse(new List<TrackingHistoryItemDto>());
            }

            var events = await _documentSession.Events.FetchStreamAsync(cargoInfo.Id, token: cancellationToken);
            var history = new List<TrackingHistoryItemDto>();
            foreach (var storedEvent in events)
            {
                switch (storedEvent.Data)
                {
                    case CargoTrackingInitiated initiated:
                        history.Add(new TrackingHistoryItemDto(
                            Status: TrackingStatus.AtOriginFacility.ToString(),
                            Location: initiated.OriginLocation,
                            Timestamp: initiated.Timestamp,
                            Remarks: "Tracking Initiated"
                        ));
                        break;

                    case CargoStatusUpdated updated:
                        history.Add(new TrackingHistoryItemDto(
                            Status: updated.NewStatus,
                            Location: updated.NewLocation,
                            Timestamp: updated.Timestamp,
                            Remarks: updated.Remarks
                        ));
                        break;

                       
                }
            }

            return new GetAllOrderTrackingHistoryQueryResponse(history);
        }
    }
}
