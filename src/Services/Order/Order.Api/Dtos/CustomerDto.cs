namespace Order.Api.Dtos
{
    public record CustomerDto
    {
        public string FullName { get; init; }
        public string Email { get; init; }
        public string Phone { get; init; }
        public AddressDto Address { get; init; }
    }

    public record CreateCustomerDto : CustomerDto
    {

    }

    public record ViewCustomerDto : CustomerDto
    {
        public Guid Id { get; init; }
        public DateTime? CreatedAt { get; init; }
        public DateTime? LastModified { get; init; }
        public string? CreatedBy { get; init; }
        public string? LastModifiedBy { get; init; }
    }
}
