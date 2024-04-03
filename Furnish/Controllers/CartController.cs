using Furnish.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Furnish.Controllers
{
    public class CartController : Controller
    {
        private StoreContext context;

        public CartController(StoreContext ctx)
        {
            context = ctx;
        }

        [HttpGet]
        [Route("[controller]/{id}")]
        public IActionResult Cart()
        {
            // Retrieve cart items from TempData or create a new list
            var cartItemsJson = TempData["CartItems"] as string;
            var cartItems = JsonSerializer.Deserialize<List<Cart>>(cartItemsJson) ?? new List<Cart>();

            return View(cartItems);
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

                    // Retrieve existing cart items from TempData or create a new list
                    var cartItems = TempData["CartItems"] as List<Cart> ?? new List<Cart>();

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
                return StatusCode(500, "Internal server error has occured.");
            }
        }
    }
}
