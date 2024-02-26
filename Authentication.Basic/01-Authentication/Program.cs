using Authentication;
using Microsoft.AspNetCore.DataProtection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ********************************************************
// Use .NET authentication
// ********************************************************
builder.Services.AddAuthentication()
    .AddCookie(Constants.AUTH_SCHEME);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/username", Handlers.Username);

// ********************************************************
// Use .NET authentication
// ********************************************************
app.UseAuthentication(); // is this necessary ???
app.MapGet("/login", Handlers.Login);

app.Run();