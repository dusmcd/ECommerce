using System.ComponentModel.DataAnnotations;

namespace ECommerceAPI.Models
{
    public struct ProductInfo
    {
        public int Id { get; set; }

        public string? Name { get; set; }
        public int Quantity { get; set; }

        public bool Delete { get; set; }
    }
    public class OrderDTO : DTO
    {
        public int Id { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [DataType(DataType.Date)]
        public DateTime OrderDate { get; set; }

        public Status OrderStatus { get; set; }

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
