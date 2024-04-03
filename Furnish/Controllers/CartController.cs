using Furnish.Models;
using Furnish.Utility;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Extensions;


namespace Furnish.Controllers
{
    public class CartController : Controller
    {
        private readonly StoreContext context;

        public CartController(StoreContext ctx)
        {
            context = ctx;
        }

        [HttpGet]
        [Route("[controller]/{id}")]
        public IActionResult Cart()
        {
            try
            {
                //string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                string token = Request.Cookies["jwtToken"];
                Console.WriteLine("Received token: " + token); // Log token value

                if (!string.IsNullOrEmpty(token))
                {
                    ViewBag.Token = token; // Set the token value in ViewBag

                    bool isAuthenticated = true; // Placeholder for actual token validation logic
                    ViewBag.IsAuthenticated = isAuthenticated;

                    // Retrieve cart items from TempData or create a new list
                    var cartItemsJson = TempData["CartItems"] as string;
                    var cartItems = string.IsNullOrEmpty(cartItemsJson)
                        ? new List<Cart>()
                        : JsonSerializer.Deserialize<List<Cart>>(cartItemsJson);

                    return View(cartItems);
                }

                // Handle case where token is missing or empty
                return BadRequest("Authentication failed: Token missing or invalid.");
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

                    var product = context.Products.FirstOrDefault(p => p.ProductId == id);

                    if (product == null)
                    {
                        return NotFound();
                    }

                    // Create a cart item using the product details
                    var cartItem = new Cart
                    {
                        ProductId = product.ProductId,
                        ProductName = product.Name,
                        Category = product.CategoryId,
                        Quantity = 1
                    };

                    // Retrieve existing cart items from session or create a new list
                    var cartItems = HttpContext.Session.Get<List<Cart>>("CartItems") ?? new List<Cart>();

                    // Check if the product is already in the cart
                    var existingItem = cartItems.FirstOrDefault(item => item.ProductId == id);
                    if (existingItem != null)
                    {
                        existingItem.Quantity++; // Increment the quantity if the product is already in the cart
                    }
                    else
                    {
                        // Add the product to the cart with initial quantity
                        cartItems.Add(cartItem);
                    }

                    // Store the updated cart items in session
                    HttpContext.Session.Set("CartItems", cartItems);

                    // Serialize the cartItems list before saving it to TempData
                    string serializedCartItems = JsonSerializer.Serialize(cartItems);
                    TempData["CartItems"] = serializedCartItems; // Store the updated cart items in TempData
                    
                    return View(cartItems); // Display the cart view with the updated cart items
                }

                return BadRequest("Please login to access the cart");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error has occurred.");
            }
        }

        [HttpPost]
        [Route("[controller]/Clear")]
        public IActionResult ClearCart()
        {
            try
            {
                // Clear the cart by removing the CartItems from session
                HttpContext.Session.Remove("CartItems");

                // Clear the TempData as well
                TempData.Remove("CartItems");

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
