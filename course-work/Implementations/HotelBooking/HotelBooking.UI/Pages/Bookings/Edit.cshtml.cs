using HotelBooking.UI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;

namespace HotelBooking.UI.Pages.Bookings
{
    public class EditModel : PageModel
    {
        private readonly HttpClient _client;
        public EditModel(IHttpClientFactory f) => _client = f.CreateClient("HotelAPI");

        [BindProperty]
        public BookingViewModel Booking { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var resp = await _client.GetAsync($"api/bookings/{id}");
            if (!resp.IsSuccessStatusCode) return RedirectToPage("Index");

            Booking = await resp.Content.ReadFromJsonAsync<BookingViewModel>() ?? new();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var resp = await _client.PutAsJsonAsync($"api/bookings/{Booking.Id}", Booking);
            if (resp.IsSuccessStatusCode) return RedirectToPage("Index");

            ModelState.AddModelError("", "Ошибка при сохранении");
            return Page();
        }
    }
}
