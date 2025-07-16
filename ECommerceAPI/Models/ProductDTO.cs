using System.ComponentModel.DataAnnotations;

namespace ECommerceAPI.Models
{
    public class ProductDTO : DTO
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Description { get; set; }

        [Required]
        public int Quantity { get; set; }

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
            product.ImageUrl = ImageUrl;
            product.Price = Price;
        }
    }
}
