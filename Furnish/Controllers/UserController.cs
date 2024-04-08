using Furnish.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Furnish.Controllers
{
    public class UserController : Controller
    {
        private StoreContext context;
        private readonly IConfiguration _config;

        public UserController(StoreContext ctx, IConfiguration config)
        {
            context = ctx;
            _config = config;
        }

		// HTTP GET method for displaying user profile
		[HttpGet("user/profile")]
        public IActionResult Profile()
        {
            bool isAuthenticated = Request.Cookies.ContainsKey("jwtToken"); // Check if jwtToken exists in cookies
            ViewBag.IsAuthenticated = isAuthenticated; // Set ViewBag.IsAuthenticated

            if (isAuthenticated)
            {
                string token = Request.Cookies["jwtToken"]; // Get the token value
                var userClaims = ValidateAndDecodeToken(token);
                if (userClaims != null)
                {
                    var currentUser = new User // Create user object with claims
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
            return RedirectToAction("Login", "Login"); // Redirect to login if token is invalid or not provided
		}

		// HTTP GET method for displaying user registration form
		[HttpGet]
        public IActionResult Register()
        {
            return View();
        }

		// HTTP POST method for handling user registration form submission
		[HttpPost]
        public IActionResult Register(User newUser)
        {
            if (ModelState.IsValid)
            {
                // Check if the username is already taken
                var existingUser = context.Users.FirstOrDefault(u => u.Username == newUser.Username);
        if (existingUser != null)
        {
            TempData["ErrorMessage"] = "Username is already taken.";
            TempData.Keep("ErrorMessage");
            ViewData["ShowErrorMessage"] = true; // Set ViewData to show error message in registration view
            return View(newUser); // Return back to the registration view
        }
                // Add the new user to the database
                context.Users.Add(newUser);
                context.SaveChanges(); // Save changes

                // Set a success message in TempData
                TempData["SuccessMessage"] = "Registration Successful";

                // Clear TempData after displaying the message
                TempData.Keep("SuccessMessage");

                // Redirect to the Login action of LoginController with the success message
                return RedirectToAction("Login", "Login", new { successMessage = TempData["SuccessMessage"] });
            }

            return BadRequest(ModelState); // Return error response with ModelState
        }


        //Token decoder used for displaying user profile information
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
