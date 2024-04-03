using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Furnish.Models;
using Microsoft.AspNetCore.Mvc;

namespace Furnish.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _config;

        public HomeController(IConfiguration config)
        {
            _config = config;
        }

        public IActionResult Index()
        {
            bool isAuthenticated = Request.Cookies.ContainsKey("jwtToken"); // Check if jwtToken exists in cookies
            ViewBag.IsAuthenticated = isAuthenticated; // Set ViewBag.IsAuthenticated

            if (isAuthenticated)
            {
                string token = Request.Cookies["jwtToken"]; // Get the token value
                var userClaims = ValidateAndDecodeToken(token);
                if (userClaims != null)
                {
                    var currentUser = new User
                    {
                        Username = userClaims.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                        Email = userClaims.FindFirst(ClaimTypes.Email)?.Value,
                        Role = userClaims.FindFirst(ClaimTypes.Role)?.Value,
                        Surname = userClaims.FindFirst(ClaimTypes.Surname)?.Value,
                        GivenName = userClaims.FindFirst(ClaimTypes.GivenName)?.Value
                    };

                    return View(currentUser);
                }
            }

            // If token is invalid or not provided, redirect to login
            return View() ; // Adjust the redirect action and controller as per your application setup
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
