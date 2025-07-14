
using System.ComponentModel.DataAnnotations;

namespace ECommerceAPI.Models
{
    public class ProductCart
    {
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int CartId { get; set; }

        [Required]
        public int Quantity { get; set; }
    }
}
