using CsvHelper;
using Furnish.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Furnish.Utility
{
    public class ProductCsvHelper : Controller
    {
        private readonly StoreContext _context;

        public ProductCsvHelper(StoreContext context)
        {
            _context = context;
        }
        [HttpPost]
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
                        ImageUrl = record.ImageUrl,
                        // Set other properties as needed
                    };

                    _context.Products.Add(product);
                }

                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Product");
            }
        }

    }
}
