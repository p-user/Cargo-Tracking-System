namespace Order.Api.Models
{
    public class Address
    {
        public string? Street { get; private set; }
        public string City { get; private set; }
        public string ZipCode { get; private set; }
        public string? Country { get; private set; }

        private Address() { }

        public Address(string? street, string city, string zipCode, string? country)
        {
            Street = street;
            City = city;
            ZipCode = zipCode;
            Country = country;
        }
    }
}
