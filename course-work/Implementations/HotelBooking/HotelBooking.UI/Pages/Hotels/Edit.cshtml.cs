using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;
using HotelBooking.UI.Models;

namespace HotelBooking.UI.Pages.Hotels
{
    public class EditModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public EditModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("HotelAPI");
        }

        [BindProperty]
        public HotelViewModel Hotel { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/hotels/{id}");
            if (!response.IsSuccessStatusCode)
                return NotFound();

            Hotel = await response.Content.ReadFromJsonAsync<HotelViewModel>();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var response = await _httpClient.PutAsJsonAsync($"api/hotels/{Hotel.Id}", Hotel);
            if (response.IsSuccessStatusCode)
                return RedirectToPage("Index");

            ModelState.AddModelError(string.Empty, "Ошибка при сохранении изменений.");
            return Page();
        }
    }
}
