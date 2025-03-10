using System.ComponentModel.DataAnnotations;

namespace ECommerceAPI.Models
{
    public struct ProductInfo
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
    }
    public class OrderDTO : DTO
    {
        [Required]
        public int CustomerId { get; set; }

        [Required]
        public List<ProductInfo> Products { get; set; } = new List<ProductInfo>();

        public decimal Promo { get; set; }

        public string? PaymentInfo { get; set; }

        public override void MapToModel<T>(T entity)
        {
            Order? order = entity as Order;
            if (order == null)
                return;

            order.Promo = Promo;
            order.PaymentInfo = PaymentInfo;
        }
    }
}
