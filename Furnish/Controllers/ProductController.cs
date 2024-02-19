using Furnish.Models;
using Microsoft.AspNetCore.Mvc;

namespace Furnish.Controllers
{
    public class ProductController : Controller
    {
        private StoreContext context;

        public ProductController(StoreContext ctx)
        {
            context = ctx;
        }

        public IActionResult Index()
        {
            return RedirectToAction("List", "Product");
        }


        [Route("[controller]s/All")]
        public IActionResult List(string name = null, string category = null)
        {
            var products = context.Products.AsQueryable();

            // Apply filters based on the parameters provided
            //if (id != null)
            //{
            //    products = products.Where(p => p.ProductId == id);
            //}

            if (!string.IsNullOrEmpty(name))
            {
                products = products.Where(p => p.Name.Contains(name));
            }

            if (!string.IsNullOrEmpty(category))
            {
                products = products.Where(p => p.CategoryId == category);
            }

            // Convert the queryable result to a list
            var productList = products.ToList();

            // Pass the list of products to the view
            return View(productList);
        }

        [HttpGet]
        public IActionResult Search()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Search(string name, string category)
        {
            return RedirectToAction("List", new { name, category });
        }
    }
}
