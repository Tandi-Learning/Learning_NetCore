using System.Security.Claims;
using AuthenticationSchema;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddAuthentication()
    .AddCookie(Constants.LOCAL_AUTH_SCHEME)
    // .AddCookie(Constants.VISITOR_AUTH_SCHEME);
    .AddScheme<CookieAuthenticationOptions, VisitorAutoHandler>(Constants.VISITOR_AUTH_SCHEME, o => {})
    .AddCookie(Constants.PATREON_AUTH_SCHEME)
    .AddOAuth(Constants.PATREON_AUTH, options => {
        options.SignInScheme = Constants.PATREON_AUTH_SCHEME;

        // https://oauth.mocklab.io/
        options.ClientId = "auth-learning";
        options.ClientSecret = "secret";

        options.AuthorizationEndpoint = "https://oauth.mocklab.io/oauth/authorize";
        options.TokenEndpoint = "https://oauth.mocklab.io/oauth/token";
        options.UserInformationEndpoint = "https://oauth.mocklab.io/oauth/userinfo";

        options.CallbackPath = "/cb-patreon";

        options.Scope.Add("profile");
        options.SaveTokens = true;
    });

builder.Services.AddAuthorization(builder => {
    builder.AddPolicy("customer", pb => {
        pb.RequireAuthenticatedUser()
        .AddAuthenticationSchemes(Constants.LOCAL_AUTH_SCHEME, Constants.VISITOR_AUTH_SCHEME);
    });
    builder.AddPolicy("user", pb => {
        pb.RequireAuthenticatedUser()
        .AddAuthenticationSchemes(Constants.LOCAL_AUTH_SCHEME);
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

app.UseAuthentication();

app.UseAuthorization();

app.MapGet("/", ctx => Task.FromResult("Hello World")).RequireAuthorization("customer");

app.MapGet("/america", Handlers.America).RequireAuthorization("us_passport");

app.MapGet("/canada", Handlers.Canada).RequireAuthorization("canada_passport");

app.MapGet("/claims", Handlers.Claims).RequireAuthorization("customer");

app.MapGet("/username", Handlers.Username);

app.MapGet("/login", Handlers.Login);

app.MapGet("/login-patreon", Handlers.LoginPatreon); //.RequireAuthorization("user");

app.Run();