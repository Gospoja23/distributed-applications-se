using System.ComponentModel.DataAnnotations;

namespace HotelBooking.API.Models
{
    public class Booking
    {
        public int Id { get; set; }

        [Required]
        public int RoomId { get; set; }
        public Room Room { get; set; }

        [Required]
        [MaxLength(100)]
        public string GuestName { get; set; }

        [Required]
        public DateTime CheckInDate { get; set; }

        [Required]
        public DateTime CheckOutDate { get; set; }

        public double TotalPrice { get; set; }
    }
}
