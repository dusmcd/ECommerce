using Microsoft.AspNetCore.Mvc;
using Client.Models;
using Client.Helpers;

namespace Client.Controllers
{
    public class ProductsController : Controller
    {

        private readonly IHttpHelper _httpHelper;

        public ProductsController(IHttpHelper httpHelper)
        {
            _httpHelper = httpHelper;
        }
        public async Task<IActionResult> Index()
        {
            var products = await _httpHelper.GetAsync<List<ProductViewModel>>("/api/products");
            return View(products);
        }

        public async Task<IActionResult> Details(int id)
        {
            if (id <= 0)
            {
                return View("Error");
            }
            var product = await _httpHelper.GetAsync<ProductViewModel>($"/api/products/{id}");
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
    }
}
