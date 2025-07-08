using Elastic.CommonSchema;
using MassTransit.Futures.Contracts;
using SharedKernel.Core.DDD;
using SharedKernel.Messaging.Events;
using System;
using System.Collections.Generic;
using Tracking.Api.Enums;
using Tracking.Api.Models.ValueObjects;

namespace Tracking.Api.Models
{
    public class CargoTracking : Aggregate<Guid>
    {

        public string TrackingId { get; private set; }//a public more readable tracking id ; this id should be related to order.api ; should I make it 6 digit long?? Research on this
        public TrackingStatus CurrentStatus { get; private set; }
        public string CurrentLocation { get; private set; }

        public Guid OrderId { get; private set; }

        private readonly List<TrackingEvent> _history = new();
        public IReadOnlyList<TrackingEvent> History => _history.AsReadOnly();

        private CargoTracking() { }


        public static CargoTracking Create(Guid orderId,  string originLocation, DateTime timestamp)
        {
            var cargo = new CargoTracking
            {
                Id = new Guid(),
                TrackingId = GenerateReadableTrackingId(),
                OrderId = orderId,
            };

            // The first event is always At Origin 
            cargo.AddEvent(TrackingStatus.AtOriginFacility, originLocation, timestamp, "Shipment created and received at origin.");
            cargo.AddDomainEvent(new CargoTrackingInitiated(cargo.Id, cargo.TrackingId));

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



        /// <summary>
        /// Adds a new tracking event to the cargo's history.
        /// This is the primary method for updating the cargo's state.
        /// </summary>
        public void AddEvent(TrackingStatus newStatus, string newLocation, DateTime timestamp, string remarks)
        {
            //validate 
            if (TrackingStatusHelper.IsTerminal(this.CurrentStatus))
            {
                throw new InvalidOperationException($"Cannot add event to cargo that is already {this.CurrentStatus}.");
            }


            if (_history.Any() && timestamp < _history.Last().Timestamp)
            {
                throw new InvalidOperationException("New event timestamp cannot be earlier than the last event.");
            }

            var trackingEvent = new TrackingEvent(newStatus, newLocation, timestamp, remarks);
            _history.Add(trackingEvent);
            CurrentStatus = newStatus;
            CurrentLocation = newLocation;

            // Raise a domain event
            AddDomainEvent(new CargoStatusUpdated(this.Id, this.TrackingId, this.CurrentStatus.ToString(), this.CurrentLocation));
        }
    }
}
