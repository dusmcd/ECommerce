using System.ComponentModel.DataAnnotations;

namespace ECommerceAPI.Models
{
    public class CartDTO : DTO
    {
        [Required]
        public ProductInfo Product { get; set; }

        [Required]
        public int CustomerId { get; set; }


    }
}
