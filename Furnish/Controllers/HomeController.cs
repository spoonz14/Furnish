using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Furnish.Models;
using Microsoft.AspNetCore.Mvc;

namespace Furnish.Controllers
{
    public class HomeController : Controller // Controller class for managing home operations
	{
        private readonly IConfiguration _config;

        public HomeController(IConfiguration config)
        {
            _config = config;
        }

		// Action method for handling the home page request
		public IActionResult Index(string successMessage = null)
        {
            bool isAuthenticated = Request.Cookies.ContainsKey("jwtToken"); // Check if jwtToken exists in cookies
            ViewBag.IsAuthenticated = isAuthenticated; // Set ViewBag.IsAuthenticated
            ViewBag.SuccessMessage = successMessage;

            if (isAuthenticated) // Check if user is authenticated
			{
                string token = Request.Cookies["jwtToken"]; // Get the token value
                var userClaims = ValidateAndDecodeToken(token); // Validate and decode the token
				if (userClaims != null) // Check if token is valid
				{
                    var currentUser = new User // Create user object with claims
					{
                        Username = userClaims.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                        Email = userClaims.FindFirst(ClaimTypes.Email)?.Value,
                        Role = userClaims.FindFirst(ClaimTypes.Role)?.Value,
                        Surname = userClaims.FindFirst(ClaimTypes.Surname)?.Value,
                        GivenName = userClaims.FindFirst(ClaimTypes.GivenName)?.Value
                    };

                    return View(currentUser); // Return view with current user data
				}
            }

            // If token is invalid or not provided, redirect to login
            return View() ; 
        }

		// Method to validate and decode JWT token
		private ClaimsPrincipal ValidateAndDecodeToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]); // Get the key from config

			SecurityToken validatedToken; // Variable to hold validated token
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
