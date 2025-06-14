using System.ComponentModel.DataAnnotations;

namespace HotelBooking.API.DTOs
{
    public class HotelCreateDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(200)]
        public string Address { get; set; }

        [MaxLength(20)]
        public string PhoneNumber { get; set; }

        [Range(1, 5)]
        public int Stars { get; set; }
    }
}
