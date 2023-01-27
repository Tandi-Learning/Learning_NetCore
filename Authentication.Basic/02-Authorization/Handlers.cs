using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;

namespace Authorization;

public static class Handlers
{
	public static Func<HttpContext, IDataProtectionProvider, string> Username = (context, dataProtector) =>
	{
        return context.User.FindFirst("usr").Value;
	};

	public static Func<HttpContext, IDataProtectionProvider, string> Login_old = (context, dataProtector) =>
	{
		var protector = dataProtector.CreateProtector("auth-cookie");
		var authCookie = context.Response.Headers["set-cookie"] = $"auth={protector.Protect("usr:tandi.sunarto")}";
		return "ok";
	};

	public static Func<AuthService, string> Login = (authService) =>
	{
		authService.SignIn();
		return "ok";
	};

}

public static class DotNetHandlers
{
	public static Action<HttpContext> Login = async (context) =>
	{
		var claims = new List<Claim>();
		claims.Add(new Claim("usr", "scarlet"));
		var identity = new ClaimsIdentity(claims, "cookie");
		var user = new ClaimsPrincipal(identity);

		await context.SignInAsync(Constants.AUTH_SCHEME, user);
	};

	public static Func<HttpContext, string> Claims = (context) => {
		IEnumerable<Claim> claims = context.User.Claims;
		return "";
	};

	public static Func<HttpContext, IDataProtectionProvider, string> Username = (context, dataProtector) =>
	{
        return context.User.FindFirst("usr").Value;
	};
}
