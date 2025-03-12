using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ECommerceAPI.Data;
using ECommerceAPI.Models;
using Microsoft.IdentityModel.Tokens;

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

        [HttpGet("session/{sessionId}")]
        public async Task<ActionResult<Customer>> GetCustomerBySessionId(string sessionId)
        {
            if (sessionId.IsNullOrEmpty())
                return BadRequest();

            var customer = await _context.Customers.Where(c => c.SessionId == sessionId).FirstOrDefaultAsync();
            if (customer == null)
                return NotFound();

            return customer;
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Customer>>> SearchCustomers(string? name)
        {
            if (name.IsNullOrEmpty())
                return await _context.Customers.ToListAsync();

            name = name!.ToLower();
            return await _context.Customers.Where(c => c.Name!.ToLower().Contains(name)).ToListAsync();
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

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(int id, CustomerDTO customerDTO)
        {
            if (id == 0 || customerDTO == null)
                return BadRequest();

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
                return NotFound();

            try
            {
                customerDTO.MapToModel(customer);
                _context.Customers.Update(customer);
                customer.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                return Ok();
            } catch(DbUpdateConcurrencyException)
            {
                throw;
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            if (id == 0)
                return BadRequest();

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
                return NotFound();
            try
            {
                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();
                return Ok();
            } catch(DbUpdateConcurrencyException)
            {
                throw;
            }
        }

        
    }
}
