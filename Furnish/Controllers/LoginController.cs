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
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost("[controller]")]
        public IActionResult Login(UserLogin userLogin)
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
                        //HttpOnly = true,
                        //Secure = true,
                        SameSite = SameSiteMode.Strict,
                        MaxAge = TimeSpan.FromMinutes(15) // Set expiration time for the cookie
                    });

                    // Pass the JWT token as a query parameter to the Index action
                    //return RedirectToAction("Index", "Home", new { jwtToken = token });
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError(string.Empty, "Invalid username or password");
            }

            // If we reach here, there were validation errors or authentication failed
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
    }
}
