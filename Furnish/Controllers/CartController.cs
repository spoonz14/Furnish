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
            if (Request.Cookies.TryGetValue("jwtToken", out string token))
            {
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
