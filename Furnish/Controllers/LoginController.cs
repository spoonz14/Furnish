using Furnish.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Furnish.Controllers
{
    public class LoginController : Controller
    {
        private IConfiguration _config;
        private StoreContext context;

        public LoginController(IConfiguration config, StoreContext ctx)
        {
            _config = config;
            context = ctx;
        }

        [HttpGet("[controller]")]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null, string successMessage = null)
        {
            ModelState.Clear();
            ViewData["ReturnUrl"] = returnUrl;
            ViewBag.SuccessMessage = successMessage;
            return View();
        }

        [AllowAnonymous]
        [HttpPost("[controller]")]
        public IActionResult Login(UserLogin userLogin, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var user = Authenticate(userLogin);

                if (user != null)
                {
                    var token = Generate(user);

                    // Create a cookie to store the token
                    Response.Cookies.Append("jwtToken", token, new CookieOptions
                    {
                        SameSite = SameSiteMode.Strict,
                        MaxAge = TimeSpan.FromMinutes(15) // Set expiration time for the cookie
                    });

                    // Redirect to the returnUrl if it is not null or empty
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    // Set a success message in TempData
                    TempData["SuccessMessage"] = "Login Successful!";

                    // Clear TempData after displaying the message
                    TempData.Keep("SuccessMessage");

                    // Redirect to the default action if returnUrl is null or empty
                    return RedirectToAction("Index", "Home", new { successMessage = TempData["SuccessMessage"] });
                }
                ModelState.AddModelError(string.Empty, "Invalid username or password");
            }

            //validation errors or authentication failed
            return View(userLogin); // Return the view with validation errors
        }


        private string Generate(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, user.Username),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim(ClaimTypes.Surname, user.Surname),
            new Claim(ClaimTypes.GivenName, user.GivenName)
        };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private User Authenticate(UserLogin userLogin)
        {
            var currentUser = context.Users.FirstOrDefault(o => o.Username.ToLower() ==
            userLogin.Username.ToLower() && o.Password == userLogin.Password);

            if (currentUser != null)
            {
                return currentUser;
            }
            return null;
        }

        public IActionResult Logout()
        {
            // Clear the JWT token cookie by setting its expiration to a past date
            Response.Cookies.Delete("jwtToken");


            return RedirectToAction("Index", "Home");
        }
    }
}

