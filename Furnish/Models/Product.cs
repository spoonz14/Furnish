using System.ComponentModel.DataAnnotations;

namespace Furnish.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        public string Name { get; set; }
        public string? CategoryId { get; set; }
        public double Price { get; set; }
        public string? ImageUrl { get; set; }
    }
}
