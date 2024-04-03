using Furnish.Models;
using Microsoft.AspNetCore.Mvc;

namespace Furnish.Controllers
{
    public class CartController : Controller
    {
        private StoreContext context;

        public CartController(StoreContext ctx)
        {
            context = ctx;
        }

        [HttpGet]
        [Route("[controller]s/{id}")]
        public IActionResult Cart(int id)
        {
            string token = Request.Cookies["jwtToken"]; // Get the token value from cookies
            ViewBag.Token = token; // Set the token value in ViewBag

            if (!string.IsNullOrEmpty(token))
            {
                bool isAuthenticated = true; // Set isAuthenticated based on token validation
                ViewBag.IsAuthenticated = isAuthenticated;

                var product = context.Products.FirstOrDefault(p => p.ProductId == id);

                if (product == null)
                {
                    return NotFound();
                }

                return View(product);
            }

            return BadRequest("Please login to access the cart");
        }
    }
}
