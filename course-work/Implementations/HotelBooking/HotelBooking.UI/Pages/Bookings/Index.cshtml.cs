using HotelBooking.UI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;

namespace HotelBooking.UI.Pages.Bookings
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient _client;
        public IndexModel(IHttpClientFactory f) => _client = f.CreateClient("HotelAPI");

        [BindProperty(SupportsGet = true)] public string? SearchGuest { get; set; }
        [BindProperty(SupportsGet = true)] public int? RoomId { get; set; }
        [BindProperty(SupportsGet = true)] public string SortBy { get; set; } = "CheckInDate";
        [BindProperty(SupportsGet = true)] public bool IsDesc { get; set; }
        [BindProperty(SupportsGet = true)] public int Page { get; set; } = 1;
        [BindProperty(SupportsGet = true)] public int PageSize { get; set; } = 10;

        public PagedResult<BookingViewModel> PagedBookings { get; set; } = new();

        public async Task OnGetAsync()
        {
            var url = $"api/bookings/search?guestName={SearchGuest}&roomId={RoomId}"
                    + $"&sortBy={SortBy}&isDescending={IsDesc}"
                    + $"&page={Page}&pageSize={PageSize}";
            PagedBookings = await _client.GetFromJsonAsync<PagedResult<BookingViewModel>>(url)
                             ?? new PagedResult<BookingViewModel>();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var r = await _client.DeleteAsync($"api/bookings/{id}");
            return r.IsSuccessStatusCode
                ? RedirectToPage(new { SearchGuest, RoomId, SortBy = SortBy, IsDesc, Page, PageSize })
                : BadRequest();
        }
    }
}
