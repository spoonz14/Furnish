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
        public IActionResult Profile()
        {
            return View();
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
                return RedirectToAction("Index", "Home");
            }

            return BadRequest(ModelState); // Return error response with ModelState
        }
    }
}
