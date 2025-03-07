using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceAPI.Models
{
    public class ProductOrder
    {
        public int Id { get; set; }

        [Column(TypeName = "decimal(3, 2)")]
        public decimal Discount { get; set; }

        [Required]
        public int Quantity { get; set; }
    }
}
