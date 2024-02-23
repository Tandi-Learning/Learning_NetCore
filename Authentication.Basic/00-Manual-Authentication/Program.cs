using System.Security.Claims;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDataProtection();

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
app.Use((context, next) => {
    var dataProctection = context.RequestServices.GetRequiredService<IDataProtectionProvider>();
    var protector = dataProctection.CreateProtector("auth-cookie");
    var protectedCookieAuth = context.Request.Cookies.FirstOrDefault(c => c.Key == "auth").Value;
    if (protectedCookieAuth is not null)
    {
        var cookieAuth = protector.Unprotect(protectedCookieAuth);
        var cookieClaims = cookieAuth.Split(";");
        var claims = new List<Claim>();
        foreach(var cookieClaim in cookieClaims) {
            var claimParts = cookieClaim.Trim().Split(":");
            claims.Add(new Claim(claimParts[0], claimParts[1]));
        }
        var claimIdentity = new ClaimsIdentity(claims);
        context.User = new ClaimsPrincipal(claimIdentity);
    }
    return next();
});

app.MapGet("/login", (HttpContext context, IDataProtectionProvider dataProctection) => {
    var protector = dataProctection.CreateProtector("auth-cookie");
    context.Response.Headers.Append("set-cookie", $"auth={protector.Protect("usr:tandi; role:admin; email:tandi.sunarto@hotmail.com")}");
    return "ok";
});

app.MapGet("/username", (HttpContext context) => {
    if (context.User.Claims.Any())
    {
        return context.User.Claims;
    }
    else
    {
        return null;
    }
});

// app.MapGet("/username", (HttpContext context, IDataProtectionProvider dataProctection) => {
//     var protector = dataProctection.CreateProtector("auth-cookie");
//     var cookieAuth = context.Request.Cookies.FirstOrDefault(c => c.Key == "auth").Value;
//     var auth = protector.Unprotect(cookieAuth);
//     var authValues = auth.Split(":");
//     return authValues[0];
// });

app.Run();