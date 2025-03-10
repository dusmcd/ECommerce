using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ECommerceAPI.Data;
using ECommerceAPI.Models;

namespace ECommerceAPI.Controllers
{
    [Route("api/customers")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly EcommerceAPIContext _context;

        public CustomersController(EcommerceAPIContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers(int limit = -1)
        {
           if (limit > 0)
           {
                return await _context.Customers.Take(limit).ToArrayAsync();
           }
            return await _context.Customers.ToListAsync();
        }

        private Customer ProcessCustomer(CustomerDTO customerDTO)
        {
            var customer = new Customer();
            customerDTO.MapToModel(customer);
            DateTime timeNow = DateTime.UtcNow;
            customer.CreatedAt = timeNow;
            customer.UpdatedAt = timeNow;

            return customer;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            if (id == 0)
                return BadRequest();

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
                return NotFound();

            return customer;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer(CustomerDTO customerDTO)
        {
            if (customerDTO == null)
                return BadRequest();

            var customer = ProcessCustomer(customerDTO);
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return Created();
        }

        
    }
}
