using Microsoft.AspNetCore.Mvc;
using Client.Models;
using Client.Helpers;

namespace Client.Controllers
{
    public class OrdersController : Controller
    {

        private readonly IHttpHelper _httpHelper;

        public OrdersController(IHttpHelper httpHelper)
        {
            _httpHelper = httpHelper;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetProductsFromOrder(int id)
        {
            if (id <= 0)
            {
                // return an empty list
                return Json(new List<ProductViewModel>());
            }

            var products = await _httpHelper.GetAsync<List<ProductViewModel>>($"/api/orders/{id}/products");
            return Json(products);
        }
    }
}
