using HotelBooking.UI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Net.Http.Json;

namespace HotelBooking.UI.Pages.Hotels
{
    public class DetailsModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public DetailsModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("HotelAPI");
        }

        [BindProperty]
        public HotelViewModel Hotel { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/hotels/{id}");
            if (!response.IsSuccessStatusCode)
                return RedirectToPage("Index");

            Hotel = await response.Content.ReadFromJsonAsync<HotelViewModel>();
            return Page();
        }
    }
}
