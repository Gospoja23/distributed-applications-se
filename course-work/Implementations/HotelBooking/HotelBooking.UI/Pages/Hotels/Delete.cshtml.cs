using HotelBooking.UI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Net.Http.Json;

namespace HotelBooking.UI.Pages.Hotels
{
    public class DeleteModel : PageModel
    {
        private readonly HttpClient _client;
        public DeleteModel(IHttpClientFactory factory)
        {
            _client = factory.CreateClient("HotelAPI");
        }

        [BindProperty]
        public HotelViewModel Hotel { get; set; }

        // Загружаем данные отеля по id
        public async Task<IActionResult> OnGetAsync(int id)
        {
            var resp = await _client.GetAsync($"api/hotels/{id}");
            if (!resp.IsSuccessStatusCode)
                return RedirectToPage("Index");

            Hotel = await resp.Content.ReadFromJsonAsync<HotelViewModel>();
            return Page();
        }

        // Обработка подтверждения удаления
        public async Task<IActionResult> OnPostAsync()
        {
            var resp = await _client.DeleteAsync($"api/hotels/{Hotel.Id}");
            if (resp.IsSuccessStatusCode)
                return RedirectToPage("Index");

            ModelState.AddModelError(string.Empty, "Не удалось удалить отель.");
            return Page();
        }
    }
}
