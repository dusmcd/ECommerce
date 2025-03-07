using System.ComponentModel.DataAnnotations;

namespace ECommerceAPI.Models
{
    public class InventoryLog
    {
        public int Id { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public Product Product { get; set; } = default!;

        [Required, DataType(DataType.Time)]
        public DateTime CreatedAt { get; set; }

        [Required, DataType(DataType.Time)]
        public DateTime UpdatedAt { get; set; }


    }
}
