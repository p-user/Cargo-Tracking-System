namespace Order.Api.Dtos
{
    public record CargoDetailsDto
    {
        public string? Description { get; set; }
        public double WeightKg { get; set; }
        public string Dimensions { get; set; }
    }
}
