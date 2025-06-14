using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;
using System.IdentityModel.Tokens.Jwt;


namespace HotelBooking.UI.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly HttpClient _client;
        public LoginModel(IHttpClientFactory f) => _client = f.CreateClient("HotelAPI");

        [BindProperty] public string Username { get; set; }
        [BindProperty] public string Password { get; set; }
        public string? ErrorMessage { get; set; }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            var resp = await _client.PostAsJsonAsync("api/auth/login", new { Username, Password });
            if (!resp.IsSuccessStatusCode)
            {
                ErrorMessage = "Неверные учётные данные";
                return Page();
            }

            var result = await resp.Content.ReadFromJsonAsync<Dictionary<string, string>>();
            var token = result!["token"];

            // создаём ClaimsPrincipal на основе JWT
            var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);

            var claims = jwt.Claims.ToList();
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            // логинимся и сохраняем JWT в сессии
            await HttpContext.SignInAsync(principal);
            HttpContext.Session.SetString("JWToken", token);

            return RedirectToPage("/Hotels/Index");
        }
    }
}
