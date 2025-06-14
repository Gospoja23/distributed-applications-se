using System.ComponentModel.DataAnnotations;

namespace HotelBooking.API.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        [MaxLength(100)]
        public string Password { get; set; }

        [Required]
        [MaxLength(20)]
        public string Role { get; set; } = "User";
    }
}
