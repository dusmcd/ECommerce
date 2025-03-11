using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceAPI.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        public Customer Customer { get; set; } = default!;

        public List<Product> Products { get; set; } = new List<Product>();

        [Column(TypeName = "decimal(3, 2)")]
        public decimal Promo { get; set; }

        public string? PaymentInfo { get; set; }

        [Required]
        public Status OrderStatus { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime OrderDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime FulfilledDate { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime ShippedDate { get; set; }

        [Required, DataType(DataType.Time)]
        public DateTime CreatedAt { get; set; }

        [Required, DataType(DataType.Time)]
        public DateTime UpdatedAt { get; set; }

    }

    public enum Status
    {
        Pending,
        Shipped,
        Fulfilled,
    }
}
