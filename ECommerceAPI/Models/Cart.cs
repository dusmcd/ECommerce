using System.ComponentModel.DataAnnotations;

namespace ECommerceAPI.Models
{
    public class Cart
    {
        public int Id { get; set; }

        [Required]
        public Customer Customer { get; set; } = default!;

        public List<Product> Products { get; set; } = new List<Product>();

        public CartStatus Status { get; set; }

        [Required, DataType(DataType.Time)]
        public DateTime CreatedAt { get; set; }

        [Required, DataType(DataType.Time)]
        public DateTime UpdatedAt { get; set; }
    }

    public enum CartStatus
    {
        Active,
        Inactive
    }
}
