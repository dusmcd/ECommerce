using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ECommerceAPI.Data;
using ECommerceAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ECommerceAPI.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly EcommerceAPIContext _context;
        public ProductsController(EcommerceAPIContext context)
        {
            _context = context;
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Product>>> SearchProducts(string? name)
        {
            if (name.IsNullOrEmpty())
                return await _context.Products.ToListAsync();


            name = name!.ToLower();
            var products = _context.Products.Where(p => p.Name!.ToLower().Contains(name!));
            return await products.ToListAsync();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts(int limit = -1)
        {
            IEnumerable<Product> products;
            if (limit > 0)
            {
                products = _context.Products.Take(limit);
                return products.ToArray();
            }
            products = await _context.Products.ToListAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            if (id == 0)
                return BadRequest();

            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            return product;
        }

        private Product ProcessNewProduct(ProductDTO productDTO)
        {
            Product product = new Product();
            productDTO.MapToModel(product);
            product.CreatedAt = DateTime.UtcNow;
            product.UpdatedAt = DateTime.UtcNow;

            return product;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductDTO productDTO)
        {

            var product = ProcessNewProduct(productDTO);
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return Created();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, ProductDTO productDTO)
        {
            if (id == 0)
                return BadRequest();

            try
            {
                var foundProduct = await _context.Products.FindAsync(id);
                if (foundProduct == null)
                    return NotFound();

                productDTO.MapToModel(foundProduct);
                foundProduct.UpdatedAt = DateTime.UtcNow;
                _context.Products.Update(foundProduct);
                await _context.SaveChangesAsync();
                return Ok();
            } catch(DbUpdateConcurrencyException)
            {
                throw;
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (id == 0)
                return BadRequest();

            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound();
            
            try
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                return Ok();
            } catch(DbUpdateConcurrencyException)
            {
                throw;
            }
        }
    }
}
