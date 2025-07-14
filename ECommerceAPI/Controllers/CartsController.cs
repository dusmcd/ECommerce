using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ECommerceAPI.Data;
using ECommerceAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Controllers
{
    [Route("api/carts")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly EcommerceAPIContext _context;

        public CartsController(EcommerceAPIContext context)
        {
            _context = context;
        }

        private async Task<bool> AddProductToCart(Cart cart, ProductInfo productInfo)
        {
            var product = await _context.Products.FindAsync(productInfo.Id);
            if (product == null)
                return false;

            cart.Products.Add(product);

            return true;
        }

        private void UpdateQuantity(ProductInfo productInfo, int cartId)
        {
            ProductCart productsCarts = _context.ProductsCarts.Where(pc => pc.CartId == cartId).First();
            productsCarts.Quantity = productInfo.Quantity < 1 ? 1 : productInfo.Quantity;

            _context.ProductsCarts.Update(productsCarts);
        }


        [HttpPost]
        public async Task<IActionResult> CreateCart(CartDTO cartDTO)
        {
            if (cartDTO == null || !ModelState.IsValid)
                return BadRequest();

            Cart cart = new Cart();
            if (!await AddProductToCart(cart, cartDTO.Product))
                return BadRequest("product does not exist");
            

            var customer = await _context.Customers.FindAsync(cartDTO.CustomerId);
            if (customer == null)
                return BadRequest("customer not found");

            cart.Customer = customer;
            DateTime now = DateTime.UtcNow;
            cart.CreatedAt = now;
            cart.UpdatedAt = now;
            cart.Status = CartStatus.Active;
            _context.Carts.Add(cart);

            await _context.SaveChangesAsync();

            UpdateQuantity(cartDTO.Product, cart.Id);

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("{id}/addproduct")]
        public async Task<IActionResult> UpdateCartAddProduct(int id, ProductInfo productInfo)
        {
            Cart? cart = await _context.Carts.FindAsync(id);
            if (cart == null)
                return NotFound();

            if (!await AddProductToCart(cart, productInfo))
                return BadRequest("product does not exist");

            cart.UpdatedAt = DateTime.UtcNow;
            _context.Carts.Update(cart);

            await _context.SaveChangesAsync();

            UpdateQuantity(productInfo, cart.Id);

            await _context.SaveChangesAsync();

            return Ok();
        }

    }
}
