using System.Security.Claims;
using Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(Constants.AUTH_SCHEME)
    .AddCookie(Constants.AUTH_SCHEME);

builder.Services.AddAuthorization(builder => {
    builder.AddPolicy("us_passport", pb => {
        pb.RequireAuthenticatedUser()
        .AddAuthenticationSchemes(Constants.AUTH_SCHEME)
        .AddRequirements()
        .RequireClaim("passport", "us");
    });
    builder.AddPolicy("canada_passport", pb => {
        pb.RequireAuthenticatedUser()
        .AddAuthenticationSchemes(Constants.AUTH_SCHEME)
        .RequireClaim("passport", "can"); 
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// app.UseAuthentication();  // do we still to call this function ??

// app.UseAuthorization();  // do we still to call this function ??

// app.Use((context, next) => {
//     if (context.Request.Path.StartsWithSegments("/login"))
//     {
//         return next(context);
//     }

//     if (!context.User.Identity.IsAuthenticated)
//     {
//         context.Response.StatusCode = 401;
//         return Task.CompletedTask;
//     }

//     if (!context.User.HasClaim(claim => claim.Type == "passport" && claim.Value == "us"))
//     {
//         context.Response.StatusCode = 403;
//         return Task.CompletedTask;
//     }
//     return next(context);
// });

app.MapGet("/america", Handlers.America)
    .RequireAuthorization("us_passport");

app.MapGet("/canada", Handlers.Canada)
    .RequireAuthorization("canada_passport");

app.MapGet("/claims", Handlers.Claims);    

app.MapGet("/username", Handlers.Username);

app.MapGet("/login", Handlers.Login)
    .AllowAnonymous();

app.Run();

