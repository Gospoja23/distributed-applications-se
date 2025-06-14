using HotelBooking.UI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Net.Http.Json;

namespace HotelBooking.UI.Pages.Rooms
{
    public class DeleteModel : PageModel
    {
        private readonly HttpClient _client;
        public DeleteModel(IHttpClientFactory f) => _client = f.CreateClient("HotelAPI");

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
            var resp = await _client.DeleteAsync($"api/rooms/{Room.Id}");
            if (resp.IsSuccessStatusCode)
                return RedirectToPage("Index");

            ModelState.AddModelError("", "Ошибка при удалении");
            return Page();
        }
    }
}
