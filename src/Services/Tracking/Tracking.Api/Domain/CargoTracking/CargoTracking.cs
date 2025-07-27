using SharedKernel.Core.DDD;
using SharedKernel.Messaging.Events;
using Tracking.Api.Domain.CargoTracking.Enums;
using Tracking.Api.Domain.CargoTracking.Events;


namespace Tracking.Api.Domain.CargoTracking
{
    public class CargoTracking : Aggregate<Guid>
    {
        public TrackingStatus CurrentStatus { get; private set; }
        public string CurrentLocation { get; private set; }
        public Guid OrderId { get; private set; }
        public int? Version { get; set; } //used by marten

        private CargoTracking() { }


        public static CargoTracking Create(Guid orderId, string originLocation, DateTime timestamp)
        {
            var cargo = new CargoTracking();
            cargo.Id =Guid.NewGuid();


            var _event = new CargoTrackingInitiated(cargo.Id,orderId, originLocation, timestamp);
            cargo.Apply(_event);
            cargo.AddDomainEvent(_event);
           ;

            return cargo;
        }

      

        public void UpdateStatus(TrackingStatus newStatus, string newLocation, DateTime timestamp, string remarks)
        {
            if (TrackingStatusHelper.IsTerminal(CurrentStatus))
            {
                throw new InvalidOperationException($"Cannot add event to cargo that is already {CurrentStatus}.");
            }

            var _event = new CargoStatusUpdated(Id, Id, newStatus.ToString(), newLocation, timestamp, remarks);

            Apply(_event);
            this.AddDomainEvent(_event);

        }


        #region Apply OVERLOADING

        private void Apply(CargoTrackingInitiated @event)
        {
            OrderId = @event.OrderId;
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
