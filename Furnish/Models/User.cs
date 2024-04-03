using System.ComponentModel.DataAnnotations;

namespace Furnish.Models
{
    public class User
    {
        [Key]
        public int userId { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Role { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public string GivenName { get; set; }
    }
}
