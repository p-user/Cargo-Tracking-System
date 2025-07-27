namespace Tracking.Api.Domain.CargoTracking.Enums
{
    public static class TrackingStatusHelper
    {

        private static readonly HashSet<TrackingStatus> _terminalStatuses = new()
        {
            TrackingStatus.Delivered,
            TrackingStatus.Lost
        };

        public static bool IsTerminal(TrackingStatus status)
        {
            return _terminalStatuses.Contains(status);
        }
    }
}
