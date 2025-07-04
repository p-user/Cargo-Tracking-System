
namespace Order.Api.Models
{
    public class Customer : Entity<Guid>
    {
        public string FullName { get; private set; }
        public string Email { get; private set; }
        public string Phone { get; private set; }
        public Address Address { get; private set; }

        private Customer() { }

        public static Customer Create(string fullname, string email, string phone, string? street, string city, string zipCode, string? country)
        {
            return new Customer
            {
                Id = new Guid(),
                FullName = fullname,
                Email = email,
                Phone = phone,
                Address =new Address(street, city, zipCode, country)
            };
            
        }
    }
}
