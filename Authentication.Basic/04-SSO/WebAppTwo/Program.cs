using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(options => {
    options.DefaultScheme= CookieAuthenticationDefaults.AuthenticationScheme;
    options.RequireAuthenticatedSignIn = false;
})
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme);

builder.Services.AddDataProtection()
    .SetApplicationName("SharedCookie")
    .PersistKeysToFileSystem(new DirectoryInfo(@"..\Key\"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.MapGet("/", () => "Hello World WebAppTwo");

app.MapGet("/login", async (HttpContext ctx) => {
    if (ctx.User.Identity.IsAuthenticated) 
        return "USER IS ALREADY SIGNED IN.";

    Console.WriteLine("Signing in user");
    await ctx.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(new []
    {
        new ClaimsIdentity(new List<Claim>()
        {
            new Claim(ClaimTypes.Name, "Tandi Sunarto"),
            new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
        },
        CookieAuthenticationDefaults.AuthenticationScheme
        )
    }));
    return "SIGNING IN USER.";
});

app.MapGet("/secret", (HttpContext ctx) => {
    if (ctx.User.Identity.IsAuthenticated) 
        return "USER HAS BEEN AUTHENTICATED";

    return "PLEASE AUTHENTICATE FIRST";
});

// var summaries = new[]
// {
//     "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
// };

// app.MapGet("/weatherforecast", () =>
// {
//     var forecast =  Enumerable.Range(1, 5).Select(index =>
//         new WeatherForecast
//         (
//             DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
//             Random.Shared.Next(-20, 55),
//             summaries[Random.Shared.Next(summaries.Length)]
//         ))
//         .ToArray();
//     return forecast;
// })
// .WithName("GetWeatherForecast")
// .WithOpenApi();

app.Run();

// record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
// {
//     public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
// }
