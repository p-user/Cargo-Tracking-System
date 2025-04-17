namespace Order.Api.Dtos
{
    public record CargoDetailsDto
    {
        public string? Description { get; set; }
        public double WeightKg { get; set; }
        public decimal LengthCm { get; set; }
        public decimal WidthCm { get; set; }
        public decimal HeightCm { get; set; }
    }
}
