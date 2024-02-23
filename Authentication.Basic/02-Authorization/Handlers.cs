using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;

namespace Authorization;

public static class Handlers
{
	public static Action<HttpContext> Login = async (context) =>
	{
		var claims = new List<Claim>() {
			new Claim("usr", "scarlet"),
			new Claim("passport", "us")
		};
		var identity = new ClaimsIdentity(claims, Constants.AUTH_SCHEME);
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

	public static Func<HttpContext, string> America = (context) => {
		// if (!context.User.Identity.IsAuthenticated)
		// {
		// 	context.Response.StatusCode = 401;
		// 	return "";
		// }

		// if (!context.User.HasClaim(claim => claim.Type == "passport" && claim.Value == "us"))
		// {
		// 	context.Response.StatusCode = 403;
		// 	return "";
		// }

		return "United State Of America";
	};

	public static Func<HttpContext, string> Canada = (context) => {
		// if (!context.User.Identity.IsAuthenticated)
		// {
		// 	context.Response.StatusCode = 401;
		// 	return "";
		// }

		// if (!context.User.HasClaim(claim => claim.Type == "passport" && claim.Value == "canada"))
		// {
		// 	context.Response.StatusCode = 403;
		// 	return "";
		// }

		return "Canada";
	};
}
