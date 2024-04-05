using Furnish.Models;
using Furnish.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Furnish.Controllers
{
    public class CartController : Controller
    {
        private readonly StoreContext _context;

        public CartController(StoreContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("[controller]/{id}")]
        public IActionResult Cart()
        {
            try
            {
                string token = Request.Cookies["jwtToken"];
                Console.WriteLine("Received token: " + token); // Log token value

                if (!string.IsNullOrEmpty(token))
                {
                    ViewBag.Token = token; // Set the token value in ViewBag

                    bool isAuthenticated = true; // Placeholder for actual token validation logic
                    ViewBag.IsAuthenticated = isAuthenticated;

                    var cartItems = HttpContext.Session.Get<List<Cart>>("CartItems") ?? new List<Cart>();
                    return View(cartItems);
                }

                // Handle case where token is missing or empty
                return RedirectToAction("Login", "Login");
            }
            catch (Exception ex)
            {
                // Log and handle exceptions appropriately
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error has occurred.");
            }
        }

        [HttpPost]
        [Route("[controller]/{id}")]
        public IActionResult Cart(int id)
        {
            try
            {
                string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                ViewBag.Token = token; // Set the token value in ViewBag

                if (!string.IsNullOrEmpty(token))
                {
                    bool isAuthenticated = true; // Set isAuthenticated based on token validation
                    ViewBag.IsAuthenticated = isAuthenticated;

                    var product = _context.Products.FirstOrDefault(p => p.ProductId == id);

                    if (product == null)
                    {
                        return NotFound();
                    }

                    var cartItem = new Cart
                    {
                        ProductId = product.ProductId,
                        ProductName = product.Name,
                        Category = product.CategoryId,
                        Price = product.Price,
                        Quantity = 1
                    };

                    var cartItems = HttpContext.Session.Get<List<Cart>>("CartItems") ?? new List<Cart>();
                    var existingItem = cartItems.FirstOrDefault(item => item.ProductId == id);
                    if (existingItem != null)
                    {
                        existingItem.Quantity++;
                    }
                    else
                    {
                        cartItems.Add(cartItem);
                    }

                    HttpContext.Session.Set("CartItems", cartItems);

                    return RedirectToAction("Cart");
                }

                return BadRequest("Please login to access the cart");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error has occurred.");
            }
        }

        [HttpGet]
        [Route("[controller]/Clear")]
        public IActionResult ClearCart()
        {
            try
            {
                // Clear the cart items from session
                HttpContext.Session.Remove("CartItems");

                // Clear the TempData as well (if needed)
                TempData.Remove("CartItems");

                // Redirect to the Cart action to display an empty cart
                return RedirectToAction("Cart");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error has occurred.");
            }
        }

    }
}
