
using System.ComponentModel.DataAnnotations;

namespace ECommerceAPI.Models
{
    public class ProductCart
    {
        public int Id { get; set; }

        [Required]
        public int Quantity { get; set; }
    }
}
