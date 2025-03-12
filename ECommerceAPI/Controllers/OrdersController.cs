using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System;
using ECommerceAPI.Data;
using ECommerceAPI.Models;
using Microsoft.EntityFrameworkCore;

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

        private int ReadOrderFromSQL(Order order, SqlDataReader reader, int orderId)
        {
            Product product;
            int currentOrderId = reader.GetInt32("OrderId");
            if (currentOrderId == orderId)
            {
                product = new Product()
                {
                    Id = reader.GetInt32("ProductId"),
                    Description = reader.GetString("Description"),
                    Price = reader.GetDecimal("Price"),
                    Name = reader.GetString("ProductName"),
                    Quantity = reader.GetInt32("Quantity")
                };
                order.Products.Add(product);
                return currentOrderId;
            }
            order.Id = reader.GetInt32("OrderId");
            order.OrderDate = reader.GetDateTime("OrderDate");
            order.ShippedDate = reader.GetDateTime("ShippedDate");
            order.FulfilledDate = reader.GetDateTime("FulfilledDate");

            int orderStatus = reader.GetInt32("OrderStatus");
            switch(orderStatus)
            {
                case 0:
                    order.OrderStatus = Status.Pending;
                    break;
                case 1:
                    order.OrderStatus = Status.Shipped;
                    break;
                case 2:
                    order.OrderStatus = Status.Fulfilled;
                    break;
                case 3:
                    order.OrderStatus = Status.Cancelled;
                    break;
            }

            Customer customer = new Customer()
            {
                Id = reader.GetInt32("CustomerId"),
                Name = reader.GetString("CustomerName"),
                Email = reader.GetString("Email")
            };
            order.Customer = customer;

            product = new Product()
            {
                Id = reader.GetInt32("ProductId"),
                Description = reader.GetString("Description"),
                Price = reader.GetDecimal("Price"),
                Name = reader.GetString("ProductName"),
                Quantity = reader.GetInt32("Quantity")
            };
            order.Products.Add(product);
            return currentOrderId;

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders(int limit = -1)
        {
            string connectionStr = "Server=(localdb)\\MSSQLLocalDB;Database=EcommerceAPIContext-1";
            string procName = "get_orders";

            try
            {
                using SqlConnection conn = new SqlConnection(connectionStr);
                conn.Open();

                using SqlCommand cmd = new SqlCommand(procName, conn);
                cmd.CommandType = CommandType.StoredProcedure;

                using SqlDataReader reader = cmd.ExecuteReader();
                int orderId = 0;
                Order order = new Order();
                List<Order> orders = new List<Order>();
                int orderLimit = limit > 0 ? limit : int.MaxValue;
                int orderCount = 0;

                while (await reader.ReadAsync())
                {
                    // check whether we have reached a new order (i.e., whether the orderId has changed)
                    // if not, then we add another product to the current order
                    if (orderId == reader.GetInt32("OrderId"))
                    {
                        ReadOrderFromSQL(order, reader, orderId);
                        continue;
                    }

                    // if the orderId has changed (i.e., we have reached a new order)
                    // then reset the order variable and start updating for the new order
                    orderCount++;
                    if (orderCount > orderLimit)
                        break;
                   
                    order = orderId == 0 ? order : new Order();
                    orderId = ReadOrderFromSQL(order, reader, orderId);
                    orders.Add(order);
                }

                return orders;

    
            } catch(Exception)
            {
                throw;
            }
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
            order.OrderStatus = Status.Pending;

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
                var entityToUpdate = productOrders.Where(po => po.ProductId == productInfo.Id).FirstOrDefault();
                if (entityToUpdate == null)
                    continue;

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
            if (!ModelState.IsValid)
                return BadRequest("constraints for Orders table violated");
            
            await _context.SaveChangesAsync();

            UpdateQuantities(orderDTO, order.Id);

            await _context.SaveChangesAsync();
            return Created();
        }

        private async Task<Order> RemoveOrderProducts(int id, OrderDTO orderDTO)
        {
            var order = await _context.Orders
                .Where(o => o.Id == id)
                .Include(o => o.Products)
                .FirstAsync();

            if (order == null)
                return null!;

            foreach(var productInfo in orderDTO.Products)
            {
                var product = order.Products.Where(p => p.Id == productInfo.Id).First();
                if (product == null)
                    continue;

                if (productInfo.Delete)
                {
                    order.Products.Remove(product);
                }
            }
            order.UpdatedAt = DateTime.UtcNow;

            return order;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrderProducts(int id, OrderDTO orderDTO)
        {
            if (id == 0 || orderDTO == null)
                return BadRequest();

            var order = await RemoveOrderProducts(id, orderDTO);
            if (order == null)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest("constraints for Orders table violated");

            if (order.Products.Count == 0)
                return BadRequest("an order must have at least one product");

            _context.Orders.Update(order);
            await _context.SaveChangesAsync();

            UpdateQuantities(orderDTO, order.Id);

            await _context.SaveChangesAsync();

            return Ok();

        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateOrderStatus(int id, Status status)
        {
            if (id == 0)
                return BadRequest();

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
                return NotFound();

            order.OrderStatus = status;
            switch(status)
            {
                case Status.Shipped:
                    order.ShippedDate = DateTime.UtcNow;
                    break;
                case Status.Fulfilled:
                    order.FulfilledDate = DateTime.UtcNow;
                    break;
                case Status.Pending:
                    order.ShippedDate = DateTime.MinValue;
                    order.FulfilledDate = DateTime.MinValue;
                    break;
            }
            order.UpdatedAt = DateTime.UtcNow;
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("{id}/cancel")]
        public async Task<IActionResult> CancelOrder(int id)
        {
            if (id == 0)
                return BadRequest();

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
                return NotFound();

            // enforce 30 day cancel policy
            DateTime today = DateTime.UtcNow;
            TimeSpan timeSpan = today.Subtract(order.OrderDate);
            if (Math.Abs(timeSpan.TotalDays) > 30)
                return BadRequest("orders cannot be cancelled after 30 days");

            order.OrderStatus = Status.Cancelled;
            order.UpdatedAt = today;
            _context.Orders.Update(order);

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            if (id == 0)
                return BadRequest();

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
                return NotFound();

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return Ok();
        }



    }
}
