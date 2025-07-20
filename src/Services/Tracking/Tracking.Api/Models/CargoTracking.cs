using Elastic.CommonSchema;
using MassTransit.Futures.Contracts;
using SharedKernel.Core.DDD;
using Tracking.Api.Enums;
using Tracking.Api.Models.Events;


namespace Tracking.Api.Models
{
    public class CargoTracking : Aggregate<Guid>
    {

        public string TrackingId { get; private set; }//a public more readable tracking id ; this id should be related to order.api 
        public TrackingStatus CurrentStatus { get; private set; }
        public string CurrentLocation { get; private set; }

        public Guid OrderId { get; private set; }
        public int? Version { get; set; } //used by marten

        private readonly List<IDomainEvent> _uncommittedEvents = new();
        public IEnumerable<IDomainEvent> GetUncommittedEvents() => _uncommittedEvents;

        private CargoTracking() { }


        public static CargoTracking Create(Guid orderId,  string originLocation, DateTime timestamp)
        {
            var cargo = new CargoTracking();
            var trackingId = GenerateReadableTrackingId();

            var _event = new CargoTrackingInitiated(orderId, trackingId, originLocation, timestamp);
            cargo.Apply(_event); 
            cargo._uncommittedEvents.Add(_event);

            return cargo;
        }

        /// <summary>
        /// Get last 3 digits of timestamp + 3 random digits(makes 6 digits)
        /// </summary>
        /// <returns></returns>
        private static string GenerateReadableTrackingId()
        {
            var timePart = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() % 1000;
            Random _random = new Random();
            var randomPart = _random.Next(100, 999);
            return $"{timePart:000}{randomPart}";
        }



        public void UpdateStatus(TrackingStatus newStatus, string newLocation, DateTime timestamp, string remarks)
        {
            if (TrackingStatusHelper.IsTerminal(this.CurrentStatus))
            {
                throw new InvalidOperationException($"Cannot add event to cargo that is already {this.CurrentStatus}.");
            }

            var _event = new CargoStatusUpdated(this.Id, this.TrackingId, newStatus.ToString(), newLocation, timestamp, remarks);

            Apply(_event);
            _uncommittedEvents.Add(_event);
        }


        #region Apply OVERLOADING

        private void Apply(CargoTrackingInitiated @event)
        {
            Id = @event.CargoId;
            TrackingId = @event.TrackingId;
            CurrentStatus = TrackingStatus.AtOriginFacility;
            CurrentLocation = @event.OriginLocation;
        }


        private void Apply(CargoStatusUpdated @event)
        {
         
            if (Enum.TryParse<TrackingStatus>(@event.NewStatus, out var status))
            {
                CurrentStatus = status;
            }
            CurrentLocation = @event.NewLocation;
        }
        #endregion
    }
}
