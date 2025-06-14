using System.ComponentModel.DataAnnotations;

namespace HotelBooking.API.Models
{
    public class Hotel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(200)]
        public string Address { get; set; }

        [MaxLength(20)]
        public string PhoneNumber { get; set; }

        public int Stars { get; set; }

        public ICollection<Room> Rooms { get; set; }
    }
}
