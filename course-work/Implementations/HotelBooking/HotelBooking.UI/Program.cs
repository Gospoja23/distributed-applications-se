using Microsoft.AspNetCore.Authentication.Cookies;
using HotelBooking.UI;       // если нужен namespace для TokenHandler

var builder = WebApplication.CreateBuilder(args);

// HTTP-клиент для API с JWT-обработчиком
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<TokenHandler>();

builder.Services.AddHttpClient("HotelAPI", client =>
{
    client.BaseAddress = new Uri("https://localhost:7001/"); // SSL‑порт вашего API
})
.AddHttpMessageHandler<TokenHandler>();


// Razor Pages с конвенциями авторизации и сессиями
builder.Services.AddRazorPages(options =>
{
    // Защитить все страницы в папках /Hotels, /Rooms, /Bookings
    options.Conventions.AuthorizeFolder("/Hotels");
    options.Conventions.AuthorizeFolder("/Rooms");
    options.Conventions.AuthorizeFolder("/Bookings");

    // Разрешить анонимный доступ к Login/Logout
    options.Conventions.AllowAnonymousToPage("/Account/Login");
    options.Conventions.AllowAnonymousToPage("/Account/Logout");
});

builder.Services.AddSession();

// Cookie аутентификация
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.Cookie.Name = "HotelBookingAuth";
    });

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Сессии и аутентификация
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

// Просто мапим Razor Pages — защита уже настроена выше
app.MapRazorPages();

app.Run();
