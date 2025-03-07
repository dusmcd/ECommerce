using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerceAPI.Data;

namespace SeedScript
{
    internal class SeedData
    {
        private readonly EcommerceAPIContext _context;

        public SeedData(EcommerceAPIContext context)
        {
            _context = context;
        }

        public void DoSomething()
        {
            var products = _context.Products.ToList();
            foreach(var product in products)
            {
                Console.WriteLine(product.Name);
            }
        }
    }
}
