
using System.Net;

namespace Order.Api.Models
{
    public class Customer : Entity<Guid>
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public Address Address { get; set; }

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
