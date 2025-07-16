using System.ComponentModel.DataAnnotations;

namespace Client.Models
{
    public class ProductViewModel
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


    }
}
