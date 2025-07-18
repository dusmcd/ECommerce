﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ECommerceAPI.Data;
using ECommerceAPI.Models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Cors;

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
        public async Task<ActionResult<IEnumerable<CustomerDTO>>> GetCustomers(int limit = -1)
        {
           if (limit > 0)
           {
                return await _context.Customers
                    .Take(limit)
                    .Select(c => new CustomerDTO
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Email = c.Email
                    }).ToListAsync();
           }

            return await _context.Customers
                 .Select(c => new CustomerDTO
                 {
                    Id = c.Id,
                    Name = c.Name,
                    Email = c.Email,
                 }).ToListAsync();
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
        public async Task<ActionResult<CustomerDTO>> GetCustomer(int id)
        {
            if (id == 0)
                return BadRequest();

            var customer = await _context.Customers
                .Where(c => c.Id == id)
                .Include(c => c.Orders)
                .Select(c => new CustomerDTO
                {
                    Id = c.Id,
                    Name = c.Name,
                    Email = c.Email,
                    PhoneNumber = c.PhoneNumber,
                    Address1 = c.Address1,
                    Address2 = c.Address2,
                    City = c.City,
                    State = c.State,
                    ZipCode = c.ZipCode,
                    Orders = c.Orders
                        .Take(10)
                        .Select(o => new OrderDTO
                        {
                            Id = o.Id,
                            OrderDate = o.OrderDate,
                            OrderStatus = o.OrderStatus,
                        })
                        .OrderBy(o => o.OrderDate)
                        .ToList()
                }).FirstAsync();

            if (customer == null)
                return NotFound();

            return customer;
        }

        [HttpPost]
        [EnableCors("AllowFrontend")]
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
