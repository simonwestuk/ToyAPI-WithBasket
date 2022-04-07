using System.ComponentModel.DataAnnotations;

namespace ToyAPI.Models
{
    public class LocationModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Postcode { get; set; }
        [Required]
        public string Phone { get; set; }

    }
}
