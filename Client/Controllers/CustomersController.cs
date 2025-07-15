using Microsoft.AspNetCore.Mvc; 
using Client.Helpers;
using Client.Models;

namespace Client.Controllers
{ 
    public class CustomersController : Controller 
    { 
        private readonly IHttpHelper _http;
        public CustomersController(IHttpHelper http)
        {
            _http = http;
        }

        public async Task<IActionResult> Index()
        {
            List<CustomerViewModel> customers = await _http.GetAsync<List<CustomerViewModel>>("/api/customers");
            return View(customers);
        }

        public async Task<IActionResult> Details(int id) {
            if (id == 0) {
                return BadRequest();
            }

            CustomerViewModel customer = await _http.GetAsync<CustomerViewModel>($"/api/customers/{id}");
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }
    }
}
