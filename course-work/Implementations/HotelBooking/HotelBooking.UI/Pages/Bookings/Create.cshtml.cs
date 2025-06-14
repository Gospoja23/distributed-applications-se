using HotelBooking.UI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;

namespace HotelBooking.UI.Pages.Bookings
{
    public class CreateModel : PageModel
    {
        private readonly HttpClient _client;
        public CreateModel(IHttpClientFactory f) => _client = f.CreateClient("HotelAPI");

        [BindProperty]
        public BookingViewModel Booking { get; set; } = new();

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var resp = await _client.PostAsJsonAsync("api/bookings", Booking);
            if (resp.IsSuccessStatusCode) return RedirectToPage("Index");

            ModelState.AddModelError("", "Ошибка при создании бронирования");
            return Page();
        }
    }
}
