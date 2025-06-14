using System;
using System.ComponentModel.DataAnnotations;

namespace HotelBooking.UI.Models
{
    public class BookingViewModel
    {
        public int Id { get; set; }

        [Required]
        public int RoomId { get; set; }

        [Required, MaxLength(100)]
        public string GuestName { get; set; }

        [Required]
        public DateTime CheckInDate { get; set; }

        [Required]
        public DateTime CheckOutDate { get; set; }

        [Range(0, double.MaxValue)]
        public double TotalPrice { get; set; }
    }
}
