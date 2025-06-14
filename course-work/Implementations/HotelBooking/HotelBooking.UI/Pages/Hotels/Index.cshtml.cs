using HotelBooking.UI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Net.Http.Json;

namespace HotelBooking.UI.Pages.Hotels
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("HotelAPI");
        }

        // Параметры поиска/сортировки/пагинации из query string
        [BindProperty(SupportsGet = true)]
        public string? SearchName { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SearchAddress { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SortBy { get; set; } = "Name";

        [BindProperty(SupportsGet = true)]
        public bool IsDescending { get; set; }

        [BindProperty(SupportsGet = true)]
        public int Page { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 10;

        public PagedResult<HotelViewModel> PagedHotels { get; set; } = new();

        public async Task OnGetAsync()
        {
            var url = $"api/hotels/search?name={SearchName}&address={SearchAddress}"
                    + $"&sortBy={SortBy}&isDescending={IsDescending}"
                    + $"&page={Page}&pageSize={PageSize}";

            PagedHotels = await _httpClient.GetFromJsonAsync<PagedResult<HotelViewModel>>(url)
                          ?? new PagedResult<HotelViewModel>();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/hotels/{id}");
            if (response.IsSuccessStatusCode)
                return RedirectToPage(new
                {
                    SearchName,
                    SearchAddress,
                    SortBy,
                    IsDescending,
                    Page,
                    PageSize
                });

            return BadRequest();
        }
    }
}
