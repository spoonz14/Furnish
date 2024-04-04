using Furnish.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Furnish.Controllers
{
    public class ProductController : Controller
    {
        private StoreContext context;
		private readonly IConfiguration _config;

		public ProductController(StoreContext ctx, IConfiguration config)
        {
			_config = config;
			context = ctx;
        }

        [Route("[controller]s/All")]
        public IActionResult List(string name = null, string category = null)
        {
            string jwtToken = Request.Cookies["jwtToken"]; // Get the token value
            ViewBag.Token = jwtToken;
            var products = context.Products.AsQueryable();

            var productList = products.ToList();

            if (!string.IsNullOrEmpty(jwtToken))
            {
                var userClaims = ValidateAndDecodeToken(jwtToken);
                if (userClaims != null)
                {

                    bool isAuthenticated = true; // Set isAuthenticated based on token validation
                    ViewBag.IsAuthenticated = isAuthenticated;

                    

                    // Pass the list of products to the view
                    return View(productList);
                }
            }

            return View(productList);
        }



        [HttpGet]
        public IActionResult Search()
        {
            string token = Request.Cookies["jwtToken"];

            if (!string.IsNullOrEmpty(token))
            {
                ViewBag.Token = token; // Set the token value in ViewBag

                bool isAuthenticated = true; // Placeholder for actual token validation logic
                ViewBag.IsAuthenticated = isAuthenticated;
                return View();
            }
            return View();
        }

        [HttpPost]
        public IActionResult Search(string name, string category)
        {
            return RedirectToAction("List", new { name, category });
        }

        [HttpGet]
        [Route("[controller]s/{id}")]
        public IActionResult Details(int id)
        {
            var product = context.Products.FirstOrDefault(p => p.ProductId == id);
            string token = Request.Cookies["jwtToken"];

            if (!string.IsNullOrEmpty(token))
            {
                ViewBag.Token = token; // Set the token value in ViewBag

                bool isAuthenticated = true; // Placeholder for actual token validation logic
                ViewBag.IsAuthenticated = isAuthenticated;
                return View(product);
            }
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

		private ClaimsPrincipal ValidateAndDecodeToken(string token)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]);

			SecurityToken validatedToken;
			var claimsPrincipal = tokenHandler.ValidateToken(token, new TokenValidationParameters
			{
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(key),
				ValidateIssuer = false,
				ValidateAudience = false,
				ClockSkew = TimeSpan.Zero
			}, out validatedToken);

			return claimsPrincipal;
		}
	}
}
