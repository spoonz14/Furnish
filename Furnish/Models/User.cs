using System.ComponentModel.DataAnnotations;

namespace Furnish.Models
{
    public class User
    {
        [Key]
        public int userId { get; set; }

        public string username { get; set; }
        public string password { get; set; }
        public string email { get; set; }

    }
}
