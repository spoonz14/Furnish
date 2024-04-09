using CsvHelper;
using Furnish.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Furnish.Utility
{
    public class ProductCsvHelper : Controller
    {
        private readonly StoreContext _context;
        private readonly IConfiguration _config;



        public ProductCsvHelper(StoreContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpGet("utility/csv")]
        public async Task<IActionResult> ImportProducts()
        {
            bool hasJwtToken = HttpContext.Request.Cookies.ContainsKey("jwtToken");


            if (hasJwtToken) // Check if user is logged in
            {
                ViewData["HasJwtToken"] = hasJwtToken;
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

                    if (currentUser.Role == "Administrator")
                    {
                        bool isAuthenticated = true;
                        ViewBag.IsAuthenticated = isAuthenticated;
                        return View();
                    }

                    //return View(currentUser); // Return view with current user data
                }
            }
            return BadRequest("Access Denied.");
        }

        [HttpPost("utility/csv")]
        public async Task<IActionResult> ImportProducts(IFormFile csvFile)
        {


            if (csvFile == null || csvFile.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            using (var stream = new StreamReader(csvFile.OpenReadStream()))
            {
                var csvReader = new CsvReader(stream, CultureInfo.InvariantCulture);
                var records = csvReader.GetRecords<Product>(); 

                foreach (var record in records)
                {
                    var product = new Product
                    {
                        Name = record.Name,
                        CategoryId = record.CategoryId,
                        Price = record.Price,
                        ImageUrl = record.ImageUrl
                    };

                    _context.Products.Add(product);
                }

                await _context.SaveChangesAsync();

                return RedirectToAction("List", "Product");
            }
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
