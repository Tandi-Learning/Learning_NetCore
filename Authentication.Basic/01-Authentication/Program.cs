using Authentication;
using Microsoft.AspNetCore.DataProtection;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ********************************************************
// manual implementation
// ********************************************************
// builder.Services.AddHttpContextAccessor();
// builder.Services.AddDataProtection();
// builder.Services.AddScoped<AuthService>();
// ********************************************************

// ********************************************************
// .NET implementation
// ********************************************************
builder.Services.AddAuthentication("cookie")
    .AddCookie("cookie");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// ********************************************************
// manual implementation
// ********************************************************
// app.Use((context, next) => {
//     var authService = context.RequestServices.GetRequiredService<AuthService>();
//     authService.Authenticate(context);

//     return next();
// });

app.MapGet("/username", Handlers.Username);

// app.MapGet("/login", Handlers.Login);
// ********************************************************

// ********************************************************
// .NET implementation
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
