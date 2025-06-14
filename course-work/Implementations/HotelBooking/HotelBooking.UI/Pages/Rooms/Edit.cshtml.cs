using HotelBooking.UI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;

namespace HotelBooking.UI.Pages.Rooms
{
    public class EditModel : PageModel
    {
        private readonly HttpClient _client;
        public EditModel(IHttpClientFactory f) => _client = f.CreateClient("HotelAPI");

        [BindProperty]
        public RoomViewModel Room { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var resp = await _client.GetAsync($"api/rooms/{id}");
            if (!resp.IsSuccessStatusCode) return RedirectToPage("Index");

            Room = await resp.Content.ReadFromJsonAsync<RoomViewModel>() ?? new();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var resp = await _client.PutAsJsonAsync($"api/rooms/{Room.Id}", Room);
            if (resp.IsSuccessStatusCode) return RedirectToPage("Index");

            ModelState.AddModelError("", "Ошибка при сохранении");
            return Page();
        }
    }
}
