using System.ComponentModel.DataAnnotations;

namespace HotelBooking.API.Models
{
    public class Room
    {
        public int Id { get; set; }

        [Required]
        public int HotelId { get; set; }
        public Hotel Hotel { get; set; }

        [Required]
        [MaxLength(50)]
        public string Type { get; set; }  // Например: Single, Double, Suite

        public double PricePerNight { get; set; }

        public bool IsAvailable { get; set; }

        public ICollection<Booking> Bookings { get; set; }
    }
}
