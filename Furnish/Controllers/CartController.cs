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
        public IActionResult Cart()
        {
            try
            {
                string token = Request.Cookies["jwtToken"];
                Console.WriteLine("Received token: " + token); // Log token value

                if (!string.IsNullOrEmpty(token))
                {
                    ViewBag.Token = token; // Set the token value in ViewBag

                    bool isAuthenticated = true;
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
        public IActionResult AddToCart(int productId)
        {
            try
            {
                // Retrieve the product with the given productId from the database
                var product = _context.Products.FirstOrDefault(p => p.ProductId == productId);

                // If the product doesn't exist, return NotFound
                if (product == null)
                {
                    return NotFound();
                }

                // Retrieve the current cart items from the session
                var cartItems = HttpContext.Session.Get<List<Cart>>("CartItems") ?? new List<Cart>();

                // Check if the product is already in the cart
                var existingItem = cartItems.FirstOrDefault(item => item.ProductId == productId);

                // If the product is already in the cart, increase its quantity
                if (existingItem != null)
                {
                    existingItem.Quantity++;
                }
                // Otherwise, add the product to the cart with quantity 1
                else
                {
                    cartItems.Add(new Cart
                    {
                        ProductId = product.ProductId,
                        ProductName = product.Name,
                        Category = product.CategoryId,
                        Price = product.Price,
                        Quantity = 1
                    });
                }

                // Update the cart items in the session
                HttpContext.Session.Set("CartItems", cartItems);

                // Redirect to the Cart action to display the updated cart
                return RedirectToAction("Cart");
            }
            catch (Exception ex)
            {
                // Log and handle exceptions appropriately
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
