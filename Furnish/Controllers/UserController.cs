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
            return RedirectToAction("Login", "Login");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User newUser)
        {
            if (ModelState.IsValid)
            {
                // Check if the username is already taken
                var existingUser = context.Users.FirstOrDefault(u => u.Username == newUser.Username);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Username", "Username is already taken.");
                    return BadRequest(ModelState); // Return error response with ModelState
                }

                // Add the new user to the database
                context.Users.Add(newUser);
                context.SaveChanges(); // Save changes synchronously

                // Set a success message in TempData
                TempData["SuccessMessage"] = "Registration Successful";

                // Clear TempData after displaying the message
                TempData.Keep("SuccessMessage");

                // Redirect to the Home/Index page
                return RedirectToAction("Login", "Login");
            }

            return BadRequest(ModelState); // Return error response with ModelState
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
