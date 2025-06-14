using HotelBooking.UI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;

namespace HotelBooking.UI.Pages.Rooms
{
    public class DetailsModel : PageModel
    {
        private readonly HttpClient _client;
        public DetailsModel(IHttpClientFactory f) => _client = f.CreateClient("HotelAPI");

        [BindProperty]
        public RoomViewModel Room { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var resp = await _client.GetAsync($"api/rooms/{id}");
            if (!resp.IsSuccessStatusCode) return RedirectToPage("Index");

            Room = await resp.Content.ReadFromJsonAsync<RoomViewModel>() ?? new();
            return Page();
        }
    }
}
