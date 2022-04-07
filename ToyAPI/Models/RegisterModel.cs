using System.ComponentModel.DataAnnotations;

namespace ToyAPI.Models
{
    public class RegisterModel
    {
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }
        [Required]
        [MaxLength(150)]
        public string Email { get; set; }
        [Required]
        [MaxLength(150)]
        public string Password { get; set;
        }
    }
}
