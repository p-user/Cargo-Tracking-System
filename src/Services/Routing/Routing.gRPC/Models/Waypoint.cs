using Grpc.Core;
using Routing.gRPC.Enums;
using SharedKernel.DDD;

namespace Routing.gRPC.Models
{
    public class Waypoint: Entity<Guid>
    {
        public Guid RouteId { get; private set; }
        public Route Route { get; set; } = default!;
        public string LocationName { get; private set; }
        public double Latitude { get; private set; }
        public double Longitude { get; private set; }
        public int Sequence { get; private set; } // Order in the route

        public WaypointStatus Status { get; private set; }


        private Waypoint() { }

        private Waypoint(Guid routeId,string locationName, double latitude, double longitude, int sequence)
        {
            Id = Guid.NewGuid();
            RouteId = routeId;
            LocationName = locationName;
            Latitude = latitude;
            Longitude = longitude;
            Sequence = sequence;
            Status = WaypointStatus.Pending;
        }

        public static Waypoint Create(Guid routeId, string locationName,double latitude,double longitude,int sequence)
        {
            if (string.IsNullOrWhiteSpace(locationName))
                throw new ArgumentException("Location name cannot be empty.");

            return new Waypoint(routeId, locationName, latitude, longitude, sequence);
        }


        public void MarkAsEnRoute()
        {
            if (Status != WaypointStatus.Pending)
                throw new InvalidOperationException($"Cannot set EnRoute. Current status: {Status}");

            Status = WaypointStatus.EnRoute;
        }

        public void MarkAsArrived()
        {
            if (Status != WaypointStatus.EnRoute)
                throw new InvalidOperationException("Waypoint must be EnRoute before arriving.");

            Status = WaypointStatus.Arrived;
        }

        public void MarkAsCompleted()
        {
            if (Status != WaypointStatus.Arrived)
                throw new InvalidOperationException("Waypoint must be Arrived before being completed.");

            Status = WaypointStatus.Completed;
        }

        public void Skip()
        {
            if (Status == WaypointStatus.Completed)
                throw new InvalidOperationException("Cannot skip a completed waypoint.");

            Status = WaypointStatus.Skipped;
        }



    }
}
