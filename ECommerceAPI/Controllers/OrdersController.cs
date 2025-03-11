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
                    Name = reader.GetString("ProductName")
                };
                order!.Products.Add(product);
                return currentOrderId;
            }
            order.Id = reader.GetInt32("OrderId");
            order.OrderDate = reader.GetDateTime("OrderDate");
            order.ShippedDate = reader.GetDateTime("ShippedDate");
            order.FulfilledDate = reader.GetDateTime("FulfilledDate");

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
                Name = reader.GetString("ProductName")
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
