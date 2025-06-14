using HotelBooking.UI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;

namespace HotelBooking.UI.Pages.Rooms
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient _client;
        public IndexModel(IHttpClientFactory factory) => _client = factory.CreateClient("HotelAPI");

        [BindProperty(SupportsGet = true)] public string? SearchType { get; set; }
        [BindProperty(SupportsGet = true)] public bool? OnlyAvailable { get; set; }
        [BindProperty(SupportsGet = true)] public string SortBy { get; set; } = "Type";
        [BindProperty(SupportsGet = true)] public bool IsDescending { get; set; }
        [BindProperty(SupportsGet = true)] public int Page { get; set; } = 1;
        [BindProperty(SupportsGet = true)] public int PageSize { get; set; } = 10;

        public PagedResult<RoomViewModel> PagedRooms { get; set; } = new();

        public async Task OnGetAsync()
        {
            var url = $"api/rooms/search?"
                + $"type={SearchType}&available={OnlyAvailable}&sortBy={SortBy}"
                + $"&isDescending={IsDescending}&page={Page}&pageSize={PageSize}";

            PagedRooms = await _client.GetFromJsonAsync<PagedResult<RoomViewModel>>(url)
                         ?? new PagedResult<RoomViewModel>();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var r = await _client.DeleteAsync($"api/rooms/{id}");
            return r.IsSuccessStatusCode
                ? RedirectToPage(new { SearchType, OnlyAvailable, SortBy, IsDescending, Page, PageSize })
                : BadRequest();
        }
    }
}
