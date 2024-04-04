using System.ComponentModel.DataAnnotations;

namespace Furnish.Models
{
    public class User
    {
        [Key]
        public int userId { get; set; }
        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Role is required.")]
        public string Role { get; set; }
        [Required(ErrorMessage = "Last Name is required.")]
        [Display(Name = "Last Name")]
        public string Surname { get; set; }
        [Required(ErrorMessage = "First Name is required.")]
        [Display(Name = "First Name")]
        public string GivenName { get; set; }
    }
}
