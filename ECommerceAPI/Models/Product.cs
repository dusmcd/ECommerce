using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceAPI.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Description { get; set; }

        public string? ImageUrl { get; set; }

        [NotMapped]
        public int Quantity { get; set; }

        [Required, Column(TypeName = "decimal(9, 2)")]
        public decimal Price { get; set; }

        [Required, DataType(DataType.Time)]
        public DateTime CreatedAt { get; set; }

        [Required, DataType(DataType.Time)]
        public DateTime UpdatedAt { get; set; }

    }
}
