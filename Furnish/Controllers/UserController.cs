using Microsoft.AspNetCore.Mvc;

namespace Furnish.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
