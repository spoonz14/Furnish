using Microsoft.AspNetCore.Mvc;

namespace Furnish.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
