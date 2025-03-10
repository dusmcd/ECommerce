using System.ComponentModel.DataAnnotations;

namespace ECommerceAPI.Models
{
    public class ProductDTO : DTO
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

        public override void MapToModel<T>(T entity)
        {

            Product? product = entity as Product;
            if (product == null)
                return;

            product!.Name = Name;
            product.Description = Description;
            product.Orders = Orders;
            product.Carts = Carts;
            product.ImageUrl = ImageUrl;
            product.Price = Price;
        }
    }
}
