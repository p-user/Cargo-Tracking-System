namespace Tracking.Api.Dtos
{
    public record CreateTrackingForOrderDto
    {
        public Guid OrderId { get; set; }
        public string OriginLocation { get; set; } = default!;
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
