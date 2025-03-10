using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ECommerceAPI.Data;
using ECommerceAPI.Models;

namespace ECommerceAPI.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly EcommerceAPIContext _context;

        public OrdersController(EcommerceAPIContext context)
        {
            _context = context;
        }

        private Order CreateInitialOrder(OrderDTO orderDTO)
        {
            var order = new Order();
            orderDTO.MapToModel(order);
            foreach(var productInfo in orderDTO.Products)
            {
                var product = _context.Products.Find(productInfo.Id);
                if (product == null)
                    continue;

                order.Products.Add(product);
            }

            var customer = _context.Customers.Find(orderDTO.CustomerId);
            if (customer == null)
                return null!; // this will process as a bad request

            order.Customer = customer;
            DateTime timeNow = DateTime.UtcNow;
            order.OrderDate = timeNow;
            order.CreatedAt = timeNow;
            order.UpdatedAt = timeNow;

            _context.Orders.Add(order);

            return order;
        }

        private void UpdateQuantities(OrderDTO orderDTO, int orderId)
        {
            // filter the productorders by this order only
            var productOrders = _context.ProductsOrders.Where(po => po.OrderId == orderId);
            foreach(var productInfo in orderDTO.Products)
            {
                // find the entry that matches the productId for this order (there should only be one)
                var entityToUpdate = productOrders.Where(po => po.ProductId == productInfo.Id).First();
                entityToUpdate.Quantity = productInfo.Quantity;
                _context.ProductsOrders.Update(entityToUpdate);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(OrderDTO orderDTO)
        {
            if (orderDTO == null)
                return BadRequest();

            var order = CreateInitialOrder(orderDTO);
            if (order == null)
                return BadRequest();

            
            await _context.SaveChangesAsync();

            UpdateQuantities(orderDTO, order.Id);

            await _context.SaveChangesAsync();
            return Created();
        }

    }
}
