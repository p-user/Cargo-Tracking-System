namespace Order.Api.Dtos
{
    public record AddressDto
    {
        public string? Street { get; init; }
        public string City { get; init; }
        public string ZipCode { get; init; }
        public string? Country { get; init; }
    }


}
