using System;
using System.Security.Claims;
using Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;

namespace Authentication;

public static class Handlers
{
	public static Func<HttpContext, string> Username = (context) =>
	// public static Func<HttpContext, IDataProtectionProvider, string> Username = (context, dataProtector) =>
	{
		return context.User.FindFirst("usr")?.Value ?? "none";
	};

	public static Action<HttpContext> Login = async (context) =>
	{
		var claims = new List<Claim>(){
			new Claim("usr", "scarlet"),
			new Claim("role", "admin"),
		};
		var identity = new ClaimsIdentity(claims, Constants.AUTH_SCHEME);
		var user = new ClaimsPrincipal(identity);

		await context.SignInAsync(Constants.AUTH_SCHEME, user);
	};
}
