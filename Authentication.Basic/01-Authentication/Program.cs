using Authentication;
using Microsoft.AspNetCore.DataProtection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ********************************************************
// Manually authenticate the http context
// ********************************************************
// builder.Services.AddHttpContextAccessor();
// builder.Services.AddDataProtection();
// builder.Services.AddScoped<AuthService>();


// ********************************************************
// Use .NET authentication
// ********************************************************
builder.Services.AddAuthentication(Constants.AUTH_SCHEME)
    .AddCookie(Constants.AUTH_SCHEME);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// ********************************************************
// Manually authenticate the http context
// ********************************************************
// app.Use((context, next) => {
//     var authService = context.RequestServices.GetRequiredService<AuthService>();
//     authService.Authenticate(context);

//     return next();
// });

// app.MapGet("/login", Handlers.Login);

app.MapGet("/username", Handlers.Username);

// ********************************************************
// Use .NET authentication
// ********************************************************
app.UseAuthentication();

app.MapGet("/login", DotNetHandlers.Login);

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
