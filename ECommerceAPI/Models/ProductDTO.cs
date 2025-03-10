using System.ComponentModel.DataAnnotations;

namespace ECommerceAPI.Models
{
    public class ProductDTO
    {
        public List<Order> Orders { get; set; } = new List<Order>();

        public List<Cart> Carts { get; set; } = new List<Cart>();

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Description { get; set; }

        public string? ImageUrl { get; set; }

        [Required]
        public decimal Price { get; set; }

        public void MapToModel(Product product)
        {
            product.Name = Name;
            product.Description = Description;
            product.Orders = Orders;
            product.Carts = Carts;
            product.ImageUrl = ImageUrl;
            product.Price = Price;
        }
    }
}
