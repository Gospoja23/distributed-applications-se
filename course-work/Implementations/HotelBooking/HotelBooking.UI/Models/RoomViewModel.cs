using System.ComponentModel.DataAnnotations;

namespace HotelBooking.UI.Models
{
    public class RoomViewModel
    {
        public int Id { get; set; }

        [Required]
        public int HotelId { get; set; }

        [Required, MaxLength(50)]
        public string Type { get; set; }

        [Range(0, double.MaxValue)]
        public double PricePerNight { get; set; }

        public bool IsAvailable { get; set; }
    }
}
