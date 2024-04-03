using System.ComponentModel.DataAnnotations;

namespace Furnish.Models
{
    public class UserLogin
    {
        [Key]
        public int loginId { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }
    }
}
