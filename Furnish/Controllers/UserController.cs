using Furnish.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Furnish.Controllers
{
    public class UserController : Controller
    {
        private StoreContext context;

        public UserController(StoreContext ctx)
        {
            context = ctx;
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

                // You can hash the password here before storing it
                // newUser.Password = HashPassword(newUser.Password);

                // Add the new user to the database
                context.Users.Add(newUser);
                context.SaveChanges(); // Save changes synchronously

                // Set a success message in TempData
                TempData["SuccessMessage"] = "Registration Successful";

                // Clear TempData after displaying the message
                TempData.Keep("SuccessMessage");

                // Redirect to the Home/Index page
                return RedirectToAction("Index", "Home");
            }

            return BadRequest(ModelState); // Return error response with ModelState
        }





        //[HttpGet("Admins")]
        //[Authorize]
        //public IActionResult AdminsEndpoint()
        //{
        //    var currentUser = GetCurrentUser();
        //    if (currentUser.Role == "Administrator")
        //    {
        //        return Ok($"Hi {currentUser.GivenName}, you are an {currentUser.Role}");
        //    }
        //    return Ok($"Hi {currentUser.GivenName}, you are a {currentUser.Role}");
        //}


        //private User GetCurrentUser()
        //{
        //    var identity = HttpContext.User.Identity as ClaimsIdentity;

        //    if (identity != null)
        //    {
        //        var userClaims = identity.Claims;

        //        return new User
        //        {
        //            Username = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value,
        //            Email = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Email)?.Value,
        //            Role = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value,
        //            Surname = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Surname)?.Value,
        //            GivenName = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.GivenName)?.Value,
        //        };
        //    }
        //    return null;
        //}


    }
}
