using System.ComponentModel.DataAnnotations;

namespace ECommerceAPI.Models
{
    public class CustomerDTO : DTO
    {
        public string? Name { get; set; }

        public List<OrderDTO> Orders { get; set; } = new List<OrderDTO>();

        [Required]
        public string? Email { get; set; }

        [Required]
        public string? Address1 { get; set; }

        public string? Address2 { get; set; }

        [Required]
        public string? City { get; set; }

        [Required]
        public string? State { get; set; }

        [Required]
        public string? ZipCode { get; set; }

        public string? PhoneNumber { get; set; }

        [Required]
        public bool IsUser { get; set; }

        public string? SessionId { get; set; }

        public override void MapToModel<T>(T entity)
        {
            Customer? customer = entity as Customer;
            if (customer == null)
                return;

            customer.Name = Name;
            customer.Email = Email;
            customer.Address1 = Address1;
            customer.Address2 = Address2;
            customer.City = City;
            customer.State = State;
            customer.ZipCode = ZipCode;
            customer.PhoneNumber = PhoneNumber;
            customer.IsUser = IsUser;
            customer.SessionId = SessionId;
        }
    }
}
