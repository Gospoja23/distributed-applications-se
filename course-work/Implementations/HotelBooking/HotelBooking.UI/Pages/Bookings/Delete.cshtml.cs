using HotelBooking.UI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Net.Http.Json;

namespace HotelBooking.UI.Pages.Bookings
{
    public class DeleteModel : PageModel
    {
        private readonly HttpClient _client;
        public DeleteModel(IHttpClientFactory f) => _client = f.CreateClient("HotelAPI");

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
            var resp = await _client.DeleteAsync($"api/bookings/{Booking.Id}");
            if (resp.IsSuccessStatusCode)
                return RedirectToPage("Index");

            ModelState.AddModelError("", "Ошибка при удалении");
            return Page();
        }
    }
}
